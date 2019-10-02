using ChensGradiusMod.Projectiles.Forces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Forces
{
  public class NeedleBydo : BydoEmbryo
  {
    public override void SetStaticDefaults()
    {
      Tooltip.SetDefault("Deploys the Needle Force.\n" +
                         "Damage and Knockback are based on the held weapon.\n" +
                         "Inherited Damage and Knockback are only 3/4 of the actual values.\n" +
                         "Any enemy projectile that comes in contact are destroyed.\n" +
                         "Press the Force Action hotkey to launch or pull it!\n" +
                         "Modified Force to enhance offensive capabilties.");
    }

    public override string Texture => "ChensGradiusMod/Sprites/NeedleForceAccessory";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).needleForce = true;
      DeployForce(player);
    }

    public override void AddRecipes()
    {
      ModRecipe recipe = new ModRecipe(mod);
      recipe.AddIngredient(ItemID.Wire, 200);
      recipe.AddIngredient(ItemID.Spike, 30);
      recipe.AddIngredient(ItemID.SpikyBall, 200);
      recipe.AddRecipeGroup("ChensGradiusMod:EvilStones", 40);
      recipe.AddRecipeGroup("ChensGradiusMod:EvilDrops", 10);
      recipe.AddTile(TileID.DemonAltar);
      recipe.SetResult(this);
      recipe.AddRecipe();
    }
    protected override int ThisProjectileType() => mod.ProjectileType<NeedleForce>();
  }
}
