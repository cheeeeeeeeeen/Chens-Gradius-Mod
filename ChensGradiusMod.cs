using Terraria.ModLoader;

namespace ChensGradiusMod
{
	public class ChensGradiusMod : Mod
	{
    public static ModHotKey forceActionKey;

    public ChensGradiusMod() { }

    public override void Load()
    {
      forceActionKey = RegisterHotKey("Force Action Toggle", "Mouse2");
    }

    public override void Unload()
    {
      forceActionKey = null;
    }
  }
}