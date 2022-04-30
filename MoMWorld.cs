using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim;
using Terraria.ID;
using Terraria.DataStructures;

namespace MythosOfMoonlight
{
    public class MoMWorld : ModSystem
    {
        public static List<int> SpawnX = new()
        {
        };
        public static List<int> SpawnY = new()
        {
        };
        public static int GetWorldSize()
        {
            if (Main.maxTilesX <= 4200)
            {
                return 1;
            }
            if (Main.maxTilesX <= 6400)
            {
                return 2;
            }
            if (Main.maxTilesX <= 8400)
            {
                return 3;
            }
            return 1;
        }
        public override void PostWorldGen()
        {
            int positionX = Main.spawnTileX - Main.rand.Next(100, 250) * (int)Main.rand.Next(new float[] { -1, 1 }) * GetWorldSize();
            int positionY = Main.spawnTileY - Main.rand.Next(-5, 5);
            bool placed = false;
            List<int> ProperBlock = new()
            {
                1,
                2,
                0,
                147,
                161,
                40,
                TileID.Mud,
                TileID.Sand
            };
            for (int offsetX = -200; offsetX <= 200; offsetX++)
            {
                for (int offsetY = -50; offsetY <= 50; offsetY++)
                {
                    int baseCheckX = positionX + offsetX;
                    int baseCheckY = positionY + offsetY;
                    bool canPlaceStatueHere = true;
                    for (int i = 0; i < 15; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (WorldGen.SolidOrSlopedTile(Framing.GetTileSafely(baseCheckX + i, baseCheckY + j)))
                            {
                                canPlaceStatueHere = false;
                                break;
                            }
                        }
                    }
                    for (int k = 0; k < 10; k++)
                    {
                        Tile tile = Framing.GetTileSafely(baseCheckX + k, baseCheckY + 4);
                        if (!WorldGen.SolidTile(tile) || !ProperBlock.Contains((int)tile.TileType))
                        {
                            canPlaceStatueHere = false;
                            break;
                        }
                    }
                    if (canPlaceStatueHere)
                    {
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
                        placed = true;
                        SpawnX.Add(baseCheckX + 8);
                        SpawnY.Add(baseCheckY + 3);
                        int num = NPC.NewNPC(new EntitySource_WorldGen(), SpawnX[0] * 16, (SpawnY[0] - 1) * 16, ModContent.NPCType<Starine_Symbol>());
                        Main.npc[num].homeTileX = SpawnX[0];
                        Main.npc[num].homeTileY = SpawnY[0] - 1;
                        Main.npc[num].direction = 1;
                        break;
                    }
                }
                if (placed)
                    break;
            }
        }
    }
}
