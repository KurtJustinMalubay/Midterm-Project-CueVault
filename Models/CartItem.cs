namespace CueVault.Models;

/// <summary>A product and quantity currently held in a customer's shopping cart.</summary>
public class CartItem
{
    public string ProductId { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; set; }

    public CartItem(string productId, string name, decimal price)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        Quantity = 1;
    }

    public decimal LineTotal => Price * Quantity;
}