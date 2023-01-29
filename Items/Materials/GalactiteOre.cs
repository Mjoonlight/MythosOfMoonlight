using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using MythosOfMoonlight.Tiles;

namespace MythosOfMoonlight.Items.Materials
{
    public class GalactiteOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Galactite Ore");
            Tooltip.SetDefault("It shines with old energy.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
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
            Item.createTile = ModContent.TileType<Tiles.Galactite>();
        }
    }
}
