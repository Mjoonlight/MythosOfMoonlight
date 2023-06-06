using Microsoft.Xna.Framework;
using RealmOne.Projectiles.Bullet;
using RealmOne.Rarities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace RealmOne.Items.Weapons
{
	public class Shrimpy : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pistol Shrimp"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("'Not as loud as an actual pistol shrimp'"
			+ "\nConverts musket balls into water bullets"
			+ $"\nUses Bullets [i:{ItemID.MusketBall}]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

		}

		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2f;
			Item.value = Item.sellPrice(silver:35);
			Item.UseSound = SoundID.Item54;

			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			Item.useAmmo = AmmoID.Bullet;
			Item.noMelee = true;
			Item.shootSpeed = 50f;
			Item.shoot = ProjectileID.Bullet;

		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{

			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 10f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
            if (Main.rand.NextBool(5))
			{
                Gore.NewGore(source, player.Center + muzzleOffset * 1, new Vector2(player.direction * -1, -0.5f) * 2, Mod.Find<ModGore>("TommyGunPellets").Type, 1f);

            }

            for (int i = 0; i < 14; i++)
			{
				Vector2 speed = Main.rand.NextVector2CircularEdge(0.3f, 0.3f);
				var d = Dust.NewDustPerfect(Main.LocalPlayer.Center, DustID.Water, speed * 5, Scale: 1f);
				d.noGravity = true;
			}


			return true;
		}

		
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.NextFloat() >= 0.20f;
		}

		public override Vector2? HoldoutOffset()
		{
			var offset = new Vector2(-5, 0);
			return offset;
		}
	}
}