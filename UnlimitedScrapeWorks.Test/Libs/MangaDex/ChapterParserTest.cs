using System;
using System.Linq;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.Libs.MangaDex;
using UnlimitedScrapeWorks.src.Sites;
using Xunit;

namespace UnlimitedScrapeWorks.Test.Libs.MangaDex
{
    public class ChapterParserTest
    {
        readonly HtmlDocument _web = new HtmlDocument();
        ChapterParser _parser { get; set; }
        GenericParser _genericParser { get; set; }

        public ChapterParserTest()
        {

        }

        private void ZippyZiggy()
        {
            _web.Load(@"Fixtures/MangaDex/ZippyZiggy.html");
            _parser = new ChapterParser(new MangaDexSite(), _web, 2, "zippy-ziggy", 172);
            // Will need to set this for only certain tests
            _genericParser = new GenericParser(_web);
        }

        private void KimiJaNakyaDameNanda()
        {
            _web.Load(@"Fixtures/MangaDex/KimiJaNakyaDameNanda.html");
            _parser = new ChapterParser(new MangaDexSite(), _web, 2001, "kimi-ja-nakya-dame-nanda", 0);
            // Will need to set this for only certain tests
            _genericParser = new GenericParser(_web);
        }

        [Theory]
        [InlineData(0, "Marzenia się spełniają!")]
        [InlineData(1, "Dreams Come True!")]
        public void FindTitleTest(int rowNumber, string title)
        {
            ZippyZiggy();
            var node = _genericParser.Chapters()[rowNumber];
            Assert.Equal(title, _parser.ChapterRowDataValue(node, "title"));
        }

        [Theory]
        [InlineData(0, "polish")]
        [InlineData(1, "english")]
        public void FindLanguageTest(int rowNumber, string language)
        {
            ZippyZiggy();
            var node = _genericParser.Chapters()[rowNumber];
            Assert.Equal(language, _parser.FindLanguage(node));
        }

        [Fact]
        public void FindVolumeTest()
        {
            ZippyZiggy();
            var node = _genericParser.Chapters()[0];
            Assert.Equal(11, _parser.FindVolume(node));
        }

        [Fact]
        public void FindChapterTest_Present()
        {
            ZippyZiggy();
            var node = _genericParser.Chapters()[0];
            Assert.Equal(86, _parser.FindChapter(node));
        }

        [Fact]
        public void FindUploadDateTest()
        {
            ZippyZiggy();
            var node = _genericParser.Chapters()[0];
            Assert.Equal("2/24/18", _parser.FindUploadDate(node));
        }

    }
}
