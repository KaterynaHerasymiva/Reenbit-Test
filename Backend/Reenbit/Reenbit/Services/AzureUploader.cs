using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Reenbit.Providers;
using System.Data.Common;
using System.Net.Mail;

namespace Reenbit.Services;

public class AzureUploader : IUploader
{
    private readonly IStorageProvider _storageProvider;
    private readonly ILogger<AzureUploader> _logger;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public AzureUploader(IStorageProvider storageProvider, IDateTimeOffsetProvider dateTimeOffsetProvider, ILogger<AzureUploader> logger)
    {
        _storageProvider = storageProvider;
        _logger = logger;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public async Task Upload(MailAddress userMailAddress, string filePath)
    {
        if (filePath == null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (userMailAddress == null)
        {
            throw new ArgumentException("Email is null.", nameof(userMailAddress));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(nameof(filePath));
        }

        var blobStorageConnectionString = _storageProvider.GetTargetPath();
        var blobStorageContainerName = _storageProvider.GetName();

        var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);

        var now = _dateTimeOffsetProvider.UtcNow.ToUnixTimeMilliseconds();
        var fileName = now + "_" + Path.GetFileName(filePath);

        var blob = container.GetBlobClient(fileName);

        await using var stream = File.OpenRead(filePath);
        await blob.UploadAsync(stream, true);

        await AddBlobMetadataAsync(blob, userMailAddress.Address);
    }

    private async Task AddBlobMetadataAsync(BlobClient blob, string email)
    {
        try
        {
            IDictionary<string, string> metadata = new Dictionary<string, string>
            {
                {"email", email}
            };

            var csBuilder = new DbConnectionStringBuilder
            {
                ConnectionString = _storageProvider.GetTargetPath()
            };

            var storageSharedKeyCredential = new StorageSharedKeyCredential((string)csBuilder["AccountName"], (string)csBuilder["AccountKey"]);

            BlobSasBuilder blobSasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                ExpiresOn = _dateTimeOffsetProvider.UtcNow.AddHours(1)
            };

            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
            var sasToken = blobSasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();
            var sasUrl = blob.Uri.AbsoluteUri + "?" + sasToken;

            metadata.Add("SasUrl", sasUrl);

            await blob.SetMetadataAsync(metadata);
        }
        catch (RequestFailedException e)
        {
            _logger.LogError(e, "Failed to add user email metadata");
        }
    }
}