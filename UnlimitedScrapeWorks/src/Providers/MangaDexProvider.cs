using System;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;
using UnlimitedScrapeWorks.src.Sites;

namespace UnlimitedScrapeWorks.src.Providers
{
    public class MangaDexProvider : IMangaDexProvider
    {
        //private readonly int TOTAL = 32000;
        private readonly IMangaDexSite _site;

        public int AdditionalPages { get; set; }
        public string TitleSlug { get; set; }
        public HtmlDocument Page { get; set; }


        public MangaDexProvider(
            IMangaDexSite site
        )
        {
            _site = site ?? throw new ArgumentNullException(nameof(site));
            AdditionalPages = 0;
            TitleSlug = null;
        }

        public async void GetAll()
        {
            // TODO: figure out how to loop based on number and batch by 400
            this.Page = await _site.GetAll(2);

            this.TitleSlug = FindTitleSlug();

            // TODO: check if you need to async this.
            var manga = ParseSitePage();

        }

        public MangaDexMangaResponse ParseSitePage()
        {
            return new MangaDexMangaResponse()
            {
                Title = SetTitle()
            };
        }

        public MangaTitle SetTitle()
        {
            return new MangaTitle()
            {
                Name = FindMangaName(),
                Origin = FindMangaOrigin()
            };
        }

        public string FindTitleSlug()
        {
            var href = Root().SelectSingleNode(@"//ul[contains(@class, 'edit')]")
                             .Descendants("li").First()
                             .Descendants("a").First()
                             .GetAttributeValue("href", null);

            // TODO: maybe think of a generic way to handle this and throw errors
            // just remember to make sure your errors are very specific because
            // I need to know what failed.
            if (String.IsNullOrEmpty(href))
            {
                throw new Exception();
            }
            return ParsedTitleSlug(href);
        }

        public string ParsedTitleSlug(string href)
        {
            return href.Split("/")[3];
        }

        public string FindMangaName()
        {
            return CardHeader().InnerText.Trim();
        }


        public string FindMangaOrigin()
        {
            return CardHeader().Descendants("img")
                               .First()
                               .GetAttributeValue("title", null);
        }

        private HtmlNode CardHeader()
        {
            return Root().SelectSingleNode(@"//h6[@class='card-header']");
        }

        private HtmlNode Root()
        {
            return Page.DocumentNode;
        }
    }
}
