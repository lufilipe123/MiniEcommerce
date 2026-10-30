using MiniEcommerce.Console.Domain;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Fact]
    public void Construtor_ComDadosValidos_CriaProduto()
    {
        var produto = new Produto("Notebook", 3500.00m, 10);

        Assert.Equal("Notebook", produto.Nome);
        Assert.Equal(3500.00m, produto.Preco);
        Assert.Equal(10, produto.QuantidadeEstoque);
    }

    [Fact]
    public void Construtor_ComNomeComEspacos_RemoveEspacos()
    {
        var produto = new Produto("  Notebook  ", 3500.00m, 10);

        Assert.Equal("Notebook", produto.Nome);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Construtor_ComNomeVazio_LancaArgumentException(string nome)
    {
        Assert.Throws<ArgumentException>(() => new Produto(nome, 10m, 1));
    }

    [Fact]
    public void Construtor_ComNomeNulo_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Produto(null!, 10m, 1));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    public void Construtor_ComPrecoNegativo_LancaArgumentException(decimal preco)
    {
        Assert.Throws<ArgumentException>(() => new Produto("Notebook", preco, 1));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-50)]
    public void Construtor_ComEstoqueNegativo_LancaArgumentException(int quantidadeEstoque)
    {
        Assert.Throws<ArgumentException>(() => new Produto("Notebook", 10m, quantidadeEstoque));
    }

    [Theory]
    [InlineData(5, true)]
    [InlineData(10, true)]
    [InlineData(11, false)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void TemEstoquePara_VerificaQuantidadeDisponivel(int quantidade, bool esperado)
    {
        var produto = new Produto("Notebook", 3500.00m, 10);

        Assert.Equal(esperado, produto.TemEstoquePara(quantidade));
    }

    [Fact]
    public void BaixarEstoque_ComQuantidadeValida_DecrementaEstoque()
    {
        var produto = new Produto("Notebook", 3500.00m, 10);

        produto.BaixarEstoque(4);

        Assert.Equal(6, produto.QuantidadeEstoque);
    }

    [Fact]
    public void BaixarEstoque_AcimaDoDisponivel_LancaInvalidOperationException()
    {
        var produto = new Produto("Notebook", 3500.00m, 2);

        var excecao = Assert.Throws<InvalidOperationException>(() => produto.BaixarEstoque(3));

        Assert.Contains("Estoque insuficiente", excecao.Message);
        Assert.Equal(2, produto.QuantidadeEstoque);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void BaixarEstoque_ComQuantidadeInvalida_LancaArgumentException(int quantidade)
    {
        var produto = new Produto("Notebook", 3500.00m, 10);

        Assert.Throws<ArgumentException>(() => produto.BaixarEstoque(quantidade));
    }

    [Fact]
    public void ReporEstoque_ComQuantidadeValida_AumentaEstoque()
    {
        var produto = new Produto("Notebook", 3500.00m, 2);

        produto.ReporEstoque(8);

        Assert.Equal(10, produto.QuantidadeEstoque);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReporEstoque_ComQuantidadeInvalida_LancaArgumentException(int quantidade)
    {
        var produto = new Produto("Notebook", 3500.00m, 2);

        Assert.Throws<ArgumentException>(() => produto.ReporEstoque(quantidade));
    }
}
