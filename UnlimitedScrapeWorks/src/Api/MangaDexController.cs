using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;
using UnlimitedScrapeWorks.src.Providers;

namespace UnlimitedScrapeWorks.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MangaDexController : ControllerBase
    {

        private readonly IMangaDexProvider _provider;


        public MangaDexController(IMangaDexProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        // POST api/mangadex/range
        [HttpPost("range")]
        public async Task<IActionResult> PostRange(
            [FromBody] MangaDexPostRangeRequest request
        )
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                return BadRequest(ModelState);
            }
            else if (request.StartAmount > request.EndAmount)
            {
                return BadRequest("Start Amount needs to be less than or equal to EndAmount");
            }

            try
            {
                await _provider.PostRange(request.StartAmount, request.EndAmount, request.BatchAmount);
                return Ok("success!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
