using System;
using Xunit;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.Providers;
using UnlimitedScrapeWorks.src.Sites;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.Test
{
    public class MangaDexProviderTest
    {
        readonly IMangaDexProvider _provider;
        readonly HtmlDocument _web = new HtmlDocument();

        public MangaDexProviderTest()
        {
            _provider = new MangaDexProvider(new MangaDexSite());
            _web.Load(@"Fixtures/MangaDex/ZippyZiggy.html");
            _provider.Page = _web;
        }

        [Fact]
        public void FindTitleSlugTest()
        {
            Assert.Equal("zippy-ziggy", _provider.FindTitleSlug());
        }

        [Fact]
        public void FindMangaNameTest()
        {
            Assert.Equal("Zippy Ziggy", _provider.FindMangaName());
        }

        [Fact]
        public void FindMangaOriginTest()
        {
            Assert.Equal("Korean", _provider.FindMangaOrigin());
        }
    }
}
