using DataAccess.Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Tests
{
    public class TextDownloaderTests
    {
        public Mock<Text.IDownloader> Downloader { get; private set; }
        public MidExchangeRates[] ExchangeRates { get; private set; }

        [SetUp]
        public void Setup()
        {
            string directoryName = TestContext.CurrentContext.TestDirectory;
            string path = Path.Combine(directoryName, "Data", "Example.json");

            Downloader = new Mock<Text.IDownloader>();
            ExchangeRates = JsonConvert.DeserializeObject<MidExchangeRates[]>(File.ReadAllText(path));

            Downloader.Setup(downloader => downloader.Download<MidExchangeRates[]>(It.IsAny<string>(), It.IsAny<Encoding>(), It.IsAny<Action<int>>())).Returns(Task.FromResult(ExchangeRates));
            Downloader.Setup(downloader => downloader.Download<It.IsAnyType>(null, It.IsAny<Encoding>(), It.IsAny<Action<int>>())).Throws<ArgumentNullException>();
            Downloader.Setup(downloader => downloader.Download<It.IsAnyType>(It.IsAny<string>(), null, It.IsAny<Action<int>>())).Throws<ArgumentNullException>();
            Downloader.Setup(downloader => downloader.Download<It.IsAnyType>(It.Is<string>(source => source != null && !Uri.IsWellFormedUriString(source, UriKind.Absolute)), It.IsAny<Encoding>(), It.IsAny<Action<int>>())).Throws<UriFormatException>();
        }

        [Test]
        public async Task Download_ShouldReturnArrayOfMidExchangeRates()
        {
            MidExchangeRates[] actual = await Downloader.Object.Download<MidExchangeRates[]>("http://example.com/file.json", Encoding.UTF8);

            Assert.AreEqual(ExchangeRates, actual);
        }

        [Test]
        public void Download_ShouldThrowArgumentNullExceptionWhenSourceIsNull()
        {
            Task<MidRate> test() => Downloader.Object.Download<MidRate>(null, Encoding.UTF8);

            Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Test]
        public void Download_ShouldThrowArgumentNullExceptionWhenEncodingIsNull()
        {
            Task<MidRate> test() => Downloader.Object.Download<MidRate>("http://example.com/file.json", null);

            Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Test]
        public void Download_ShouldThrowUriFormatExceptionWhenSourceIsEmpty()
        {
            Task<MidRate> test() => Downloader.Object.Download<MidRate>("", Encoding.UTF8);

            Assert.ThrowsAsync<UriFormatException>(test);
        }
    }
}