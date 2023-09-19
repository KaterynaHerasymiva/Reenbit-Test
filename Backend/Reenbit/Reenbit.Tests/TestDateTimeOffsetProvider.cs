using Reenbit.Providers;

namespace Reenbit.Tests;

public class TestDateTimeOffsetProvider : IDateTimeOffsetProvider
{
    public DateTimeOffset Now => new(2023, 1, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public DateTimeOffset UtcNow => new(2023, 1, 1, 1, 0, 0, 0, TimeSpan.Zero);
}