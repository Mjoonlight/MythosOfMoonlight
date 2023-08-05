using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Items.Materials
{
    public class GalactiteOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Galactite Ore");
            // Tooltip.SetDefault("It shines with old energy.");
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 0, 0, 2);
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 26;
            Item.height = 24;
            Item.createTile = ModContent.TileType<Tiles.Galactite>();
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D tex = Helper.GetTex(Texture + "_Glow");
            spriteBatch.Draw(tex, Item.Center - Main.screenPosition, null, Color.White, rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);
        }
    }
}
