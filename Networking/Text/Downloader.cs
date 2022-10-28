using System;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Text
{
    public abstract class Downloader : IDownloader
    {
        private Networking.Downloader _downloader;

        public Downloader()
        {
            _downloader = new Networking.Downloader();
        }

        /// <summary>
        /// Downloads data from specified url as object of type T.
        /// </summary>
        /// <typeparam name="T">Type to deserialize received string</typeparam>
        /// <param name="url">Url address to data one the web</param>
        /// <param name="encoding">Encoding used to parse bytes into string</param>
        /// <param name="progressCallback">Callback to inform about download progress in range 0 - 100</param>
        /// <returns>
        /// Deserialized object of type T.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// url is null -or- encoding is null
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
        /// <exception cref="System.Net.ProtocolViolationException">
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
        /// <exception cref="System.Net.WebException">
        /// System.Net.HttpWebRequest.Abort was previously called. -or- The time-out period
        /// for the request expired. -or- An error occurred while processing the request.
        /// </exception>
        /// <exception cref="Exceptions.DeserializationException">
        /// An error occurred while deserializing the object.
        /// </exception>
        public async Task<T> Download<T>(string url, Encoding encoding, Action<int> progressCallback = null) where T : class
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            byte[] data = await _downloader.Download(url, progressCallback);

            return Deserialize<T>(data, encoding);
        }

        protected abstract T Deserialize<T>(byte[] data, Encoding encoding);
    }
}
