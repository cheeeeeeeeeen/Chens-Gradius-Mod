using ChensGradiusMod.Items.Bags;
using ChensGradiusMod.Items.Banners;
using ChensGradiusMod.Items.Placeables.MusicBoxes;
using ChensGradiusMod.Items.Spawners;
using ChensGradiusMod.Items.Weapons.Magic;
using ChensGradiusMod.Items.Weapons.Melee;
using ChensGradiusMod.Items.Weapons.Summon;
using ChensGradiusMod.NPCs;
using ChensGradiusMod.Projectiles.Enemies;
using ChensGradiusMod.Projectiles.Options;
using ChensGradiusMod.Tiles.MusicBoxes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static ChensGradiusMod.GradiusHelper;

namespace ChensGradiusMod
{
    public class ChensGradiusMod : Mod
    {
        public static Mod gradiusMod;
        public static Mod bossChecklist;
        public static Mod herosMod;
        public static ModHotKey forceActionKey;
        public static ModHotKey optionActionKey;

        public ChensGradiusMod()
        {
        }

        public override void Load()
        {
            forceActionKey = RegisterHotKey("Force Action Toggle", "Mouse3");
            optionActionKey = RegisterHotKey("Option Action Key", "Mouse2");

            if (!Main.dedServ)
            {
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/PositionLight"),
                            ModContent.ItemType<PositionLightMusicBox>(),
                            ModContent.TileType<PositionLightMusicBoxTile>());
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/DepartureForSpace"),
                            ModContent.ItemType<DepartureForSpaceMusicBox>(),
                            ModContent.TileType<DepartureForSpaceMusicBoxTile>());
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Tabidachi"),
                            ModContent.ItemType<TabidachiMusicBox>(),
                            ModContent.TileType<TabidachiMusicBoxTile>());
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Intermezzo"),
                            ModContent.ItemType<IntermezzoMusicBox>(),
                            ModContent.TileType<IntermezzoMusicBoxTile>());
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Starfield"),
                            ModContent.ItemType<StarfieldMusicBox>(),
                            ModContent.TileType<StarfieldMusicBoxTile>());
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Sensation"),
                            ModContent.ItemType<SensationMusicBox>(),
                            ModContent.TileType<SensationMusicBoxTile>());
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/AircraftCarrier"),
                            ModContent.ItemType<AircraftCarrierMusicBox>(),
                            ModContent.TileType<AircraftCarrierMusicBoxTile>());
            }

            gradiusMod = this;
            bossChecklist = ModLoader.GetMod("BossChecklist");
            herosMod = ModLoader.GetMod("HEROsMod");
        }

        public override void Unload()
        {
            forceActionKey = null;
            optionActionKey = null;
            bossChecklist = null;
            herosMod = null;
            gradiusMod = null;
        }

        public override object Call(params object[] args)
        {
            try
            {
                string functionName = args[0] as string;
                switch (functionName)
                {
                    case "AddWeaponRule":
                        {
                            // Vanilla Weapon Rule
                            //   args[1]: Mode ("Ban" or "Allow")
                            //   args[2]: Vanilla Weapon Type. e.g. ItemID.BeesKnees
                            // Modded Weapon Rule
                            //   args[1]: Mode ("Ban" or "Allow")
                            //   args[2]: Your Mod's Internal Name. e.g. "ChensGradiusMod"
                            //   args[3]: Your Weapon's Internal name. e.g. "NewBowWeapon"

                            if (args.Length > 4 && args.Length < 3)
                            {
                                throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                                    "Wrong number of arguments.");
                            }

                            string mode = args[1] as string;
                            if (mode != "Ban" && mode != "Allow")
                            {
                                throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                                    "Unsupported mode. \"Ban\" and \"Allow\" are the supported modes.");
                            }

                            bool result = false;
                            if (args.Length == 3) result = OptionRules.AddWeaponRule(mode, Convert.ToInt32(args[2]));
                            else result = OptionRules.AddWeaponRule(mode, args[2] as string, args[3] as string);

                            return result;
                        }

                    case "AddProjectileRule":
                        {
                            // Vanilla Projectile Rule
                            //   args[1]: Mode ("Ban" or "Allow")
                            //   args[2]: Vanilla Projectile Type. e.g. ProjectileID.BeeArrow
                            // Mod Projectile Rule
                            //   args[1]: Mode ("Ban" or "Allow")
                            //   args[2]: Your Mod's Internal Name. e.g. "ChensGradiusMod"
                            //   args[3]: Your Projectile's Internal name. e.g. "NewArrow"

                            if (args.Length > 4 && args.Length < 3)
                            {
                                throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                                    "Wrong number of arguments.");
                            }

                            string mode = args[1] as string;
                            if (mode != "Ban" && mode != "Allow")
                            {
                                throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                                    "Unsupported mode. \"Ban\" and \"Allow\" are the supported modes.");
                            }

                            bool result = false;
                            if (args.Length == 3) result = OptionRules.AddProjectileRule(mode, Convert.ToInt32(args[2]));
                            else result = OptionRules.AddProjectileRule(mode, args[2] as string, args[3] as string);

                            return result;
                        }

                    case "AddWeaponProjectilePairRule":
                        {
                            // Vanilla Weapon Projectile Pair Rule (or if the mod owns the modded weapons and projectiles)
                            //   args[1]: Mode ("Ban" or "Allow")
                            //   args[2]: Vanilla Weapon Type. e.g. ItemID.BeesKnees or ModContent.ItemType<NewGradiusGun>()
                            //   args[3]: Vanilla Projectile Type. e.g. ProjectileID.BeeArrow or ModContent.ProjectileType<GradiusLazor>()
                            // Mod Projectile Rule
                            //   args[1]: Mode ("Ban" or "Allow")
                            //   args[2]: Your Mod's Internal Name. e.g. "ChensGradiusMod"
                            //   args[3]: Vanilla Projectile Type. e.g. "NewGradiusGun"
                            //   args[4]: Your Projectile's Internal name. e.g. "GradiusLazor"

                            if (args.Length > 5 && args.Length < 4)
                            {
                                throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                                    "Wrong number of arguments.");
                            }

                            string mode = args[1] as string;
                            if (mode != "Ban" && mode != "Allow")
                            {
                                throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                                    "Unsupported mode. \"Ban\" and \"Allow\" are the supported modes.");
                            }

                            bool result = false;
                            if (args.Length == 4) result = OptionRules.AddWeaponProjectilePairRule(mode, Convert.ToInt32(args[2]), Convert.ToInt32(args[3]));
                            else result = OptionRules.AddWeaponProjectilePairRule(mode, args[2] as string, args[3] as string, args[4] as string);

                            return result;
                        }

                    case "AddCustomDamage":
                        {
                            // Global Projectile Way
                            //   args[1]: Your Mod's Internal Name. e.g. "ChensGradiusMod"
                            //   args[2]: Internal class name of GlobalProjectile containing
                            //            the custom damage type. e.g. "MyGlobalProjectile"
                            //   args[3]: Internal boolean variable name of your custom damage.
                            // Mod Projectile Way
                            //   args[1]: Your Mod's Internal Name. e.g. "ChensGradiusMod"
                            //   args[2]: Internal boolean variable name of your custom damage
                            //            found in your ModProjectile.

                            if (!(args.Length == 4 || args.Length == 3))
                            {
                                throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                                    "Wrong number of arguments.");
                            }

                            bool result = false;
                            if (args.Length == 4)
                            {
                                result = OptionRules.ImportDamageType(args[1] as string, args[2] as string,
                                                                      args[3] as string);
                            }
                            else if (args.Length == 3)
                            {
                                result = OptionRules.ImportDamageType(args[1] as string, args[2] as string);
                            }
                            return result;
                        }

                    default:
                        {
                            throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                                "The function does not exist. Please visit the Wiki for the updated API.");
                        }
                }
            }
            catch (Exception e)
            {
                Logger.Error("ChensGradiusMod Call Error: " + e.Message + e.StackTrace);
            }

            return null;
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("Ebonstone Block or Crimstone Block"), new int[]
            {
                ItemID.CrimstoneBlock,
                ItemID.EbonstoneBlock
            });
            RecipeGroup.RegisterGroup("ChensGradiusMod:EvilStones", group);

            group = new RecipeGroup(() => Language.GetTextValue("Tissue Sample or Shadow Scale"), new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });
            RecipeGroup.RegisterGroup("ChensGradiusMod:EvilDrops", group);

            group = new RecipeGroup(() => Language.GetTextValue("Gold Bar or Platinum Bar"), new int[]
            {
                ItemID.PlatinumBar,
                ItemID.GoldBar
            });
            RecipeGroup.RegisterGroup("ChensGradiusMod:GoldTierBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("Cobalt Bar or Palladium Bar"), new int[]
            {
                ItemID.PalladiumBar,
                ItemID.CobaltBar
            });
            RecipeGroup.RegisterGroup("ChensGradiusMod:CobaltTierBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("Copper Bar or Tin Bar"), new int[]
            {
                ItemID.TinBar,
                ItemID.CopperBar
            });
            RecipeGroup.RegisterGroup("ChensGradiusMod:TinTierBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("Any Mechanical Boss Soul"), new int[]
            {
                ItemID.SoulofMight,
                ItemID.SoulofSight,
                ItemID.SoulofFright,
            });
            RecipeGroup.RegisterGroup("ChensGradiusMod:MechSoul", group);

            group = new RecipeGroup(() => Language.GetTextValue("Silver Bar or Tungsten Bar"), new int[]
            {
                ItemID.TungstenBar,
                ItemID.SilverBar
            });
            RecipeGroup.RegisterGroup("ChensGradiusMod:SilverTierBar", group);

            group = new RecipeGroup(() => Language.GetTextValue("Any Evil Mushroom"), new int[]
            {
                ItemID.VileMushroom,
                ItemID.ViciousMushroom
            });
            RecipeGroup.RegisterGroup("ChensGradiusMod:EvilMushroom", group);

            group = new RecipeGroup(() => Language.GetTextValue("Any Evil Bow"), new int[]
            {
                ItemID.TendonBow,
                ItemID.DemonBow
            });
            RecipeGroup.RegisterGroup("ChensGradiusMod:EvilBow", group);
        }

        public override void PostSetupContent()
        {
            if (bossChecklist != null)
            {
                List<int> collectionDrops = new List<int>
                {
                    ModContent.ItemType<BigCoreCustomBanner>(),
                    ModContent.ItemType<AircraftCarrierMusicBox>()
                };
                List<int> lootDrops = new List<int>
                {
                    ModContent.ItemType<ZalkYoyo>(),
                    ModContent.ItemType<Death2Weapon>(),
                    ModContent.ItemType<MiniCoveredCoreWeapon>(),
                    ModContent.ItemType<BigCoreCustomBag>(),
                    ItemID.SuperHealingPotion
                };
                bossChecklist.Call("AddBoss", 15f, ModContent.NPCType<BigCoreCustom>(), this, "Big Core Custom",
                                   (Func<bool>)GradiusModWorld.IsBigCoreDowned, ModContent.ItemType<BigBlueBizarreLens>(),
                                   collectionDrops, lootDrops, "Use Big Blue Bizarre Lens.",
                                   "The aircraft carrier leaves combat airspace.",
                                   "ChensGradiusMod/Sprites/BigCoreCustomBossLog");
            }

            if (herosMod != null)
            {
                herosMod.Call("AddPermission", "UpdateConfig", "Update Gradius Config", (Action<bool>)(groupUpdated => { }));
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            PacketMessageType msgType = (PacketMessageType)reader.ReadByte();

            switch (msgType)
            {
                case PacketMessageType.GradiusModSyncPlayer:
                    {
                        byte playerIndex = reader.ReadByte();
                        GradiusPlayer(playerIndex).isFreezing = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).rotateMode = reader.ReadInt32();
                        GradiusPlayer(playerIndex).revolveDirection = reader.ReadSByte();
                        GradiusPlayer(playerIndex).wasHolding = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).forceBase = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).needleForce = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).optionOne = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).optionTwo = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).optionThree = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).optionFour = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).normalOption = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).freezeOption = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).rotateOption = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).optionSeed = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).seedRotateDirection = reader.ReadSByte();
                        GradiusPlayer(playerIndex).chargeMultiple = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).chargeMode = reader.ReadInt32();
                        GradiusPlayer(playerIndex).aimOption = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).searchOption = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).isSearching = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).recurveOption = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).spreadOption = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).turretOption = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).isTurreting = reader.ReadBoolean();
                        break;
                    }

                case PacketMessageType.ClientChangesFreezeOption:
                    {
                        byte playerIndex = reader.ReadByte();
                        GradiusPlayer(playerIndex).isFreezing = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).wasHolding = reader.ReadBoolean();

                        if (IsServer())
                        {
                            ModPacket packet = GetPacket();
                            packet.Write((byte)PacketMessageType.ClientChangesFreezeOption);
                            packet.Write(playerIndex);
                            packet.Write(GradiusPlayer(playerIndex).isFreezing);
                            packet.Write(GradiusPlayer(playerIndex).wasHolding);
                            packet.Send(-1, playerIndex);
                        }
                        break;
                    }

                case PacketMessageType.ClientChangesRotateOption:
                    {
                        byte playerIndex = reader.ReadByte();
                        GradiusPlayer(playerIndex).rotateMode = reader.ReadInt32();
                        GradiusPlayer(playerIndex).revolveDirection = reader.ReadSByte();
                        GradiusPlayer(playerIndex).wasHolding = reader.ReadBoolean();

                        if (IsServer())
                        {
                            ModPacket packet = GetPacket();
                            packet.Write((byte)PacketMessageType.ClientChangesRotateOption);
                            packet.Write(playerIndex);
                            packet.Write(GradiusPlayer(playerIndex).rotateMode);
                            packet.Write(GradiusPlayer(playerIndex).revolveDirection);
                            packet.Write(GradiusPlayer(playerIndex).wasHolding);
                            packet.Send(-1, playerIndex);
                        }
                        break;
                    }

                case PacketMessageType.ClientChangesSeedDirection:
                    {
                        byte playerIndex = reader.ReadByte();
                        GradiusPlayer(playerIndex).seedRotateDirection = reader.ReadSByte();

                        if (IsServer())
                        {
                            ModPacket packet = GetPacket();
                            packet.Write((byte)PacketMessageType.ClientChangesSeedDirection);
                            packet.Write(playerIndex);
                            packet.Write(GradiusPlayer(playerIndex).seedRotateDirection);
                            packet.Send(-1, playerIndex);
                        }
                        break;
                    }

                case PacketMessageType.ClientChangesChargeMultiple:
                    {
                        byte playerIndex = reader.ReadByte();
                        GradiusPlayer(playerIndex).chargeMode = reader.ReadInt32();
                        GradiusPlayer(playerIndex).wasHolding = reader.ReadBoolean();

                        if (IsServer())
                        {
                            ModPacket packet = GetPacket();
                            packet.Write((byte)PacketMessageType.ClientChangesChargeMultiple);
                            packet.Write(playerIndex);
                            packet.Write(GradiusPlayer(playerIndex).chargeMode);
                            packet.Write(GradiusPlayer(playerIndex).wasHolding);
                            packet.Send(-1, playerIndex);
                        }
                        break;
                    }

                case PacketMessageType.SpawnRetaliationBullet:
                    if (IsServer())
                    {
                        Vector2 spawnPoint = reader.ReadVector2();
                        Vector2 spawnVelocity = reader.ReadVector2();
                        int dmg = reader.ReadInt32();
                        float kb = reader.ReadSingle();

                        Projectile.NewProjectile(spawnPoint, spawnVelocity,
                                                 ModContent.ProjectileType<BacterionBullet>(),
                                                 dmg, kb, Main.myPlayer);
                    }
                    break;

                case PacketMessageType.ClientChangesSearchOption:
                    {
                        byte playerIndex = reader.ReadByte();
                        GradiusPlayer(playerIndex).isSearching = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).wasHolding = reader.ReadBoolean();

                        if (IsServer())
                        {
                            ModPacket packet = GetPacket();
                            packet.Write((byte)PacketMessageType.ClientChangesSearchOption);
                            packet.Write(playerIndex);
                            packet.Write(GradiusPlayer(playerIndex).isSearching);
                            packet.Write(GradiusPlayer(playerIndex).wasHolding);
                            packet.Send(-1, playerIndex);
                        }
                        break;
                    }

                case PacketMessageType.BroadcastSound:
                    SoundPacketType soundPacketType = (SoundPacketType)reader.ReadByte();
                    switch (soundPacketType)
                    {
                        case SoundPacketType.Vanilla:
                            {
                                ushort soundType = reader.ReadUInt16();
                                Vector2 soundPosition = reader.ReadVector2();
                                byte soundStyle = reader.ReadByte();

                                if (IsServer())
                                {
                                    ModPacket packet = GetPacket();
                                    packet.Write((byte)PacketMessageType.BroadcastSound);
                                    packet.Write((byte)SoundPacketType.Vanilla);
                                    packet.Write(soundType);
                                    packet.WriteVector2(soundPosition);
                                    packet.Write(soundStyle);
                                    packet.Send();
                                }
                                else Main.PlaySound(soundType, soundPosition, soundStyle);
                                break;
                            }

                        case SoundPacketType.Legacy:
                            {
                                string customSound = reader.ReadString();
                                Vector2 soundPosition = reader.ReadVector2();

                                if (IsServer())
                                {
                                    ModPacket packet = GetPacket();
                                    packet.Write((byte)PacketMessageType.BroadcastSound);
                                    packet.Write((byte)SoundPacketType.Legacy);
                                    packet.Write(customSound);
                                    packet.WriteVector2(soundPosition);
                                    packet.Send();
                                }
                                else Main.PlaySound(GetLegacySoundSlot(SoundType.Custom, customSound), soundPosition);
                                break;
                            }
                    }
                    break;

                case PacketMessageType.SpawnBoss:
                    Vector2 initialPosition = reader.ReadVector2();
                    if (IsServer())
                    {
                        Vector2 target = reader.ReadVector2();
                        int npcType = reader.ReadInt32();
                        bool center = reader.ReadBoolean();
                        int npcIndex = NewNPC(target.X, target.Y, npcType, center: center);
                        NPC npc = Main.npc[npcIndex];
                        NetMessage.BroadcastChatMessage(NetworkText.FromKey(Language.GetTextValue("Announcement.HasAwoken", npc.GivenOrTypeName)), new Color(175, 75, 255));
                        ModPacket packet = GetPacket();
                        packet.Write((byte)PacketMessageType.BroadcastSound);
                        packet.Write((byte)SoundPacketType.Vanilla);
                        packet.Write((ushort)SoundID.Roar);
                        packet.WriteVector2(initialPosition);
                        packet.Write((byte)0);
                        packet.Send();
                    }
                    break;

                case PacketMessageType.RecurveUpdatePositions:
                    {
                        byte playerIndex = reader.ReadByte();
                        GradiusPlayer(playerIndex).PopulateOptionFlight();
                        GradiusPlayer(playerIndex).optionFlightPath[OptionBaseObject.DistanceInterval - 1] =
                          reader.ReadVector2();
                        GradiusPlayer(playerIndex).optionFlightPath[OptionBaseObject.DistanceInterval * 2 - 1] =
                          reader.ReadVector2();
                        GradiusPlayer(playerIndex).optionFlightPath[OptionBaseObject.DistanceInterval * 3 - 1] =
                          reader.ReadVector2();
                        GradiusPlayer(playerIndex).optionFlightPath[OptionBaseObject.DistanceInterval * 4 - 1] =
                          reader.ReadVector2();

                        if (IsServer())
                        {
                            ModPacket packet = GetPacket();
                            packet.Write((byte)PacketMessageType.RecurveUpdatePositions);
                            packet.Write(playerIndex);
                            packet.WriteVector2(
                              GradiusPlayer(playerIndex).optionFlightPath[OptionBaseObject.DistanceInterval - 1]
                            );
                            packet.WriteVector2(
                              GradiusPlayer(playerIndex).optionFlightPath[OptionBaseObject.DistanceInterval * 2 - 1]
                            );
                            packet.WriteVector2(
                              GradiusPlayer(playerIndex).optionFlightPath[OptionBaseObject.DistanceInterval * 3 - 1]
                            );
                            packet.WriteVector2(
                              GradiusPlayer(playerIndex).optionFlightPath[OptionBaseObject.DistanceInterval * 4 - 1]
                            );
                            packet.Send(-1, playerIndex);
                        }
                        break;
                    }

                case PacketMessageType.ClientChangesTurretOption:
                    {
                        byte playerIndex = reader.ReadByte();
                        GradiusPlayer(playerIndex).isTurreting = reader.ReadBoolean();
                        GradiusPlayer(playerIndex).wasHolding = reader.ReadBoolean();

                        if (IsServer())
                        {
                            ModPacket packet = GetPacket();
                            packet.Write((byte)PacketMessageType.ClientChangesFreezeOption);
                            packet.Write(playerIndex);
                            packet.Write(GradiusPlayer(playerIndex).isFreezing);
                            packet.Write(GradiusPlayer(playerIndex).wasHolding);
                            packet.Send(-1, playerIndex);
                        }
                        break;
                    }
            }
        }

        private GradiusModPlayer GradiusPlayer(int whoAmI) => Main.player[whoAmI].GetModPlayer<GradiusModPlayer>();

        internal enum PacketMessageType : byte
        {
            GradiusModSyncPlayer,
            ClientChangesFreezeOption,
            ClientChangesRotateOption,
            ClientChangesSeedDirection,
            ClientChangesChargeMultiple,
            SpawnRetaliationBullet,
            ClientChangesSearchOption,
            BroadcastSound,
            SpawnBoss,
            RecurveUpdatePositions,
            ClientChangesTurretOption
        };

        internal enum SoundPacketType : byte
        {
            Vanilla,
            Legacy
        }
    }
}