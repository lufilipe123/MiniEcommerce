# MiniEcommerce

Aplicação de console em .NET para gerenciar um pequeno e-commerce: cadastro de
produtos, catálogo, carrinho de compras e fechamento de pedidos com baixa
automática de estoque.

O projeto foi desenvolvido com **TDD básico** (testes unitários em xUnit) e
segue o padrão **Conventional Commits** no histórico do Git.

## Tecnologias

- .NET 10 / C#
- xUnit (testes unitários)
- Git (versionamento)

## Estrutura do projeto

```
MiniEcommerce.sln
├── MiniEcommerce.Console/
│   ├── Program.cs                  # Menu interativo (interface de console)
│   └── Domain/
│       ├── Produto.cs              # Entidade de produto e regras de estoque
│       ├── CatalogoProdutos.cs     # Cadastro e consulta de produtos
│       ├── ItemCarrinho.cs         # Item do carrinho (quantidade e preço)
│       ├── CarrinhoDeCompras.cs    # Adição, remoção, total e checkout
│       ├── ItemPedido.cs           # Item imutável do pedido finalizado
│       └── Pedido.cs               # Pedido com resumo financeiro
└── MiniEcommerce.Tests/
    ├── ProdutoTests.cs
    ├── CatalogoProdutosTests.cs
    ├── CarrinhoDeComprasTests.cs
    └── CheckoutTests.cs
```

A camada `Domain` concentra as regras de negócio e não depende do console. O
`Program.cs` apenas orquestra a interação com o usuário, o que mantém a lógica
testável de forma isolada.

## Como executar

Pré-requisito: .NET SDK 10 instalado.

```bash
dotnet restore
dotnet run --project MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

### Menu da aplicação

| Opção | Ação |
| ----- | ---- |
| 1 | Cadastrar produto (nome, preço e estoque) |
| 2 | Listar catálogo com preços e estoques atuais |
| 3 | Adicionar produto ao carrinho |
| 4 | Remover item (ou quantidade) do carrinho |
| 5 | Visualizar carrinho e total |
| 6 | Finalizar pedido (checkout) |
| 0 | Sair |

## Como executar os testes

```bash
dotnet test
```

## Regras de negócio

- **Produto**: nome obrigatório e único (comparação sem diferenciar
  maiúsculas/minúsculas), preço não pode ser negativo e estoque não pode ser
  negativo.
- **Carrinho**: a quantidade adicionada deve ser maior que zero; a soma das
  quantidades de um mesmo produto não pode ultrapassar o estoque disponível.
- **Checkout**: o carrinho não pode estar vazio; todos os itens são validados
  antes de qualquer baixa de estoque (evitando baixa parcial); o pedido registra
  o preço unitário do momento da compra e o carrinho é esvaziado ao final.

## Padrão de commits

O histórico segue o padrão Conventional Commits, por exemplo:

```
test(produto): add unit tests for product validation rules
feat(produto): implement product entity and validation logic
test(carrinho): add unit tests for cart addition and stock limits
feat(carrinho): implement shopping cart logic and stock verification
test(pedido): add unit tests for checkout and stock deduction
feat(pedido): implement order processing and inventory update
feat(console): implement interactive menu for e-commerce store
refactor(core): improve code readability and organize namespaces
docs: add comprehensive instructions in README.md
```
