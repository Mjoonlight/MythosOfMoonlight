using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using MythosOfMoonlight.NPCs.Critters;

namespace MythosOfMoonlight.Items.Critters
{
    public class TheFishItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Yellow Boxfish");
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(0, 0, 5, 5);
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 20;
            Item.noMelee = true;
            Item.consumable = true;
            Item.autoReuse = true;

        }

        public override bool? UseItem(Player player)
        {
            int index = NPC.NewNPC(new EntitySource_SpawnNPC(), (int)(player.position.X + Main.rand.Next(-20, 20)), (int)(player.position.Y - 0f), ModContent.NPCType<TheFish>());

            if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                NetMessage.SendData(MessageID.SyncNPC, number: index);
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ItemID.Sashimi)
               .AddIngredient(ModContent.ItemType<TheFishItem>())
               .AddTile(TileID.WorkBenches)
               .Register();
        }
    }
}