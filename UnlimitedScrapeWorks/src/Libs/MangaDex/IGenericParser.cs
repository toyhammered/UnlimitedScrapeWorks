using System;
using HtmlAgilityPack;

namespace UnlimitedScrapeWorks.src.Libs.MangaDex
{
    public interface IGenericParser
    {
        HtmlNode Root();
        HtmlNode CardBodyMangaDetails();
        HtmlNode CardBodymangaImage();
        HtmlNode CardBodyMangaDetail();
        HtmlNode CardBody();
        HtmlNode CardHeader();
        HtmlNode ChaptersNav();
    }
}
