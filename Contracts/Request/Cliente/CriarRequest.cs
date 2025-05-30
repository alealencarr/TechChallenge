using System.ComponentModel.DataAnnotations;
using Aplicacao.Common;

namespace Contracts.Request.Cliente
{
    public class CriarRequest
    {
        [Required(ErrorMessage = "Favor informar o CPF.")]
        [MaxLength(14)]

        public required string CPF { get; set; }  

        public string Nome { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public CriarRequest(string cpf, string nome, string email)
        {
            CPF = cpf.FormataCpfSemPontuacao();
            Nome = nome;
            Email = email;
        }
    }
}
