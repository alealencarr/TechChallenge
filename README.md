# 🚀 TechChallenge - HUNGRY 

# 📚 Documentação & Recursos

- 📄 [Documentação da API](docs/api.html)
- 🌐 [Como rodar o projeto (index.html)](docs/index.html)
- 🧪 [Download da Collection Postman](postman/TechChallenge.postman_collection.json)

## 🧭 Diagramas

| Tipo                        | Visualização                                                  |
|-----------------------------|---------------------------------------------------------------|
| 🧠 Diagrama de Negócio      | ![Negócio](docs/negocio.png)                                  |
| 🧱 Diagrama de Infraestrutura | ![Infraestrutura](docs/infraestrutura.png)                  |
| 🛠️ Resolução de Problema     | ![Resolução do Problema levantado](docs/resolucao.png)                             |

---

Sistema completo para gerenciamento de pedidos e operações em uma hamburgueria, utilizando **DDD**, **Clean ARCH** e boas práticas modernas de desenvolvimento backend em .NET.
 
## 🧱 Estrutura da Solução

| TechChallenge
- ├── API             → Ponto de entrada da aplicação (autenticação)
- ├── Application     → Casos de uso (Use Cases), Controllers, Gateways, Presenter e Interfaces de Data Sources
- ├── Shared          → DTOs, Helpers, Results, Requests e Responses
- ├── Domain          → Entidades, Aggregates, Regras de Negócio (DDD puro)
- ├── Infrastructure  → Implementações concretas (EF Core, Repositórios, serviços, implementações de Data Sources, etc.)


---

## ✨ Tecnologias Utilizadas

- .NET 9
- ASP.NET Core
- Entity Framework Core
- C# 12
- Injeção de dependência manual
- Arquitetura Limpa (Seguida a risca)
- Domain-Driven Design (DDD)
- Swagger para documentação

---

## ✅ Funcionalidades

- Gerenciamento de **Clientes**, **Produtos**, **Ingredientes**, **Pedidos** e **Categorias**
- Criação de pedidos com ou sem cliente
- Montagem de produtos com ou sem ingredientes (ex: lanches personalizados)
- Pagamento e alteração de status dos pedidos
- Separação clara de responsabilidades entre camadas
- Documentação via Swagger/OpenAPI

🧪 API e Swagger

Após subir a aplicação, acesse a documentação interativa no navegador. Acompanhe também a WIKI.

📄 Licença

Este projeto está licenciado sob os termos da licença MIT.  
Consulte o arquivo [LICENSE](./LICENSE) para mais detalhes.