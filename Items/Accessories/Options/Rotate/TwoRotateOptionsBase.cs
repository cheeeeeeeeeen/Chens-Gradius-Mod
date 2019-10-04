using Terraria;

namespace ChensGradiusMod.Items.Accessories.Options.Rotate
{
  public abstract class TwoRotateOptionsBase : RotateOptionBase
  {
    public override void SetStaticDefaults() => Tooltip.SetDefault(OptionTooltip);

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 38;
      item.height = 30;
    }

    public override string Texture => "ChensGradiusMod/Sprites/TwoOptions";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      for (int i = 0; i < 2; i++)
      {
        CreateOption(player, OptionPosition[i], ProjectileName[i]);
        CreationOrderingBypass(player, OptionPosition[i]);
      }
    }

    protected new virtual string[] ProjectileName { get; } = { "1", "2" };

    protected new virtual int[] OptionPosition { get; } = { 1, 2 };

    protected override string OptionTooltip =>
     "Deploys two Options type Rotate.\n" +
     "Some projectiles you create are copied by the drones.\n" +
     "The drones will follow your flight path.\n" +
     "Hold the Option Action Key to have the drones revolve you!\n";
  }
}
