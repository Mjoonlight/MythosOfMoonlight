using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SunHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Increased health regeneration");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.lifeRegen++;
        }
        public override void SetDefaults()
        {
            Item.Size = new Vector2(36, 18);
            Item.rare = ItemRarityID.Green;
            Item.defense = 2;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 1, 50, 0);
        }
    }
}
