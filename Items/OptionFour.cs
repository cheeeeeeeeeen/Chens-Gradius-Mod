using Terraria;

namespace ChensGradiusMod.Items
{
  public class OptionFour : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (Fourth)");
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      player.GetModPlayer<GradiusModPlayer>().optionFour = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot) => player.GetModPlayer<GradiusModPlayer>().optionThree;

    public override string ProjectileName => "OptionFourObject";
  }
}
