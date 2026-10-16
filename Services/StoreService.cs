using System.Collections.Concurrent;
using CueVault.Models;

namespace CueVault.Services;

// ── Service ──────────────────────────────────────────────────────────────────

/// <summary>
/// Singleton in-memory store. No database — data lives only while the app runs.
/// Seeded with billiards product and order sample data.
/// </summary>
public class StoreService
{
    private readonly ConcurrentBag<ProductEntry> _products = new();
    private readonly ConcurrentBag<OrderEntry>   _orders   = new();
    private readonly ConcurrentBag<ReviewEntry>  _reviews  = new();

    public StoreService()
    {
        // ── Seed products ───────────────────────────────────────────
        _products.Add(new(Id(), "Predator Revo 12.9mm Shaft",
            "Carbon fiber shaft delivering unmatched consistency and low deflection.",
            "Cues", 289.99m, 14, "New", Ago(1), ["cue", "shaft", "predator"]));
        _products.Add(new(Id(), "Aramith Premium Ball Set",
            "Belgian phenolic resin balls — the gold standard in pool rooms worldwide.",
            "Balls", 189.99m, 8, "New", Ago(2), ["balls", "aramith", "premium"]));
        _products.Add(new(Id(), "Simonis 860 Cloth (9ft)",
            "Tournament-grade worsted wool felt in championship green.",
            "Accessories", 149.99m, 5, "New", Ago(3), ["cloth", "felt", "simonis"]));
        _products.Add(new(Id(), "Kamui Black Tip (Medium)",
            "Japanese layered leather tip with superior grip and shape retention.",
            "Accessories", 24.99m, 42, "New", Ago(4), ["tip", "kamui", "leather"]));
        _products.Add(new(Id(), "Ozone 4-Piece Break Cue",
            "Stiff low-deflection break cue designed to maximize power and accuracy.",
            "Cues", 159.99m, 6, "New", Ago(5), ["cue", "break", "ozone"]));
        _products.Add(new(Id(), "Master Chalk (Dozen)",
            "Classic blue Master chalk — reliable and used in halls across the globe.",
            "Accessories", 8.99m, 120, "New", Ago(6), ["chalk", "master", "accessories"]));
        _products.Add(new(Id(), "Players C-960 House Cue",
            "Solid maple house cue with Irish linen wrap — great for beginners.",
            "Cues", 49.99m, 20, "Used", Ago(10), ["cue", "beginner", "maple"]));
        _products.Add(new(Id(), "Triangle Rack (Wooden)",
            "Smooth hardwood rack for consistent 8-ball and 9-ball setups.",
            "Accessories", 19.99m, 30, "New", Ago(8), ["rack", "accessories"]));

        // ── Seed orders ─────────────────────────────────────────────
        var shaft   = _products.First(p => p.Name.Contains("Shaft"));
        var balls   = _products.First(p => p.Name.Contains("Ball Set"));
        var chalk   = _products.First(p => p.Name.Contains("Chalk"));

        _orders.Add(new(Id(), "Marco Santos",  "marco@example.com",  shaft.Id, shaft.Name,   1, shaft.Price,       "Shipped",    Ago(0)));
        _orders.Add(new(Id(), "Lea Fontaine",  "lea@example.com",    balls.Id, balls.Name,   1, balls.Price,       "Processing", Ago(1)));
        _orders.Add(new(Id(), "Roy Nakamura",  "roy@example.com",    chalk.Id, chalk.Name,   3, chalk.Price * 3,   "Delivered",  Ago(3)));
        _orders.Add(new(Id(), "Sandra Pierce", "sandra@example.com", shaft.Id, shaft.Name,   1, shaft.Price,       "Delivered",  Ago(5)));

        // ── Seed reviews ────────────────────────────────────────────
        _reviews.Add(new(Id(), shaft.Id, shaft.Name, "Marco S.",  5, "Incredible feel, zero squirt. Worth every penny!", Ago(0)));
        _reviews.Add(new(Id(), balls.Id, balls.Name, "Lea F.",    5, "These balls roll true every time. Tournament quality.", Ago(2)));
        _reviews.Add(new(Id(), chalk.Id, chalk.Name, "Roy N.",    4, "Classic chalk, does the job. Bought 3 boxes.", Ago(3)));
    }

