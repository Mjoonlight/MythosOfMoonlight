using MythosOfMoonlight.Projectiles.ThornDart;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using MythosOfMoonlight.Projectiles.CenturySpewer.CenturySpewerSpore;

namespace MythosOfMoonlight.Items.CenturySet
{
	public class CenturySprayer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Century Sprayer");
			Tooltip.SetDefault("Suffocate enemies with century-old spores");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.damage = 0;
			item.magic = true;
			item.noMelee = true;
			item.useTime = 3;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 5;
			item.UseSound = SoundID.Item34;
			item.autoReuse = true;
			item.mana = 5;
			item.shoot = ProjectileType<CenturySpewerSpore>();	
			item.shootSpeed = 10f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			var velocity = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
			Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}