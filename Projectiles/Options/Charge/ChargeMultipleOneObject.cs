using ChensGradiusMod.Items.Accessories.Options.Charge;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
  public class ChargeMultipleOneObject : ChargeMultipleBaseObject
  {
    public override int Position => 1;

    public override bool PlayerHasAccessory() => ModOwner.optionOne;

    protected override int OptionAccessoryType => ModContent.ItemType<ChargeMultipleOne>();
  }
}