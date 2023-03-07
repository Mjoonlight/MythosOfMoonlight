using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Terraria.ID.ContentSamples.CreativeHelper;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace MythosOfMoonlight.Tiles.Furniture
{
    public class PriestessStatue : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            Main.tileLighted[Type] = true;
            DustType = DustID.Stone;
            TileObjectData.newTile.DrawYOffset = 2;
            //ItemDrop = ModContent.ItemType<PriestessStatueI>();
            TileObjectData.newTile.Height = 11;
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            //TileObjectData.newTile.AnchorBottom = new Terraria.DataStructures.AnchorData(Terraria.Enums.AnchorType.SolidBottom, 16, 0);
            TileObjectData.newTile.Origin = new Terraria.DataStructures.Point16(4, 10);
            TileObjectData.addTile(Type);
            AddMapEntry(Color.Gray);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = g = b = 1;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Point pos = new Point(i * 16, j * 16);
            if (Main.netMode == NetmodeID.SinglePlayer)
                pos = new Point((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y);
            Item.NewItem(new EntitySource_TileBreak(i, j), pos.X, pos.Y, 16, 16, ModContent.ItemType<PriestessStatueI>());
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D glow = Helper.GetTex("MythosOfMoonlight/Tiles/Furniture/PriestessStatue_Glow");
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            spriteBatch.Draw(glow, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), Color.White);
        }
    }
    public class PriestessStatueI : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Priestess Statue");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.rare = 0;
            Item.useTurn = true;
            Item.rare = 0;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.rare = 5;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<PriestessStatue>();
        }
    }
}
