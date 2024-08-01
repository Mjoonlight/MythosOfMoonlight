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

namespace MythosOfMoonlight.Common.Systems
{
    public class ChestSpawnSystem : ModSystem
    {
        void RustyWaraxe()
        {
            int[] chest = { ModContent.ItemType<RustyWaraxe>() };
            int chestchoice = 0;
            for (int WchestIndex = 0; WchestIndex < 1000; WchestIndex++)
            {
                Chest Wchest = Main.chest[WchestIndex];
                if (Wchest != null && Main.tile[Wchest.x, Wchest.y].TileType == TileID.Containers && Main.tile[Wchest.x, Wchest.y].TileFrameX == 1 * 36)
                {

                    for (int WinventoryIndex = 0; WinventoryIndex < 40; WinventoryIndex++)
                    {
                        if (WorldGen.genRand.NextBool(5))
                        {

                            Wchest.item[WinventoryIndex].SetDefaults(chest[chestchoice]);

                            Wchest.item[WinventoryIndex].stack = WorldGen.genRand.Next(0, 1);

                            chestchoice = (chestchoice + 1) % chest.Length;
                            //Wchest.item[WinventoryIndex].SetDefaults(Main.rand.Next(WinventoryIndex));
                            break;
                        }
                    }
                }
            }
        }
        void ShrimpChest()
        {
            int[] waterchest = { ModContent.ItemType<Shrimpy>() };
            int waterchestchoice = 0;
            for (int WchestIndex = 0; WchestIndex < 1000; WchestIndex++)

            {

                Chest Wchest = Main.chest[WchestIndex];
                if (Wchest != null && Main.tile[Wchest.x, Wchest.y].TileType == TileID.Containers && Main.tile[Wchest.x, Wchest.y].TileFrameX == 17 * 36)
                {

                    for (int WinventoryIndex = 0; WinventoryIndex < 40; WinventoryIndex++)
                    {
                        if (WorldGen.genRand.NextBool(5))
                        {

                            Wchest.item[WinventoryIndex].SetDefaults(waterchest[waterchestchoice]);

                            Wchest.item[WinventoryIndex].stack = WorldGen.genRand.Next(0, 1);

                            waterchestchoice = (waterchestchoice + 1) % waterchest.Length;
                            //Wchest.item[WinventoryIndex].SetDefaults(Main.rand.Next(WinventoryIndex));
                            break;
                        }
                    }
                }
            }
        }
        public override void PostWorldGen()
        {
            ShrimpChest();
            RustyWaraxe(); //This is such a bad way to do it but i genuinely cant be assed
        }
    }
}
