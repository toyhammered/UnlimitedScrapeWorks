using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private readonly IMangaDexSite _site;
        private readonly IStorageHelper _storage;

        public MangaDexProvider(IMangaDexSite site, IStorageHelper storage)
        {
            _site = site ?? throw new ArgumentNullException(nameof(site));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public async Task<string> PostRange(int startAmount, int endAmount, int batchAmount)
        {
            var startTime = DateTime.UtcNow;
            var throttler = new SemaphoreSlim(2, 4);
            var mangaIds = Enumerable.Range(startAmount, endAmount - startAmount + 1);
            var TaskList = mangaIds.Select(async mangaId =>
            {
                await throttler.WaitAsync();
                MangaDexMangaResponse manga;

                try
                {
                    var page = await _site.GetPage(mangaId);

                    manga = await new MangaParser(page, mangaId).Process();
                    manga.Chapters = await new ChapterParser(_site, page, mangaId, manga.Title.Slug, manga.TotalChapters).Process();

                    await _storage.AddRecord(manga);
                }
                catch (Exception)
                {
                    // Log something here as to why it is being skipped
                }
                finally
                {
                    throttler.Release();
                }
            });

            await Task.WhenAll(TaskList);
            await _storage.CreateAndUploadFiles(batchAmount);
            Console.WriteLine($"Start: {startTime} - End: {DateTime.UtcNow}");
            Console.WriteLine($"Total: {(startTime - DateTime.UtcNow).TotalSeconds} seconds");
            Console.WriteLine($"Total Requests Made: {_site.TOTAL_REQUESTS}");
            Console.WriteLine("**************");
            return "Success";
        }
    }
}
