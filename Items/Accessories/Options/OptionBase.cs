using Terraria;
using Terraria.DataStructures;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public abstract class OptionBase : ParentGradiusAccessory
  {
    public override void SetStaticDefaults()
    {
      Tooltip.SetDefault(OptionTooltip);
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
      CreateOption(player, hideVisual, OptionPosition, ProjectileName);
      CreationOrderingBypass(player, hideVisual);
    }

    protected virtual string ProjectileName => "OptionObject";

    protected virtual int OptionPosition => 0;

    protected virtual string OptionTooltip =>
      "Deploys an Option.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "This advanced drone uses Wreek technology,\n" +
      "infusing both technology and psychic elements together.";

    protected virtual bool ModeChecks(Player player, bool hideVisual)
    {
      return !ModPlayer(player).freezeOption &&
             !ModPlayer(player).rotateOption;
    }

    private bool IsOptionNotDeployed(Player player, string projectileName)
    {
      return player.ownedProjectileCounts[mod.ProjectileType(projectileName)] <= 0 &&
             GradiusHelper.IsSameClientOwner(player);
    }

    private void CreationOrderingBypass(Player player, bool hideVisual)
    {
      switch (OptionPosition)
      {
        case 1:
          CreateOption(player, hideVisual, 2, "OptionTwoObject");
          goto case 2;
        case 2:
          CreateOption(player, hideVisual, 3, "OptionThreeObject");
          goto case 3;
        case 3:
          CreateOption(player, hideVisual, 4, "OptionFourObject");
          goto case 4;
        case 4:
          break;
      }
    }

    private void CreateOption(Player player, bool hideVisual, int optionPosition, string projectileName)
    {
      if (GradiusHelper.OptionsPredecessorRequirement(ModPlayer(player), optionPosition) &&
          ModeChecks(player, hideVisual) && IsOptionNotDeployed(player, projectileName))
      {
        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f,
                                 mod.ProjectileType(projectileName), 0, 0f,
                                 player.whoAmI, 0f, 0f);
      }
    }
  }
}
