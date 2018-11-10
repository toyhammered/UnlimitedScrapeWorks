using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;
using UnlimitedScrapeWorks.src.Libs.MangaDex;
using UnlimitedScrapeWorks.src.Sites;

namespace UnlimitedScrapeWorks.src.Providers
{
    public class MangaDexProvider : IMangaDexProvider
    {
        private readonly IMangaDexSite _site;

        public MangaDexProvider(IMangaDexSite site)
        {
            _site = site ?? throw new ArgumentNullException(nameof(site));
        }

        public async Task<string> GetAll()
        {
            // TODO: add looping here, we need to catch and just skip errors
            MangaDexMangaResponse manga;

            try
            {
                // TODO: figure out how to loop based on number and batch by 400
                var page = await _site.GetAll(2);


                // TODO: check if you need to async this.
                manga = await new MangaParser(page).Process();
                //manga.TotalChapters = "";

            }
            catch (Exception ex)
            {

            }

            return "Success";
        }
    }
}
