using Microsoft.AspNetCore.Mvc;
using Reenbit.Services;
using System.Net.Http.Headers;
using System.Net.Mail;

namespace Reenbit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileUploader : ControllerBase
    {
        private readonly ILogger<FileUploader> _logger;
        private readonly IUploader _uploader;

        public FileUploader(IUploader uploader, ILogger<FileUploader> logger)
        {
            _logger = logger;
            _uploader = uploader;
        }


        [HttpPost("UploadFile"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            string fullPath = string.Empty;

            if (!Request.Form.TryGetValue("email", out var userEmail) || !MailAddress.TryCreate(userEmail, out var email))
            {
                return BadRequest(nameof(userEmail));
            }

            if (!Request.Form.Files.Any())
            {
                return BadRequest("No files to process.");
            }

            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');
                    fullPath = Path.Combine(Path.GetTempPath(), fileName);

                    await using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    await _uploader.Upload(email, fullPath);

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            finally
            {
                try
                {
                    System.IO.File.Delete(fullPath);
                }
                catch (Exception)
                {
                    _logger.LogWarning($"Failed to remove file {fullPath}");
                }
            }
        }
    }
}