using CueVault.Models;

namespace CueVault.Services;

/// <summary>Category-matched billiards photography used throughout the storefront.</summary>
public static class StoreImages
{
    public const string CueOnTable = "https://thumb.wikimedia.org/wikipedia/commons/thumb/d/d0/Pool_Table_with_Cue_%26_Ball_%2814516530503%29.jpg/1280px-Pool_Table_with_Cue_%26_Ball_%2814516530503%29.jpg";
    public const string PoolBalls = "https://thumb.wikimedia.org/wikipedia/commons/thumb/a/af/Pool_balls_%284854121682%29.jpg/960px-Pool_balls_%284854121682%29.jpg";
    public const string CueBallAndChalk = "https://thumb.wikimedia.org/wikipedia/commons/thumb/a/a0/Pool_Cue_Ball_With_Chalk_%2814309956967%29.jpg/960px-Pool_Cue_Ball_With_Chalk_%2814309956967%29.jpg";
    public const string TriangleRack = "https://thumb.wikimedia.org/wikipedia/commons/thumb/f/f2/Three-ball_rack_in_std_triangle_1.jpg/500px-Three-ball_rack_in_std_triangle_1.jpg";
    public const string CueTip = "https://thumb.wikimedia.org/wikipedia/commons/thumb/2/29/Ferrule_for_snooker_cue.jpg/960px-Ferrule_for_snooker_cue.jpg";

    public static string ForProduct(ProductEntry? product)
    {
        if (product is null)
            return PoolBalls;

        if (HasTag(product, "rack") || product.Name.Contains("rack", StringComparison.OrdinalIgnoreCase))
            return TriangleRack;
        if (HasTag(product, "tip") || product.Name.Contains("tip", StringComparison.OrdinalIgnoreCase))
            return CueTip;
        if (HasTag(product, "chalk") || product.Name.Contains("chalk", StringComparison.OrdinalIgnoreCase) ||
            HasTag(product, "cloth") || HasTag(product, "felt"))
            return CueBallAndChalk;

        return product.Category switch
        {
            "Cues" => CueOnTable,
            "Balls" => PoolBalls,
            _ => CueBallAndChalk
        };
    }

    static bool HasTag(ProductEntry product, string tag) =>
        product.Tags.Any(value => value.Equals(tag, StringComparison.OrdinalIgnoreCase));
}
