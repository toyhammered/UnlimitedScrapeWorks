using System;
using HtmlAgilityPack;

namespace UnlimitedScrapeWorks.src.Libs.MangaDex
{
    public interface IGenericParser
    {
        HtmlNode Root();
        HtmlNode CardBodyMangaDetails();
        HtmlNode CardBodyMangaImage();
        HtmlNode CardBodyMangaDetail(string detailType);
        HtmlNode CardBody();
        HtmlNode CardHeader();
        HtmlNode ChaptersNav();
    }
}
