using Terraria;

namespace ChensGradiusMod.Items.Accessories.Options.Charge
{
  public class ChargeMultipleTwo : ChargeMultipleBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Charge Multiple (Second)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 4;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionTwo = true;
      ModPlayer(player).chargeMultiple = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return base.CanEquipAccessory(player, slot) &&
             player.GetModPlayer<GradiusModPlayer>().chargeMultiple &&
             player.GetModPlayer<GradiusModPlayer>().optionOne;
    }

    protected override string ProjectileName => "MultipleTwoObject";

    protected override int OptionPosition => 2;

    public override void AddRecipes()
    {
      
    }
  }
}
