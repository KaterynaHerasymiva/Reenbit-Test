namespace Reenbit.Providers;

public interface IDateTimeOffsetProvider
{
    DateTimeOffset Now { get; }

    DateTimeOffset UtcNow { get; }
}