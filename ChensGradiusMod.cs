using ChensGradiusMod.Items.Placeables.MusicBoxes;
using ChensGradiusMod.Tiles.MusicBoxes;
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

    public ChensGradiusMod() { }

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
      }

      Mod achievementLib = ModLoader.GetMod("AchievementLib");
      if (achievementLib != null)
      {
        achievementLib.Call("AddAchievement", this, "Bydo Technology",
                            "Create an indestructible weapon made of alien flesh.",
                            GradiusAchievement.TextureString("PlaceholderAchievement", locked: true),
                            GradiusAchievement.TextureString("PlaceholderAchievement", locked: false));
        achievementLib.Call("AddAchievement", this, "Wreek Weapon",
                            "Create an Option, an invulnerable drone which copies the host's attacks.",
                            GradiusAchievement.TextureString("PlaceholderAchievement", locked: true),
                            GradiusAchievement.TextureString("PlaceholderAchievement", locked: false));
        achievementLib.Call("AddAchievement", this, "From Myth To Legend",
                            "Create your own legend by destroying a Big Core.",
                            GradiusAchievement.TextureString("PlaceholderAchievement", locked: true),
                            GradiusAchievement.TextureString("PlaceholderAchievement", locked: false));
      }
    }

    public override void Unload()
    {
      base.Unload();

      forceActionKey = null;
      optionActionKey = null;
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
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
      byte playerNumber;
      GradiusModPlayer modPlayer;
      PacketMessageType msgType = (PacketMessageType)reader.ReadByte();

      switch (msgType)
      {
        case PacketMessageType.GradiusModSyncPlayer:
          playerNumber = reader.ReadByte();
          modPlayer = Main.player[playerNumber].GetModPlayer<GradiusModPlayer>();
          modPlayer.isFreezing = reader.ReadBoolean();
          modPlayer.rotateMode = reader.ReadInt32();
          modPlayer.revolveDirection = reader.ReadInt32();
          modPlayer.wasHolding = reader.ReadBoolean();
          modPlayer.forceBase = reader.ReadBoolean();
          modPlayer.needleForce = reader.ReadBoolean();
          modPlayer.optionOne = reader.ReadBoolean();
          modPlayer.optionTwo = reader.ReadBoolean();
          modPlayer.optionThree = reader.ReadBoolean();
          modPlayer.optionFour = reader.ReadBoolean();
          modPlayer.normalOption = reader.ReadBoolean();
          modPlayer.freezeOption = reader.ReadBoolean();
          modPlayer.rotateOption = reader.ReadBoolean();
          int listCount = reader.ReadInt32();
          for (int i = 0; i < listCount; i++) modPlayer.optionFlightPath.Add(reader.ReadVector2());
          break;

        case PacketMessageType.ClientChangesFreezeOption:
          playerNumber = reader.ReadByte();
          modPlayer = Main.player[playerNumber].GetModPlayer<GradiusModPlayer>();

          modPlayer.isFreezing = reader.ReadBoolean();

          if (Main.netMode == NetmodeID.Server)
          {
            ModPacket packet = GetPacket();
            packet.Write((byte)PacketMessageType.ClientChangesFreezeOption);
            packet.Write(playerNumber);
            packet.Write(modPlayer.isFreezing);
            packet.Send(-1, playerNumber);
          }
          break;

        case PacketMessageType.ClientChangesRotateOption:
          playerNumber = reader.ReadByte();
          modPlayer = Main.player[playerNumber].GetModPlayer<GradiusModPlayer>();

          modPlayer.rotateMode = reader.ReadInt32();
          modPlayer.revolveDirection = reader.ReadInt32();

          if (Main.netMode == NetmodeID.Server)
          {
            ModPacket packet = GetPacket();
            packet.Write((byte)PacketMessageType.ClientChangesRotateOption);
            packet.Write(playerNumber);
            packet.Write(modPlayer.rotateMode);
            packet.Write(modPlayer.revolveDirection);
            packet.Write(modPlayer.wasHolding);
            packet.Send(-1, playerNumber);
          }
          break;
      }
    }

    internal enum PacketMessageType : byte
    {
      GradiusModSyncPlayer,
      ClientChangesFreezeOption,
      ClientChangesRotateOption
    }
  }
}