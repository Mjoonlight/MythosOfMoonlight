using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.NPCs.Critters.PurpleComet;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace MythosOfMoonlight.Items.PurpleComet.Critters
{
    public class JellyWandererItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jelly Wanderer");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
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
            int index = NPC.NewNPC(new EntitySource_SpawnNPC(), (int)(player.position.X + Main.rand.Next(-20, 20)), (int)(player.position.Y - 0f), ModContent.NPCType<JellyWanderer>());

            if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                NetMessage.SendData(MessageID.SyncNPC, number: index);
            return true;
        }
    }
}