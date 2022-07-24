using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using MythosOfMoonlight.Projectiles.CenturySpewer;
using Terraria.GameContent.Creative;

namespace MythosOfMoonlight.Items.CenturySet
{
    public class CenturySprayer : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Suffocate enemies with century-old spores");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.damage = 0;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.useTime = 5;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.rare = ItemRarityID.Blue;
			Item.knockBack = 5;
			Item.UseSound = SoundID.Item34;
			Item.autoReuse = true;
			Item.mana = 5;
			Item.shoot = ProjectileType<CenturySpewerSpore>();	
			Item.shootSpeed = 6f;
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
		}
	}
}