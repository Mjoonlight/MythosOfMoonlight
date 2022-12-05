using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim;
using MythosOfMoonlight.Tiles;
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
    public class SymbolGenSystem : WorldgenSystem
    {
        public override void OnModLoad()
        {
            StructureSize = new Vector2(10, 4);
        }
        public override void GenTask(Point16 topLeft)
        {
<<<<<<< Updated upstream
            int baseCheckX = topLeft.X - 10;
            int baseCheckY = topLeft.Y - 4;
=======
            int baseCheckX = topLeft.X;
            int baseCheckY = topLeft.Y;
>>>>>>> Stashed changes
            for (int l = 0; l < 10; l++)
            {
                for (int m = 0; m < 4; m++)
                {
                    WorldGen.KillTile(baseCheckX + l, baseCheckY + m, false, false, false);
                }
                WorldGen.PlaceTile(baseCheckX + l, baseCheckY + 4, TileID.IceBrick, false, true, -1, 0);
                Tile t = Main.tile[baseCheckX + l, baseCheckY + 4];
                t.Slope = SlopeType.Solid;
            }
            WorldGen.PlaceTile(baseCheckX + 1, baseCheckY + 3, TileID.Bookcases, false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 3, baseCheckY + 3, TileID.TeamBlockBluePlatform, false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 3, baseCheckY + 2, TileID.PlatinumCandle, false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 5, baseCheckY + 3, TileID.Beds, false, true, -1, 0);
            WorldGen.PlaceTile(baseCheckX + 7, baseCheckY + 1, ModContent.TileType<SymbolPointTile>(), false, true, -1, 0);
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Generating Starine Symbol Spawnpoint", delegate (GenerationProgress progress, GameConfiguration configuration)
                {
<<<<<<< Updated upstream
                    StructureGenCheck(.05f, new List<int> { TileID.Dirt, TileID.Grass }, .45f, .55f, .2f, (float)(Main.worldSurface / Main.maxTilesY), 1);
=======
                    StructureGenCheck(.1f, new List<int> { TileID.Grass }, new List<int> { TileID.Grass, TileID.Dirt }, .45f, .55f, .2f, (float)(Main.worldSurface / Main.maxTilesY), 2);
>>>>>>> Stashed changes
                }));
            }
        }
    }
}
