using CueVault.Models;

namespace CueVault.Services;

/// <summary>
/// Scoped (per browser session) shopping cart.
/// Notifies subscribers via Changed event when the cart mutates.
/// </summary>
public class CartService
{
    private readonly List<CartItem> _items = new();

    public event Action? Changed;

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public int     TotalCount => _items.Sum(i => i.Quantity);
    public decimal Subtotal   => _items.Sum(i => i.LineTotal);
    public decimal Shipping   => Subtotal > 0 && Subtotal < 75 ? 9.99m : 0m;
    public decimal Total      => Subtotal + Shipping;

    public void Add(string productId, string name, decimal price)
    {
        var existing = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existing is not null) existing.Quantity++;
        else _items.Add(new CartItem(productId, name, price));
        Changed?.Invoke();
    }

    public void Remove(string productId)
    {
        _items.RemoveAll(i => i.ProductId == productId);
        Changed?.Invoke();
    }

    public void UpdateQuantity(string productId, int qty)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return;
        if (qty <= 0) Remove(productId);
        else { item.Quantity = qty; Changed?.Invoke(); }
    }

    public void Clear()
    {
        _items.Clear();
        Changed?.Invoke();
    }
}
