using Aplicacao.Common;
using Contracts.DTO.Produto;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produtos.Remover
{
    public class RemoverHandler
    {
        private readonly IProdutoRepository _produtoRepository;

        public RemoverHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Contracts.Response<ProdutoDTO?>> Handle(string id)
        {

            try
            {
                var produto = await _produtoRepository.BuscarPorID(id);

                if (produto is null)
                    return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, "Produto não encontrado com base neste Id.");

                await _produtoRepository.Remover(produto);

                ProdutoDTO produtoDto = new ProdutoDTO(produto.Nome, produto.Preco, produto.Categoria, produto.Imagens, produto.Descricao, produto.Id.ToString(), produto.Ingredientes);

                return new Contracts.Response<ProdutoDTO?>(data: produtoDto, code: System.Net.HttpStatusCode.Created, "Produto excluido com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível excluir o produto.");
            }
        }
    }
}
