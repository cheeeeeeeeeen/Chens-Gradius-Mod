using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Accessories.Options.Recurve
{
  public abstract class RecurveOptionBase : OptionBase
  {
    public const float AdjustSpeed = 4f;
    public const float FixedAxisDistance = 32f;
    public const float LeastAdjustment = 8f;
    public const float CapAdjustment = 240f;

    public override OptionTypes OptionType => OptionTypes.Recurve;

    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 5));
    }

    public override void PostUpdate()
    {
      Lighting.AddLight(item.Center, 0f, 1f, 1f);
    }

    protected override string ProjectileType => "Recurve";

    protected override string OptionTooltip =>
      "Deploys an Option type Recurve.\n" +
      "Some projectiles you create are copied by the drone.\n" +
      "The drone will stay fixed in position depending on where your cursor is.\n" +
      "Hold the Option Action Key to spread or suppress its distance from you!";

    protected override void UpgradeUsualStations(ModRecipe recipe)
    {
      recipe.AddTile(TileID.SkyMill);
    }
  }
}