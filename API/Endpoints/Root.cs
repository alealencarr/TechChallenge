namespace API.Endpoints;

internal sealed class Root : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => Results.Ok("Minha API .NET está funcionando!") );
    }
}


  