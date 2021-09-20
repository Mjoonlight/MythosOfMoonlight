using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MythosOfMoonlight.Items.Mortiflora.MortMask
{
	[AutoloadEquip(EquipType.Head)]
	public class MortMask : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("");
		}

		public override void SetDefaults() {
			item.width = 24;
			item.height = 32;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
		}
	}
}