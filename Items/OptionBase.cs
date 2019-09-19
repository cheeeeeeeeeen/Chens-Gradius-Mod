using Terraria;

namespace ChensGradiusMod.Items
{
  public abstract class OptionBase : ParentGradiusAccessory
  {
    // private readonly string optionTexture = "ChensGradiusMod/Items/OptionBase";

    public int spawnedProjectileIndex;

    public override void SetStaticDefaults()
    {
      Tooltip.SetDefault("Deploys an Option.");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      // item.width = ;
      // item.height = ;
    }

    // public override string Texture => optionTexture;

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      if (IsOptionDeployed(player))
      {
        spawnedProjectileIndex = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f,
                                 mod.ProjectileType(ProjectileName), 0, 0f,
                                 player.whoAmI, 0f, 0f);
      }
    }

    public bool IsOptionDeployed(Player player)
    {
      return player.ownedProjectileCounts[mod.ProjectileType(ProjectileName)] <= 0 &&
             player.whoAmI == Main.myPlayer;
    }

    public virtual string ProjectileName => "OptionOneObject";
  }
}
