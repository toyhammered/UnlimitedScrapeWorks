using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnlimitedScrapeWorks.src.ContractModels
{
    public class QueueRequests
    {
        public List<Task> Manga { get; set; }
        public List<Task> Chapter { get; set; }
    }
}
