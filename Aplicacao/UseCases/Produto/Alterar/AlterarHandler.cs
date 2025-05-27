using Aplicacao.Common;
using Contracts.DTO.Ingrediente;
using Contracts.DTO.Produto;
using Domain.Entidades;
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
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IIngredienteRepository _ingredienteRepository;
        public AlterarHandler(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IIngredienteRepository ingredienteRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<Contracts.Response<ProdutoDTO?>> Handle(AlterarPorIdCommand command)
        {

            try
            {
                Domain.Entidades.Categoria _categoria = await _categoriaRepository.GetById(command.CategoriaId.ToString());

                if (_categoria is null)
                    return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, $"Categoria com ID {command.CategoriaId.ToString()} não encontrada.");
 
                bool _isLanche = _categoria.IsLanche();

                var produto = await _produtoRepository.BuscarPorID(command.Id);

                if (produto is null)
                    return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, $"Produto não encontrado com base neste Id {command.Id}.");

                produto.Nome = command.Nome;
                produto.Preco = command.Preco;
                produto.CategoriaId = command.CategoriaId;
                produto.Descricao = command.Descricao;

                ICollection<ProdutoIngrediente> _produtoIngredientes = [];
                List<Domain.Entidades.Ingrediente>? _ingredientesDto = null;

                if (_isLanche)
                {
                    if ((command.Ingredientes?.Count ?? 0) == 0)
                        return new Contracts.Response<ProdutoDTO?>(data: null, HttpStatusCode.BadRequest, message: "É necessário informar pelo menos um ingrediente para criar um produto do tipo Lanche.");

                    _ingredientesDto = [];

                    foreach (var idIngrediente in command.Ingredientes!)
                    {
                        var ingredienteDb = await _ingredienteRepository.GetById(idIngrediente.ToString());
                        if (ingredienteDb is null)
                            return new Contracts.Response<ProdutoDTO?>(data: null, HttpStatusCode.BadRequest, message: $"Ingrediente com ID {idIngrediente.ToString()} não encontrado.");

                        _ingredientesDto.Add(ingredienteDb);

                        ProdutoIngrediente _produtoIngrediente = new(produto.Id, ingredienteDb.Id);
                        _produtoIngredientes.Add(_produtoIngrediente);
                    }
                }

                produto.VinculaIngredientes(_produtoIngredientes);


                ICollection<ProdutoImagem> _produtoImagens = [];

                if (command.Imagens is not null)
                {

                    foreach (var imagemDto in command.Imagens)
                    {
                        var imagem = new ProdutoImagem(
                            idProduto: produto.Id,
                            blob: imagemDto.Blob,
                            nome: imagemDto.Nome
                        );

                        _produtoImagens.Add(imagem);
                    }
 
                }

                produto.VinculaImagens(_produtoImagens);


                await _produtoRepository.Alterar(produto);

                ProdutoDTO produtoDto = new ProdutoDTO(command.Nome, command.Preco, _categoria, [.. produto.ProdutoImagens.Select(img => new ProdutoImagemDTO
                                                                                                    {
                                                                                                        Nome = img.Nome,
                                                                                                        Blob = img.Blob
                                                                                                    })], command.Descricao, produto.Id.ToString()
                                                                                                   , [.. produto.ProdutoIngredientes.Select(ing => new ProdutoIngredienteDTO
                                                                                                    {
                                                                                                        IdProduto = ing.IdProduto,
                                                                                                        IdIngrediente = ing.IdIngrediente
                                                                                                    })]);

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
