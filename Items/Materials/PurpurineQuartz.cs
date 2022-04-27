using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Items.Materials
{
    public class PurpurineQuartz : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purpurine Quartz");
        }
        public override void SetDefaults()
        {
            item.value = Item.buyPrice(0, 0, 0, 1);
            item.rare = ItemRarityID.Green;
            item.material = true;
            item.maxStack = 99;
        }
    }
}
