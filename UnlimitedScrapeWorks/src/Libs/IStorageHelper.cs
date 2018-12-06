using System;
using System.Threading.Tasks;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs
{
    public interface IStorageHelper
    {
        Task AddRecord(MangaDexMangaResponse manga);
        Task CreateAndUploadFiles(int batchAmount);
    }
}
