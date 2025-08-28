# 🚀 TechChallenge

Sistema completo para gerenciamento de pedidos e operações em uma hamburgueria, utilizando **DDD**, **arquitetura hexagonal** e boas práticas modernas de desenvolvimento backend em .NET.
 
## 🧱 Estrutura da Solução

| TechChallenge
- ├── API             → Ponto de entrada da aplicação (autenticação)
- ├── Aplicacao       → Casos de uso (Use Cases) com Handlers e Commands
- ├── Adapters        → Adaptações de inbounds (controllers) e outbounds (repositories)
- ├── Contracts       → DTOs, Requests e Responses
- ├── Domain          → Entidades, Ports (Interfaces), Aggregates, Regras de Negócio (DDD puro)
- ├── Infraestrutura  → Implementações concretas (EF Core, Repositórios, Unit of Work, etc.)


---

## ✨ Tecnologias Utilizadas

- .NET 8
- ASP.NET Core
- Entity Framework Core
- C# 12
- Injeção de dependência nativa
- Arquitetura Hexagonal (Ports & Adapters)
- Domain-Driven Design (DDD)
- CQRS com Commands e Handlers
- JWT para autenticação
- RESTful APIs
- Swagger para documentação

---

## ✅ Funcionalidades

- Gerenciamento de **Clientes**, **Produtos**, **Ingredientes**, **Pedidos** e **Categorias**
- Criação de pedidos com ou sem cliente
- Montagem de produtos com ou sem ingredientes (ex: lanches personalizados)
- Pagamento e alteração de status dos pedidos
- Separação clara de responsabilidades entre camadas
- Documentação via Swagger/OpenAPI
- Suporte a testes automatizados 

---

## 🛠️ Como rodar o projeto

1. Clone o repositório:
  ```bash
   git clone https://github.com/alealencarr/TechChallenge.git
 ```
 
2. Abra o docker desktop, navegue até a raiz do projeto e rode:
  ```bash
  docker-compose up --build
   ```
🧪 API e Swagger

Após subir a aplicação, acesse a documentação interativa no navegador. Acompanhe também a WIKI.

📄 Licença

Este projeto está licenciado sob os termos da licença MIT.  
Consulte o arquivo [LICENSE](./LICENSE) para mais detalhes.

