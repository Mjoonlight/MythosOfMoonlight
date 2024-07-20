using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Items;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Misc
{
    public class ThePebble : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.PoopBlock;
        }
        public override void SetDefaults()
        {
            Item.Size = new Vector2(18, 14);
            Item.value = 1;
        }
    }
}
