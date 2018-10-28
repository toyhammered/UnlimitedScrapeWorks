using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UnlimitedScrapeWorks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaDexController : ControllerBase
    {
        // GET api/mangadex/all
        [HttpGet("all")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value3", "value2" };
        }
    }
}
