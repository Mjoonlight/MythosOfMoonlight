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
			item.damage = 5;
			item.magic = true;
			item.noMelee = true;
			item.useTime = 3;
			item.useAnimation = 6;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 5;
			item.UseSound = SoundID.Item34;
			item.autoReuse = true;
			item.mana = 9;
			item.shoot = ProjectileType<CenturySpewerSpore>();	
			item.shootSpeed = 16f;
		}
	}
}