﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;
using UnlimitedScrapeWorks.src.CustomExceptions.MangaDex;
using UnlimitedScrapeWorks.src.Sites;

namespace UnlimitedScrapeWorks.src.Libs.MangaDex
{
    public class ChapterParser : IChapterParser
    {
        private readonly IMangaDexSite _site;

        public HtmlDocument FirstPage { get; set; }
        public string MangaTitleSlug { get; }
        public int MangaId { get; }
        public int MangaTotalChapters { get; }
        public int AdditionalPages { get; }
        public List<MangaChapter> Chapters { get; set; }

        public ChapterParser(
            IMangaDexSite site, HtmlDocument page, int mangaId,
            string mangaTitleSlug, int mangaTotalChapters
        )
        {
            _site = site ?? throw new ArgumentNullException(nameof(site));

            FirstPage = page;
            MangaId = mangaId;
            MangaTitleSlug = mangaTitleSlug;
            MangaTotalChapters = mangaTotalChapters;
            AdditionalPages = CalculateAdditionalPages(mangaTotalChapters);
            Chapters = new List<MangaChapter>();
        }

        public async Task<List<MangaChapter>> Process()
        {
            await Parse(FirstPage);
            await ParseAdditionalPages();

            return Chapters;
        }

        public async Task ParseAdditionalPages()
        {
            if (AdditionalPages == 0)
            {
                return;
            }

            for (int i = 1; i <= AdditionalPages; i++)
            {
                // need to add 1 to the page for the correct "ID"
                var page = await _site.GetExtraPages(MangaId, MangaTitleSlug, i + 1);
                await Parse(page);
            }
        }

        public async Task Parse(HtmlDocument page)
        {
            if (MangaTotalChapters == 0)
            {
                return;
            }

            var genericParser = new GenericParser(page);

            await Task.Run(() =>
            {
                foreach (var chapterNode in genericParser.Chapters())
                {
                    try
                    {
                        var chapter = new MangaChapter()
                        {
                            AltTitles = FindAltTitles(chapterNode),
                            Volume = FindVolume(chapterNode),
                            Chapter = FindChapter(chapterNode),
                            UploadDate = FindUploadDate(chapterNode)
                        };

                        // TODO: extract this so it can be reused with chapter.
                        var existingChapterIndex = Chapters.FindIndex(c => c.Chapter.Equals(chapter.Chapter) && c.Volume.Equals(chapter.Volume));

                        if (existingChapterIndex == -1)
                        {
                            Chapters.Add(chapter);
                        }
                        else
                        {
                            CombineChapters(existingChapterIndex, chapter);
                        }
                    }
                    catch (InvalidChapterException)
                    {

                    }
                }
            });
        }

        public void CombineChapters(int existingChapterIndex, MangaChapter chapter)
        {
            foreach (var altTitle in chapter.AltTitles)
            {
                Chapters[existingChapterIndex].AltTitles.Add(altTitle.Key, altTitle.Value);
            }
        }

        public Dictionary<string, string> FindAltTitles(HtmlNode node)
        {
            var altTitles = new Dictionary<string, string>();
            var language = FindLanguage(node);
            var title = ChapterRowDataValue(node, "title");

            altTitles.Add(language, title);

            return altTitles;
        }

        public string ChapterRowDataValue(HtmlNode node, string key)
        {
            return node.SelectSingleNode("div/div").GetAttributeValue($"data-{key}", null);
        }

        public string FindLanguage(HtmlNode node)
        {
            try
            {
                return node.SelectSingleNode("div/div/div[contains(@class, 'chapter-list-flag')]/img")
                           .GetAttributeValue("title", null).ToLower();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int? FindVolume(HtmlNode node)
        {
            var volume = ChapterRowDataValue(node, "volume");
            int? parsedVolume;

            Int32.TryParse(volume, out int result);
            parsedVolume = result;
            return parsedVolume == 0 ? null : parsedVolume;
        }

        public int FindChapter(HtmlNode node)
        {
            try
            {
                return Int32.Parse(ChapterRowDataValue(node, "chapter"));
            }
            catch (Exception)
            {
                // specifically throw an exception so that this can be skipped.
                // Example: 79.2 will be skipped, currently don't support decimals.
                throw new InvalidChapterException();
            }
        }

        public string FindUploadDate(HtmlNode node)
        {
            try
            {
                long converted = Convert.ToInt64(ChapterRowDataValue(node, "timestamp"));
                return DateTimeOffset.FromUnixTimeSeconds(converted).ToString().Split(" ").First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int CalculateAdditionalPages(int mangaTotalChapters)
        {
            double result = ((double)mangaTotalChapters / 100.0);
            // we subtract 1 because we already have the first page.
            return result.Equals(0) ? 0 : Convert.ToInt32(Math.Ceiling(result)) - 1;
        }
    }
}
