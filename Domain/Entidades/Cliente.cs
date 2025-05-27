using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    public class Cliente
    {
        public Cliente(string cpf, string nome, string email)
        {
            CPF =  new CpfVo(cpf);
            Nome= nome;
            Email= email;
            ClienteIdentificado = true;
            Id = Guid.NewGuid();

        }

        public Cliente()
        {
            Id = Guid.NewGuid();
            ClienteIdentificado = false;
        }

        public Guid Id { get; private set;  }
        public CpfVo? CPF { get; set; }
        public string Nome { get; set; } = string.Empty;

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
     
        public string Email { get; set; } = string.Empty;

        [NotMapped]
        public bool ClienteIdentificado { get; private set; }       

    }
}
