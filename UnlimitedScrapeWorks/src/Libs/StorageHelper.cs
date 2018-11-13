using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs
{
    public class StorageHelper : IStorageHelper
    {
        private readonly IFileHelper _file;
        public Dictionary<string, List<MangaDexMangaResponse>> MangaDexRecords { get; set; }

        public StorageHelper(IFileHelper file)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
            MangaDexRecords = new Dictionary<string, List<MangaDexMangaResponse>>();
        }

        // We can overload this if we need to use other sites.
        public async Task AddRecord(string key, MangaDexMangaResponse manga)
        {
            await Task.Run(() =>
            {
                if (MangaDexRecords.ContainsKey(key))
                {
                    MangaDexRecords[key].Add(manga);
                }
                else
                {
                    MangaDexRecords[key] = new List<MangaDexMangaResponse> { manga };
                }
            });
        }

        public async Task CreateFileCheck(int position)
        {
            foreach (var key in MangaDexRecords.Keys)
            {
                // key is 1-40 so if position is greater that means
                // everything has been added (hopefully).
                if (position >= Convert.ToInt32(key.Split("-").Last()))
                {
                    await _file.SaveMangaRecords(MangaDexRecords[key], key);
                    MangaDexRecords.Remove(key);
                    return;
                }
            }
        }
    }
}
