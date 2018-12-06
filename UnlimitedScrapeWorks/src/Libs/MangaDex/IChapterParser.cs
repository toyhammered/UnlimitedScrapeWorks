using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs.MangaDex
{
    public interface IChapterParser
    {
        string MangaTitleSlug { get; }
        int MangaId { get; }
        int MangaTotalChapters { get; }
        int AdditionalPages { get; }

        Task<List<MangaChapter>> Process();
        Task Parse(HtmlDocument page);

        Dictionary<string, string> FindAltTitles(HtmlNode node);
        string ChapterRowDataValue(HtmlNode node, string key);
        string FindLanguage(HtmlNode node);
        int? FindVolume(HtmlNode node);
        int FindChapter(HtmlNode node);
        string FindUploadDate(HtmlNode node);
        int CalculateAdditionalPages(int mangaTotalChapters);
    }
}
