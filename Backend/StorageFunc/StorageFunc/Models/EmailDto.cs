using SendGrid.Helpers.Mail;
using System;

namespace StorageFunc.Models;

public class EmailDto
{
    private readonly EmailAddress _sender;
    private readonly EmailAddress _receiver;
    private readonly string _subject;

    public EmailAddress Sender
    {
        get => _sender;
        init => _sender = value ?? throw new ArgumentNullException("Shouldn't be null", nameof(Sender));
    }

    public EmailAddress Receiver
    {
        get => _receiver;
        init => _receiver = value ?? throw new ArgumentNullException("Shouldn't be null", nameof(Receiver));
    }

    public string Subject
    {
        get => _subject;
        init => _subject = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Shouldn't be null or whitespace", nameof(Subject))
            : value;
    }

    public string Body { get; init; }
}