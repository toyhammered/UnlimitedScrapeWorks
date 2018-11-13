using System;
using Xunit;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.Libs.MangaDex;

namespace UnlimitedScrapeWorks.Test.Libs.MangaDex
{
    public class MangaParserTest
    {
        readonly HtmlDocument _web = new HtmlDocument();
        MangaParser _parser { get; set; }

        public MangaParserTest()
        {

        }

        public void ZippyZiggy()
        {
            _web.Load(@"Fixtures/MangaDex/ZippyZiggy.html");
            _parser = new MangaParser(_web, 2);
        }

        [Fact]
        public void FindSlugTest()
        {
            ZippyZiggy();
            Assert.Equal("zippy-ziggy", _parser.FindSlug());
        }

        [Fact]
        public void FindIdTest()
        {
            ZippyZiggy();
            Assert.Equal("2", _parser.FindId());
        }

        [Fact]
        public void FindOriginTest()
        {
            ZippyZiggy();
            Assert.Equal("Korean", _parser.FindOrigin());
        }

        [Fact]
        public void FindAltTitlesTest_CorrectAmount()
        {
            ZippyZiggy();
            Assert.Equal(3, _parser.FindAltTitles().Count);
        }

        [Theory]
        [InlineData("Быстрый Зигги")]
        [InlineData("知彼知己")]
        [InlineData("지피지기")]
        public void FindAltTitlesTest_CorrectTitles(string title)
        {
            ZippyZiggy();
            Assert.Contains(title, _parser.FindAltTitles());
        }

        [Fact]
        public void FindGenreTagsTest_CorrectAmount()
        {
            ZippyZiggy();
            Assert.Equal(6, _parser.FindGenreTags().Count);
        }

        [Theory]
        [InlineData("Action")]
        [InlineData("Comedy")]
        [InlineData("Ecchi")]
        [InlineData("Martial Arts")]
        [InlineData("Romance")]
        [InlineData("School Life")]
        public void FindGenreTagsTest_CorrectGenres(string genre)
        {
            ZippyZiggy();
            Assert.Contains(genre, _parser.FindGenreTags());
        }

        [Fact]
        public void FindThumbnailTest()
        {
            ZippyZiggy();
            Assert.Equal("https://mangadex.org/images/manga/2.jpg", _parser.FindThumbnail());
        }

        [Fact]
        public void FindPublishStatusTest()
        {
            ZippyZiggy();
            Assert.Equal("Completed", _parser.FindPublishStatus());
        }

        [Fact]
        public void FindDemographicTest()
        {
            ZippyZiggy();
            Assert.Equal("Shounen", _parser.FindDemographic());
        }

        [Fact]
        public void FindAuthorTest()
        {
            ZippyZiggy();
            Assert.Equal("Kim Eun-jung", _parser.FindAuthor());
        }

        [Fact]
        public void FindArtistTest()
        {
            ZippyZiggy();
            Assert.Equal("Hwang Seung-man", _parser.FindArtist());
        }

        [Fact]
        public void FindDescriptionTest()
        {
            ZippyZiggy();
            Assert.Contains("Reputation is everything in", _parser.FindDescription());
        }

        [Fact]
        public void FindTotalChaptersTest()
        {
            ZippyZiggy();
            Assert.Equal(172, _parser.FindTotalChapters());
        }

        [Fact]
        public void FindHentaiTest_NotPresent()
        {
            ZippyZiggy();
            Assert.False(_parser.FindHentai());
        }

        [Theory]
        [InlineData("MangaUpdates", "https://www.mangaupdates.com/series.html?id=9717")]
        [InlineData("MyAnimeList", "https://myanimelist.net/manga/3368")]
        public void FindExternalLinksTest_Present(string site, string href)
        {
            ZippyZiggy();
            var externalLinks = _parser.FindExternalLinks();
            Assert.Equal(href, externalLinks[site]);
        }
    }
}
