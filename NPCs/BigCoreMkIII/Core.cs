using Terraria;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public class Core : Part
    {
        public States currentState = States.Closed;

        private const int TimeToOpen = 600;

        private int openTimeTick = 0;

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
            npc.lifeMax = 20;
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 156, 6);
            ScaleStats();
        }

        public override void AI()
        {
            switch (currentState)
            {
                case States.Closed:
                    if (++openTimeTick >= TimeToOpen) currentState = States.Opening;
                    break;

                case States.Opening:
                    if (++FrameTick >= FrameSpeed)
                    {
                        FrameTick = 0;
                        if (++FrameCounter >= 4) currentState = States.Open;
                    }
                    break;

                case States.Open:
                    break;
            }
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/Core";

        protected override int FrameSpeed => 10;

        public enum States : byte
        {
            Closed,
            Opening,
            Open
        }
    }
}