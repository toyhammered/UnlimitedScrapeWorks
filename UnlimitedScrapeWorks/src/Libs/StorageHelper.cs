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
        public List<MangaDexMangaResponse> MangaDexRecords { get; set; }

        public StorageHelper(IFileHelper file)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
            MangaDexRecords = new List<MangaDexMangaResponse>();
        }

        // We can overload this if we need to use other sites.
        public async Task AddRecord(MangaDexMangaResponse manga)
        {

            await Task.Run(() =>
            {
                MangaDexRecords.Add(manga);
            });
        }

        public async Task CreateFile()
        {
            int batchAmount = 3;
            int batchCount = (MangaDexRecords.Count + batchAmount - 1) / batchAmount;

            for (int i = 1; i <= batchCount; i++)
            {
                int skipAmount = batchAmount * (i - 1);
                var batch = MangaDexRecords.Skip(skipAmount).Take(batchAmount).ToList();
                await _file.SaveMangaRecords(batch, $"manga-batch-{i}");
            }
        }
    }
}
