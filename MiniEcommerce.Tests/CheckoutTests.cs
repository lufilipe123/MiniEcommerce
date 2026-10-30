using MiniEcommerce.Console.Domain;

namespace MiniEcommerce.Tests;

public class CheckoutTests
{
    [Fact]
    public void FinalizarPedido_BaixaEstoqueDosProdutosComprados()
    {
        var carrinho = new CarrinhoDeCompras();
        var notebook = new Produto("Notebook", 3500m, 10);
        var mouse = new Produto("Mouse", 150m, 20);
        carrinho.AdicionarProduto(notebook, 2);
        carrinho.AdicionarProduto(mouse, 5);

        carrinho.FinalizarPedido();

        Assert.Equal(8, notebook.QuantidadeEstoque);
        Assert.Equal(15, mouse.QuantidadeEstoque);
    }

    [Fact]
    public void FinalizarPedido_RetornaPedidoComTotalCorreto()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarProduto(new Produto("Notebook", 3500m, 10), 1);
        carrinho.AdicionarProduto(new Produto("Mouse", 150m, 20), 2);

        var pedido = carrinho.FinalizarPedido();

        Assert.Equal(2, pedido.Itens.Count);
        Assert.Equal(3800m, pedido.Total);
        Assert.Equal("Notebook", pedido.Itens[0].NomeProduto);
        Assert.Equal(3500m, pedido.Itens[0].PrecoUnitario);
        Assert.Equal(1, pedido.Itens[0].Quantidade);
        Assert.Equal(300m, pedido.Itens[1].Subtotal);
    }

    [Fact]
    public void FinalizarPedido_RegistraDataDeCriacao()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarProduto(new Produto("Notebook", 3500m, 10), 1);
        var antes = DateTime.Now;

        var pedido = carrinho.FinalizarPedido();

        Assert.InRange(pedido.DataCriacao, antes, DateTime.Now);
    }

    [Fact]
    public void FinalizarPedido_EsvaziaOCarrinho()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarProduto(new Produto("Notebook", 3500m, 10), 1);

        carrinho.FinalizarPedido();

        Assert.True(carrinho.EstaVazio);
        Assert.Equal(0m, carrinho.Total);
    }

    [Fact]
    public void FinalizarPedido_ComCarrinhoVazio_LancaInvalidOperationException()
    {
        var carrinho = new CarrinhoDeCompras();

        var excecao = Assert.Throws<InvalidOperationException>(() => carrinho.FinalizarPedido());

        Assert.Contains("carrinho vazio", excecao.Message);
    }

    [Fact]
    public void FinalizarPedido_ComItemSemEstoque_NaoBaixaEstoqueDeNenhumProduto()
    {
        var carrinho = new CarrinhoDeCompras();
        var notebook = new Produto("Notebook", 3500m, 10);
        var mouse = new Produto("Mouse", 150m, 2);
        carrinho.AdicionarProduto(notebook, 2);
        carrinho.AdicionarProduto(mouse, 2);

        // Simula outra venda que consumiu o estoque depois que o item entrou no carrinho.
        mouse.BaixarEstoque(2);

        var excecao = Assert.Throws<InvalidOperationException>(() => carrinho.FinalizarPedido());

        Assert.Contains("Estoque insuficiente", excecao.Message);
        Assert.Equal(10, notebook.QuantidadeEstoque);
        Assert.False(carrinho.EstaVazio);
    }

    [Fact]
    public void Pedido_ComListaVazia_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Pedido([]));
    }
}
