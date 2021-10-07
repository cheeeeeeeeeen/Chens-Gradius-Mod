using Terraria;
using Terraria.ID;

namespace ChensGradiusMod.Items.Accessories.Options
{
    public class OptionThreeFour : TwoOptionsBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Options (3rd & 4th)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.rare = ItemRarityID.Yellow; // 8
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ModPlayer(player).optionThree = true;
            ModPlayer(player).optionFour = true;
            ModPlayer(player).normalOption = true;

            base.UpdateAccessory(player, hideVisual);
        }

        protected override string[] ProjectileName { get; } = { "OptionThreeObject",
                                                            "OptionFourObject" };

        protected override int[] OptionPosition { get; } = { 3, 4 };
    }
}