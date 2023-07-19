using MythosOfMoonlight.Tiles.Trophies;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace MythosOfMoonlight.Items.Mortiflora.MortTrophy
{
    public class MortTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mortiflora Trophy");
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileType<BossTrophy>(), 0);
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 99;
            Item.value = 50000;
            Item.rare = ItemRarityID.Blue;
        }
    }
}