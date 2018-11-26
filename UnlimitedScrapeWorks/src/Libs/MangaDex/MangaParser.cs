using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;
using UnlimitedScrapeWorks.src.CustomExceptions.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs.MangaDex
{
    public class MangaParser : IMangaParser
    {
        readonly string THUMBNAIL_URL = "https://mangadex.org/images/manga/";

        GenericParser _genericParser { get; set; }
        public string TitleSlug { get; set; }
        public int Id { get; }

        public MangaParser(HtmlDocument page, int mangaId)
        {
            _genericParser = new GenericParser(page);
            Id = mangaId;
        }

        public async Task<MangaDexMangaResponse> Process()
        {
            this.TitleSlug = FindSlug();

            return await Task.Run(
                () => new MangaDexMangaResponse()
                {
                    Id = Id,
                    Title = SetTitle(),
                    AltTitles = FindAltTitles(),
                    GenreTags = FindGenreTags(),
                    Thumbnail = FindThumbnail(),
                    PublishStatus = FindPublishStatus(),
                    Demographic = FindDemographic(),
                    Author = FindAuthor(),
                    Artist = FindArtist(),
                    Description = FindDescription(),
                    TotalChapters = FindTotalChapters(),
                    Hentai = FindHentai(),
                    ExternalLinks = FindExternalLinks()
                }
            );
        }

        public string ParsedMangaHref(int position)
        {
            var href = _genericParser.ChaptersNav().GetAttributeValue("href", null);

            if (String.IsNullOrEmpty(href))
            {
                throw new Exception();
            }

            return href.Split("/")[position];
        }

        public string FindId()
        {
            return ParsedMangaHref(2);
        }

        public MangaTitle SetTitle()
        {
            return new MangaTitle()
            {
                Name = FindName(),
                Origin = FindOrigin(),
                Slug = FindSlug()

            };
        }

        public string FindName()
        {
            return _genericParser.CardHeader().InnerText.Trim();
        }


        public string FindOrigin()
        {
            return _genericParser.CardHeader().Descendants("img")
                               .First()
                               .GetAttributeValue("title", null);
        }

        public string FindSlug()
        {
            return ParsedMangaHref(3);
        }

        public List<string> FindAltTitles()
        {
            var altTitles = new List<string>();

            try
            {
                // First div is "Alt Title(s)"
                // Second div contains the list of alt titles (ul > li)
                _genericParser.CardBodyMangaDetail("Alt name(s)").SelectNodes(@"div[2]/ul/li")
                                               .ToList()
                                               .ForEach(node => altTitles.Add(node.InnerText.Trim()));

            }
            catch (NodeNotFoundException) { }
            catch (ArgumentNullException) { }

            return altTitles;
        }

        public List<string> FindGenreTags()
        {
            var genreTags = new List<string>();

            try
            {
                _genericParser.CardBodyMangaDetail("Genres").SelectNodes(@"div[2]/span/a")
                                             .ToList()
                                             .ForEach(node => genreTags.Add(node.InnerText.Trim()));
            }
            catch (NodeNotFoundException) { }
            catch (ArgumentNullException) { }

            return genreTags;
        }

        public string FindThumbnail()
        {
            return $"{THUMBNAIL_URL}{Id}.jpg";
        }

        public string FindPublishStatus()
        {
            return _genericParser.CardBodyMangaDetail("Pub. status").SelectSingleNode(@"div[2]")
                                                     .InnerText.Trim();
        }

        public string FindDemographic()
        {
            return _genericParser.CardBodyMangaDetail("Demographic").SelectSingleNode(@"div[2]/span/a")
                                                .InnerText.Trim();
        }

        public string FindAuthor()
        {
            return _genericParser.CardBodyMangaDetail("Author").SelectSingleNode(@"div[2]/a")
                                                .InnerText.Trim();
        }

        public string FindArtist()
        {
            return _genericParser.CardBodyMangaDetail("Artist").SelectSingleNode(@"div[2]/a")
                                                .InnerText.Trim();
        }

        public string FindDescription()
        {
            return _genericParser.CardBodyMangaDetail("Description").SelectSingleNode(@"div[2]")
                                                     .InnerText.Trim();
        }

        public int FindTotalChapters()
        {
            int totalChapters;

            try
            {
                // Will also do additional pages.
                totalChapters = Convert.ToInt32(
                   _genericParser.CardBodyMangaDetail("Stats").SelectNodes(@"div[2]/ul/li")
                                               .ToList()
                                               .First(node => node.FirstChild.GetAttributeValue("title", "").Equals("Total chapters"))
                                               .InnerText.Trim()
                );
            }
            catch (Exception)
            {
                totalChapters = 0;
            }

            return totalChapters;
        }

        public bool FindHentai()
        {

            var hentaiNode = _genericParser.CardHeader().SelectSingleNode("span[contains(@class, 'badge-danger')]");
            return hentaiNode != null;
        }

        public Dictionary<string, string> FindExternalLinks()
        {
            var results = new Dictionary<string, string>();

            try
            {
                var nodes = _genericParser.CardBodyMangaDetail("Links").SelectNodes("div[2]/ul/li").ToList();

                if (nodes.Any())
                {
                    foreach (var node in nodes)
                    {
                        var linkNode = node.SelectSingleNode("a");
                        results.Add(
                            linkNode.InnerText,
                            linkNode.GetAttributeValue("href", null)
                        );
                    }
                }
            }
            catch (MissingMangaDetailException) { }

            return results;
        }
    }
}
