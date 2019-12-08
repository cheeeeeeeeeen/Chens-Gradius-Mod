using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Accessories.Options.Search
{
  public abstract class SearchOptionBase : OptionBase
  {
    public override OptionTypes OptionType => OptionTypes.Search;

    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(9, 6));
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

    protected override void UpgradeUsualStations(ModRecipe recipe)
    {
      recipe.AddTile(TileID.HeavyWorkBench);
    }
  }
}