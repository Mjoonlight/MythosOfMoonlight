using MythosOfMoonlight.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.Tiles
{
	public class Galactite : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = false;
			Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 410;
            Main.tileShine2[Type] = false;
			Main.tileShine[Type] = 975;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = false;
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Galactite Ore");
			AddMapEntry(new Color(94, 71, 142), name);
			DustType = ModContent.DustType<PurpurineStoneDust>();
            HitSound = SoundID.Tink;
			MinPick = 55;
		}
	}
}