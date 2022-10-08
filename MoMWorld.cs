using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using System.IO;
using System;

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
                if (Math.Abs(offsetX) <= 20) break;
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
                        SpawnX.Add(baseCheckX + 8);
                        SpawnY.Add(baseCheckY + 3);
                        int num = NPC.NewNPC(new EntitySource_WorldGen(), SpawnX[0] * 16, (SpawnY[0] - 2) * 16, ModContent.NPCType<Starine_Symbol>());
                        Main.npc[num].homeTileX = SpawnX[0];
                        Main.npc[num].homeTileY = SpawnY[0] - 2;
                        Main.npc[num].direction = 1;
                        SymbolRespawnSystem.SymbolHome = new Vector2(SpawnX[0], SpawnY[0] - 2);
                        placed = true;
                        break;
                    }
                }
                if (placed)
                    break;
            }
        }
    }
    public class SymbolRespawnSystem : ModSystem
    {
        public static Vector2 SymbolHome = Vector2.Zero;
        public override void SaveWorldData(TagCompound tag)
        {
            var home = new List<Vector2>();
            if (SymbolHome != Vector2.Zero)
            {
                home.Add(SymbolHome);
            }
        }
        public override void OnWorldLoad()
        {
           // Starine_Symbol.symbol = null;
            if (SymbolHome != Vector2.Zero)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<Starine_Symbol>()))
                    {
                        NPC symbol = NPC.NewNPCDirect(null, SymbolHome + new Vector2(0, 16), ModContent.NPCType<Starine_Symbol>());
                        symbol.homeTileX = (int)SymbolHome.X;
                        symbol.homeTileY = (int)SymbolHome.Y + 16;
                    }
                }
            }
        }
        public override void LoadWorldData(TagCompound tag)
        {
            var home = tag.GetList<Vector2>("home");
            SymbolHome = home[0];
        }
        public override void NetSend(BinaryWriter writer)
        {
            Vector2 flag = SymbolHome;
            writer.WriteVector2(flag);
        }
        public override void NetReceive(BinaryReader reader)
        {
            SymbolHome = reader.ReadVector2();
        }
    }
}
