using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Providers
{
    public interface IMangaDexProvider
    {
        Task<string> GetAll();
    }
}
