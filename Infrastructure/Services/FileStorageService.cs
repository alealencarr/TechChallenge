using Application.Interfaces.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infrastructure.Configurations;

namespace Infrastructure.Services;
public class FileStorageService : IFileStorageService
{
    private readonly BlobContainerClient _containerClient;

    public FileStorageService(FileStorageSettings settings)
    {
        if (string.IsNullOrEmpty(settings.StorageConnectionString))
            throw new ArgumentNullException(nameof(settings.StorageConnectionString), "A connection string do Azure Storage não pode ser nula.");

        if (string.IsNullOrEmpty(settings.ContainerName))
            throw new ArgumentNullException(nameof(settings.ContainerName), "O nome do contêiner não pode ser nulo.");

        var blobServiceClient = new BlobServiceClient(settings.StorageConnectionString);

        _containerClient = blobServiceClient.GetBlobContainerClient(settings.ContainerName);

        _containerClient.CreateIfNotExists(PublicAccessType.Blob);
    }
    public async Task<List<string>> SaveFilesAsync(List<byte[]> content, List<string> fileName, List<string> folder)
    {
        var files = content.Zip(fileName, (c, f) => new { Content = c, FileName = f })
                             .Zip(folder, (cf, fld) => new { cf.Content, cf.FileName, Folder = fld });

        var tasks = files.Select(file =>
            SaveFileAsync(file.Content, file.FileName, file.Folder)
        );

        var paths = await Task.WhenAll(tasks);
        return paths.ToList();
    }

    public async Task<string> SaveFileAsync(byte[] content, string fileName, string folder)
    {
      
        var blobName = Path.Combine(folder, fileName).Replace("\\", "/");
 
        var blobClient = _containerClient.GetBlobClient(blobName);
 
        using (var stream = new MemoryStream(content))
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }
        return blobClient.Uri.AbsoluteUri;
    }
    public void CleanFolder(string folder)
    {
        var blobsToDelete = _containerClient.GetBlobs(prefix: folder);

        foreach (var blob in blobsToDelete)
        {
            _containerClient.DeleteBlobIfExists(blob.Name);
        }
    }
}
//public class FileStorageService : IFileStorageService
//{
//    private readonly FileStorageSettings _settings;

//    public FileStorageService(FileStorageSettings settings)
//    {
//        _settings = settings;
//    }

//    public async Task<List<string>> SaveFilesAsync(List<byte[]> content, List<string> fileName, List<string> folder)
//    {
//        var files = content.Zip(fileName, (c, f) => new { Content = c, FileName = f })
//                           .Zip(folder, (cf, fld) => new { cf.Content, cf.FileName, Folder = fld });

//        var tasks = files.Select(file =>
//            SaveFileAsync(file.Content, file.FileName, file.Folder)
//        );

//        var paths = await Task.WhenAll(tasks);
//        return paths.ToList();
//    }
//    public async Task<string> SaveFileAsync(byte[] content, string fileName, string folder)
//    {            
//        var pastaDestino = Path.Combine(_settings.FileBasePath, folder);

//        if (!Directory.Exists(pastaDestino))
//            Directory.CreateDirectory(pastaDestino);

//        var caminhoArquivo = Path.Combine(pastaDestino, fileName);
//        await File.WriteAllBytesAsync(caminhoArquivo, content);

//        return $"/{folder}/{fileName}";
//    }

//    public void CleanFolder(string folder)
//    {
//        var folderCompleted = Path.Combine(_settings.FileBasePath, folder);

//        if (Directory.Exists(folderCompleted))
//        {
//            Directory.Delete(folderCompleted, true);
//        }
//    }

//}

