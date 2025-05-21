﻿using Aplicacao.Common;
using Aplicacao.UseCases.Cliente;
using Domain.Entidades;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Categoria.BuscarTodos
{
    public class BuscarTodosHandler
    {
        private ICategoriaRepository _categoriaRepository;

        public BuscarTodosHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Response<List<CategoriaDTO>>> Handler()
        {
            try
            {
                var categories = await _categoriaRepository.GetAll();

                if (categories is null)
                    return new Response<List<CategoriaDTO>>(data: null, code: System.Net.HttpStatusCode.NotFound, "Nenhuma categoria encontrada.");

                var categoriesDto = new List<CategoriaDTO>();

                categoriesDto = categories
             .Select(x => new CategoriaDTO { Id = x.Id.ToString(), Nome = x.Nome })
             .ToList();


                return new Response<List<CategoriaDTO>>(data: categoriesDto, code: System.Net.HttpStatusCode.OK, "Categorias encontradas.");

            }
            catch (ArgumentException ex)
            {
                return new Response<List<CategoriaDTO>>(data: null, code: HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return new Response<List<CategoriaDTO>>(data: null, code: HttpStatusCode.InternalServerError, "Não foi possível localizar as categorias.");
            }


        }
    }
}
