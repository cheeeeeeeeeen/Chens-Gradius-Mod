using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public class Barrier : Part
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrier");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.width = 4;
            npc.height = 12;
            npc.lifeMax = 20;
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 14, 1);
            ScaleStats();
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/Barrier";
    }
}