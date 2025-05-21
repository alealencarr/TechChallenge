using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class CpfVo
    {
        public string Valor { get; }

        protected CpfVo() { }

        public CpfVo(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("CPF não pode ser vazio.");

            valor = new string(valor.Where(char.IsDigit).ToArray());

            if (!IsValid(valor))
                throw new ArgumentException("CPF inválido.");

            Valor = valor;
        }

        public override string ToString()
        {
            if (Valor.Length != 11)
                return Valor;

            return $"{Valor[..3]}.{Valor[3..6]}.{Valor[6..9]}-{Valor[9..]}";
        }

        public static bool IsValid(string cpf)
        {
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            int digito1 = soma % 11;
            digito1 = digito1 < 2 ? 0 : 11 - digito1;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            int digito2 = soma % 11;
            digito2 = digito2 < 2 ? 0 : 11 - digito2;

            return cpf[9] - '0' == digito1 && cpf[10] - '0' == digito2;
        }

        public override bool Equals(object? obj)
        {
            return obj is CpfVo other && Valor == other.Valor;
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }
    }
}
