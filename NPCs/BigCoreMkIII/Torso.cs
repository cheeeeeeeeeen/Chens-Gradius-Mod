using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public class Torso : Part
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Core");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.width = 124;
            npc.height = 32;
            npc.lifeMax = 1;
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 34, 1);
            ScaleStats();
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/Torso";
    }
}