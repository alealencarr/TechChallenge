# 🚀 TechChallenge - HUNGRY 

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

---

## 🛠️ Como rodar o projeto

1. Clone o repositório:
  ```bash
   git clone https://github.com/alealencarr/TechChallenge.git
   ```
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

