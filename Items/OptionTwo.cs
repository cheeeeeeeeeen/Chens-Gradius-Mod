using Terraria;

namespace ChensGradiusMod.Items
{
  public class OptionTwo : OptionBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Option (Second)");
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionTwo = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot) => player.GetModPlayer<GradiusModPlayer>().optionOne;

    public override string ProjectileName => "OptionTwoObject";
  }
}
