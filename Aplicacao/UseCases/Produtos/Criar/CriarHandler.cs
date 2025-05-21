using Aplicacao.Common;
using Aplicacao.UseCases.produto;
using Domain.Entidades;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produtos.Criar
{
    public class CriarHandler
    {
        private readonly IProdutoRepository _produtoRepository;

        public CriarHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Response<ProdutoDTO?>> Handle(CriarCommand command)
        {

            try
            {
                if (command.Categoria.Nome.ToUpper().Equals("LANCHE") && (command.Ingredientes?.Count ?? 0) == 0)
                    return new Response<ProdutoDTO?>(data: null, HttpStatusCode.BadRequest, message: "É necessário informar pelo menos um ingrediente para criar um produto do tipo Lanche.");

                if (!command.Categoria.Nome.ToUpper().Equals("LANCHE"))
                    command.Ingredientes = null;

                var produto = new Domain.Entidades.Produto(command.Nome, command.Preco, command.Categoria, command.Imagens, command.Descricao, command.Ingredientes);

                await _produtoRepository.Adicionar(produto);

                ProdutoDTO produtoDto = new ProdutoDTO(command.Nome, command.Preco, command.Categoria, command.Imagens, command.Descricao, command.Ingredientes, produto.Id.ToString());

                return new Response<ProdutoDTO?>(data: produtoDto, code: System.Net.HttpStatusCode.Created, "Produto criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Response<ProdutoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível criar o produto.");
            }
        }

    }
}
