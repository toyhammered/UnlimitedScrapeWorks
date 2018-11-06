using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Providers
{
    public interface IMangaDexProvider
    {
        int AdditionalPages { get; set; }
        string TitleSlug { get; set; }
        HtmlDocument Page { get; set; }

        void GetAll();
        MangaDexMangaResponse ParseSitePage();

        MangaTitle SetTitle();
        string FindTitleSlug();
        string ParsedTitleSlug(string href);
        string FindMangaName();
        string FindMangaOrigin();
    }
}
