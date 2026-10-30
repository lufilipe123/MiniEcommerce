namespace MiniEcommerce.Console.Domain;

/// <summary>
/// Item adicionado ao carrinho, com o preço unitário registrado no momento da inclusão.
/// </summary>
public class ItemCarrinho
{
    internal ItemCarrinho(Produto produto, int quantidade)
    {
        Produto = produto;
        Quantidade = quantidade;
        PrecoUnitario = produto.Preco;
    }

    public Produto Produto { get; }

    public int Quantidade { get; private set; }

    public decimal PrecoUnitario { get; }

    public decimal Subtotal => Quantidade * PrecoUnitario;

    internal void AdicionarQuantidade(int quantidade) => Quantidade += quantidade;

    internal void RemoverQuantidade(int quantidade) => Quantidade -= quantidade;
}
