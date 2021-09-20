using MythosOfMoonlight.Projectiles.MortJavelin;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Mortiflora.MortJavelin
{
	public class MortJavelinItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornbella");
			Tooltip.SetDefault("Bounces towards nearby enemies, inflicting poison");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.melee = false;
			item.width = 52;
			item.height = 58;
			item.useTime = 17;
			item.useAnimation = 17;
			item.useStyle = 1;
			item.knockBack = 2.5f;
			item.value = 10000;
			item.rare = 2;
			item.shoot = ModContent.ProjectileType<MortJavelinProjectile>();
			item.shootSpeed = 12;
			item.UseSound = SoundID.Item1;
			item.scale = 0f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			var direction = new Vector2(speedX, speedY);
			direction.Normalize();
			position += new Vector2(28f, 74f) * direction;
			return true;
		}
	}
}