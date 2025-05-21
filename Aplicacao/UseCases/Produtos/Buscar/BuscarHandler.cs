using Aplicacao.Common;
using Aplicacao.UseCases.Produto;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produtos.Buscar
{
    public class BuscarHandler
    {
        private readonly IProdutoRepository _produtoRepository;

        public BuscarHandler(IProdutoRepository ProdutoRepository)
        {
            _produtoRepository = ProdutoRepository;
        }

        public async Task<Response<List<ProdutoDTO>?>> Handler(BuscarCommand command)
        {
            try
            {
                var products = await _produtoRepository.Buscar(command.Categoria)

                if (categorie is null)
                    return new Response<ProdutoDTO?>(data: null, code: System.Net.HttpStatusCode.NotFound, "Produto não encontrada.");

                var categorieDto = new ProdutoDTO(categorie.Id.ToString(), categorie.Nome);

                return new Response<ProdutoDTO?>(data: categorieDto, code: System.Net.HttpStatusCode.OK, "Produto encontrada.");

            }
            catch (ArgumentException ex)
            {
                return new Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Response<ProdutoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar a Produto.");
            }

        }
    }
}
