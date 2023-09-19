namespace Reenbit.Providers;

public class AzureStorageProvider : IStorageProvider
{
    private const string AzureStorageConnection = "AzureStorageConnection";

    private readonly string? _connectionString;

    public AzureStorageProvider(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(AzureStorageConnection);
    }

    public string GetName()
    {
        return "testtask";
    }

    public string? GetTargetPath()
    {
        return _connectionString;
    }
}