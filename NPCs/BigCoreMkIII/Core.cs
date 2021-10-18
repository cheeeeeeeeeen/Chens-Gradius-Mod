using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public class Core : Part
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Core");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.width = 24;
            npc.height = 24;
            npc.lifeMax = 4000;
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 156, 6);
            ScaleStats();
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/Core";
    }
}