using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;
using UnlimitedScrapeWorks.src.Libs;
using UnlimitedScrapeWorks.src.Libs.MangaDex;
using UnlimitedScrapeWorks.src.Sites;

namespace UnlimitedScrapeWorks.src.Providers
{
    public class MangaDexProvider : IMangaDexProvider
    {
        private readonly int START_AMOUNT = 2;
        private readonly int END_AMOUNT = 2;
        private readonly int BATCH_AMOUNT = 400;

        private readonly IMangaDexSite _site;
        private readonly IStorageHelper _storage;

        public MangaDexProvider(IMangaDexSite site, IStorageHelper storage)
        {
            _site = site ?? throw new ArgumentNullException(nameof(site));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public async Task<string> GetAll()
        {
            // TODO: add looping here, we need to catch and just skip errors
            for (int i = START_AMOUNT; i <= END_AMOUNT; i++)
            {
                MangaDexMangaResponse manga;

                try
                {
                    //TODO: figure out how to loop based on number and batch by 400
                    var page = await _site.GetAll(i);


                    // TODO: check if you need to async this.
                    manga = await new MangaParser(page, i).Process();
                    manga.Chapters = await new ChapterParser(_site, page, i, manga.Title.Slug, manga.TotalChapters).Process();

                    await _storage.AddRecord($"{START_AMOUNT}-{END_AMOUNT}", manga);
                    await _storage.CreateFileCheck(i);
                }
                catch (Exception)
                {
                    // Log something here as to why it is being skipped
                }
            }

            return "Success";
        }
    }
}
