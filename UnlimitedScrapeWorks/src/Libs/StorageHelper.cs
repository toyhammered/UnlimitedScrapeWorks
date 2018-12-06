using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;
using UnlimitedScrapeWorks.src.Libs.Aws;

namespace UnlimitedScrapeWorks.src.Libs
{
    public class StorageHelper : IStorageHelper
    {
        private readonly IFileHelper _file;
        private readonly IS3Wrapper _s3_client;
        public List<MangaDexMangaResponse> MangaDexRecords { get; set; }

        public StorageHelper(IFileHelper file, IS3Wrapper s3_client)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
            _s3_client = s3_client ?? throw new ArgumentNullException(nameof(s3_client));

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

        public async Task CreateAndUploadFiles(int batchAmount)
        {
            int batchCount = (MangaDexRecords.Count + batchAmount - 1) / batchAmount;

            for (int i = 1; i <= batchCount; i++)
            {
                int skipAmount = batchAmount * (i - 1);
                var batch = MangaDexRecords.Skip(skipAmount).Take(batchAmount).ToList();
                await _file.SaveMangaRecords(batch, $"manga-batch-{i}");
            }

            await _s3_client.BatchUpload(_file.FilePath);
        }
    }
}
