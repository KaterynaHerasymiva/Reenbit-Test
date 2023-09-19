namespace Reenbit.Providers;

public class SystemDateTimeOffsetProvider : IDateTimeOffsetProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;

    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}