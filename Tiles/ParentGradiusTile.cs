using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChensGradiusMod.Tiles
{
  public abstract class ParentGradiusTile : ModTile
  {
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

    public override void AnimateTile(ref int frame, ref int frameCounter)
    {
      if (MaxYFrames > 0 && FrameSpeed > 0)
      {
        if (++frameCounter >= FrameSpeed)
        {
          frameCounter = 0;
          if (++frame >= MaxYFrames) frame = 0;
        }
      }
    }

    protected virtual Color MinimapColor => new Color(0, 0, 0);

    protected virtual string MapName => "";

    protected virtual int ItemType => 0;

    protected virtual string Texture => "";

    protected virtual int FrameSpeed => 0;

    protected virtual int MaxYFrames => 0;
  }
}