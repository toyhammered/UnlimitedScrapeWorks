using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnlimitedScrapeWorks.src.ContractModels.MangaDex;

namespace UnlimitedScrapeWorks.src.Libs
{
    public class FileHelper : IFileHelper
    {
        public string TimeStamp = $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss}";
        public string FilePath { get; }

        public FileHelper()
        {
            FilePath = Path.Combine(Directory.GetCurrentDirectory(), "src/FileUploads/", TimeStamp);
            Directory.CreateDirectory(FilePath);

        }

        public async Task SaveMangaRecords(List<MangaDexMangaResponse> records, string key)
        {
            using (var sw = new StreamWriter(Path.Combine(FilePath, $"{key}.json"), true))
            {
                await sw.WriteAsync(JsonConvert.SerializeObject(records, JsonSettings()));
                sw.Close();
            }
        }

        private JsonSerializerSettings JsonSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };
        }
    }
}

