using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod.NPCs.BigCoreMkIII
{
    [AutoloadBossHead]
    public class BigCoreMkIII : GradiusEnemy
    {
        private bool initialized = false;
        private readonly Dictionary<NPC, Vector2> anchors = new Dictionary<NPC, Vector2>();
        private readonly List<NPC> parts = new List<NPC>();
        private readonly Dictionary<NPC, Rectangle> sourceRectangles = new Dictionary<NPC, Rectangle>();

        // Parts

        private NPC partCore1;
        private NPC partCore2;
        private NPC partCore3;
        private NPC upperBarrier1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Core Mk. III");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            npc.width = 206;
            npc.height = 136;
            npc.damage = 200;
            npc.lifeMax = 1000000;
            npc.value = Item.sellPrice(gold: 50);
            npc.knockBackResist = 1000f;
            npc.defense = 0;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.npcSlots = 1;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/DarkForce");
            //bannerItem = ModContent.ItemType<BigCoreCustomBanner>();
            //bossBag = ModContent.ItemType<BigCoreCustomBag>();
            ComputeCenterFromHitbox(npc, ref drawOffsetY, 138, 1);

            ScaleStats();
        }

        public override string Texture => "ChensGradiusMod/Sprites/BigCore3/MainBody";

        public override string BossHeadTexture => "ChensGradiusMod/Sprites/BigCore3/BossHead";

        public override bool PreAI()
        {
            if (!initialized)
            {
                partCore1 = NewNPCDirect(npc.position.X, npc.position.Y, ModContent.NPCType<Core>(), ai0: npc.whoAmI);
                partCore2 = NewNPCDirect(npc.position.X, npc.position.Y, ModContent.NPCType<Core>(), ai0: npc.whoAmI);
                partCore3 = NewNPCDirect(npc.position.X, npc.position.Y, ModContent.NPCType<Core>(), ai0: npc.whoAmI);
                upperBarrier1 = NewNPCDirect(npc.position.X, npc.position.Y, ModContent.NPCType<Barrier>(), ai0: npc.whoAmI);
                AssignParts();
                AssignAnchors();
                initialized = true;
            }
            return initialized;
        }

        public override void AI()
        {
            // Update positions before this
            npc.spriteDirection = 1;
            AlignParts();
            UpdateSourceRectangles();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            foreach (var part in parts)
            {
                if (part.active)
                {
                    SpriteEffects effects = part.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    spriteBatch.Draw(Main.npcTexture[part.type], part.TopLeft - Main.screenPosition, sourceRectangles[part],
                                     Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            damage = 0;
            knockback = 0;
            crit = false;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = 0;
            knockback = 0;
            crit = false;
        }

        public override bool? CanBeHitByItem(Player player, Item item) => false;

        public override bool? CanBeHitByProjectile(Projectile projectile) => false;

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f;

        protected override Types EnemyType => Types.Boss;

        private void AssignParts()
        {
            parts.Add(partCore1);
            parts.Add(partCore2);
            parts.Add(partCore3);
            parts.Add(upperBarrier1);
        }

        private void AssignAnchors()
        {
            anchors[partCore1] = new Vector2(64, 64);
            anchors[partCore2] = new Vector2(64, 96);
            anchors[partCore3] = new Vector2(92, 80);
            anchors[upperBarrier1] = new Vector2(14, 58);
        }

        private void AlignParts()
        {
            foreach (var part in parts)
            {
                if (npc.spriteDirection < 0) part.Bottom = npc.position + anchors[part];
                else part.Top = npc.BottomRight - anchors[part];
            }
        }

        private void UpdateSourceRectangles()
        {
            sourceRectangles[partCore1] = new Rectangle(0, 0, 24, 26);
            sourceRectangles[partCore2] = new Rectangle(0, 0, 24, 26);
            sourceRectangles[partCore3] = new Rectangle(0, 0, 24, 26);
            sourceRectangles[upperBarrier1] = new Rectangle(0, 0, 4, 14);
        }
    }
}