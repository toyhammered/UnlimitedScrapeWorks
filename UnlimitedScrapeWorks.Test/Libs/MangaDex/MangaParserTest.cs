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

        private void ZippyZiggy()
        {
            _web.Load(@"Fixtures/MangaDex/ZippyZiggy.html");
            _parser = new MangaParser(_web, 2);
        }

        // Missing information
        private void KimiJaNakyaDameNanda()
        {
            _web.Load(@"Fixtures/MangaDex/KimiJaNakyaDameNanda.html");
            _parser = new MangaParser(_web, 2001);

        }

        // Hentai
        private void FutariNoMeikyuuOujo()
        {
            _web.Load(@"Fixtures/MangaDex/FutariNoMeikyuuOujo.html");
            _parser = new MangaParser(_web, 31238);
        }

        // Related in Body (sequels, side stories, etc..)
        private void Major()
        {
            _web.Load(@"Fixtures/MangaDex/Major.html");
            _parser = new MangaParser(_web, 927);
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
        public void FindNameTest()
        {
            ZippyZiggy();
            Assert.Equal("Zippy Ziggy", _parser.FindName());
        }

        [Fact]
        public void FindNameTest_Hentai()
        {
            FutariNoMeikyuuOujo();
            Assert.Equal("Futari no Meikyuu Oujo", _parser.FindName());
        }

        [Fact]
        public void FindRelated_Amount()
        {
            Major();
            Assert.Equal(2, _parser.FindRelated().Count);
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

        [Fact]
        public void FindGenreTagsTest_NotPresent()
        {
            FutariNoMeikyuuOujo();
            Assert.Empty(_parser.FindGenreTags());
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
            Assert.Equal("completed", _parser.FindPublishStatus());
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
        public void FindDescriptionTest_Present()
        {
            ZippyZiggy();
            Assert.Contains("Reputation is everything in", _parser.FindDescription());
        }

        [Fact]
        public void FindDescriptionTest_NotPresent()
        {
            FutariNoMeikyuuOujo();
            Assert.Null(_parser.FindDescription());
        }

        [Fact]
        public void FindTotalChaptersTest()
        {
            ZippyZiggy();
            Assert.Equal(172, _parser.FindTotalChapters());
        }

        [Fact]
        public void FindTotalChaptersTest_NoChapters()
        {
            KimiJaNakyaDameNanda();
            Assert.Equal(0, _parser.FindTotalChapters());
        }

        [Fact]
        public void FindHentaiTest_NotPresent()
        {
            ZippyZiggy();
            Assert.False(_parser.FindHentai());
        }

        [Fact]
        public void FindHentaiTest_Present()
        {
            FutariNoMeikyuuOujo();
            Assert.True(_parser.FindHentai());
        }

        [Fact]
        public void FindExternalLinksTest_NotPresent()
        {
            FutariNoMeikyuuOujo();
            Assert.Empty(_parser.FindExternalLinks());
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
