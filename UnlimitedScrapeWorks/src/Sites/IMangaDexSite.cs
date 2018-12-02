using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace UnlimitedScrapeWorks.src.Sites
{
    public interface IMangaDexSite
    {
        Task<HtmlDocument> GetPage(int mangaId);
        Task<HtmlDocument> GetExtraPages(int mangaId, string mangaSlug, int currentPage);

        List<string> COMPLETED_REQUESTS { get; set; }
    }
}
