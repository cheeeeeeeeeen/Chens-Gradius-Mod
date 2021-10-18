using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public class CircleBarrier : Part
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrier");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.width = 8;
            npc.height = 8;
            npc.lifeMax = 1;
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 10, 1);
            ScaleStats();
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/Barrier2";
    }
}