using MiniEcommerce.Console.Domain;

namespace MiniEcommerce.Tests;

public class CatalogoProdutosTests
{
    [Fact]
    public void CadastrarProduto_AdicionaProdutoAoCatalogo()
    {
        var catalogo = new CatalogoProdutos();

        var produto = catalogo.CadastrarProduto("Notebook", 3500.00m, 10);

        Assert.Same(produto, Assert.Single(catalogo.ListarProdutos()));
    }

    [Fact]
    public void CadastrarProduto_ComNomeDuplicado_LancaInvalidOperationException()
    {
        var catalogo = new CatalogoProdutos();
        catalogo.CadastrarProduto("Notebook", 3500.00m, 10);

        var excecao = Assert.Throws<InvalidOperationException>(
            () => catalogo.CadastrarProduto("Notebook", 4000.00m, 5));

        Assert.Contains("Já existe um produto", excecao.Message);
        Assert.Single(catalogo.ListarProdutos());
    }

    [Fact]
    public void CadastrarProduto_ComNomeDuplicadoIgnorandoCase_LancaInvalidOperationException()
    {
        var catalogo = new CatalogoProdutos();
        catalogo.CadastrarProduto("Notebook", 3500.00m, 10);

        Assert.Throws<InvalidOperationException>(
            () => catalogo.CadastrarProduto("NOTEBOOK", 4000.00m, 5));
    }

    [Fact]
    public void CadastrarProduto_ComPrecoNegativo_NaoAdicionaAoCatalogo()
    {
        var catalogo = new CatalogoProdutos();

        Assert.Throws<ArgumentException>(() => catalogo.CadastrarProduto("Notebook", -1m, 10));
        Assert.Empty(catalogo.ListarProdutos());
    }

    [Fact]
    public void BuscarPorNome_RetornaProdutoIgnorandoCase()
    {
        var catalogo = new CatalogoProdutos();
        var produto = catalogo.CadastrarProduto("Notebook", 3500.00m, 10);

        var encontrado = catalogo.BuscarPorNome("NOTEBOOK");

        Assert.Same(produto, encontrado);
    }

    [Fact]
    public void BuscarPorNome_Inexistente_RetornaNull()
    {
        var catalogo = new CatalogoProdutos();
        catalogo.CadastrarProduto("Notebook", 3500.00m, 10);

        Assert.Null(catalogo.BuscarPorNome("Mouse"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void BuscarPorNome_ComNomeVazio_RetornaNull(string nome)
    {
        var catalogo = new CatalogoProdutos();
        catalogo.CadastrarProduto("Notebook", 3500.00m, 10);

        Assert.Null(catalogo.BuscarPorNome(nome));
    }

    [Fact]
    public void ListarProdutos_RetornaTodosOsProdutosCadastrados()
    {
        var catalogo = new CatalogoProdutos();
        catalogo.CadastrarProduto("Notebook", 3500.00m, 10);
        catalogo.CadastrarProduto("Mouse", 150.00m, 20);

        var produtos = catalogo.ListarProdutos();

        Assert.Equal(2, produtos.Count);
        Assert.Collection(
            produtos,
            produto => Assert.Equal("Notebook", produto.Nome),
            produto => Assert.Equal("Mouse", produto.Nome));
    }
}
