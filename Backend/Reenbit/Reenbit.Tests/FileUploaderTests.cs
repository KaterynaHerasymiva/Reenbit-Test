using Microsoft.Extensions.Logging;
using Moq;
using Reenbit.Services;
using System.Net.Mail;

namespace Reenbit.Tests
{
    public class FileUploaderTests
    {
        private Mock<ILogger<AzureUploader>> _loggerMock;
        private TestDateTimeOffsetProvider _dateTimeProvider;
        private TestStorageProvider _storageProvider;
        private AzureUploader _uploader;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<AzureUploader>>();
            _dateTimeProvider = new TestDateTimeOffsetProvider();
            _storageProvider = new TestStorageProvider();
            _uploader = new AzureUploader(_storageProvider, _dateTimeProvider, _loggerMock.Object);
        }

        [Test]
        public async Task TestMissingUserMailAddress()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _uploader.Upload(null, "Data\\test.xml"));
            Assert.That(ex.Message, Is.EqualTo("Email is null. (Parameter 'userMailAddress')"));
        }

        [Test]
        public void TestMissingFilePath()
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _uploader.Upload(new MailAddress("1@1"), null));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'filePath')"));
        }

        [Test]
        public void TestFileNotExistPath()
        {
            var ex = Assert.ThrowsAsync<FileNotFoundException>(async () => await _uploader.Upload(new MailAddress("1@1"), "test.xml"));
            Assert.That(ex.Message, Is.EqualTo("filePath"));
        }
    }
}