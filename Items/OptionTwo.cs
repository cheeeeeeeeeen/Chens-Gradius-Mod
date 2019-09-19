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
      GradiusModPlayer modPlayer = player.GetModPlayer<GradiusModPlayer>();
      modPlayer.optionTwo = true;

      base.UpdateAccessory(player, hideVisual);

      // if (IsOptionDeployed(player)) modPlayer.optionTwoIndex = spawnedProjectileIndex;
    }

    public override bool CanEquipAccessory(Player player, int slot) => player.GetModPlayer<GradiusModPlayer>().optionOne;

    public override string ProjectileName => "OptionTwoObject";
  }
}
