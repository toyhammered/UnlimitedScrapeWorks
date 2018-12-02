using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace UnlimitedScrapeWorks.src.ContractModels.MangaDex
{
    public class MangaDexPostRangeRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "StartAmount: must be greater than 0")]
        public int StartAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EndAmount: must be greater than 0")]
        public int EndAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "BatchAmount: must be greater than 0")]
        public int BatchAmount { get; set; }
    }
}
