using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora
{
	public class poo : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("stinkejuwilsdedrtfyghujygtrfedw");
			ItemID.Sets.SortingPriorityBossSpawns[item.type] = 999999;
		}
		public override void SetDefaults() {
			item.width = 38;
			item.height = 68;
			item.maxStack = 1;
			item.value = 9999999;
			item.rare = 6;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.consumable = false;
		}
		public override bool CanUseItem(Player player) {
			return !NPC.AnyNPCs(mod.NPCType("Mortiflora"));
		}
		public override bool UseItem(Player player) {
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Mortiflora>());
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}
	}
}