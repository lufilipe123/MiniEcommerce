namespace MiniEcommerce.Console.Domain;

/// <summary>
/// Representa um produto do catálogo, com preço e controle de estoque.
/// </summary>
public class Produto
{
    public string Nome { get; }

    public decimal Preco { get; }

    public int QuantidadeEstoque { get; private set; }

    public Produto(string nome, decimal preco, int quantidadeEstoque)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome do produto é obrigatório.", nameof(nome));
        }

        if (preco < 0)
        {
            throw new ArgumentException("O preço do produto não pode ser negativo.", nameof(preco));
        }

        if (quantidadeEstoque < 0)
        {
            throw new ArgumentException("A quantidade em estoque não pode ser negativa.", nameof(quantidadeEstoque));
        }

        Nome = nome.Trim();
        Preco = preco;
        QuantidadeEstoque = quantidadeEstoque;
    }

    public bool TemEstoquePara(int quantidade) => quantidade > 0 && QuantidadeEstoque >= quantidade;

    public void BaixarEstoque(int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new ArgumentException("A quantidade a baixar deve ser maior que zero.", nameof(quantidade));
        }

        if (QuantidadeEstoque < quantidade)
        {
            throw new InvalidOperationException(
                $"Estoque insuficiente para o produto '{Nome}'. Disponível: {QuantidadeEstoque}, solicitado: {quantidade}.");
        }

        QuantidadeEstoque -= quantidade;
    }

    public void ReporEstoque(int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new ArgumentException("A quantidade a repor deve ser maior que zero.", nameof(quantidade));
        }

        QuantidadeEstoque += quantidade;
    }
}
