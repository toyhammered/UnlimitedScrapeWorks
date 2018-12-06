//using System;
//using Xunit;
//using HtmlAgilityPack;
//using UnlimitedScrapeWorks.src.Providers;
//using UnlimitedScrapeWorks.src.Sites;
//using UnlimitedScrapeWorks.src.ContractModels.MangaDex;
//using System.Collections.Generic;

//namespace UnlimitedScrapeWorks.Test
//{
//    public class MangaDexProviderTest
//    {
//        readonly IMangaDexProvider _provider;


//        public MangaDexProviderTest()
//        {
//            _provider = new MangaDexProvider(new MangaDexSite());
//            _web.Load(@"Fixtures/MangaDex/ZippyZiggy.html");
//            _provider.Page = _web;
//        }

//        // look into how to test this with async??
//        //[Fact]
//        //public async void SetMangaIdTest()
//        //{
//        //    await _provider.SetTitleSlugAndMangaId();
//        //    Assert.Equal("zippy-ziggy", _provider.TitleSlug);
//        //}

//        //[Fact]
//        //public async void FindMangaNameTest()
//        //{
//        //    await _provider.SetTitleSlugAndMangaId();
//        //    Assert.Equal("2", _provider.MangaId);
//        //}

//        [Fact]
//        public void FindMangaOriginTest()
//        {
//            Assert.Equal("Korean", _provider.FindMangaOrigin());
//        }

//        [Fact]
//        public void FindAltTitlesTest_CorrectAmount()
//        {
//            Assert.Equal(3, _provider.FindAltTitles().Count);
//        }

//        [Theory]
//        [InlineData("Быстрый Зигги")]
//        [InlineData("知彼知己")]
//        [InlineData("지피지기")]
//        public void FindAltTitlesTest_CorrectTitles(string title)
//        {
//            Assert.Contains(title, _provider.FindAltTitles());
//        }

//        [Fact]
//        public void FindGenreTagsTest_CorrectAmount()
//        {
//            Assert.Equal(6, _provider.FindGenreTags().Count);
//        }

//        [Theory]
//        [InlineData("Action")]
//        [InlineData("Comedy")]
//        [InlineData("Ecchi")]
//        [InlineData("Martial Arts")]
//        [InlineData("Romance")]
//        [InlineData("School Life")]
//        public void FindGenreTagsTest_CorrectGenres(string genre)
//        {
//            Assert.Contains(genre, _provider.FindGenreTags());
//        }

//        [Fact]
//        public void FindThumbnailTest()
//        {
//            _provider.MangaId = "2";
//            Assert.Equal("https://mangadex.org/images/manga/2.jpg", _provider.FindThumbnail());
//        }

//        [Fact]
//        public void FindPublishStatusTest()
//        {
//            Assert.Equal("Completed", _provider.FindPublishStatus());
//        }

//        [Fact]
//        public void FindDemographicTest()
//        {
//            Assert.Equal("Shounen", _provider.FindDemographic());
//        }

//        [Fact]
//        public void FindAuthorTest()
//        {
//            Assert.Equal("Kim Eun-jung", _provider.FindAuthor());
//        }

//        [Fact]
//        public void FindArtistTest()
//        {
//            Assert.Equal("Hwang Seung-man", _provider.FindArtist());
//        }

//        [Fact]
//        public void FindDescriptionTest()
//        {
//            Assert.Contains("Reputation is everything in", _provider.FindDescription());
//        }

//        [Fact]
//        public void FindTotalChaptersTest()
//        {
//            Assert.Equal(173, _provider.FindTotalChapters());
//        }
//    }
//}
