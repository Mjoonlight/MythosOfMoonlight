using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace MythosOfMoonlight.Items.ShadowHex
{
    [AutoloadEquip(EquipType.Body)]
    public class ShadowHexBody : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shadow-Hexer Coat");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 32;
            Item.value = 15000;
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }
    }
}