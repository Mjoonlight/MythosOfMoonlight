using Microsoft.Xna.Framework;
using MythosOfMoonlight.Items.Accessories;
using MythosOfMoonlight.Items.Weapons.Ranged;
using MythosOfMoonlight.Projectiles;
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
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.GetModPlayer<MoMPlayer>().GoldenTip)
                if (item.useAmmo == AmmoID.Arrow)
                {
                    player.GetModPlayer<MoMPlayer>().GoldenTipI++;
                    if (player.GetModPlayer<MoMPlayer>().GoldenTipI > 4)
                    {
                        type = ModContent.ProjectileType<GoldTippedArrow>();

                        player.GetModPlayer<MoMPlayer>().GoldenTipI = 0;
                    }
                }
        }
    }
}
