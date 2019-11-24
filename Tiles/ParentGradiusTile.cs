using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChensGradiusMod.Tiles
{
  public abstract class ParentGradiusTile : ModTile
  {
    protected virtual Color MinimapColor => new Color(0, 0, 0);

    protected virtual string MapName => "";

    protected virtual int ItemType => 0;

    protected virtual string Texture => "";

    public override bool Autoload(ref string name, ref string texture)
    {
      if (Texture != "") texture = Texture;
      return mod.Properties.Autoload;
    }

    public override void SetDefaults()
    {
      TileObjectData.addTile(Type);
      ModTranslation name = CreateMapEntryName();
      name.SetDefault(MapName);
      AddMapEntry(MinimapColor, name);
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
      Item.NewItem(i * 16, j * 16, 16, 48, ItemType);
    }
  }
}