using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Bags
{
    public abstract class BaseBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.rare = ItemRarityID.Expert;
            item.expert = true;
            AssignItemDimensions(item, 32, 32, false);
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            throw new System.NotImplementedException("OpenBossBag is not implemented.");
        }

        public override int BossBagNPC => throw new System.NotImplementedException("BossBagNPC is not implemented.");
    }
}