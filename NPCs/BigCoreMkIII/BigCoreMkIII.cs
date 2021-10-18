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
        private NPC upperBarrier2;
        private NPC upperBarrier3;
        private NPC upperBarrier4;
        private NPC lowerBarrier1;
        private NPC lowerBarrier2;
        private NPC lowerBarrier3;
        private NPC lowerBarrier4;
        private NPC middleBarrier1;
        private NPC middleBarrier2;
        private NPC middleBarrier3;
        private NPC middleBarrier4;
        private NPC partTorso;
        private NPC middleBarrier5;
        private NPC middleBarrier6;
        private NPC middleBarrier7;
        private NPC middleBarrier8;

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
                AssignParts();
                AssignAnchors();
                initialized = true;
            }
            return initialized;
        }

        public override void AI()
        {
            // Update positions before this
            npc.spriteDirection = -1;
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

        private NPC CreatePart<T>() where T : Part
        {
            NPC newPart = NewNPCDirect(npc.position.X, npc.position.Y, ModContent.NPCType<T>(), ai0: npc.whoAmI);
            parts.Add(newPart);
            return newPart;
        }

        private void AssignParts()
        {
            middleBarrier1 = CreatePart<Barrier>();
            middleBarrier2 = CreatePart<Barrier>();
            middleBarrier3 = CreatePart<Barrier>();
            middleBarrier4 = CreatePart<CircleBarrier>();
            middleBarrier5 = CreatePart<CircleBarrier>();
            middleBarrier6 = CreatePart<Barrier>();
            middleBarrier7 = CreatePart<Barrier>();
            middleBarrier8 = CreatePart<Barrier>();
            partTorso = CreatePart<Torso>();
            upperBarrier1 = CreatePart<Barrier>();
            upperBarrier2 = CreatePart<Barrier>();
            upperBarrier3 = CreatePart<Barrier>();
            upperBarrier4 = CreatePart<CircleBarrier>();
            lowerBarrier1 = CreatePart<Barrier>();
            lowerBarrier2 = CreatePart<Barrier>();
            lowerBarrier3 = CreatePart<Barrier>();
            lowerBarrier4 = CreatePart<CircleBarrier>();
            partCore1 = CreatePart<Core>();
            partCore2 = CreatePart<Core>();
            partCore3 = CreatePart<Core>();
        }

        private void AssignAnchors()
        {
            anchors[partCore1] = new Vector2(64, 64);
            anchors[partCore2] = new Vector2(64, 96);
            anchors[partCore3] = new Vector2(92, 80);
            anchors[upperBarrier1] = new Vector2(14, 58);
            anchors[upperBarrier2] = new Vector2(22, 58);
            anchors[upperBarrier3] = new Vector2(30, 58);
            anchors[upperBarrier4] = new Vector2(46, 56);
            anchors[lowerBarrier1] = new Vector2(14, 90);
            anchors[lowerBarrier2] = new Vector2(22, 90);
            anchors[lowerBarrier3] = new Vector2(30, 90);
            anchors[lowerBarrier4] = new Vector2(46, 88);
            anchors[middleBarrier1] = new Vector2(18, 74);
            anchors[middleBarrier2] = new Vector2(30, 74);
            anchors[middleBarrier3] = new Vector2(42, 74);
            anchors[middleBarrier4] = new Vector2(70, 72);
            anchors[partTorso] = new Vector2(18, 84);
            anchors[middleBarrier5] = new Vector2(110, 72);
            anchors[middleBarrier6] = new Vector2(122, 74);
            anchors[middleBarrier7] = new Vector2(132, 74);
            anchors[middleBarrier8] = new Vector2(142, 74);
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
            sourceRectangles[upperBarrier2] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[upperBarrier3] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[upperBarrier4] = new Rectangle(0, 0, 8, 10);
            sourceRectangles[lowerBarrier1] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[lowerBarrier2] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[lowerBarrier3] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[lowerBarrier4] = new Rectangle(0, 0, 8, 10);
            sourceRectangles[middleBarrier1] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[middleBarrier2] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[middleBarrier3] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[middleBarrier4] = new Rectangle(0, 0, 8, 10);
            sourceRectangles[partTorso] = new Rectangle(0, 0, 124, 34);
            sourceRectangles[middleBarrier5] = new Rectangle(0, 0, 8, 10);
            sourceRectangles[middleBarrier6] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[middleBarrier7] = new Rectangle(0, 0, 4, 14);
            sourceRectangles[middleBarrier8] = new Rectangle(0, 0, 4, 14);
        }
    }
}