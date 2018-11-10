using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs.MangaDex
{
    public interface IMangaParser
    {
        string FindId();

        MangaTitle SetTitle();
        string FindName();
        string FindOrigin();
        string FindSlug();

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
