namespace Contracts.DTO.Produto;

public class ProdutoIngredienteDTO
{
    public int Quantidade { get; set; } 

 
    public string Id { get; set; } 
    public ProdutoIngredienteDTO(string id, int quantidade )
    { 
        Quantidade = quantidade;
        Id = id;
    }

    public ProdutoIngredienteDTO() { }
}
