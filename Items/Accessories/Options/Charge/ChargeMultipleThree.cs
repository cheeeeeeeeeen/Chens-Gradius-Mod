using Terraria;

namespace ChensGradiusMod.Items.Accessories.Options.Charge
{
  public class ChargeMultipleThree : ChargeMultipleBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Charge Multiple (Third)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 6;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionThree = true;
      ModPlayer(player).chargeMultiple = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return base.CanEquipAccessory(player, slot) &&
             player.GetModPlayer<GradiusModPlayer>().chargeMultiple &&
             player.GetModPlayer<GradiusModPlayer>().optionTwo &&
             player.GetModPlayer<GradiusModPlayer>().optionOne;
    }

    protected override string ProjectileName => "MultipleThreeObject";

    protected override int OptionPosition => 3;

    public override void AddRecipes()
    {
      
    }
  }
}
