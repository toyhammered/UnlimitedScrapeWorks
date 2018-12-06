using System;
using System.Threading.Tasks;

namespace UnlimitedScrapeWorks.src.Libs.Aws
{
    public interface IS3Wrapper
    {
        Task BatchUpload(string directoryPath);
    }
}
