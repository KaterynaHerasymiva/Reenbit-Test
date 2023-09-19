using SendGrid.Helpers.Mail;
using StorageFunc.Models;

[TestFixture]
public class EmailDtoTests
{
    [Test]
    public void EmailDto_InitializeWithValidValues_ShouldSucceed()
    {
        // Arrange
        var sender = new EmailAddress("sender@example.com");
        var receiver = new EmailAddress("receiver@example.com");
        var subject = "Test Subject";
        var body = "Test Body";

        // Act
        var emailDto = new EmailDto
        {
            Sender = sender,
            Receiver = receiver,
            Subject = subject,
            Body = body
        };

        // Assert
        Assert.That(emailDto.Sender, Is.EqualTo(sender));
        Assert.That(emailDto.Receiver, Is.EqualTo(receiver));
        Assert.That(emailDto.Subject, Is.EqualTo(subject));
        Assert.That(emailDto.Body, Is.EqualTo(body));
    }

    [Test]
    public void EmailDto_InitializeWithNullSender_ShouldThrowArgumentNullException()
    {
        // Arrange
        var receiver = new EmailAddress("receiver@example.com");
        var subject = "Test Subject";
        var body = "Test Body";

        // Act & Assert
        Assert.That(() => new EmailDto
        {
            Sender = null,
            Receiver = receiver,
            Subject = subject,
            Body = body
        }, Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void EmailDto_InitializeWithNullReceiver_ShouldThrowArgumentNullException()
    {
        // Arrange
        var sender = new EmailAddress("sender@example.com");
        var subject = "Test Subject";
        var body = "Test Body";

        // Act & Assert
        Assert.That(() => new EmailDto
        {
            Sender = sender,
            Receiver = null,
            Subject = subject,
            Body = body
        }, Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void EmailDto_InitializeWithNullSubject_ShouldThrowArgumentException()
    {
        // Arrange
        var sender = new EmailAddress("sender@example.com");
        var receiver = new EmailAddress("receiver@example.com");
        var subject = "   ";
        var body = "Test Body";

        // Act & Assert
        Assert.That(() => new EmailDto
        {
            Sender = sender,
            Receiver = receiver,
            Subject = subject,
            Body = body
        }, Throws.TypeOf<ArgumentException>());
    }
}