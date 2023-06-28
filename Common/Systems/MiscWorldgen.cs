using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim;
using MythosOfMoonlight.Tiles;
using MythosOfMoonlight.Tiles.Furniture.Pilgrim;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
namespace MythosOfMoonlight.Common.Systems
{
    public class MiscWorldgen : ModSystem
    {
        public void GenStruct(GenerationProgress progress, GameConfiguration _)
        {
            int baseCheckX = Main.maxTilesX / 2 + 100;
            int baseCheckY = 200;

            for (int it = 0; it < 13; it++)
            {
                while (!Main.tile[baseCheckX + it, baseCheckY + 5].HasTile || Main.tile[baseCheckX + it, baseCheckY].TileType == TileID.Cloud || Main.tile[baseCheckX + it, baseCheckY].TileType == TileID.Sunplate)
                    baseCheckY++;
            }
            for (int l = 0; l < 10; l++)
            {
            }
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 7, new PassLegacy("Generating the Priestess Shrine", GenStruct));
            }
        }
    }
}
