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
        private readonly Dictionary<NPC, Rectangle?> sourceRectangles = new Dictionary<NPC, Rectangle?>();
        private readonly List<NPC> lids = new List<NPC>();

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
        private NPC backRods;
        private NPC partTendons;
        private NPC upperLid;
        private NPC lowerLid;

        // Mod Type Parts

        private Core modPartCore1;
        private Core modPartCore2;
        private Core modPartCore3;
        private Barrier modUpperBarrier1;
        private Barrier modUpperBarrier2;
        private Barrier modUpperBarrier3;
        private CircleBarrier modUpperBarrier4;
        private Barrier modLowerBarrier1;
        private Barrier modLowerBarrier2;
        private Barrier modLowerBarrier3;
        private CircleBarrier modLowerBarrier4;
        private Barrier modMiddleBarrier1;
        private Barrier modMiddleBarrier2;
        private Barrier modMiddleBarrier3;
        private CircleBarrier modMiddleBarrier4;
        private Torso modPartTorso;
        private CircleBarrier modMiddleBarrier5;
        private Barrier modMiddleBarrier6;
        private Barrier modMiddleBarrier7;
        private Barrier modMiddleBarrier8;
        private BackRods modBackRods;
        private Tendons modPartTendons;
        private UpperLid modUpperLid;
        private LowerLid modLowerLid;

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
                AssignSourceRectangles();
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
                if (!part.active || part.hide) continue;

                SpriteEffects effects = SpriteEffects.None;
                Vector2 drawPosition = part.TopLeft - Main.screenPosition;
                if (part.spriteDirection > 0)
                {
                    effects |= SpriteEffects.FlipHorizontally;
                    if (lids.Contains(part))
                    {
                        effects |= SpriteEffects.FlipVertically;
                        drawPosition += new Vector2(0, -2);
                    }

                    //effects = SpriteEffects.FlipHorizontally;
                }
                spriteBatch.Draw(Main.npcTexture[part.type], drawPosition, sourceRectangles[part],
                                 Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
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

        private NPC CreatePart<T>(ref T modNPC) where T : Part
        {
            NPC newPart = NewNPCDirect(npc.position.X, npc.position.Y, ModContent.NPCType<T>(), ai0: npc.whoAmI);
            parts.Add(newPart);
            modNPC = newPart.modNPC as T;
            return newPart;
        }

        private void AssignParts()
        {
            backRods = CreatePart(ref modBackRods);
            partTendons = CreatePart(ref modPartTendons);
            middleBarrier1 = CreatePart(ref modMiddleBarrier1);
            middleBarrier2 = CreatePart(ref modMiddleBarrier2);
            middleBarrier3 = CreatePart(ref modMiddleBarrier3);
            middleBarrier4 = CreatePart(ref modMiddleBarrier4);
            middleBarrier5 = CreatePart(ref modMiddleBarrier5);
            middleBarrier6 = CreatePart(ref modMiddleBarrier6);
            middleBarrier7 = CreatePart(ref modMiddleBarrier7);
            middleBarrier8 = CreatePart(ref modMiddleBarrier8);
            partTorso = CreatePart(ref modPartTorso);
            upperBarrier1 = CreatePart(ref modUpperBarrier1);
            upperBarrier2 = CreatePart(ref modUpperBarrier2);
            upperBarrier3 = CreatePart(ref modUpperBarrier3);
            upperBarrier4 = CreatePart(ref modUpperBarrier4);
            lowerBarrier1 = CreatePart(ref modLowerBarrier1);
            lowerBarrier2 = CreatePart(ref modLowerBarrier2);
            lowerBarrier3 = CreatePart(ref modLowerBarrier3);
            lowerBarrier4 = CreatePart(ref modLowerBarrier4);
            partCore1 = CreatePart(ref modPartCore1);
            partCore2 = CreatePart(ref modPartCore2);
            partCore3 = CreatePart(ref modPartCore3);
            upperLid = CreatePart(ref modUpperLid);
            lowerLid = CreatePart(ref modLowerLid);

            lids.Add(upperLid);
            lids.Add(lowerLid);
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
            anchors[backRods] = new Vector2(196, 98);
            anchors[partTendons] = new Vector2(72, 196);
            anchors[upperLid] = new Vector2(103, 68);
            anchors[lowerLid] = new Vector2(103, 142);
        }

        private void AssignSourceRectangles()
        {
            sourceRectangles[partCore1] = new Rectangle(0, 0, 24, 26);
            sourceRectangles[partCore2] = new Rectangle(0, 0, 24, 26);
            sourceRectangles[partCore3] = new Rectangle(0, 0, 24, 26);
            sourceRectangles[upperBarrier1] = null;
            sourceRectangles[upperBarrier2] = null;
            sourceRectangles[upperBarrier3] = null;
            sourceRectangles[upperBarrier4] = null;
            sourceRectangles[lowerBarrier1] = null;
            sourceRectangles[lowerBarrier2] = null;
            sourceRectangles[lowerBarrier3] = null;
            sourceRectangles[lowerBarrier4] = null;
            sourceRectangles[middleBarrier1] = null;
            sourceRectangles[middleBarrier2] = null;
            sourceRectangles[middleBarrier3] = null;
            sourceRectangles[middleBarrier4] = null;
            sourceRectangles[partTorso] = null;
            sourceRectangles[middleBarrier5] = null;
            sourceRectangles[middleBarrier6] = null;
            sourceRectangles[middleBarrier7] = null;
            sourceRectangles[middleBarrier8] = null;
            sourceRectangles[backRods] = null;
            sourceRectangles[partTendons] = null;
            sourceRectangles[upperLid] = null;
            sourceRectangles[lowerLid] = null;
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
            sourceRectangles[partCore1] = new Rectangle(0, modPartCore1.CurrentFrame * 26, 24, 26);
            sourceRectangles[partCore2] = new Rectangle(0, modPartCore2.CurrentFrame * 26, 24, 26);
            sourceRectangles[partCore3] = new Rectangle(0, modPartCore3.CurrentFrame * 26, 24, 26);
        }
    }
}