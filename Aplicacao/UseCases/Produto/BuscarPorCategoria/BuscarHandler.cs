using Aplicacao.Common;
using Contracts.DTO.Produto;
using Domain.Entidades;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Aplicacao.UseCases.Produto.BuscarPorCategoria
{
    public class BuscarHandler
    {
        private readonly IProdutoRepository _produtoRepository;

        public BuscarHandler(IProdutoRepository ProdutoRepository)
        {
            _produtoRepository = ProdutoRepository;
        }

        public async Task<Contracts.Response<List<ProdutoDTO>?>> Handle(string? id, string? name)
        {
            try
            {
                var products = await _produtoRepository.Buscar(id, name); 

                var produtosDto = new List<ProdutoDTO>();

                produtosDto = [.. products
                .Select(x => new ProdutoDTO
                {
                    Id = x.Id.ToString(),
                    Nome = x.Nome,
                    Categoria =  new Contracts.DTO.Categoria.CategoriaDTO(x.Categoria.Id.ToString(), x.Categoria.Nome),  
                    Descricao = x.Descricao,
                    Preco = x.Preco,
                    Ingredientes = [.. x.ProdutoIngredientes.Select(ing => new ProdutoIngredienteDTO
                    {
                       Quantidade = ing.Quantidade,
                        Id = ing.IngredienteId.ToString()
                    })],
                    Imagens = [.. x.ProdutoImagens.Select(img => new ProdutoImagemDTO
                    {
                        Nome = img.Nome,
                        Blob = img.Blob
                    })]
                })];

               
                return (products.Count == 0) ? new Contracts.Response<List<ProdutoDTO>?>(data: null, code: System.Net.HttpStatusCode.BadRequest, "Nenhum produto encontrado.")
                    : new Contracts.Response<List<ProdutoDTO>?>(data: produtosDto, code: System.Net.HttpStatusCode.OK, "Produto encontrado com sucesso.");

            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<List<ProdutoDTO>?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                return new Contracts.Response<List<ProdutoDTO>?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar este Produto.");
            }

        }
    }
}
