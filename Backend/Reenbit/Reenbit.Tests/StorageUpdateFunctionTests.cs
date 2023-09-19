using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Moq;
using StorageFunc;

namespace Reenbit.Tests
{
    public class StorageUpdateFunctionTests
    {
        private Mock<ILogger<StorageUpdateFunctionTests>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<StorageUpdateFunctionTests>>();
        }

        [Test]
        public void TestMissingSas()
        {
            var userEmail = "1@1";
            var bp = new BlobProperties();
            bp.Metadata.Add("email", userEmail);

            var mock = new Mock<BlobClient>();
            var responseMock = new Mock<Response>();
            mock
                .Setup(m => m.GetPropertiesAsync(null, CancellationToken.None).Result)
                .Returns(Response.FromValue<BlobProperties>(bp, responseMock.Object));


            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await new StorageUpdateFunction().Run(mock.Object, _loggerMock.Object));
            Assert.That(ex.Message, Is.EqualTo("sasUrl"));
        }

        [Test]
        public void TestMissingUserEmail()
        {
            var bp = new BlobProperties();
            bp.Metadata.Add("SasUrl", "htp://123");

            var mock = new Mock<BlobClient>();
            var responseMock = new Mock<Response>();
            mock
                .Setup(m => m.GetPropertiesAsync(null, CancellationToken.None).Result)
                .Returns(Response.FromValue<BlobProperties>(bp, responseMock.Object));


            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await new StorageUpdateFunction().Run(mock.Object, _loggerMock.Object));
            Assert.That(ex.Message, Is.EqualTo("userEmail"));
        }
    }
}