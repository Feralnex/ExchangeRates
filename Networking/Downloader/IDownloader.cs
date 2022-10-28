using System;
using System.Threading.Tasks;

namespace Networking
{
    public interface IDownloader
    {
        Task<byte[]> Download(string source, Action<int> progressCallback = null);
    }
}