    // ── Events ──────────────────────────────────────────────────────
    public event Action? OnProductChanged;
    public event Action? OnOrderChanged;
    public event Action? OnReviewChanged;

    // ── Products ─────────────────────────────────────────────────────
    public void AddProduct(string name, string description, string category,
                           decimal price, int stock, string condition, string[] tags)
    {
        _products.Add(new(Id(), name, description, category, price, stock, condition, DateTime.Now, tags));
        OnProductChanged?.Invoke();
    }

    public void UpdateProductStock(string id, int newStock)
    {
        ReplaceInBag(_products, p => p.Id == id, p => p with { Stock = newStock });
        OnProductChanged?.Invoke();
    }

    public IEnumerable<ProductEntry> GetAllProducts() =>
        _products.OrderByDescending(p => p.CreatedAt);

    public IEnumerable<ProductEntry> GetLowStock(int threshold = 5) =>
        _products.Where(p => p.Stock <= threshold);

    public IEnumerable<string> GetAllCategories() =>
        _products.Select(p => p.Category).Distinct().OrderBy(c => c);

    public IEnumerable<string> GetAllTags() =>
        _products.SelectMany(p => p.Tags)
                 .Concat(_orders.SelectMany(_ => Array.Empty<string>()))
                 .Distinct(StringComparer.OrdinalIgnoreCase)
                 .OrderBy(t => t);

    // ── Orders ───────────────────────────────────────────────────────
    public void AddOrder(string customerName, string customerEmail,
                         string productId, string productName, int quantity, decimal total)
    {
        _orders.Add(new(Id(), customerName, customerEmail, productId, productName, quantity, total, "Processing", DateTime.Now));
        OnOrderChanged?.Invoke();
    }

    public void UpdateOrderStatus(string id, string status)
    {
        ReplaceInBag(_orders, o => o.Id == id, o => o with { Status = status });
        OnOrderChanged?.Invoke();
    }

    public IEnumerable<OrderEntry> GetAllOrders() =>
        _orders.OrderByDescending(o => o.CreatedAt);

    public IEnumerable<OrderEntry> GetRecentOrders(int count) =>
        GetAllOrders().Take(count);

    // ── Reviews ──────────────────────────────────────────────────────
    public void AddReview(string productId, string productName, string author, int rating, string comment)
    {
        _reviews.Add(new(Id(), productId, productName, author, rating, comment, DateTime.Now));
        OnReviewChanged?.Invoke();
    }

    public IEnumerable<ReviewEntry> GetAllReviews() =>
        _reviews.OrderByDescending(r => r.CreatedAt);

    public IEnumerable<ReviewEntry> GetReviewsFor(string productId) =>
        _reviews.Where(r => r.ProductId == productId).OrderByDescending(r => r.CreatedAt);

    // ── Dashboard stats ───────────────────────────────────────────────
    public int     TotalProducts     => _products.Count;
    public int     TotalOrders       => _orders.Count;
    public decimal TotalRevenue      => _orders.Sum(o => o.Total);
    public int     PendingOrders     => _orders.Count(o => o is { Status: "Processing" or "Shipped" });
    public double  AverageRating     => _reviews.Any() ? _reviews.Average(r => r.Rating) : 0;
    public int     OrdersThisWeek    => _orders.Count(o => o.CreatedAt >= DateTime.Now.AddDays(-7));

    // ── Helpers ───────────────────────────────────────────────────────
    static string Id() => Guid.NewGuid().ToString();
    static DateTime Ago(int days) => DateTime.Now.AddDays(-days);

    /// <summary>ConcurrentBag doesn't support in-place update; drain and rebuild.</summary>
    static void ReplaceInBag<T>(ConcurrentBag<T> bag, Func<T, bool> match, Func<T, T> update)
    {
        var list = bag.ToList();
        while (bag.TryTake(out _)) { }
        foreach (var item in list)
            bag.Add(match(item) ? update(item) : item);
    }
}
