using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Networking.Tests
{
    public class DownloaderTests
    {
        public Mock<IDownloader> Downloader { get; private set; }

        [SetUp]
        public void Setup()
        {
            Downloader = new Mock<IDownloader>();

            Downloader.Setup(downloader => downloader.Download(It.IsAny<string>(), It.IsAny<Action<int>>())).Returns(Task.FromResult(new byte[] { 1, 2, 3, 4, 5 }));
            Downloader.Setup(downloader => downloader.Download(null, It.IsAny<Action<int>>())).Throws<ArgumentNullException>();
            Downloader.Setup(downloader => downloader.Download(It.Is<string>(source => source != null && !Uri.IsWellFormedUriString(source, UriKind.Absolute)), It.IsAny<Action<int>>())).Throws<UriFormatException>();
        }

        [Test(ExpectedResult = new byte[] { 1, 2, 3, 4, 5 })]
        public async Task<byte[]> Download_ShouldReturnByteArray()
        {
            return await Downloader.Object.Download("http://example.com/file.json");
        }

        [Test]
        public void Download_ShouldThrowArgumentNullExceptionWhenSourceIsNull()
        {
            Task<byte[]> test() => Downloader.Object.Download(null);

            Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Test]
        public void Download_ShouldThrowUriFormatExceptionWhenSourceIsEmpty()
        {
            Task<byte[]> test() => Downloader.Object.Download("");

            Assert.ThrowsAsync<UriFormatException>(test);
        }
    }
}