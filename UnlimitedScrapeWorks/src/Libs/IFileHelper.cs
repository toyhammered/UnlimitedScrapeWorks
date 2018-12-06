using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs
{
    public interface IFileHelper
    {
        String TimeStamp { get; }
        String FilePath { get; }

        Task SaveMangaRecords(List<MangaDexMangaResponse> records, string key);
    }
}
