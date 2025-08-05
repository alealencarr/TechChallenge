using Application.Interfaces.Services;
using Infrastructure.Configurations;

namespace Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly FileStorageSettings _settings;

        public FileStorageService(FileStorageSettings settings)
        {
            _settings = settings;
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
            var pastaDestino = Path.Combine(_settings.FileBasePath, folder);

            if (!Directory.Exists(pastaDestino))
                Directory.CreateDirectory(pastaDestino);

            var caminhoArquivo = Path.Combine(pastaDestino, fileName);
            await File.WriteAllBytesAsync(caminhoArquivo, content);
            
            return $"/{folder}/{fileName}";
        }

        public void CleanFolder(string folder)
        {
            var folderCompleted = Path.Combine(_settings.FileBasePath, folder);

            if (Directory.Exists(folderCompleted))
            {
                Directory.Delete(folderCompleted, true);
            }
        }
 
    }
}
