using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items
{
	public class Starine_Spotlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Spotlight");
            Tooltip.SetDefault("Shines out a powerful ray of Starine light that burns foes. \nIlluminating!");
        }
        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 24;
            item.height = 38;
            item.useAnimation = 10;
            item.useTime = 10;
            item.shootSpeed = 0f;
            item.knockBack = 0f;
            item.damage = 7;
            item.rare = ItemRarityID.Green;
            item.magic = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.Starine_Spotlight>();
        }
    }
}