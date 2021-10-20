using ChensGradiusMod.Items.Accessories.Forces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using static ChensGradiusMod.ChensGradiusMod;

namespace ChensGradiusMod
{
    public static class GradiusHelper
    {
        public const string InternalModName = "ChensGradiusMod";
        public const float FullAngle = 360f;
        public const float HalfAngle = 180f;
        public const float MinRotate = -180f;
        public const float MaxRotate = 180f;
        public const int LowerAmmoSlot = 54;
        public const int HigherAmmoSlot = 57;
        public const int LowerAccessorySlot = 3;
        public const int HigherAccessorySlot = 9;

        public static readonly Color bigCoreColor = new Color(16, 136, 224);
        public static readonly Color bigCoreStripesColor = new Color(224, 192, 96);

        public static readonly List<int> universalPrefixes = new List<int>()
        {
            PrefixID.Keen, PrefixID.Superior, PrefixID.Forceful, PrefixID.Broken, PrefixID.Damaged, PrefixID.Shoddy,
            PrefixID.Hurtful, PrefixID.Strong, PrefixID.Unpleasant, PrefixID.Weak, PrefixID.Ruthless, PrefixID.Godly,
            PrefixID.Demonic, PrefixID.Zealous
        };

        public static void FreeListData(ref List<int> list)
        {
            int buffer = ModContent.GetInstance<GradiusModConfig>().projectileDuplicationLimit;
            if (list.Count > buffer)
            {
                int bufferExcess = list.Count - buffer;
                for (int j = 0; j < bufferExcess; j++)
                {
                    Main.projectile[list[0]].active = false;
                    list.RemoveAt(0);
                }
            }
        }

        public static bool OptionsPredecessorRequirement(GradiusModPlayer gp, int pos)
        {
            if (pos == 1) return true;
            else if (pos == 2) return gp.optionOne;
            else if (pos == 3) return gp.optionOne && gp.optionTwo;
            else if (pos == 4) return gp.optionOne && gp.optionTwo && gp.optionThree;
            else return false;
        }

        public static bool OptionOwnPositionCheck(GradiusModPlayer gp, int pos)
        {
            if (pos == 1) return gp.optionOne;
            else if (pos == 2) return gp.optionTwo;
            else if (pos == 3) return gp.optionThree;
            else if (pos == 4) return gp.optionFour;
            else return false;
        }

        public static bool OptionCheckSelfAndPredecessors(GradiusModPlayer gp, int pos)
        {
            return OptionsPredecessorRequirement(gp, pos) &&
                   OptionOwnPositionCheck(gp, pos);
        }

        public static Vector2 MoveToward(Vector2 origin, Vector2 destination, float speed = 1)
        {
            //float hypotenuse = Vector2.Distance(origin, destination);
            //float opposite = destination.Y - origin.Y;
            //float adjacent = destination.X - origin.X;

            //float direction = (float)Math.Acos(Math.Abs(adjacent) / hypotenuse);

            //float newX = (float)Math.Cos(direction) * Math.Sign(adjacent) * speed;
            //float newY = -(float)Math.Sin(direction) * -Math.Sign(opposite) * speed;

            //return new Vector2(newX, newY);

            Vector2 velocity = destination - origin;
            velocity.Normalize();
            return velocity * speed;
        }

        public static float GetBearingUpwards(Vector2 origin, Vector2 destination)
        {
            float hypotenuse = Vector2.Distance(origin, destination);
            float opposite = (destination.Y - origin.Y) * -1;
            float adjacent = destination.X - origin.X;

            float direction = (float)Math.Asin(Math.Abs(opposite) / hypotenuse);
            direction = MathHelper.ToDegrees(direction);

            if (adjacent < 0 && opposite < 0)
            {
                direction = HalfAngle + direction;
            }
            else if (adjacent < 0)
            {
                direction = HalfAngle - direction;
            }
            else if (opposite < 0)
            {
                direction = FullAngle - direction;
            }

            return direction;
        }

