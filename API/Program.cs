using Adapters.Inbound.Controllers;
using Adapters.Outbound.Repositories;
using API.Service;
using Aplicacao.Services;
using Aplicacao.Services.Pagamento;
using Aplicacao.Services.QRCode;
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
builder.Services.AddScoped<Aplicacao.UseCases.Pedido.BuscarPorId.BuscarPorIdHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Pedido.AlterarStatus.AlterarStatusHandler>();

///

//produto
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<Aplicacao.UseCases.Produto.Criar.CriarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Produto.Alterar.AlterarPorIdHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Produto.BuscarPorCategoria.BuscarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Produto.Remover.RemoverHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Produto.BuscarPorId.BuscarPorIdHandler>();
///

//Ingrediente
builder.Services.AddScoped<IIngredienteRepository, IngredienteRepository>();
builder.Services.AddScoped<Aplicacao.UseCases.Ingrediente.Criar.CriarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Ingrediente.Alterar.AlterarHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Ingrediente.BuscarPorId.BuscarPorIdHandler>();
builder.Services.AddScoped<Aplicacao.UseCases.Ingrediente.BuscarTodos.BuscarTodosHandler>();
///

///Services 
///Pagamento
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddScoped<IQRCodeService, QRCodeService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<IFileSaver, FileSaver>();
builder.Services.AddHttpContextAccessor();
///


builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
});


var cnnStr = builder.Configuration.GetConnectionString("Default") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(x => { x.UseSqlServer(cnnStr); });

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(cnnStr, options =>
    {
        options.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        );
    });
});


var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    var retryCount = 0;
    var maxRetries = 10;
    var delay = TimeSpan.FromSeconds(5);

    while (true)
    {
        try
        {
            context.Database.Migrate();
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            Console.WriteLine($"Erro ao aplicar migrations (tentativa {retryCount}): {ex.Message}");

            if (retryCount >= maxRetries)
                throw;

            Thread.Sleep(delay);
        }
    }
}

app.Run();
