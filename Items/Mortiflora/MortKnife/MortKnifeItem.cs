using MythosOfMoonlight.Projectiles.MortKnife;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace MythosOfMoonlight.Items.Mortiflora.MortKnife
{
    public class MortKnifeItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chloroccyx");
			// Tooltip.SetDefault("Launches up to two bloodthirsty knives that aim at enemies near the cursor");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Melee;
			Item.width = 52;
			Item.height = 58;
			Item.useTime = 12;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2.5f;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<MortKnifeProjectile>();
			Item.shootSpeed = 12;
			Item.UseSound = SoundID.Item1;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			velocity = velocity.RotatedByRandom(30);
		}
	}
}