        public static float GetBearing(Vector2 origin, Vector2 destination, bool degrees = false)
        {
            float angle = MoveToward(origin, destination).ToRotation();
            if (degrees) angle = MathHelper.ToDegrees(angle);
            return angle;
        }

        public static bool CanDamage(Projectile proj) => proj.damage > 0;

        public static bool CanDamage(Item item) => item.damage > 0;

        public static string KnockbackTooltip(float knockback)
        {
            if (knockback <= 0f) return "No knockback";
            else if (knockback <= 1.5f) return "Extremely weak knockback";
            else if (knockback <= 3f) return "Very weak knockback";
            else if (knockback <= 4f) return "Weak knockback";
            else if (knockback <= 6f) return "Average knockback";
            else if (knockback <= 7f) return "Strong knockback";
            else if (knockback <= 11f) return "Extremely strong knockback";
            else return "Insane knockback";
        }

        public static bool IsEqualWithThreshold(Vector2 questionedPoint, Vector2 referencePoint, float threshold)
        {
            return questionedPoint.X <= (referencePoint.X + threshold) &&
                   questionedPoint.Y <= (referencePoint.Y + threshold) &&
                   questionedPoint.X >= (referencePoint.X - threshold) &&
                   questionedPoint.Y >= (referencePoint.Y - threshold);
        }

        public static bool IsEqualWithThreshold(float questionedNumber, float referenceNumber, float threshold)
        {
            return questionedNumber <= (referenceNumber + threshold) &&
                   questionedNumber >= (referenceNumber - threshold);
        }

        public static void NormalizeAngleDegrees(ref float angleDegrees)
        {
            if (angleDegrees >= FullAngle) angleDegrees -= FullAngle;
            else if (angleDegrees < 0) angleDegrees += FullAngle;
        }

        public static bool IsSameClientOwner(Projectile proj) => Main.myPlayer == proj.owner;

        public static bool IsSameClientOwner(Item item) => Main.myPlayer == item.owner;

        public static bool IsSameClientOwner(Player player) => Main.myPlayer == player.whoAmI;

        public static bool IsSameClientOwner(int playerIndex) => Main.myPlayer == playerIndex;

        public static bool IsNotMultiplayerClient() => Main.netMode != NetmodeID.MultiplayerClient;

        public static bool IsMultiplayerClient() => Main.netMode == NetmodeID.MultiplayerClient;

        public static bool IsSinglePlayer() => Main.netMode == NetmodeID.SinglePlayer;

        public static bool IsNotSinglePlayer() => Main.netMode != NetmodeID.SinglePlayer;

        public static bool IsServer() => Main.netMode == NetmodeID.Server;

        public static bool IsBydoAccessory(ModItem modItem)
        {
            return modItem is BydoEmbryo ||
                   modItem is NeedleBydo;
        }

        public static int RoundOffToWhole(float num) => (int)Math.Round(num, 0, MidpointRounding.AwayFromZero);

        public static void FlipAngleDirection(ref float angleDegrees, int direction)
        {
            if (direction < 0)
            {
                angleDegrees += 180f;
                NormalizeAngleDegrees(ref angleDegrees);
            }
        }

        public static int NewNPC(float X, float Y, int Type, int Start = 0, int ai0 = 0,
                                 int ai1 = 0, int ai2 = 0, int ai3 = 0, int Target = 255,
                                 bool center = false)
        {
            int npcIndex = NPC.NewNPC((int)X, (int)Y, Type, Start, ai0, ai1, ai2, ai3, Target);
            if (center) Main.npc[npcIndex].Center = new Vector2(X, Y);
            else Main.npc[npcIndex].Bottom = new Vector2(X, Y);
            Main.npc[npcIndex].netUpdate = true;
            return npcIndex;
        }

        public static NPC NewNPCDirect(float X, float Y, int Type, int Start = 0, int ai0 = 0,
                                       int ai1 = 0, int ai2 = 0, int ai3 = 0, int Target = 255,
                                       bool center = false)
        {
            int npcIndex = NewNPC(X, Y, Type, Start, ai0, ai1, ai2, ai3, Target, center);
            return Main.npc[npcIndex];
        }

