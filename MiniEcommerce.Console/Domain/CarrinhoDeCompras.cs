namespace MiniEcommerce.Console.Domain;

/// <summary>
/// Carrinho de compras: adiciona, remove e totaliza itens, respeitando o estoque.
/// </summary>
public class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> _itens = [];

    public IReadOnlyList<ItemCarrinho> Itens => _itens.AsReadOnly();

    public decimal Total => _itens.Sum(item => item.Subtotal);

    public bool EstaVazio => _itens.Count == 0;

    public void AdicionarProduto(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (quantidade <= 0)
        {
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));
        }

        var itemExistente = _itens.FirstOrDefault(item => item.Produto == produto);
        var quantidadeTotal = quantidade + (itemExistente?.Quantidade ?? 0);

        if (!produto.TemEstoquePara(quantidadeTotal))
        {
            throw new InvalidOperationException(
                $"Estoque insuficiente para o produto '{produto.Nome}'. Disponível: {produto.QuantidadeEstoque}, solicitado: {quantidadeTotal}.");
        }

        if (itemExistente is null)
        {
            _itens.Add(new ItemCarrinho(produto, quantidade));
        }
        else
        {
            itemExistente.AdicionarQuantidade(quantidade);
        }
    }

    public void RemoverProduto(Produto produto)
    {
        ArgumentNullException.ThrowIfNull(produto);

        var item = ObterItem(produto);
        _itens.Remove(item);
    }

    public void RemoverQuantidade(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (quantidade <= 0)
        {
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));
        }

        var item = ObterItem(produto);

        if (quantidade > item.Quantidade)
        {
            throw new InvalidOperationException(
                $"Não é possível remover {quantidade} unidade(s) de '{produto.Nome}': o carrinho possui apenas {item.Quantidade}.");
        }

        if (quantidade == item.Quantidade)
        {
            _itens.Remove(item);
        }
        else
        {
            item.RemoverQuantidade(quantidade);
        }
    }

    public void Limpar() => _itens.Clear();

    public Pedido FinalizarPedido()
    {
        if (EstaVazio)
        {
            throw new InvalidOperationException("Não é possível finalizar um pedido com o carrinho vazio.");
        }

        // Valida todos os itens antes de baixar qualquer estoque, evitando baixa parcial.
        var itemSemEstoque = _itens.FirstOrDefault(item => !item.Produto.TemEstoquePara(item.Quantidade));

        if (itemSemEstoque is not null)
        {
            throw new InvalidOperationException(
                $"Estoque insuficiente para o produto '{itemSemEstoque.Produto.Nome}'. Disponível: {itemSemEstoque.Produto.QuantidadeEstoque}, solicitado: {itemSemEstoque.Quantidade}.");
        }

        foreach (var item in _itens)
        {
            item.Produto.BaixarEstoque(item.Quantidade);
        }

        var pedido = new Pedido(_itens.Select(item =>
            new ItemPedido(item.Produto.Nome, item.PrecoUnitario, item.Quantidade)));

        _itens.Clear();
        return pedido;
    }

    private ItemCarrinho ObterItem(Produto produto) =>
        _itens.FirstOrDefault(item => item.Produto == produto)
        ?? throw new InvalidOperationException($"O produto '{produto.Nome}' não está no carrinho.");
}
