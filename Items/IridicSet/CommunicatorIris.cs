using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MythosOfMoonLight.Projectiles.IridicProjectiles;
using MythosOfMoonlight.Items.Materials;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Projectiles.IridicProjectiles;

namespace MythosOfMoonlight.Items.IridicSet
{
    public class CommunicatorIris : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Communicator-Iris");
            Tooltip.SetDefault("Minions become intertwined with Iridic flares, \n" +
                "occasionally releasing them when hitting an enemy. ");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MoMPlayer>().CommunicatorEquip = true;
        }
    }
}
