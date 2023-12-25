using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Projectiles;

namespace MythosOfMoonlight.Items.Weapons
{
    public class WardingStar : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 4f;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.channel = true;
            Item.value = Item.buyPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<WardingStarP>();
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<WardingStarP>()] < 1 && player.statMana > 5;
        }
    }
}
