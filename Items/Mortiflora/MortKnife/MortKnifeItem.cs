using MythosOfMoonlight.Projectiles.MortKnife;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Mortiflora.MortKnife
{
	public class MortKnifeItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("C");
			Tooltip.SetDefault("kniv");
		}

		public override void SetDefaults()
		{
			item.damage = 15;
			item.melee = false;
			item.width = 52;
			item.height = 58;
			item.useTime = 17;
			item.useAnimation = 17;
			item.useStyle = 1;
			item.knockBack = 2.5f;
			item.value = 10000;
			item.rare = 2;
			item.shoot = ModContent.ProjectileType<MortKnifeProjectile>();
			item.shootSpeed = 12;
			item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			var velocity = new Vector2(speedX, speedY).RotatedByRandom(30);
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
			return false;
        }
	}
}