namespace MiniEcommerce.Console.Domain;

/// <summary>
/// Registro imutável de um item efetivamente comprado.
/// </summary>
public sealed record ItemPedido(string NomeProduto, decimal PrecoUnitario, int Quantidade)
{
    public decimal Subtotal => PrecoUnitario * Quantidade;
}
