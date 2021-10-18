using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    public abstract class Part : GradiusEnemy
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.damage = 200;
            npc.value = 0;
            npc.knockBackResist = 1000f;
            npc.defense = 100;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.npcSlots = 0;
        }

        public override string Texture => throw new NotImplementedException("Please add texture to this part of Big Core Mk. III");

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

        public override void AI()
        {
            npc.velocity = Vector2.Zero;
            npc.spriteDirection = Parent.spriteDirection;
        }

        public override void FindFrame(int frameHeight)
        {
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;

        protected override Types EnemyType => Types.Large;

        protected NPC Parent => Main.npc[(int)npc.ai[0]];
    }
}