using ChensGradiusMod.Items.Placeables.MusicBoxes;
using ChensGradiusMod.Items.Weapons.Magic;
using ChensGradiusMod.Items.Weapons.Melee;
using ChensGradiusMod.Items.Weapons.Ranged;
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
                Tuple.Create(ModContent.ItemType<AircraftCarrierMusicBox>(), 1d),
                Tuple.Create(0, 4d)
            ));
            AddDropTable(player, new WeightedRandom<int>(Main.rand.Next(),
                Tuple.Create(ModContent.ItemType<MiniCoveredCoreWeapon>(), 1d),
                Tuple.Create(ModContent.ItemType<ZalkYoyo>(), 1d),
                Tuple.Create(ModContent.ItemType<Death2Weapon>(), 1d),
                Tuple.Create(ModContent.ItemType<GarunLauncher>(), 1d)
            ));
        }

        public override int BossBagNPC => ModContent.NPCType<BigCoreCustom>();
    }
}