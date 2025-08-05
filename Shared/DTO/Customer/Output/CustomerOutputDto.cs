using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTO.Categorie.Output
{
    public record CustomerOutputDto
    {
        public Guid Id { get; set; }

        public string Cpf { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

    }


}
