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
        private readonly int START_AMOUNT = 9;
        private readonly int END_AMOUNT = 9;
        //private readonly int BATCH_AMOUNT = 400;

        private readonly IMangaDexSite _site;
        private readonly IStorageHelper _storage;

        public MangaDexProvider(IMangaDexSite site, IStorageHelper storage)
        {
            _site = site ?? throw new ArgumentNullException(nameof(site));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public async Task<string> GetAll()
        {
            var startTime = DateTime.UtcNow;
            var throttler = new SemaphoreSlim(2, 4);
            var mangaIds = Enumerable.Range(START_AMOUNT, END_AMOUNT - START_AMOUNT + 1);
            var TaskList = mangaIds.Select(async mangaId =>
            {
                await throttler.WaitAsync();
                MangaDexMangaResponse manga;

                try
                {
                    //TODO: figure out how to loop based on number and batch by 400
                    var page = await _site.GetAll(mangaId);

                    manga = await new MangaParser(page, mangaId).Process();
                    manga.Chapters = await new ChapterParser(_site, page, mangaId, manga.Title.Slug, manga.TotalChapters).Process();

                    await _storage.AddRecord(manga);
                    Console.WriteLine($"{DateTime.UtcNow} - Completed Manga: {mangaId}.");
                    //await _storage.CreateFileCheck(i);
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
            await _storage.CreateFile();
            Console.WriteLine($"Start: {startTime} - End: {DateTime.UtcNow}");
            Console.WriteLine($"Total: {(startTime - DateTime.UtcNow).TotalSeconds} seconds");
            return "Success";
        }
    }
}
