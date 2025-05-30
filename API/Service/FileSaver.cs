using Aplicacao.Services;

namespace API.Service
{
    public class FileSaver : IFileSaver
    {
        private readonly IWebHostEnvironment _env;

        public FileSaver(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task SalvarArquivo(byte[] dados, string nomeArquivo, string subpasta)
        {
            var pastaDestino = Path.Combine(_env.WebRootPath, subpasta);

            if (!Directory.Exists(pastaDestino))
                Directory.CreateDirectory(pastaDestino);

            var caminhoArquivo = Path.Combine(pastaDestino, nomeArquivo);
            await File.WriteAllBytesAsync(caminhoArquivo, dados);
        }
    }
}
