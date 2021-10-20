using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public class LowerLid : Part
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Core Mk. III");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.width = 274;
            npc.height = 74;
            npc.lifeMax = 1000000;
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 76, 1);
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/LowerLid";

        protected override PartTypes CurrentType => PartTypes.Invulnerable;
    }
}