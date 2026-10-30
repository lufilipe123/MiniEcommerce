namespace MiniEcommerce.Console.Domain;

/// <summary>
/// Pedido finalizado, contendo o resumo financeiro dos itens comprados.
/// </summary>
public class Pedido
{
    public Pedido(IEnumerable<ItemPedido> itens)
    {
        var listaItens = itens?.ToList() ?? [];

        if (listaItens.Count == 0)
        {
            throw new ArgumentException("O pedido deve conter pelo menos um item.", nameof(itens));
        }

        Itens = listaItens.AsReadOnly();
        DataCriacao = DateTime.Now;
    }

    public IReadOnlyList<ItemPedido> Itens { get; }

    public decimal Total => Itens.Sum(item => item.Subtotal);

    public DateTime DataCriacao { get; }
}
