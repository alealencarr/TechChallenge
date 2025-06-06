﻿using Contracts.DTO.Ingrediente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.UseCases.Ingrediente.BuscarPorId
{
    public interface IBuscarPorIdHandler
    {
        Task<Contracts.Response<IngredienteDTO?>> Handler(BuscarPorIdCommand command);
    }
}
