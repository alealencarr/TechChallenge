

namespace Domain.Entidades
{
    public class Categoria
    {
        public Categoria(string nome)
        {
            Nome = nome;
            Id = Guid.NewGuid();
        }
 
        public Guid Id { get; private set; }

        public string Nome { get; set; }

        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();

        public bool IsLanche()
        {
            return Nome.Equals("Lanche", StringComparison.OrdinalIgnoreCase);
        }
    }
}
