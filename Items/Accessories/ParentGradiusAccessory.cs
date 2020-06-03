using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChensGradiusMod.Items.Accessories
{
  public abstract class ParentGradiusAccessory : ModItem
  {
    public override void SetDefaults()
    {
      item.accessory = true;
      item.width = 64;
      item.height = 64;
      item.rare = ItemRarityID.White; // 0
    }

    public override string Texture => "ChensGradiusMod/Sprites/placeholder";

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }

    protected GradiusModPlayer ModPlayer(Player p) => p.GetModPlayer<GradiusModPlayer>();
  }
}