using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Agregados.AgregadoPedido
{
    public class ItemPedido 
    {
        public decimal Preco { get; private set; } = 0M;

        private readonly List<IngredienteLanche>? _ingredientes = new();
        public IReadOnlyCollection<IngredienteLanche>? Ingredientes => _ingredientes?.AsReadOnly();

        public int Quantidade { get; set; }

        protected ItemPedido() { }
        public ItemPedido(Guid pedidoId, Guid produtoId, decimal preco, int quantidade)
        {
            PedidoId = pedidoId;
            Id = Guid.NewGuid();
            Preco = preco * quantidade;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }
        internal decimal ObterPrecoTotal()
        {
            var adicionais = Ingredientes?.Where(x => x.Adicional == true).Sum(x => x.Preco * x.Quantidade) ?? 0;
            this.Preco = Preco + adicionais;
            return Preco;
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
        public Pedido? Pedido { get; set; }
        public Guid Id { get; private set; }

    }

}


