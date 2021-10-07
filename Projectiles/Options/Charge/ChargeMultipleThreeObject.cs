using ChensGradiusMod.Items.Accessories.Options.Charge;
using Terraria.ModLoader;

namespace ChensGradiusMod.Projectiles.Options.Charge
{
    public class ChargeMultipleThreeObject : ChargeMultipleBaseObject
    {
        public override int Position => 3;

        public override bool PlayerHasAccessory() => ModOwner.optionThree;

        protected override int OptionAccessoryType => ModContent.ItemType<ChargeMultipleThree>();
    }
}