using Terraria;
using Terraria.DataStructures;

namespace ChensGradiusMod.Items.Accessories.Options
{
  public abstract class TwoOptionsBase : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();
      Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 9));
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.width = 38;
      item.height = 30;
    }

    public override string Texture => $"ChensGradiusMod/Sprites/TwoOptions{OptionPosition[0]}{OptionPosition[1]}";

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      StoreProjectileCounts(player);
      for (int i = 0; i < 2; i++)
      {
        CreateOption(player, OptionPosition[i], ProjectileName[i]);
        CreationOrderingBypass(player, OptionPosition[i]);
      }
      ResetProjectileCounts(player);
    }

    protected new virtual string[] ProjectileName { get; } = { "1", "2" };

    protected new virtual int[] OptionPosition { get; } = { 1, 2 };

    protected override string OptionTooltip =>
      "Deploys two Options.\n" +
      "Some projectiles you create are copied by the drones.\n" +
      "The drones will follow your flight path.\n" +
      "These advanced drones uses Wreek technology,\n" +
      "infusing both technology and psychic elements together.";
  }
}
