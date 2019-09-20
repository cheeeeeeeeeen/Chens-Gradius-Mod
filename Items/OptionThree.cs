using Terraria;

namespace ChensGradiusMod.Items
{
  public class OptionThree : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (Third)");
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionThree = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot) => player.GetModPlayer<GradiusModPlayer>().optionTwo;

    public override string ProjectileName => "OptionThreeObject";
  }
}
