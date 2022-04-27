using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Critters.PurpleComet;

namespace MythosOfMoonlight.Items.PurpleComet.Critters
{
	public class SparkleSkittlerItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sparkle Skittler");
		}


		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 26;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 5, 5);
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 20;
			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = true;

		}

		public override bool UseItem(Player player)
		{
			int index = NPC.NewNPC((int)(player.position.X), (int)(player.position.Y),
			  ModContent.NPCType<SparkleSkittler>());

			return true;
		}

	}
}