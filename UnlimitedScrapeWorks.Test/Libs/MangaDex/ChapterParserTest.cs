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
            _web.Load(@"Fixtures/MangaDex/ZippyZiggy.html");
            _parser = new ChapterParser(new MangaDexSite(), _web, 2, "zippy-ziggy", 172);
            // Will need to set this for only certain tests
            _genericParser = new GenericParser(_web);
        }

        [Theory]
        [InlineData(0, "Marzenia się spełniają!")]
        [InlineData(1, "Dreams Come True!")]
        public void FindTitleTest(int rowNumber, string title)
        {
            var node = _genericParser.Chapters()[rowNumber];
            Assert.Equal(title, _parser.ChapterRowDataValue(node, "title"));
        }

        [Theory]
        [InlineData(0, "Polish")]
        [InlineData(1, "English")]
        public void FindLanguageTest(int rowNumber, string language)
        {
            var node = _genericParser.Chapters()[rowNumber];
            Assert.Equal(language, _parser.FindLanguage(node));
        }

        [Fact]
        public void FindVolumeTest()
        {
            var node = _genericParser.Chapters()[0];
            Assert.Equal(11, _parser.FindVolume(node));
        }

        [Fact]
        public void FindChapterTest()
        {
            var node = _genericParser.Chapters()[0];
            Assert.Equal(86, _parser.FindChapter(node));
        }

        [Fact]
        public void FindUploadDateTest()
        {
            var node = _genericParser.Chapters()[0];
            Assert.Equal("1519465023", _parser.FindUploadDate(node));
        }

    }
}
