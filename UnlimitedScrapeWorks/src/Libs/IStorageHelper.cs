using System;
using System.Threading.Tasks;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs
{
    public interface IStorageHelper
    {
        Task AddRecord(string key, MangaDexMangaResponse manga);
        Task CreateFileCheck(int position);
    }
}
