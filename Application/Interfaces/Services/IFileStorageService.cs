namespace Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(byte[] content, string fileName, string folder);
        Task<List<string>> SaveFilesAsync(List<byte[]> content, List<string> fileName, List<string> folder);
        void CleanFolder(string folder);
    }
}
