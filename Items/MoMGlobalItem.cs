using MythosOfMoonlight.Items.Weapons.Ranged;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items
{
    public class MoMGlobalItem : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.OceanCrate || item.type == ItemID.OceanCrateHard)
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Shrimpy>(), 5));
            }
        }
    }
}
