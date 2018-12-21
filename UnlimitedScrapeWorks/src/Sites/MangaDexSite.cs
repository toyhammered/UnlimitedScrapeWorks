using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using UnlimitedScrapeWorks.src.ContractModels;

namespace UnlimitedScrapeWorks.src.Sites
{
    public class MangaDexSite : IMangaDexSite
    {
        private readonly string URL = @"https://mangadex.org/title/";
        private readonly HtmlWeb _web = new HtmlWeb();
        private int Retry { get; set; }

        public QueueRequests FIFO_QUEUE { get; set; }
        public QueueResults QUEUE_RESULTS { get; set; }
        public List<string> COMPLETED_REQUESTS { get; set; }
        public int TOTAL_REQUESTS { get; set; }

        public MangaDexSite()
        {
            FIFO_QUEUE = new QueueRequests()
            {
                Manga = new List<Task>(),
                Chapter = new List<Task>()
            };

            QUEUE_RESULTS = new QueueResults()
            {
                Manga = new Dictionary<string, HtmlDocument>(),
                Chapter = new Dictionary<string, HtmlDocument>()
            };

            TOTAL_REQUESTS = 0;
            Retry = 0;

            var ts = new ThreadStart(GetThrottled);
            var backgroundThread = new Thread(ts)
            {
                Name = "FIFO_QUEUE_CHECKER"
            };
            backgroundThread.Start();
        }

        public async Task<HtmlDocument> GetPage(int currentPage)
        {
            var key = $"{currentPage}";
            var task = new Task(async () =>
            {
                QUEUE_RESULTS.Manga.Add(
                   key,
                   await _web.LoadFromWebAsync($"{URL}{currentPage}")
                );
            });

            FIFO_QUEUE.Manga.Add(task);

            return await CheckMangaQueue(key);
        }

        public async Task<HtmlDocument> GetExtraPages(int mangaId, string mangaSlug, int currentPage)
        {
            var key = $"{mangaId}-{currentPage}";
            var task = new Task(async () =>
            {
                QUEUE_RESULTS.Chapter.Add(
                    $"{key}",
                    await _web.LoadFromWebAsync($"{URL}{mangaId}/{mangaSlug}/chapters/{currentPage}")
                );
            });

            FIFO_QUEUE.Chapter.Add(task);

            return await CheckChapterQueue(key);
        }

        private void GetThrottled()
        {
            while (Retry < 10)
            {
                // sleep or delay.
                Thread.Sleep(1000);

                if (FIFO_QUEUE.Chapter.Count > 0)
                {
                    var task = FIFO_QUEUE.Chapter[0];
                    FIFO_QUEUE.Chapter.RemoveAt(0);
                    task.Start();
                    Task.WaitAny(task);
                    this.Retry = 0;
                }
                else if (FIFO_QUEUE.Manga.Count > 0)
                {
                    var task = FIFO_QUEUE.Manga[0];
                    FIFO_QUEUE.Manga.RemoveAt(0);
                    task.Start();
                    Task.WaitAny(task);
                    this.Retry = 0;
                }
                else
                {
                    this.Retry += 1;
                }
            }
        }

        private async Task<HtmlDocument> CheckMangaQueue(string key)
        {
            while (true)
            {
                Thread.Sleep(550);

                if (QUEUE_RESULTS.Manga.ContainsKey(key))
                {
                    return await Task.Run(() =>
                    {
                        var page = QUEUE_RESULTS.Manga[key];
                        TOTAL_REQUESTS += 1;
                        Console.WriteLine($"Time: {DateTime.UtcNow.ToString()} - Manga: {key}");
                        QUEUE_RESULTS.Manga.Remove(key);
                        return page;
                    });

                }
            }
        }

        private async Task<HtmlDocument> CheckChapterQueue(string key)
        {
            while (true)
            {
                Thread.Sleep(550);

                if (QUEUE_RESULTS.Chapter.ContainsKey(key))
                {
                    return await Task.Run(() =>
                    {
                        var page = QUEUE_RESULTS.Chapter[key];
                        TOTAL_REQUESTS += 1;
                        Console.WriteLine($"Time: {DateTime.UtcNow.ToString()} - Chapter: {key}");
                        QUEUE_RESULTS.Chapter.Remove(key);
                        return page;
                    });

                }
            }
        }
    }
}
