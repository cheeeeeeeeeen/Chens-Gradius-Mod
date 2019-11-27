using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Freeze
{
  public abstract class FreezeOptionBase : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 6));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 44;
      item.height = 58;
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, 0f, .749f, 1f);
    }

    protected override string ProjectileType => "Freeze";

    protected override string OptionTooltip =>
      "Deploys an Option type Freeze.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to perform a different movement behavior!";

    protected override bool ModeChecks(Player player, bool includeSelf = true)
    {
      bool result = true;
      if (includeSelf) result &= ModPlayer(player).freezeOption;
      return result &&
             !ModPlayer(player).rotateOption &&
             !ModPlayer(player).normalOption &&
             !ModPlayer(player).chargeMultiple &&
             !ModPlayer(player).aimOption &&
             !ModPlayer(player).recurveOption &&
             !ModPlayer(player).searchOption;
    }

    protected override void UpgradeUsualStations(ModRecipe recipe)
    {
      recipe.AddTile(TileID.IceMachine);
    }
  }
}