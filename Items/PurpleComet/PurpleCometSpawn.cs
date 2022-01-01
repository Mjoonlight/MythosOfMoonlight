using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Items.PurpleComet
{
	public class PurpleCometSpawn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purple Comet's Offering");
			Tooltip.SetDefault("Use at nighttime to call upon the Purple Comet.");
		}

		public override void SetDefaults()
		{
			item.width = item.height = 16;
            item.rare = ItemRarityID.Pink;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 10;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;

			item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player)
		{
			if (Main.dayTime) {
				Main.NewText("With light already shining in the skies, there is no possible way for the Purple Comet to illuminate for all to witness.", 179, 0, 255, true);
				return false;
			}
			if (PurpleCometEvent.PurpleComet)
            {
                return false;
            }
			return true;
		}

		public override bool UseItem(Player player)
		{
			Main.NewText("You start to feel like levitating...", 179, 0, 255, true);
			Main.PlaySound(SoundID.Roar, (int)player.position.X, (int)player.position.Y, 0);
			if (!Main.dayTime) {
				PurpleCometEvent.PurpleComet = true;
			}
			return true;
        }
	}
}
