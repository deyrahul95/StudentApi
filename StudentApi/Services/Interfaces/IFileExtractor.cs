namespace StudentApi.Services.Interfaces;

public interface IFileExtractor 
{
    Task<List<T>> ExtractFileData<T>(
        IFormFile file,
        CancellationToken token,
        Action<int, string>? onRowError = null) where T : class, new();
}
