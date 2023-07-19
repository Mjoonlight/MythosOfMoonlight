using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace MythosOfMoonlight.Items.Weapons
{
    public class Starine_Strobulb : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Starbulb");
            /* Tooltip.SetDefault("Charges a powerful ray of Starine light that burns foes." +
                "\nIlluminating!"); */
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 24;
            Item.height = 38;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.knockBack = 0f;
            Item.damage = 7;
            Item.rare = ItemRarityID.Green;
            Item.DamageType = DamageClass.Magic;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Starine_Strobulb>();
        }
	}
}