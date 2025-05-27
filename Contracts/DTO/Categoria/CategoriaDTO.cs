namespace Contracts.DTO.Categoria
{
    public class CategoriaDTO
    {
        public string Id { get; set; }

        public string Nome { get; set; }

        public CategoriaDTO() { }
        public CategoriaDTO(string id, string nome)
        {
            Nome = nome;
            Id = id;
        }
    }
}
