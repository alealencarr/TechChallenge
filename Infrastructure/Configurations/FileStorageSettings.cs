using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Configurations
{
    [ExcludeFromCodeCoverage]
    public class FileStorageSettings
    {
        public string StorageConnectionString { get; set; } 
        public string ContainerName { get; set; } = "imagens";
        public string FileBasePath { get; set; } 
    }
}
