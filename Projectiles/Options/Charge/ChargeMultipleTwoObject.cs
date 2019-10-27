using ChensGradiusMod.Items.Accessories.Options.Charge;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
  public class ChargeMultipleTwoObject : ChargeMultipleBaseObject
  {
    public override int Position => 2;

    public override bool PlayerHasAccessory() => ModOwner.optionTwo;

    protected override int OptionAccessoryType => ModContent.ItemType<ChargeMultipleTwo>();
  }
}