        public static int UnderworldTilesYLocation => Main.maxTilesY - 200;

        public static int SkyTilesYLocation => RoundOffToWhole((float)Main.worldSurface * .35f);

        public static float AngularCycle(float value, float min, float max)
        {
            float delta = max - min;
            float result = (value - min) % delta;

            if (result < 0) result += delta;

            return result + min;
        }

        public static float AngularRotate(float currentAngle, float targetAngle,
                                          float min, float max, float speed)
        {
            float diff = AngularCycle(targetAngle - currentAngle, min, max);

            if (diff < -speed) return currentAngle - speed;
            else if (diff > speed) return currentAngle + speed;
            else return targetAngle;
        }

        public static float AngularRotateRadians(float currentAngle, float targetAngle,
                                                 float min, float max, float speed)
        {
            currentAngle = MathHelper.ToDegrees(currentAngle);
            targetAngle = MathHelper.ToDegrees(targetAngle);
            float angleDegrees = AngularRotate(currentAngle, targetAngle, min, max, speed);
            return MathHelper.ToRadians(angleDegrees);
        }

        public static float AngularLerp(Vector2 currentDirection, Vector2 origin, Vector2 destination, float interpolateValue)
        {
            Vector2 desiredDirection = destination - origin;
            desiredDirection.Normalize();
            currentDirection += (desiredDirection - currentDirection) * interpolateValue;
            return (float)Math.Atan2(currentDirection.Y, currentDirection.X);
        }

        public static float ApproachValue(float currentValue, float targetValue, float speed)
        {
            if (currentValue < targetValue)
            {
                currentValue += speed;
                if (currentValue > targetValue)
                    return targetValue;
            }
            else
            {
                currentValue -= speed;
                if (currentValue < targetValue)
                    return targetValue;
            }

            return currentValue;
        }

        public static void ProjectileDestroy(Projectile proj)
        {
            try { proj.Kill(); }
            catch { proj.active = false; }
        }

        public static int? FindEquippedAccessory(Player player, int accType)
        {
            for (int i = LowerAccessorySlot; i <= HigherAccessorySlot; i++)
            {
                if (player.armor[i].type == accType) return i;
            }

            return null;
        }

