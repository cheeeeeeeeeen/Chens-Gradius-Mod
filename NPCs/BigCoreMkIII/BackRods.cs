using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public class BackRods : Part
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Core Mk. III");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.width = 24;
            npc.height = 60;
            npc.lifeMax = 1000000;
            npc.hide = true;
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 62, 1);
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/BackRods";

        protected override PartTypes CurrentType => PartTypes.Invisible;
    }
}