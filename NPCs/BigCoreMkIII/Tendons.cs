using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public class Tendons : Part
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Core Mk. III");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.width = 48;
            npc.height = 256;
            npc.lifeMax = 1000000;
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 258, 1);
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/Tendons";

        public override bool? CanBeHitByItem(Player player, Item item) => false;

        public override bool? CanBeHitByProjectile(Projectile projectile) => false;
    }
}