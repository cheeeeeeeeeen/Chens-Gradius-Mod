using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Charge
{
  public abstract class ChargeMultipleBase : OptionBase
  {
    public enum States : int { Following, Charging, Dying };

    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 5));
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, 0.8667f, 0.6275f, 0.8667f);
    }

    protected override string ProjectileType => "Charge";

    protected override string ProjectileName => "MultipleObject";

    protected override string OptionName => "Multiple";

    protected override string OptionTooltip =>
      "Deploys a Charge Multiple.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to have the drone charge energy!\n" +
      "Release the key to activate the drone's special attack sequence!";

    protected override bool ModeChecks(Player player, bool includeSelf = true)
    {
      bool result = true;
      if (includeSelf) result &= ModPlayer(player).chargeMultiple;
      return result &&
             !ModPlayer(player).rotateOption &&
             !ModPlayer(player).freezeOption &&
             !ModPlayer(player).normalOption &&
             !ModPlayer(player).aimOption &&
             !ModPlayer(player).searchOption;
    }

    protected override void UpgradeUsualStations(ModRecipe recipe)
    {
      recipe.AddTile(TileID.BewitchingTable);
    }
  }
}
