using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public abstract class OptionBase : ParentGradiusAccessory
  {
    public override void SetStaticDefaults()
    {
      Tooltip.SetDefault(OptionTooltip);
      ItemID.Sets.ItemNoGravity[item.type] = true;
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 5));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();
      item.width = 44;
      item.height = 52;
    }

    public override string Texture => $"ChensGradiusMod/Sprites/OptionInv{OptionPosition}";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      CreateOption(player, OptionPosition, ProjectileType + ProjectileName);
      CreationOrderingBypass(player, OptionPosition);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return ModeChecks(player, true);
    }

    protected virtual string ProjectileType => "";

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
      return ModPlayer(player).normalOption &&
             !ModPlayer(player).freezeOption &&
             !ModPlayer(player).rotateOption;
    }

    protected void CreateOption(Player player, int optionPosition, string projectileName)
    {
      if (GradiusHelper.OptionsPredecessorRequirement(ModPlayer(player), optionPosition) &&
          ModeChecks(player, false) && IsOptionNotDeployed(player, projectileName))
      {
        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f,
                                 mod.ProjectileType(projectileName), 0, 0f,
                                 player.whoAmI, 0f, 0f);
      }
    }

    protected void CreationOrderingBypass(Player player, int position)
    {
      switch (position)
      {
        case 1:
          CreateOption(player, 2, ProjectileType + "OptionTwoObject");
          goto case 2;
        case 2:
          CreateOption(player, 3, ProjectileType + "OptionThreeObject");
          goto case 3;
        case 3:
          CreateOption(player, 4, ProjectileType + "OptionFourObject");
          goto case 4;
        case 4:
          break;
      }
    }

    private bool IsOptionNotDeployed(Player player, string projectileName)
    {
      return player.ownedProjectileCounts[mod.ProjectileType(projectileName)] <= 0 &&
             GradiusHelper.IsSameClientOwner(player);
    }
  }
}
