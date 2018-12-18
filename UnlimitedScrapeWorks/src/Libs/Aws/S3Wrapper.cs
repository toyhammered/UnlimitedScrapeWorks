using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace UnlimitedScrapeWorks.src.Libs.Aws
{
    public class S3Wrapper : IS3Wrapper
    {
        private readonly AmazonS3Config config = new AmazonS3Config();
        private readonly string bucketName = DotNetEnv.Env.GetString("S3_BUCKET");
        private readonly string filePath = DotNetEnv.Env.GetString("S3_PATH");
        private readonly RegionEndpoint bucketRegion = Amazon.RegionEndpoint.GetBySystemName(DotNetEnv.Env.GetString("S3_REGION"));

        private readonly AmazonS3Client _client;

        public S3Wrapper()
        {
            config.RegionEndpoint = bucketRegion;
            _client = new AmazonS3Client(
                config
            );
        }

        public async Task BatchUpload(string directoryPath)
        {
            var folder = directoryPath.Split(@"/").Last();
            try
            {
                var directoryTransferUtility = new TransferUtility(_client);

                await directoryTransferUtility.UploadDirectoryAsync(
                    directoryPath,
                    $@"{bucketName}/{filePath}{folder}"
                );
            }
            catch (AmazonS3Exception ex)
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}
