using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories
{
  public abstract class ParentGradiusAccessory : ModItem
  {
    protected string RealItemTexture => base.Texture;

    protected GradiusModPlayer ModPlayer(Player p) => p.GetModPlayer<GradiusModPlayer>();

    public override void SetDefaults()
    {
      item.accessory = true;
      item.width = 128;
      item.height = 128;
      item.rare = 0;
    }

    public override string Texture => "ChensGradiusMod/Items/placeholder";

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
  }
}
