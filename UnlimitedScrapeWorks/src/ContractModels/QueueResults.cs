using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace UnlimitedScrapeWorks.src.ContractModels
{
    public class QueueResults
    {
        public Dictionary<string, HtmlDocument> Manga { get; set; }
        public Dictionary<string, HtmlDocument> Chapter { get; set; }
    }
}
