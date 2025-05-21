using Adapters.Inbound.Controllers;
using Adapters.Outbound.Repositories;
using Aplicacao.UseCases.Categoria.BuscarPorId;
using Aplicacao.UseCases.Categoria.BuscarTodos;
using Aplicacao.UseCases.Cliente.Alterar;
using Aplicacao.UseCases.Cliente.BuscarPorCPF;
using Aplicacao.UseCases.Cliente.Criar;
using Aplicacao.UseCases.Pedido.Criar;
using Aplicacao.UseCases.Pedido.Finalizar;
using Domain.Ports;
using Infraestrutura;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .PartManager.ApplicationParts.Add(new Microsoft.AspNetCore.Mvc.ApplicationParts.AssemblyPart(typeof(ClienteController).Assembly));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Cliente
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<CriarHandler>();
builder.Services.AddScoped<AlterarHandler>();
builder.Services.AddScoped<BuscarPorCPFHandler>();
///

//Categoria
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<BuscarPorIdHandler>();
builder.Services.AddScoped<BuscarTodosHandler>();
///

//Pedido
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<CriarHandler>();
builder.Services.AddScoped<AlterarHandler>();
builder.Services.AddScoped<FinalizarHandler>();
///


builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
});
 

var cnnStr = builder.Configuration.GetConnectionString("minhaconnectionstring") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(x => { x.UseSqlServer(cnnStr); });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();  

    DbInitializer.SeedCategorias(db);
}

app.Run();
