using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura
{
    public static class DbInitializer
    {
        public static void SeedCategorias(AppDbContext context)
        {
            if (!context.Categorias.Any())
            {
                var categorias = new List<Categoria>();

                string[] categoriasDefinidas = { "Lanche", "Acompanhamento", "Bebida", "Sobremesa" };
                
                foreach(string c in categoriasDefinidas)
                {
                    categorias.Add(new Categoria(c));
                }

                context.Categorias.AddRange(categorias);
                context.SaveChanges();
            }
        }
    }

}
