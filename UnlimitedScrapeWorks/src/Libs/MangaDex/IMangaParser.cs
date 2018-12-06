using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs.MangaDex
{
    public interface IMangaParser
    {
        Task<MangaDexMangaResponse> Process();

        string TitleSlug { get; set; }
        int Id { get; }

        string FindId();

        MangaTitle SetTitle();
        List<MangaTitle> FindRelated();

        string FindName();
        string FindName(HtmlNode node);
        string FindOrigin();
        string FindSlug();
        string FindSlug(HtmlNode node);

        string ParsedMangaHref(int position);

        List<string> FindAltTitles();
        List<string> FindGenreTags();
        string FindThumbnail();
        string FindPublishStatus();
        string FindDemographic();
        string FindAuthor();
        string FindArtist();
        string FindDescription();
        int FindTotalChapters();

        bool FindHentai();
        Dictionary<string, string> FindExternalLinks();

    }
}
