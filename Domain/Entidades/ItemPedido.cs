using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class ItemPedido 
    {
        public decimal Preco { get; private set; } = 0M;

        private readonly List<IngredientePersonalizado>? _ingredientes = new();
        public IReadOnlyCollection<IngredientePersonalizado>? Ingredientes => _ingredientes?.AsReadOnly();
        public ItemPedido(Guid pedidoId, decimal preco, List<IngredientePersonalizado>? ingredientes)
        {
            PedidoId = pedidoId;

            Preco = preco;

            _ingredientes = [.. ingredientes!];
        }
        internal decimal ObterPrecoTotal()
        {
            var adicionais = Ingredientes?.Where(x => x.Adicional == true).Sum(x => x.Preco) ?? 0;
            return Preco + adicionais;
        }
        public void RemoverIngrediente(IngredientePersonalizado? ingrediente)
        {
            if (_ingredientes?.Count > 0)
                _ingredientes.Remove(ingrediente!);
        }

        public void AdicionarIngrediente(IngredientePersonalizado? ingrediente)
        {
            _ingredientes?.Add(ingrediente!);
        }

        public Guid PedidoId { get; private set; }
    }

}


