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

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      GradiusModPlayer modPlayer = player.GetModPlayer<GradiusModPlayer>();
      modPlayer.optionOne = true;

      base.UpdateAccessory(player, hideVisual);

      // if (IsOptionDeployed(player)) modPlayer.optionOneIndex = spawnedProjectileIndex;
    }
  }
}
