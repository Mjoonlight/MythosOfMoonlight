using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class EntropicLegs : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.Size = new Vector2(36, 18);
            Item.rare = ItemRarityID.Green;
            Item.defense = 3;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 1, 50, 0);
        }
    }
}
