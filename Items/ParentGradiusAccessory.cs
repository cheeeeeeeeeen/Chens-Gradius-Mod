using Terraria.ModLoader;

namespace ChensGradiusMod.Items
{
  public abstract class ParentGradiusAccessory : ModItem
  {
    private readonly string placeHolderTexture = "ChensGradiusMod/Items/placeholder";

    protected string RealItemTexture => base.Texture;

    public override void SetDefaults()
    {
      item.accessory = true;
      item.width = 128;
      item.height = 128;
      item.rare = 0;
    }

    public override string Texture => placeHolderTexture;

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
