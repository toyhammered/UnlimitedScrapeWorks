using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace UnlimitedScrapeWorks.src.Sites
{
    public class MangaDexSite : IMangaDexSite
    {
        private readonly string URL = @"https://mangadex.org/title/";
        private readonly HtmlWeb _web = new HtmlWeb();

        //public MangaDexSite()
        //{
        //   
        //}

        public async Task<HtmlDocument> GetAll(int currentPage)
        {
            return await _web.LoadFromWebAsync($"{URL}{currentPage}");
        }

        public async Task<HtmlDocument> GetExtraPages(int mangaId, string mangaSlug, int currentPage)
        {
            return await _web.LoadFromWebAsync($"{URL}{mangaId}/{mangaSlug}/chapters/{currentPage}");
        }
    }
}
