using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

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
            banner = 0;
        }

        public override string Texture => throw new NotImplementedException("Please add texture to this part of Big Core Mk. III");

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

        public override void AI()
        {
            if (!Parent.active && Parent.life <= 0) SelfDestruct();
            npc.velocity = Vector2.Zero;
            npc.spriteDirection = Parent.spriteDirection;
        }

        public override void FindFrame(int frameHeight)
        {
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (CurrentType == PartTypes.Invisible || (CurrentType == PartTypes.Normal && Prerequisite != null && Prerequisite.active)) return false;
            else if (CurrentType == PartTypes.Invulnerable || (CurrentType == PartTypes.Normal && (Prerequisite == null || !Prerequisite.active))) return null;

            return null;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (CurrentType == PartTypes.Invisible || (CurrentType == PartTypes.Normal && Prerequisite != null && Prerequisite.active)) return false;
            else if (CurrentType == PartTypes.Invulnerable || (CurrentType == PartTypes.Normal && (Prerequisite == null || !Prerequisite.active))) return null;

            return null;
        }

        public int CurrentFrame => FrameCounter;

        public NPC Prerequisite { get; set; } = null;

        protected virtual PartTypes CurrentType => PartTypes.Normal;

        protected virtual int SelfDestructTime => 15;

        protected virtual int SelfDestructTick { get; set; } = 0;

        protected override Types EnemyType => Types.Large;

        protected NPC Parent => Main.npc[(int)npc.ai[0]];

        protected BigCoreMkIII ModParent => Parent.modNPC as BigCoreMkIII;

        protected void SelfDestruct()
        {
            if (++SelfDestructTick >= SelfDestructTime)
            {
                if (npc.active && npc.life > 0) KillNPC(npc);
            }
        }

        public enum PartTypes : byte
        {
            Normal,
            Invulnerable,
            Invisible
        }
    }
}