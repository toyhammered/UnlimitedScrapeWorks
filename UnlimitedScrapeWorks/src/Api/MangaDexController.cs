using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // GET api/mangadex/all
        [HttpGet("all")]
        public Task<string> GetAll()
        {

            try
            {
                return _provider.GetAll();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
