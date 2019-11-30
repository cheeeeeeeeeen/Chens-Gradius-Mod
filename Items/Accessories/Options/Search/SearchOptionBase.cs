using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChensGradiusMod.Items.Accessories.Options.Search
{
  public abstract class SearchOptionBase : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 5));
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, 1f, .8431f, 0f);
    }

    protected override string ProjectileType => "Search";

    protected override string OptionTooltip =>
      "Deploys an Option type Search.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will follow your flight path.\n" +
      "Hold the Option Action Key to have the Options seek and pursue nearby targets!";

    protected override bool ModeChecks(Player player, bool includeSelf = true)
    {
      bool result = true;
      if (includeSelf) result &= ModPlayer(player).searchOption;
      return result &&
             !ModPlayer(player).rotateOption &&
             !ModPlayer(player).normalOption &&
             !ModPlayer(player).chargeMultiple &&
             !ModPlayer(player).aimOption &&
             !ModPlayer(player).recurveOption &&
             !ModPlayer(player).freezeOption;
    }

    protected override void UpgradeUsualStations(ModRecipe recipe)
    {
      recipe.AddTile(TileID.HeavyWorkBench);
    }
  }
}