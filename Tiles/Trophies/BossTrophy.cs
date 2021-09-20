using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using MythosOfMoonlight.Items.Mortiflora.MortTrophy;

namespace MythosOfMoonlight.Tiles.Trophies
{
    public class BossTrophy : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            dustType = 7;
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
			name.SetDefault("Trophy");
            AddMapEntry(new Color(120, 85, 60));
        }

      	public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			int item = 0;
			switch (frameX / 54) {
				case 0:
					item = ModContent.ItemType<MortTrophy>();
					break;
			}
			if (item > 0) {
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}
}