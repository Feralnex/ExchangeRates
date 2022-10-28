using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Networking
{
    public class Downloader : IDownloader
    {
        public static int DEFAULT_SEGMENT_SIZE = 1024;
        public static int STANDARD_PERCENTAGE_FACTOR = 100;
        public static string GET_METHOD = "Get";

        /// <summary>
        /// Downloads data from specified url as byte array.
        /// </summary>
        /// <param name="url">Url address to data one the web</param>
        /// <param name="progressCallback">Callback to inform about download progress in range 0 - 100</param>
        /// <returns>
        /// Byte array containing downloaded data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// url is null
        /// </exception>
        /// <exception cref="UriFormatException">
        /// In the .NET for Windows Store apps or the Portable Class Library, catch the base
        /// class exception, System.FormatException, instead. url is a zero-length string
        /// or contains only spaces. -or- The parsing routine detected a scheme in an invalid
        /// form. -or- The parser detected more than two consecutive slashes in a URI that
        /// does not use the "file" scheme. -or- url is not a valid URI.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The request cache validator indicated that the response for this request can
        /// be served from the cache; however, this request includes data to be sent to the
        /// server. Requests that send data must not use the cache. This exception can occur
        /// if you are using a custom cache validator that is incorrectly implemented.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have System.Net.WebPermissionAttribute permission to connect
        /// to the requested URI or a URI that the request is redirected to.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The stream is already in use by a previous call to System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object).
        /// -or- System.Net.HttpWebRequest.TransferEncoding is set to a value and System.Net.HttpWebRequest.SendChunked is false.
        /// </exception>
        /// <exception cref="ProtocolViolationException">
        /// System.Net.HttpWebRequest.Method is GET or HEAD, and either System.Net.HttpWebRequest.ContentLength
        /// is greater or equal to zero or System.Net.HttpWebRequest.SendChunked is true.
        /// -or- System.Net.HttpWebRequest.KeepAlive is true, System.Net.HttpWebRequest.AllowWriteStreamBuffering
        /// is false, System.Net.HttpWebRequest.ContentLength is -1, System.Net.HttpWebRequest.SendChunked
        /// is false, and System.Net.HttpWebRequest.Method is POST or PUT. -or- The System.Net.HttpWebRequest
        /// has an entity body but the System.Net.HttpWebRequest.GetResponse method is called
        /// without calling the System.Net.HttpWebRequest.GetRequestStream method. -or- The
        /// System.Net.HttpWebRequest.ContentLength is greater than zero, but the application
        /// does not write all of the promised data.
        /// </exception>
        /// <exception cref="WebException">
        /// System.Net.HttpWebRequest.Abort was previously called. -or- The time-out period
        /// for the request expired. -or- An error occurred while processing the request.
        /// </exception>
        public async Task<byte[]> Download(string url, Action<int> progressCallback = null)
        {
            UriBuilder uriBuilder = new UriBuilder(url);
            Uri uri = uriBuilder.Uri;
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            byte[] task() => Download(request, progressCallback);

            return await Task.Run(task);
        }

        private byte[] Download(HttpWebRequest request, Action<int> progressCallback = null)
        {
            request.Method = GET_METHOD;

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    long size = response.ContentLength;
                    byte[] data = new byte[size];
                    byte[] buffer = new byte[DEFAULT_SEGMENT_SIZE];
                    int progress = 0;
                    int index = 0;
                    int receivedLength = responseStream.Read(buffer, 0, buffer.Length);

                    while (receivedLength != 0)
                    {
                        Array.Copy(buffer, 0, data, index, receivedLength);

                        index += receivedLength;
                        progress = (int)(data.Length * STANDARD_PERCENTAGE_FACTOR / size);

                        progressCallback?.Invoke(progress);

                        receivedLength = responseStream.Read(buffer, 0, buffer.Length);
                    }

                    return data;
                }
            }
        }
    }
}
