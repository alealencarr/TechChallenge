namespace Contracts.DTO.Ingrediente
{
    public class IngredienteDTO
    {
        public decimal Preco { get; set; } 

        public string Nome { get; set; } = string.Empty;

        public string Id { get; set; } 
        public IngredienteDTO(string id, decimal preco, string nome)
        {
            Nome = nome;
            Preco = preco;
            Id = id;
        }
    }
}
