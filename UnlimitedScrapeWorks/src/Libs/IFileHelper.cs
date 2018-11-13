using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs
{
    public interface IFileHelper
    {
        Task SaveMangaRecords(List<MangaDexMangaResponse> records, string key);
    }
}
