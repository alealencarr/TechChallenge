using Domain.Entidades;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.ItemPedido
{
    public class LanchePedido 
    {
        private readonly List<Ingrediente> _ingredientes = new();
        public IReadOnlyCollection<Ingrediente> Ingredientes => _ingredientes.AsReadOnly();

        public LanchePedido(Domain.Entidades.ItemCardapio.LancheCardapio lancheBase)
        {
            _ingredientes = [.. lancheBase.Ingredientes];
        }

        public void RemoverIngrediente(Ingrediente ingrediente)
        {
            if (_ingredientes.Count > 0)
                _ingredientes.Remove(ingrediente);
        }

        public void AdicionarIngrediente(Ingrediente ingrediente)
        {
            _ingredientes.Add(ingrediente);
        }
    }

}


