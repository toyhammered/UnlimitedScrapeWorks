using System;
using System.Collections.Generic;

namespace UnlimitedScrapeWorks.src.ContractModels.MangaDex
{
    public class MangaDexMangaResponse
    {
        public String Id { get; set; }

        public MangaTitle Title { get; set; }
        public List<string> AltTitles { get; set; }
        public List<string> GenreTags { get; set; }

        public String Thumbnail { get; set; }
        public String PublishStatus { get; set; }
        public String Demographic { get; set; }
        public String Author { get; set; }
        public String Artist { get; set; }
        public String Description { get; set; }
        public int TotalChapters { get; set; }

        public Boolean Hentai { get; set; }

        public Dictionary<string, string> ExternalLinks { get; set; }
        public List<MangaChapters> Chapters { get; set; }
    }

    public class MangaTitle
    {
        public String Name { get; set; }
        public String Slug { get; set; }
        public String Origin { get; set; }
    }

    public class MangaChapters
    {
        //private string _uploadDate;

        public List<string> AltTitles { get; set; }
        public int Volume { get; set; }
        public int Chapter { get; set; }
        public String UploadDate { get; set; }
        //public String UploadDate
        //{
        //    get
        //    {
        //        return this._uploadDate;
        //    }

        //    set
        //    {
        //        _uploadDate = $"{value}xxxxx";
        //    }
        //}
    }
}

