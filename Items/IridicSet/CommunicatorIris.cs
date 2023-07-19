using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace MythosOfMoonlight.Items.IridicSet
{
    public class CommunicatorIris : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Communicator-Iris");
            /* Tooltip.SetDefault("Minions become intertwined with Iridic flares, \n" +
                "occasionally releasing them when hitting an enemy. "); */
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MoMPlayer>().CommunicatorEquip = true;
        }
    }
}
