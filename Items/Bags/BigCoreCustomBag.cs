using ChensGradiusMod.Items.Placeables.MusicBoxes;
using ChensGradiusMod.Items.Weapons.Summon;
using ChensGradiusMod.NPCs;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.Items.Bags
{
    public class BigCoreCustomBag : BaseBag
    {
        public override string Texture => "ChensGradiusMod/Sprites/BigCoreTreasureBag";

        public override void OpenBossBag(Player player)
        {
            AddDropTable(player, new WeightedRandom<int>(Main.rand.Next(),
                Tuple.Create(ModContent.ItemType<AircraftCarrierMusicBox>(), 20d),
                Tuple.Create(0, 80d)
            ));
            AddDropTable(player, new WeightedRandom<int>(Main.rand.Next(),
                Tuple.Create(ModContent.ItemType<MiniCoveredCoreWeapon>(), 20d),
                Tuple.Create(0, 82d)
            ));
        }

        public override int BossBagNPC => ModContent.NPCType<BigCoreCustom>();
    }
}