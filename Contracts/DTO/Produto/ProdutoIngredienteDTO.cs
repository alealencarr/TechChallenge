namespace Contracts.DTO.Produto;

public class ProdutoIngredienteDTO
{
    public string Id { get; set; }

    public int Quantidade { get; set; } 
 
    public ProdutoIngredienteDTO(string id, int quantidade )
    { 
        Quantidade = quantidade;
        Id = id;
    }

    public ProdutoIngredienteDTO() { }
}
