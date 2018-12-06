using System;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using UnlimitedScrapeWorks.src.CustomExceptions.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs.MangaDex
{
    public class GenericParser : IGenericParser
    {
        HtmlDocument Page { get; }

        public GenericParser(HtmlDocument page)
        {
            this.Page = page;
        }

        public HtmlNode Root()
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
            try
            {
                return CardBodyMangaDetails().ChildNodes
                             .Where(node => node.Name.Equals("div"))
                             .First(node => node.Name.Equals("div") &&
                                    node.SelectSingleNode("div[contains(@class, 'strong')]").InnerText.Contains(detailType));
            }
            catch (InvalidOperationException)
            {
                throw new MissingMangaDetailException(detailType);
            }

        }

        public HtmlNode ChaptersNav()
        {
            return Root().SelectSingleNode(@"//ul[contains(@class, 'edit')]/li[1]/a[1]");
        }

        public List<HtmlNode> Chapters()
        {
            try
            {
                var chapterNodes = Root().SelectSingleNode(@"//div[@id='content']/div[contains(@class, 'edit')]/div")
                         .ChildNodes
                         .Where(node => node.Name.Equals("div"))
                         .ToList();

                // Remove the first node because it just holds images and no content
                chapterNodes.RemoveAt(0);
                return chapterNodes;
            }
            catch (Exception)
            {
                return new List<HtmlNode>();
            }

        }
    }
}
