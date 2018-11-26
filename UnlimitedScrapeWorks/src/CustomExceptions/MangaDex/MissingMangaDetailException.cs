using System;
namespace UnlimitedScrapeWorks.src.CustomExceptions.MangaDex
{
    public class MissingMangaDetailException : Exception
    {
        public MissingMangaDetailException()
        {
        }

        public MissingMangaDetailException(string section)
            : base($"{section} - does not exist")
        {
        }


    }
}
