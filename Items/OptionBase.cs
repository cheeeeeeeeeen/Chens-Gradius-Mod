using Terraria;
using Terraria.DataStructures;

namespace ChensGradiusMod.Items
{
  public abstract class OptionBase : ParentGradiusAccessory
  {
    private readonly string optionTexture = "ChensGradiusMod/Sprites/OptionSheet";

    public GradiusModPlayer ModPlayer(Player p) => p.GetModPlayer<GradiusModPlayer>();

    public override void SetStaticDefaults()
    {
      Tooltip.SetDefault("Deploys an Option.");
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 9));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 28;
      item.height = 20;
    }

    public override string Texture => optionTexture;

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
