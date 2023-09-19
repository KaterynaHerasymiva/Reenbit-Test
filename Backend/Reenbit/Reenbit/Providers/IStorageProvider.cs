namespace Reenbit.Providers;

public interface IStorageProvider
{
    string GetName();

    string? GetTargetPath();
}