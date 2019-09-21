using Terraria;

namespace ChensGradiusMod.Items
{
  public class OptionOne : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (First)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 2;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionOne = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override string ProjectileName => "OptionOneObject";

    public override int OptionPosition => 1;
  }
}
