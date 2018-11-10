using System;
using HtmlAgilityPack;
using System.Linq;

namespace UnlimitedScrapeWorks.src.Libs.MangaDex
{
    public class GenericParser
    {
        HtmlDocument Page { get; }

        public GenericParser(HtmlDocument page)
        {
            this.Page = page;
        }


        HtmlNode Root()
        {
            return Page.DocumentNode;
        }

        public HtmlNode CardHeader()
        {
            return Root().SelectSingleNode(@"//h6[contains(@class, 'card-header')]");
        }

        public HtmlNode CardBody()
        {
            return Root().SelectSingleNode(@"//div[contains(@class, 'card-body')]/div[1]");
        }

        public HtmlNode CardBodyMangaDetails()
        {
            return CardBody().SelectSingleNode(@"div[2]");
        }

        public HtmlNode CardBodyMangaImage()
        {
            return CardBody().SelectSingleNode(@"div[1]");
        }

        public HtmlNode CardBodyMangaDetail(string detailType)
        {
            return CardBodyMangaDetails().ChildNodes
                                         .Where(node => node.Name.Equals("div"))
                                         .First(node => node.Name.Equals("div") &&
                                                node.SelectSingleNode("div[contains(@class, 'strong')]").InnerText.Contains(detailType));
        }

        public HtmlNode ChaptersNav()
        {
            return Root().SelectSingleNode(@"//ul[contains(@class, 'edit')]/li[1]/a[1]");
        }
    }
}
