using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Food
{
    public class FieldSnack : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;

            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

            ItemID.Sets.DrinkParticleColors[Type] = new Color[2] {
                new Color(77, 49, 24),
                new Color(171, 111, 34 ),
            };
            ItemID.Sets.IsFood[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(34, 34, BuffID.Regeneration, 3600 * 2);
            Item.value = Item.buyPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Green;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.Sunflower, 3600);
            return true;
        }
    }
}
