using Reenbit.Providers;

namespace Reenbit.Tests;

public class TestStorageProvider : IStorageProvider
{
    public string GetName()
    {
        return "TestName";
    }

    public string? GetTargetPath()
    {
        return "TestPath";
    }
}