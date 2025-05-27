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

        private readonly List<IngredienteLanche>? _ingredientes = new();
        public IReadOnlyCollection<IngredienteLanche>? Ingredientes => _ingredientes?.AsReadOnly();
        public ItemPedido(Guid pedidoId, Guid produtoId, decimal preco)
        {
            PedidoId = pedidoId;
            ItemId = Guid.NewGuid();
            Preco = preco;
            ProdutoId = produtoId;
        }
        internal decimal ObterPrecoTotal()
        {
            var adicionais = Ingredientes?.Where(x => x.Adicional == true).Sum(x => x.Preco) ?? 0;
            return Preco + adicionais;
        }
        public void RemoverIngrediente(IngredienteLanche? ingrediente)
        {
            if (_ingredientes?.Count > 0)
                _ingredientes.Remove(ingrediente!);
        }

        public void AdicionarIngrediente(IngredienteLanche? ingrediente)
        {
            _ingredientes?.Add(ingrediente!);
        }

        public Guid ProdutoId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ItemId { get; private set; }

    }

}


