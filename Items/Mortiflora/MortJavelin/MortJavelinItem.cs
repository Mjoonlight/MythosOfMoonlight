using MythosOfMoonlight.Projectiles.MortJavelin;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace MythosOfMoonlight.Items.Mortiflora.MortJavelin
{
    public class MortJavelinItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Thornbella");
			// Tooltip.SetDefault("Bounces towards nearby enemies, inflicting poison");
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Melee;
			Item.width = 52;
			Item.height = 58;
			Item.useTime = 17;
			Item.useAnimation = 17;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2.5f;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<MortJavelinProjectile>();
			Item.shootSpeed = 12;
			Item.UseSound = SoundID.Item1;
			Item.noUseGraphic = true;
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			position = player.Center + new Vector2(0, -16);
		}
	}
}