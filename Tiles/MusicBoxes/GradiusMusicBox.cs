using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChensGradiusMod.Tiles.MusicBoxes
{
  public abstract class GradiusMusicBox : ModTile
  {
    protected virtual Color MinimapColor => new Color(0, 0, 0);

    protected virtual string MusicName => "";

    protected virtual int ItemType => 0;

    public override void SetDefaults()
    {
      Main.tileFrameImportant[Type] = true;
      Main.tileObsidianKill[Type] = true;
      TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
      TileObjectData.newTile.Origin = new Point16(0, 1);
      TileObjectData.newTile.LavaDeath = false;
      TileObjectData.newTile.DrawYOffset = 2;
      TileObjectData.addTile(Type);
      disableSmartCursor = true;
      ModTranslation name = CreateMapEntryName();
      name.SetDefault($"Music Box ({ MusicName })");
      AddMapEntry(MinimapColor, name);
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
      Item.NewItem(i * 16, j * 16, 16, 48, ItemType);
    }

    public override void MouseOver(int i, int j)
    {
      Player player = Main.LocalPlayer;
      player.noThrow = 2;
      player.showItemIcon = true;
      player.showItemIcon2 = ItemType;
    }
  }
}