using MiniEcommerce.Console.Domain;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
    private static Produto CriarProduto(string nome = "Notebook", decimal preco = 100m, int estoque = 10) =>
        new(nome, preco, estoque);

    [Fact]
    public void CarrinhoNovo_EstaVazio()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.True(carrinho.EstaVazio);
        Assert.Empty(carrinho.Itens);
        Assert.Equal(0m, carrinho.Total);
    }

    [Fact]
    public void AdicionarProduto_AdicionaItemComSubtotal()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto(preco: 150.50m);

        carrinho.AdicionarProduto(produto, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Same(produto, item.Produto);
        Assert.Equal(3, item.Quantidade);
        Assert.Equal(150.50m, item.PrecoUnitario);
        Assert.Equal(451.50m, item.Subtotal);
        Assert.False(carrinho.EstaVazio);
    }

    [Fact]
    public void AdicionarProduto_MesmoProduto_SomaQuantidade()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto();

        carrinho.AdicionarProduto(produto, 2);
        carrinho.AdicionarProduto(produto, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(5, item.Quantidade);
        Assert.Equal(500m, carrinho.Total);
    }

    [Fact]
    public void AdicionarProduto_AcimaDoEstoque_LancaInvalidOperationException()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto(estoque: 2);

        var excecao = Assert.Throws<InvalidOperationException>(
            () => carrinho.AdicionarProduto(produto, 3));

        Assert.Contains("Estoque insuficiente", excecao.Message);
        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void AdicionarProduto_SomandoAcimaDoEstoque_LancaInvalidOperationException()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto(estoque: 5);
        carrinho.AdicionarProduto(produto, 4);

        Assert.Throws<InvalidOperationException>(() => carrinho.AdicionarProduto(produto, 2));
        Assert.Equal(4, Assert.Single(carrinho.Itens).Quantidade);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AdicionarProduto_ComQuantidadeInvalida_LancaArgumentException(int quantidade)
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<ArgumentException>(() => carrinho.AdicionarProduto(CriarProduto(), quantidade));
    }

    [Fact]
    public void AdicionarProduto_Nulo_LancaArgumentNullException()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<ArgumentNullException>(() => carrinho.AdicionarProduto(null!, 1));
    }

    [Fact]
    public void Total_SomaSubtotaisDeTodosOsItens()
    {
        var carrinho = new CarrinhoDeCompras();
        var notebook = CriarProduto("Notebook", 3500m, 10);
        var mouse = CriarProduto("Mouse", 150m, 20);

        carrinho.AdicionarProduto(notebook, 1);
        carrinho.AdicionarProduto(mouse, 2);

        Assert.Equal(3800m, carrinho.Total);
    }

    [Fact]
    public void RemoverProduto_RemoveItemDoCarrinho()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto();
        carrinho.AdicionarProduto(produto, 2);

        carrinho.RemoverProduto(produto);

        Assert.True(carrinho.EstaVazio);
    }

    [Fact]
    public void RemoverProduto_Inexistente_LancaInvalidOperationException()
    {
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => carrinho.RemoverProduto(CriarProduto()));
    }

    [Fact]
    public void RemoverQuantidade_Parcial_MantemItemComQuantidadeRestante()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto();
        carrinho.AdicionarProduto(produto, 5);

        carrinho.RemoverQuantidade(produto, 2);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(3, item.Quantidade);
        Assert.Equal(300m, carrinho.Total);
    }

    [Fact]
    public void RemoverQuantidade_Total_RemoveItem()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto();
        carrinho.AdicionarProduto(produto, 2);

        carrinho.RemoverQuantidade(produto, 2);

        Assert.True(carrinho.EstaVazio);
    }

    [Fact]
    public void RemoverQuantidade_MaiorQueAdicionada_LancaInvalidOperationException()
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto();
        carrinho.AdicionarProduto(produto, 2);

        Assert.Throws<InvalidOperationException>(() => carrinho.RemoverQuantidade(produto, 3));
        Assert.Equal(2, Assert.Single(carrinho.Itens).Quantidade);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RemoverQuantidade_ComQuantidadeInvalida_LancaArgumentException(int quantidade)
    {
        var carrinho = new CarrinhoDeCompras();
        var produto = CriarProduto();
        carrinho.AdicionarProduto(produto, 2);

        Assert.Throws<ArgumentException>(() => carrinho.RemoverQuantidade(produto, quantidade));
    }

    [Fact]
    public void Limpar_RemoveTodosOsItens()
    {
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarProduto(CriarProduto("Notebook"), 1);
        carrinho.AdicionarProduto(CriarProduto("Mouse", 150m), 2);

        carrinho.Limpar();

        Assert.True(carrinho.EstaVazio);
        Assert.Equal(0m, carrinho.Total);
    }
}
