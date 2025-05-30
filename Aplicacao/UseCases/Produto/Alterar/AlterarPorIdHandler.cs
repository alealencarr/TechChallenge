using Contracts.DTO.Produto;
using Domain.Entidades.Agregados.AgregadoProduto;
using Domain.Ports;
using System.Net;

namespace Aplicacao.UseCases.Produto.Alterar
{
    public class AlterarPorIdHandler
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IIngredienteRepository _ingredienteRepository;
        public AlterarPorIdHandler(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IIngredienteRepository ingredienteRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<Contracts.Response<ProdutoDTO?>> Handle(AlterarPorIdCommand command)
        {

            try
            {
                Domain.Entidades.Categoria? _categoria = await _categoriaRepository.GetById(command.CategoriaId.ToString());

                if (_categoria is null)
                    return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, $"Categoria com ID {command.CategoriaId.ToString()} não encontrada.");

                bool _isLanche = _categoria.IsLanche();

                var produto = await _produtoRepository.GetById(command.Id);

                if (produto is null)
                    return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, $"Produto não encontrado com base neste Id {command.Id}.");

                produto.Nome = command.Nome;
                produto.Preco = command.Preco;
                produto.CategoriaId = command.CategoriaId;
                produto.Descricao = command.Descricao;

                ICollection<ProdutoIngrediente> _produtoIngredientes = [];

                if (_isLanche)
                {
                    if ((command.Ingredientes?.Count ?? 0) == 0)
                        return new Contracts.Response<ProdutoDTO?>(data: null, HttpStatusCode.BadRequest, message: "É necessário informar pelo menos um ingrediente para criar um produto do tipo Lanche.");

                    List<Guid> ingredientesIds = command.Ingredientes
                    .Select(i => i.Id)
                    .Distinct()
                    .ToList();

                    var ingredientesDb = await _ingredienteRepository.GetByIds(ingredientesIds);
                    var ingredientesDict = ingredientesDb.ToDictionary(i => i.Id, i => i);
                    var ingredientesEncontrados = ingredientesDict.Keys.ToHashSet();
                    var ingredientesNaoEncontrados = ingredientesIds
                        .Where(id => !ingredientesEncontrados.Contains(id))
                        .ToList();

                    if (ingredientesNaoEncontrados.Any())
                    {
                        var idsFaltando = string.Join(", ", ingredientesNaoEncontrados);
                        return new Contracts.Response<ProdutoDTO?>(null, HttpStatusCode.BadRequest,
                            $"Os seguintes ingredientes não foram encontrados: {idsFaltando}");
                    }

                    foreach (var ingrediente in command.Ingredientes!)
                    {
                        ProdutoIngrediente _produtoIngrediente = new(produto.Id, ingrediente.Id, ingrediente.Quantidade);
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

                ProdutoDTO produtoDto = new ProdutoDTO(command.Nome, command.Preco, new Contracts.DTO.Categoria.CategoriaDTO(_categoria.Id.ToString(), _categoria.Nome),
                                                                                        [.. produto.ProdutoImagens.Select(img => new ProdutoImagemDTO
                                                                                                    {
                                                                                                        Nome = img.Nome,
                                                                                                        Blob = img.Blob
                                                                                                    })], command.Descricao, produto.Id.ToString()
                                                                                                    , [.. produto.ProdutoIngredientes.Select(ing => new ProdutoIngredienteDTO
                                                                                                    {
                                                                                                        Quantidade = ing.Quantidade,
                                                                                                        Id = ing.IngredienteId.ToString()
                                                                                                    })]);

                return new Contracts.Response<ProdutoDTO?>(data: produtoDto, code: HttpStatusCode.OK, "Produto alterado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível alterar o produto.");
            }
        }



    }
}