        public static void SpawnClonedItem(Item clonedItem, Vector2 center, Vector2 velocity, int stack = 1)
        {
            int index = Item.NewItem(center, clonedItem.type, stack, false, -1, false, false);
            Main.item[index] = clonedItem.Clone();
            Main.item[index].whoAmI = index;
            Main.item[index].Center = center;
            Main.item[index].velocity = velocity;
            if (stack != Main.item[index].stack)
            {
                Main.item[index].stack = stack;
            }
            if (IsMultiplayerClient())
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, index, 1f, 0f, 0f, 0, 0, 0);
            }
        }

        public static int ToTileCoordinate(float coordinate)
        {
            return (int)(coordinate / 16f);
        }

        public static Vector2 ToPositionCoordinate(int i, int j)
        {
            return new Vector2
            {
                X = i * 16f + 8f,
                Y = j * 16f + 8f
            };
        }

        public static int FindTarget(Projectile projectile, Vector2 ownPosition, float range, bool needLineOfSight)
        {
            float shortestDistance = range;
            int target = -1;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC selectNpc = Main.npc[i];
                float distance = Vector2.Distance(projectile.Center, selectNpc.Center);
                float enemyDistance = Vector2.Distance(ownPosition, selectNpc.Center);

                if (enemyDistance <= range && distance < shortestDistance && selectNpc.CanBeChasedBy() &&
                    (NPCLoader.CanBeHitByProjectile(selectNpc, projectile) ?? true) &&
                    (!needLineOfSight || Collision.CanHit(projectile.Center, 1, 1, selectNpc.Center, 1, 1)))
                {
                    shortestDistance = distance;
                    target = i;
                }
            }

            return target;
        }

        public static bool IsCritter(NPC npc)
        {
            return npc.damage <= 0 && npc.lifeMax <= 5;
        }

        public static bool SummonBoss(Mod mod, Player player, int npcType, float tileDistance = 0)
        {
            if (IsSinglePlayer() || (IsMultiplayerClient() && IsSameClientOwner(player.whoAmI)))
            {
                Vector2 initialPosition = player.Center;
                float randomAngle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                Vector2 offset = randomAngle.ToRotationVector2() * tileDistance * 16f;
                Vector2 target = new Vector2(initialPosition.X + offset.X, initialPosition.Y + offset.Y);
                if (IsSinglePlayer())
                {
                    int npcIndex = NewNPC(target.X, target.Y, npcType, center: true);
                    NPC npc = Main.npc[npcIndex];
                    Main.NewText(Language.GetTextValue("Announcement.HasAwoken", npc.GivenOrTypeName), 175, 75, 255);
                    Main.PlaySound(SoundID.Roar, initialPosition, 0);
                }
                else
                {
                    ModPacket packet = mod.GetPacket();
                    packet.Write((byte)PacketMessageType.SpawnBoss);
                    packet.WriteVector2(initialPosition);
                    packet.WriteVector2(target);
                    packet.Write(npcType);
                    packet.Write(true);
                    packet.Send();
                }
            }

            return true;
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                                 @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static bool IsPlayerLocalServerOwner(int whoAmI)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                RemoteClient client = Netplay.Clients[i];
                if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
                {
                    return true;
                }
            }
            return false;
        }

        public static void AddDropTable(NPC npc, WeightedRandom<int> table, Dictionary<int, int> stacks = null)
        {
            if (table > 0)
            {
                Item.NewItem(npc.getRect(), table);
                if (stacks == null) Item.NewItem(npc.getRect(), table);
                else if (stacks.TryGetValue(table, out int stack))
                {
                    Item.NewItem(npc.getRect(), table, stack);
                }
                else Item.NewItem(npc.getRect(), table);
            }
        }

        public static void AddDropTable(Player player, WeightedRandom<int> table, Dictionary<int, int> stacks = null)
        {
            if (table > 0)
            {
                if (stacks == null) player.QuickSpawnItem(table);
                else if (stacks.TryGetValue(table, out int stack))
                {
                    player.QuickSpawnItem(table, stack);
                }
                else player.QuickSpawnItem(table);
            }
        }

        public static void AnimateProjectile(Projectile projectile, int frameSpeed)
        {
            if (++projectile.frameCounter >= frameSpeed)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }

        public static void ComputeCenterFromHitbox(Projectile projectile, ref int offsetX, ref int offsetY, int textureWidth, int textureHeight, int frames)
        {
            int frameWidth = textureWidth;
            int frameHeight = textureHeight / frames;
            offsetX = -(frameWidth - projectile.width) / 2;
            offsetY = -(frameHeight - projectile.height) / 2;
        }

        public static void ComputeCenterFromHitbox(NPC npc, ref float offsetY, int textureHeight, int frames)
        {
            int frameHeight = textureHeight / frames;
            offsetY = (frameHeight - npc.height) / 2 - 3;
        }

        public static void AssignItemDimensions(Item item, int originalWidth, int originalHeight, bool floating)
        {
            item.width = originalWidth;
            if (!floating && originalWidth > 10) item.width -= 8;
            else if (floating && originalWidth > 6) item.width -= 4;
            item.height = originalHeight;
            if (!floating && originalHeight > 10) item.height -= 8;
            else if (floating && originalWidth > 2) item.width -= 2;
        }

        public static void KillNPC(NPC npc)
        {
            if (IsSinglePlayer()) npc.StrikeNPCNoInteraction(npc.lifeMax, 0f, -npc.direction);
            else
            {
                Mod mod = ModContent.GetInstance<ChensGradiusMod>();
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)PacketMessageType.KillNPC);
                packet.Write(npc.whoAmI);
                packet.Send();
            }
        }
    }
}