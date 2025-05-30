﻿using Aplicacao.Common;
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

namespace Aplicacao.UseCases.Produto.BuscarPorId
{
    public class BuscarPorIdHandler
    {
        private readonly IProdutoRepository _produtoRepository;

        public BuscarPorIdHandler(IProdutoRepository ProdutoRepository)
        {
            _produtoRepository = ProdutoRepository;
        }

        public async Task<Contracts.Response<ProdutoDTO?>> Handle(string id)
        {
            try
            {
                var product = await _produtoRepository.GetById(id);

                if (product is null)
                    return new Contracts.Response<ProdutoDTO?>(data: null, code: System.Net.HttpStatusCode.BadRequest, $"Produto não encontrado com base neste Id: {id}.");

                var produtoDto =  new ProdutoDTO
                {
                    Id = product.Id.ToString(),
                    Nome = product.Nome,
                    Categoria =  new Contracts.DTO.Categoria.CategoriaDTO(product.Categoria.Id.ToString(), product.Categoria.Nome),  
                    Descricao = product.Descricao,
                    Preco = product.Preco,
                    Ingredientes = [.. product.ProdutoIngredientes.Select(ing => new ProdutoIngredienteDTO
                    {
                       Quantidade = ing.Quantidade,
                        Id = ing.IngredienteId.ToString()
                    })],
                    Imagens = [.. product.ProdutoImagens.Select(img => new ProdutoImagemDTO
                    {
                        Nome = img.Nome,
                        Blob = img.Blob
                    })]
                };

               
                return new Contracts.Response<ProdutoDTO?>(data: produtoDto, code: System.Net.HttpStatusCode.OK, "Produto encontrado com sucesso.");

            }
            catch (ArgumentException ex)
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                return new Contracts.Response<ProdutoDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar este Produto.");
            }

        }
    }
}
