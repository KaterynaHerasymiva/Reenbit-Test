using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using StorageFunc.Models;
using System;
using System.Threading.Tasks;

namespace StorageFunc
{
    [StorageAccount("BlobConnStr")]
    public class StorageUpdateFunction
    {

        [FunctionName("StorageUpdateFunction")]
        public async Task Run([BlobTrigger("testtask/{name}")] BlobClient blob, ILogger logger)
        {
            BlobProperties properties = await blob.GetPropertiesAsync();
            if (!properties.Metadata.TryGetValue("email", out var userEmail))
            {
                logger.LogError("Failed to get user email.");
                throw new ArgumentException(nameof(userEmail));
            }

            if (!properties.Metadata.TryGetValue("SasUrl", out var sasUrl))
            {
                logger.LogError("Failed to get sas url.");
                throw new ArgumentException(nameof(sasUrl));
            }

            try
            {
                var prefixIndex = blob.Name.IndexOf('_');
                var fileName = blob.Name.Substring(prefixIndex + 1, blob.Name.Length - prefixIndex - 1);


                var mailData = new EmailDto
                {
                    Body = sasUrl,
                    Receiver = new EmailAddress(userEmail),
                    Sender = new EmailAddress("Kateryna.Herasymiva@gmail.com", "Kateryna"),
                    Subject = $"The file [{fileName}] has been successfully uploaded."
                };

                var sendGridKey = Environment.GetEnvironmentVariable("SendgridAPIKey");
                var client = new SendGridClient(sendGridKey);
                var sendGridMessage = new SendGridMessage
                {
                    From = mailData.Sender,
                };
                sendGridMessage.AddTo(mailData.Receiver);
                sendGridMessage.SetSubject(mailData.Subject);
                sendGridMessage.AddContent("text/html", mailData.Body);

                var response = await client.SendEmailAsync(sendGridMessage);
                logger.LogInformation($"Email response status code: {response.StatusCode}");
            }
            catch (RequestFailedException ex)
            {
                logger.LogError(ex, "Failed to get user email.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while processing, Exception - {ex.InnerException}");
            }
        }
    }
}
