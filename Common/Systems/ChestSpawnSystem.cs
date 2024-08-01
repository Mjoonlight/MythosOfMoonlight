using MythosOfMoonlight.Items.Weapons.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Items.Weapons.Ranged;
using MythosOfMoonlight.Items.Accessories;

namespace MythosOfMoonlight.Common.Systems
{
    public class ChestSpawnSystem : ModSystem
    {
        void FillChests()
        {
            int[] goldChestMainLoot = { ModContent.ItemType<RustyWaraxe>() };
            int[] goldChestSecondaryLoot = { ModContent.ItemType<GoldenTip>() };
            int[] waterChestMainLoot = { ModContent.ItemType<Shrimpy>() };
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers)
                {
                    if (Main.tile[chest.x, chest.y].TileFrameX == 1 * 36)
                    {
                        if (WorldGen.genRand.NextBool(5))
                            chest.item[0].SetDefaults(Main.rand.Next(goldChestMainLoot));
                        if (WorldGen.genRand.NextBool(5))
                            chest.item[1].SetDefaults(Main.rand.Next(goldChestSecondaryLoot));
                    }
                    if (Main.tile[chest.x, chest.y].TileFrameX == 17 * 36)
                    {
                        if (WorldGen.genRand.NextBool(5))
                        {
                            chest.item[0].SetDefaults(Main.rand.Next(waterChestMainLoot));
                        }
                    }
                }
            }
        }
        public override void PostWorldGen()
        {
            FillChests();
        }
    }
}
