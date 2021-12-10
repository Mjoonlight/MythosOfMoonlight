using MythosOfMoonlight.Projectiles.ThornDart;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MythosOfMoonlight.Items.Mortiflora.MortStaff
{
	public class MortStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leech Life");
			Tooltip.SetDefault("Shoots a dart that splits into 4 healing orbs on hit.");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.damage = 11;
			item.magic = true;
			item.noMelee = true;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.rare = ItemRarityID.Green;
			item.knockBack = 5;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;

			item.shoot = ProjectileType<ThornDart>();
			item.shootSpeed = 16f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.statMana >= 15;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// (int)((manaCost + 100) / 100)
			int manaCost = (int)MathHelper.Max(30, player.statMana / 2);
			player.statMana -= manaCost;
			var projectile = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, item.owner, MathHelper.Clamp((manaCost + 50) / 33, 0, 3));
			return false;
		}
	}
}