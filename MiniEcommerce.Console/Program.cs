using System.Globalization;
using MiniEcommerce.Console.Domain;
using Console = System.Console;

var catalogo = new CatalogoProdutos();
var carrinho = new CarrinhoDeCompras();
var executando = true;

while (executando)
{
    ExibirMenu();
    var opcao = Console.ReadLine()?.Trim();

    Console.WriteLine();

    try
    {
        switch (opcao)
        {
            case "1":
                CadastrarProduto();
                break;
            case "2":
                ListarCatalogo();
                break;
            case "3":
                AdicionarAoCarrinho();
                break;
            case "4":
                RemoverDoCarrinho();
                break;
            case "5":
                ExibirCarrinho();
                break;
            case "6":
                FinalizarCompra();
                break;
            case "0":
                executando = false;
                Console.WriteLine("Encerrando o MiniEcommerce. Até logo!");
                break;
            default:
                Console.WriteLine("Opção inválida. Escolha uma das opções do menu.");
                break;
        }
    }
    catch (Exception excecao) when (excecao is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"Erro: {excecao.Message}");
    }

    if (executando)
    {
        Console.WriteLine();
        Console.WriteLine("Pressione Enter para continuar...");
        Console.ReadLine();
    }
}

void ExibirMenu()
{
    if (!Console.IsOutputRedirected)
    {
        Console.Clear();
    }

    Console.WriteLine("=================================");
    Console.WriteLine("       MiniEcommerce - Menu");
    Console.WriteLine("=================================");
    Console.WriteLine("1 - Cadastrar produto");
    Console.WriteLine("2 - Listar catálogo");
    Console.WriteLine("3 - Adicionar produto ao carrinho");
    Console.WriteLine("4 - Remover item do carrinho");
    Console.WriteLine("5 - Visualizar carrinho");
    Console.WriteLine("6 - Finalizar pedido");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");
}

void CadastrarProduto()
{
    Console.WriteLine("== Cadastro de produto ==");
    var nome = LerTexto("Nome: ");
    var preco = LerDecimal("Preço (R$): ");
    var estoque = LerInteiro("Quantidade em estoque: ");

    var produto = catalogo.CadastrarProduto(nome, preco, estoque);
    Console.WriteLine($"Produto '{produto.Nome}' cadastrado com sucesso.");
}

void ListarCatalogo()
{
    Console.WriteLine("== Catálogo de produtos ==");
    var produtos = catalogo.ListarProdutos();

    if (produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return;
    }

    Console.WriteLine($"{"#",-4}{"Produto",-25}{"Preço",12}{"Estoque",10}");
    for (var indice = 0; indice < produtos.Count; indice++)
    {
        var produto = produtos[indice];
        Console.WriteLine(
            $"{indice + 1,-4}{produto.Nome,-25}{produto.Preco,12:C}{produto.QuantidadeEstoque,10}");
    }
}

void AdicionarAoCarrinho()
{
    Console.WriteLine("== Adicionar produto ao carrinho ==");
    var produto = EscolherProduto();

    if (produto is null)
    {
        return;
    }

    var quantidade = LerInteiro("Quantidade desejada: ");
    carrinho.AdicionarProduto(produto, quantidade);
    Console.WriteLine($"{quantidade} unidade(s) de '{produto.Nome}' adicionada(s) ao carrinho.");
}

void RemoverDoCarrinho()
{
    Console.WriteLine("== Remover item do carrinho ==");

    if (carrinho.EstaVazio)
    {
        Console.WriteLine("O carrinho está vazio.");
        return;
    }

    var itens = carrinho.Itens;
    for (var indice = 0; indice < itens.Count; indice++)
    {
        var item = itens[indice];
        Console.WriteLine(
            $"{indice + 1} - {item.Quantidade} x {item.Produto.Nome} ({item.PrecoUnitario:C})");
    }

    var numeroItem = LerInteiro("Número do item: ");

    if (numeroItem < 1 || numeroItem > itens.Count)
    {
        Console.WriteLine("Item inválido.");
        return;
    }

    var itemSelecionado = itens[numeroItem - 1];
    var quantidade = LerInteiro(
        $"Quantidade a remover (máximo {itemSelecionado.Quantidade}, 0 para remover tudo): ");

    if (quantidade == 0)
    {
        carrinho.RemoverProduto(itemSelecionado.Produto);
        Console.WriteLine("Item removido do carrinho.");
        return;
    }

    carrinho.RemoverQuantidade(itemSelecionado.Produto, quantidade);
    Console.WriteLine("Item atualizado no carrinho.");
}

void ExibirCarrinho()
{
    Console.WriteLine("== Carrinho de compras ==");

    if (carrinho.EstaVazio)
    {
        Console.WriteLine("O carrinho está vazio.");
        return;
    }

    foreach (var item in carrinho.Itens)
    {
        Console.WriteLine(
            $"{item.Quantidade} x {item.Produto.Nome} ({item.PrecoUnitario:C}) = {item.Subtotal:C}");
    }

    Console.WriteLine($"Total: {carrinho.Total:C}");
}

void FinalizarCompra()
{
    Console.WriteLine("== Fechamento de pedido ==");
    var pedido = carrinho.FinalizarPedido();

    Console.WriteLine($"Pedido finalizado em {pedido.DataCriacao:dd/MM/yyyy HH:mm}.");
    foreach (var item in pedido.Itens)
    {
        Console.WriteLine(
            $"{item.Quantidade} x {item.NomeProduto} ({item.PrecoUnitario:C}) = {item.Subtotal:C}");
    }

    Console.WriteLine($"Total do pedido: {pedido.Total:C}");
    Console.WriteLine("Estoque atualizado com sucesso.");
}

Produto? EscolherProduto()
{
    var produtos = catalogo.ListarProdutos();

    if (produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return null;
    }

    for (var indice = 0; indice < produtos.Count; indice++)
    {
        var produto = produtos[indice];
        Console.WriteLine(
            $"{indice + 1} - {produto.Nome} ({produto.Preco:C}) - estoque: {produto.QuantidadeEstoque}");
    }

    while (true)
    {
        var numeroProduto = LerInteiro("Número do produto: ");

        if (numeroProduto >= 1 && numeroProduto <= produtos.Count)
        {
            return produtos[numeroProduto - 1];
        }

        Console.WriteLine("Produto inválido.");
    }
}

string LerTexto(string mensagem)
{
    while (true)
    {
        Console.Write(mensagem);
        var entrada = Console.ReadLine()?.Trim();

        if (!string.IsNullOrWhiteSpace(entrada))
        {
            return entrada;
        }

        Console.WriteLine("O valor informado não pode ser vazio.");
    }
}

decimal LerDecimal(string mensagem)
{
    var culturaPtBr = CultureInfo.GetCultureInfo("pt-BR");

    while (true)
    {
        Console.Write(mensagem);
        var entrada = Console.ReadLine()?.Trim();

        if (decimal.TryParse(entrada, NumberStyles.Number, culturaPtBr, out var valor) ||
            decimal.TryParse(entrada, NumberStyles.Number, CultureInfo.InvariantCulture, out valor))
        {
            return valor;
        }

        Console.WriteLine("Valor inválido. Digite um número (ex.: 19,90).");
    }
}

int LerInteiro(string mensagem)
{
    while (true)
    {
        Console.Write(mensagem);
        var entrada = Console.ReadLine()?.Trim();

        if (int.TryParse(entrada, NumberStyles.Integer, CultureInfo.InvariantCulture, out var valor))
        {
            return valor;
        }

        Console.WriteLine("Número inválido. Digite um número inteiro.");
    }
}
