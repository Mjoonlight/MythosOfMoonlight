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
    /*public class SymbolGenSystem : WorldgenSystem
    {
        public override void OnModLoad()
        {
            StructureSize = new Vector2(10, 4);
        }
        public override void GenTask(Point16 topLeft)
        {
            int baseCheckX = topLeft.X;
            int baseCheckY = (int)Main.worldSurface - 200;

            for (int it = 0; it < 13; it++)
            {
                while (!Main.tile[baseCheckX + it, baseCheckY].HasTile || Main.tile[baseCheckX + it, baseCheckY].TileType == TileID.Cloud || Main.tile[baseCheckX + it, baseCheckY].TileType == TileID.Sunplate)
                    baseCheckY++;
            }
            for (int l = 0; l < 10; l++)
            {
                for (int m = 0; m < 4; m++)
                {
                    WorldGen.PlaceTile(baseCheckX + l, baseCheckY + 4 + m, TileID.Dirt, forced: true);
                    Tile tile = Main.tile[baseCheckX + l, baseCheckY + 4 + m];
                    tile.Slope = SlopeType.Solid;
                }
                for (int m = 0; m < 30; m++)
                {
                    if (Main.tile[baseCheckX + l, baseCheckY - m].TileType == TileID.Trees)
                        WorldGen.KillTile(baseCheckX + l, baseCheckY - m, false, false, false);
                }
                WorldGen.PlaceTile(baseCheckX + l, baseCheckY + 4, TileID.Dirt, false, true, -1, 0);
                Tile t = Main.tile[baseCheckX + l, baseCheckY + 4];
                t.Slope = SlopeType.Solid;
            }
            WorldGen.PlaceTile(baseCheckX + 1, baseCheckY + 3, ModContent.TileType<PilgrimLamp>(), false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 3, baseCheckY + 3, ModContent.TileType<PilgrimCan>(), false, true, -1, 0);
            //WorldGen.PlaceTile(baseCheckX + 3, baseCheckY + 2, TileID.PlatinumCandle, false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 5, baseCheckY + 3, ModContent.TileType<PilgrimBed>(), false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 7, baseCheckY + 1, ModContent.TileType<SymbolPointTile>(), false, true, -1, 0);
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Generating Starine Symbol Spawnpoint", delegate (GenerationProgress progress, GameConfiguration configuration)
                {
                    StructureGenCheck(1f, new List<int> { TileID.Grass, }, new List<int> { TileID.Grass }, .45f, .55f, .2f, (float)(Main.worldSurface / Main.maxTilesY), 2);
                }));
            }
        }
    }*/
    public class SymbolgenSystem : ModSystem
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
                for (int m = 0; m < 4; m++)
                {
                    WorldGen.PlaceTile(baseCheckX + l, baseCheckY + 4 + m, TileID.Dirt, forced: true);
                    Tile tile = Main.tile[baseCheckX + l, baseCheckY + 4 + m];
                    tile.Slope = SlopeType.Solid;
                }
                for (int m = 0; m < 30; m++)
                {
                    if (Main.tile[baseCheckX + l, baseCheckY - m].TileType == TileID.Trees)
                        WorldGen.KillTile(baseCheckX + l, baseCheckY - m, false, false, false);
                }
                WorldGen.PlaceTile(baseCheckX + l, baseCheckY + 4, TileID.Dirt, false, true, -1, 0);
                Tile t = Main.tile[baseCheckX + l, baseCheckY + 4];
                t.Slope = SlopeType.Solid;
            }
            WorldGen.PlaceTile(baseCheckX + 1, baseCheckY + 3, ModContent.TileType<PilgrimLamp>(), false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 3, baseCheckY + 3, ModContent.TileType<PilgrimCan>(), false, true, -1, 0);
            //WorldGen.PlaceTile(baseCheckX + 3, baseCheckY + 2, TileID.PlatinumCandle, false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 5, baseCheckY + 3, ModContent.TileType<PilgrimBed>(), false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 7, baseCheckY + 1, ModContent.TileType<SymbolPointTile>(), false, true, -1, 0);
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 6, new PassLegacy("Generating the Starine Site", GenStruct));
            }
        }
    }
}
