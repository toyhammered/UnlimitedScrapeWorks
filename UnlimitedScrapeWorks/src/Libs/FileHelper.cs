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
        public string TimeStamp { get; }
        public string FilePath { get; }

        public FileHelper()
        {
            TimeStamp = $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss}";
            FilePath = Path.Combine(Directory.GetCurrentDirectory(), "src/FileUploads/", TimeStamp);
            Directory.CreateDirectory(FilePath);

        }

        public async Task SaveMangaRecords(List<MangaDexMangaResponse> records, string key)
        {
            using (var sw = new StreamWriter(Path.Combine(FilePath, $"{key}.ndjson"), true))
            {
                await ToNewLineDelimitedJson(sw, records);
                //await sw.WriteAsync(JsonConvert.SerializeObject(records, JsonSettings()));
                sw.Close();
            }
        }

        private async Task ToNewLineDelimitedJson(TextWriter textWriter, List<MangaDexMangaResponse> records)
        {
            var serializer = JsonSerializer.CreateDefault();

            await Task.Run(() =>
            {
                foreach (var record in records)
                {
                    using (var writer = new JsonTextWriter(textWriter) { Formatting = Formatting.None, CloseOutput = false })
                    {
                        serializer.Serialize(writer, record);
                    }

                    textWriter.Write("\n");
                }
            });
        }

        //private JsonSerializerSettings JsonSettings()
        //{
        //    return new JsonSerializerSettings
        //    {
        //        ContractResolver = new DefaultContractResolver
        //        {
        //            NamingStrategy = new SnakeCaseNamingStrategy()
        //        },
        //        Formatting = Formatting.Indented
        //    };
        //}
    }
}

