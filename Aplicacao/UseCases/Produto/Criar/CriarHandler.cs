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

namespace Aplicacao.UseCases.Produtos.Criar
{
    public class CriarHandler
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IIngredienteRepository _ingredienteRepository;
        public CriarHandler(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IIngredienteRepository ingredienteRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<Contracts.Response<ProdutoDTO?>> Handle(CriarCommand command)
        {

            try
            {
                Domain.Entidades.Categoria _categoria = await _categoriaRepository.GetById(command.CategoriaId.ToString()) ?? throw new ArgumentException($"Categoria com ID {command.CategoriaId.ToString()} não encontrada.");
                bool _isLanche = _categoria.IsLanche();

                ICollection<ProdutoIngrediente> _produtoIngredientes = null;
                List<Domain.Entidades.Ingrediente>? _ingredientesDto = null;
                var produto = new Domain.Entidades.Produto(command.Nome, command.Preco, command.CategoriaId,_categoria, command.Imagens, command.Descricao);

                if (_isLanche)
                {
                    if ((command.Ingredientes?.Count ?? 0) == 0)
                        return new Contracts.Response<ProdutoDTO?>(data: null, HttpStatusCode.BadRequest, message: "É necessário informar pelo menos um ingrediente para criar um produto do tipo Lanche.");

                    _produtoIngredientes = [];
                    _ingredientesDto = [];

                    foreach (var idIngrediente in command.Ingredientes!)
                    {
                        var ingredienteDb = await _ingredienteRepository.GetById(idIngrediente.ToString()) ?? throw new ArgumentException($"Ingrediente com ID {idIngrediente.ToString()} não encontrado.");
                        _ingredientesDto.Add(ingredienteDb);

                        ProdutoIngrediente _produtoIngrediente = new(produto.Id, ingredienteDb.Id, produto, ingredienteDb);
                        _produtoIngredientes.Add(_produtoIngrediente);
                    }

                    produto.VinculaIngredientes(_produtoIngredientes);
                }                

                await _produtoRepository.Adicionar(produto);

                ProdutoDTO produtoDto = new(command.Nome, command.Preco, _categoria, command.Imagens, command.Descricao, produto.Id.ToString(), _ingredientesDto);

                return new Contracts.Response<ProdutoDTO?>(data: produtoDto, code: System.Net.HttpStatusCode.Created, "Produto criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível criar o produto.");
            }
        }

    }
}
