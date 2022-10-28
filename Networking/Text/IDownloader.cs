using System;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Text
{
    public interface IDownloader
    {
        Task<T> Download<T>(string source, Encoding encoding, Action<int> progressCallback = null) where T : class;
    }
}
