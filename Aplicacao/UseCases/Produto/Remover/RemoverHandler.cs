using Aplicacao.Common;
using Aplicacao.Services;
using Contracts.DTO.Produto;
using Domain.Entidades;
using Domain.Ports;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Produto.Remover
{
    public class RemoverHandler : IRemoverHandler
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileSaver _fileSaver;
        public RemoverHandler(IProdutoRepository produtoRepository, IHttpContextAccessor httpContextAccessor, IFileSaver fileSaver)
        {
            _produtoRepository = produtoRepository;
            _httpContextAccessor = httpContextAccessor;
            _fileSaver = fileSaver;
        }

        public async Task<Contracts.Response<ProdutoDTO?>> Handle(string id)
        {

            try
            {
                var produto = await _produtoRepository.GetById(id);

                if (produto is null)
                    return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, "Produto não encontrado com base neste Id.");

                var imagePath = $"produtos/imagens/produto-{produto.Id.ToString()}";

                _fileSaver.LimparPasta(imagePath);

                await _produtoRepository.Remover(produto);

                var urlRoot = _httpContextAccessor == null ? $"http://localhost:7057" :
             $"{_httpContextAccessor?.HttpContext.Request.Scheme}://{_httpContextAccessor?.HttpContext.Request.Host}";

                ProdutoDTO produtoDto = new ProdutoDTO(produto.Nome, produto.Preco, new Contracts.DTO.Categoria.CategoriaDTO(produto.Categoria.Id.ToString(), produto.Categoria.Nome),
                                                                                         [.. produto.ProdutoImagens.Select(img => new ProdutoImagemDTO
                                                                                                    {
                                                                                                        Url = $"{urlRoot}/{img.ImagePath}/{img.FileName}",
                                                                                                        Nome = img.Nome,
                                                                                                        Mimetype = img.MimeType
                                                                                                    })], produto.Descricao, produto.Id.ToString()
                                                                                                     , [.. produto.ProdutoIngredientes.Select(ing => new ProdutoIngredienteDTO
                                                                                                    {
                                                                                                        Quantidade = ing.Quantidade,
                                                                                                        Id = ing.IngredienteId.ToString()
                                                                                                    })]);


                return new Contracts.Response<ProdutoDTO?>(data: produtoDto, code: System.Net.HttpStatusCode.OK, "Produto excluido com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch  
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível excluir o produto.");
            }
        }
    }
}
