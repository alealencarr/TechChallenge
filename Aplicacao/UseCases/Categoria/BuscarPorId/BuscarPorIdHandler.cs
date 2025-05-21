﻿using Aplicacao.Common;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Categoria.BuscarPorId
{
    public class BuscarPorIdHandler
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public BuscarPorIdHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Response<CategoriaDTO?>> Handler(string id)
        {
            try
            {
                var categorie = await _categoriaRepository.GetById(id);

                if (categorie is null)
                    return new Response<CategoriaDTO?>(data: null, code: System.Net.HttpStatusCode.NotFound, "Categoria não encontrada.");

                var categorieDto = new CategoriaDTO(categorie.Id.ToString(), categorie.Nome);       

                return new Response<CategoriaDTO?>(data: categorieDto, code: System.Net.HttpStatusCode.OK, "Categoria encontrada.");

            }
            catch (ArgumentException ex)
            {
                return new Response<CategoriaDTO?>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Response<CategoriaDTO?>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar a categoria.");
            }

        }
    }
}
