using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public class Torso : Part
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Core Mk. III");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.width = 124;
            npc.height = 32;
            npc.lifeMax = 1000000;
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 34, 1);
        }

        public override void AI()
        {
            base.AI();

            if (!ModParent.partCore1.active && !ModParent.partCore2.active) SelfDestruct();
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/Torso";

        protected override PartTypes CurrentType => PartTypes.Invisible;

        protected override int SelfDestructTime => 60;
    }
}