using Terraria;
using Terraria.DataStructures;

namespace ChensGradiusMod.Items
{
  public abstract class OptionBase : ParentGradiusAccessory
  {
    public override void SetStaticDefaults()
    {
      Tooltip.SetDefault("Deploys an Option.\n" +
                         "Some projectiles you create are copied by the drone.\n" +
                         "The drone will follow your flight path.\n" +
                         "This advanced drone uses Wreek technology,\n" +
                         "infusing both technology and psychic elements together.");
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 9));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 28;
      item.height = 20;
    }

    public override string Texture => "ChensGradiusMod/Sprites/OptionSheet";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      if (GradiusHelper.OptionsPredecessorRequirement(ModPlayer(player), OptionPosition) &&
          IsOptionNotDeployed(player))
      {
        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f,
                                 mod.ProjectileType(ProjectileName), 0, 0f,
                                 player.whoAmI, 0f, 0f);
      }
    }

    public virtual string ProjectileName => "OptionObject";

    public virtual int OptionPosition => 0;

    private bool IsOptionNotDeployed(Player player)
    {
      return player.ownedProjectileCounts[mod.ProjectileType(ProjectileName)] <= 0 &&
             player.whoAmI == Main.myPlayer;
    }
  }
}
