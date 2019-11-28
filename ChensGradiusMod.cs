using ChensGradiusMod.Items.Placeables.MusicBoxes;
using ChensGradiusMod.Projectiles.Enemies;
using ChensGradiusMod.Projectiles.Options;
using ChensGradiusMod.Tiles.MusicBoxes;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChensGradiusMod
{
  public class ChensGradiusMod : Mod
  {
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
      }
    }

    public override void Unload()
    {
      base.Unload();

      forceActionKey = null;
      optionActionKey = null;
    }

    public override object Call(params object[] args)
    {
      try
      {
        string functionName = args[0] as string;
        switch (functionName)
        {
          case "AddOptionRule":
            {
              // Vanilla Projectile Rule
              // args[1]: Vanilla Projectile Type. e.g. ProjectileID.Bee
              // ... or Mod Projectile Rule
              // args[1]: Your Mod's Internal Name. e.g. "ChensGradiusMod"
              // args[2]: Your Projectile's Internal name. e.g. "NewArrowsProjectile"

              if (args.Length > 3 && args.Length < 2)
              {
                Logger.Error($"ChensGradiusMod {functionName} Error: " +
                             "Wrong number of arguments.");
                throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                    "Wrong number of arguments.");
              }

              bool? result;
              if (args.Length == 2)
              {
                if (args[1] == null) result = null;
                else result = OptionRules.ImportOptionRule(Convert.ToInt32(args[1]));
              }
              else result = OptionRules.ImportOptionRule(args[1] as string, args[2] as string);

              if (result == null)
              {
                Logger.Warn($"ChensGradiusMod {functionName} Warning:" +
                            "Given projectile type is null. This projectile type is not added.");
                result = false;
              }
              return result;
            }

          case "AddCustomDamage":
            {
              // Global Projectile Way
              // args[1]: Your Mod's Internal Name. e.g. "ChensGradiusMod"
              // args[2]: Internal class name of GlobalProjectile containing
              //          the custom damage type. e.g. "MyGlobalProjectile"
              // args[3]: Internal boolean variable name of your custom damage.
              // ... or Mod Projectile Way
              // args[1]: Your Mod's Internal Name. e.g. "ChensGradiusMod"
              // args[2]: Internal boolean variable name of your custom damage
              //          found in your ModProjectile.

              if (!(args.Length == 4 || args.Length == 2))
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

          case "ProjectileBanCheck":
            {
              // args[1]: integer Type of the projectile to check. (projectile.type)

              if (args.Length > 2)
              {
                throw new Exception($"ChensGradiusMod {functionName} Error: " +
                                    "Wrong number of arguments.");
              }

              bool? result;
              if (args[1] == null)
              {
                result = null;
                Logger.Warn($"ChensGradiusMod {functionName} Warning: " +
                            "Given projectile type is null.");
              }
              else result = OptionRules.IsBanned(Convert.ToInt32(args[1]));
              return result;
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
            break;
          }

        case PacketMessageType.ClientChangesFreezeOption:
          {
            byte playerIndex = reader.ReadByte();
            GradiusPlayer(playerIndex).isFreezing = reader.ReadBoolean();
            GradiusPlayer(playerIndex).wasHolding = reader.ReadBoolean();

            if (GradiusHelper.IsServer())
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

            if (GradiusHelper.IsServer())
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

            if (GradiusHelper.IsServer())
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

            if (GradiusHelper.IsServer())
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
          if (GradiusHelper.IsServer())
          {
            Vector2 spawnPoint = reader.ReadVector2();
            Vector2 spawnVelocity = reader.ReadVector2();
            int dmg = reader.ReadInt32();
            float kb = reader.ReadSingle();

            Projectile.NewProjectile(spawnPoint, spawnVelocity,
                                     ModContent.ProjectileType<GradiusEnemyBullet>(),
                                     dmg, kb, Main.myPlayer);
          }
          break;

        case PacketMessageType.ClientChangesSearchOption:
          {
            byte playerIndex = reader.ReadByte();
            GradiusPlayer(playerIndex).isSearching = reader.ReadBoolean();
            GradiusPlayer(playerIndex).wasHolding = reader.ReadBoolean();

            if (GradiusHelper.IsServer())
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
          ushort soundType = reader.ReadUInt16();
          Vector2 soundPosition = reader.ReadVector2();
          byte soundStyle = reader.ReadByte();

          if (GradiusHelper.IsServer())
          {
            ModPacket packet = GetPacket();
            packet.Write((byte)PacketMessageType.BroadcastSound);
            packet.Write(soundType);
            packet.WriteVector2(soundPosition);
            packet.Write(soundStyle);
            packet.Send(-1, whoAmI);
          }
          else Main.PlaySound(soundType, soundPosition, soundStyle);
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

            if (GradiusHelper.IsServer())
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
      RecurveUpdatePositions
    }
  }
}