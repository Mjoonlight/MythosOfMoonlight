using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace MythosOfMoonlight.Items.Materials
{
    public class PurpurineQuartz : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Iridic Boards");
            // Tooltip.SetDefault("Alien tech, you don't understand it very well");
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.value = Item.buyPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
        }
    }
}
