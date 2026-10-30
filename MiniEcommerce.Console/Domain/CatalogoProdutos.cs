namespace MiniEcommerce.Console.Domain;

/// <summary>
/// Gerencia o cadastro e a consulta dos produtos disponíveis na loja.
/// </summary>
public class CatalogoProdutos
{
    private readonly List<Produto> _produtos = [];

    public IReadOnlyList<Produto> Produtos => _produtos.AsReadOnly();

    public Produto CadastrarProduto(string nome, decimal preco, int quantidadeEstoque)
    {
        var produto = new Produto(nome, preco, quantidadeEstoque);

        if (BuscarPorNome(produto.Nome) is not null)
        {
            throw new InvalidOperationException($"Já existe um produto cadastrado com o nome '{produto.Nome}'.");
        }

        _produtos.Add(produto);
        return produto;
    }

    public Produto? BuscarPorNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return null;
        }

        return _produtos.FirstOrDefault(produto =>
            string.Equals(produto.Nome, nome.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<Produto> ListarProdutos() => _produtos.AsReadOnly();
}
