using System.Net.Mail;

namespace Reenbit.Services;

public interface IUploader
{
    Task Upload(MailAddress userMailAddress, string filePath);
}