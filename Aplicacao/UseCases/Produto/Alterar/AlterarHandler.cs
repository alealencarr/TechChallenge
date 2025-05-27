using Aplicacao.Common;
using Contracts.DTO.Produto;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produtos.Alterar
{
    public class AlterarHandler
    {
        private readonly IProdutoRepository _produtoRepository;

        public AlterarHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Contracts.Response<ProdutoDTO?>> Handle(AlterarPorIdCommand command)
        {

            try
            {
                if (command.Categoria.IsLanche() && (command.Ingredientes?.Count ?? 0) == 0)
                    return new Contracts.Response<ProdutoDTO?>(data: null, HttpStatusCode.BadRequest, message: "É necessário informar pelo menos um ingrediente para alterar um produto do tipo Lanche.");

                var produto = await _produtoRepository.BuscarPorID(command.Id);

                if (produto is null)
                    return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, "Produto não encontrado com base neste Id.");

                produto.Nome = command.Nome;
                produto.Preco = command.Preco;
                produto.Imagens = command.Imagens;
                produto.Categoria = command.Categoria;
                produto.Descricao = command.Descricao;
                if (command.Categoria.IsLanche())
                    produto.Ingredientes = command.Ingredientes;
                else
                    command.Ingredientes = null;
                                
                await _produtoRepository.Alterar(produto);

                ProdutoDTO produtoDto = new ProdutoDTO(command.Nome, command.Preco, command.Categoria, command.Imagens, command.Descricao, produto.Id.ToString(), command.Ingredientes);

                return new Contracts.Response<ProdutoDTO?>(data: produtoDto, code: System.Net.HttpStatusCode.OK, "Produto alterado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível alterar o produto.");
            }
        }
    }
}
