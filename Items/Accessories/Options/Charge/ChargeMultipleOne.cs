using Terraria;

namespace ChensGradiusMod.Items.Accessories.Options.Charge
{
  public class ChargeMultipleOne : ChargeMultipleBase
  {
    public override void SetStaticDefaults()
    {
      base.SetStaticDefaults();

      DisplayName.SetDefault("Charge Multiple (First)");
    }

    public override void SetDefaults()
    {
      base.SetDefaults();

      item.rare = 3;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
      ModPlayer(player).optionOne = true;
      ModPlayer(player).chargeMultiple = true;

      base.UpdateAccessory(player, hideVisual);
    }

    public override bool CanEquipAccessory(Player player, int slot)
    {
      return !ModPlayer(player).freezeOption &&
             !ModPlayer(player).normalOption &&
             !ModPlayer(player).rotateOption;
    }

    protected override string ProjectileName => "MultipleOneObject";

    protected override int OptionPosition => 1;

    public override void AddRecipes()
    {
      
    }
  }
}
