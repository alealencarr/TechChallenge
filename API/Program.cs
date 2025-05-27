using Adapters.Inbound.Controllers;
using Adapters.Outbound.Repositories;
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
builder.Services.AddScoped<Aplicacao.UseCases.Cliente.Criar.CriarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Cliente.Alterar.AlterarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Cliente.BuscarPorCPF.BuscarPorCPFHandler>();
///

//Categoria
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<Aplicacao.UseCases.Categoria.BuscarPorId.BuscarPorIdHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Categoria.BuscarTodos.BuscarTodosHandler>();
///

//Pedido
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<Aplicacao.UseCases.Pedido.Criar.CriarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Pedido.Finalizar.FinalizarHandler>();
///

//Pedido
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<Aplicacao.UseCases.Produtos.Criar.CriarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Produtos.Alterar.AlterarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Produtos.Buscar.BuscarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Produtos.Remover.RemoverHandler>();
///

//Ingrediente
builder.Services.AddScoped<IIngredienteRepository, IngredienteRepository>();
builder.Services.AddScoped<Aplicacao.UseCases.Ingrediente.Criar.CriarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Ingrediente.Alterar.AlterarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Ingrediente.BuscarPorId.BuscarPorIdHandler>();
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
