# language: pt
Funcionalidade: Criar Pedido
    Como um sistema de autoatendimento
    Gostaria de criar pedidos com produtos e ingredientes extras
    Para que a cozinha possa preparar

    Cenario: Criar pedido com cliente identificado e itens validos
        Dado que existe um cliente na base com ID "cli-01"
        E que existe um produto com ID "prod-01" Nome "X-Burger" e Preco 20.00
        E que existe um ingrediente com ID "ing-01" Nome "Bacon Extra" e Preco 3.00
        Quando eu monto um pedido para o cliente "cli-01"
        E adiciono 1 unidade do produto "prod-01"
        E adiciono o ingrediente "ing-01" no produto "prod-01"
        Entao o pedido deve ser salvo com sucesso
        E o gateway de pedidos deve ter sido chamado

    Cenario: Criar pedido anonimo com sucesso
        Dado que existe um produto com ID "prod-02" Nome "Coca-Cola" e Preco 5.00
        Quando eu monto um pedido sem cliente identificado
        E adiciono 2 unidades do produto "prod-02"
        Entao o pedido deve ser salvo com sucesso

    Cenario: Falhar ao tentar criar pedido para cliente inexistente
        Dado que o cliente com ID "cli-99" nao existe
        E que existe um produto com ID "prod-01" Nome "X-Burger" e Preco 20.00
        Quando eu monto um pedido para o cliente "cli-99"
        E adiciono 1 unidade do produto "prod-01"
        Entao o sistema deve retornar um erro contendo "Cliente com ID" e "não encontrado"

    Cenario: Falhar ao tentar pedir produto inexistente
        Dado que existe um cliente na base com ID "cli-01"
        Mas o produto com ID "prod-99" nao existe na base
        Quando eu monto um pedido para o cliente "cli-01"
        E adiciono 1 unidade do produto "prod-99"
        Entao o sistema deve retornar um erro contendo "Os seguintes produtos não foram encontrados"

    Cenario: Falhar ao tentar pedir ingrediente inexistente
        Dado que existe um produto com ID "prod-01" Nome "Lanche" e Preco 10.00
        Mas o ingrediente com ID "ing-99" nao existe na base
        Quando eu monto um pedido sem cliente identificado
        E adiciono 1 unidade do produto "prod-01"
        E adiciono o ingrediente "ing-99" no produto "prod-01"
        Entao o sistema deve retornar um erro contendo "Os seguintes ingredientes não foram encontrados"