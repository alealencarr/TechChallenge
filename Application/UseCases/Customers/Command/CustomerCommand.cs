namespace Application.UseCases.Customers.Command;

public class CustomerCommand
{
    public CustomerCommand(Guid id, string cpf, string name, string mail)
    {
        Cpf = cpf;
        Name = name;
        Mail = mail;
        Id = id;
    }

    public CustomerCommand(string cpf, string name, string mail)
    {
        Cpf = cpf;
        Name = name;
        Mail = mail;
    }

    public string Cpf { get; private set; }
    public string Name { get; private set; } 
    public string Mail { get; private set; } 
    public Guid Id { get; private set; }
}