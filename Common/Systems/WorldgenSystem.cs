using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Common.Systems
{
    public class WorldgenSystem : ModSystem
    {
        public Vector2 StructureSize;
        public List<int> SafeBlock = new()
        {
            TileID.Trees,
            TileID.VanityTreeSakura,
            TileID.VanityTreeYellowWillow,
            TileID.PalmTree,
            TileID.PineTree,
            TileID.VanityTreeSakuraSaplings,
            TileID.VanityTreeWillowSaplings,
            TileID.Grass,
            TileID.JungleGrass,
            TileID.HallowedGrass,
            TileID.MushroomGrass,
            TileID.BloomingHerbs,
            TileID.ImmatureHerbs,
            TileID.MatureHerbs,
            TileID.LargePiles,
            TileID.LargePiles2,
            TileID.SmallPiles,
            TileID.ShellPile,
            TileID.BeachPiles
            };
        public virtual void GenTask(Point16 topLeft)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">TileID</param>
        /// <param name="amount">Exact amount of ores generated in world</param>
        /// <param name="strength"></param>
        /// <param name="steps"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="mindepth"></param>
        /// <param name="maxdepth"></param>
        public void OreGen(int type, int amount = -1, float strength = 5, int steps = 5, float left = -1, float right = -1, int mindepth = -1, int maxdepth = -1)
        /*
         * type: TileID. amount: exact number of ores generated in world. strength & steps: OreRunner's passes. left & right: the min and max horizonal position(ratio). mindepth and maxdepth: the min and max depth(world surface as the start).
         */
        {
            if (WorldGen.noTileActions) return;
            if (type < 0) return;
            if (left < 0) left = 0;
            if (right < 0) right = 1;
            if (mindepth < 0) mindepth = 0;
            if (maxdepth < 0) maxdepth = 100;
            /*For default, the gen area is 100 tiles below the surface, across the whole world.
             */
            if (amount < 0) amount = (int)((float)Main.maxTilesX / 4200 * 100);
            /*For default, the gen amount is 100 blocks.
             */
            for (int i = 0; i < amount; i++)
            {
                int posX = WorldGen.genRand.Next((int)(float)(Main.maxTilesX * left), (int)(float)(Main.maxTilesX * right));
                int posY = WorldGen.genRand.Next(mindepth + (int)Main.worldSurface, maxdepth + (int)Main.worldSurface);
                WorldGen.OreRunner(posX, posY, strength, steps, (ushort)type);
            }
        }
        public void KillParticularTile(List<int> types, float x1 = 0, float x2 = 1, float y1 = 0, float y2 = 1, bool killLiquid = true)
        /*
         * types: a list of TileIDs that you want to remove. x1 and x2: the left and right boundary of the check. y1 and y2: the top and bottom boundary of the check. killLiquid: remove liquid.
         */
        {
            int minX = (int)(x1 * Main.maxTilesX);
            int maxX = (int)(x2 * Main.maxTilesX);
            int minY = (int)(y1 * Main.maxTilesY);
            int maxY = (int)(y2 * Main.maxTilesY);
            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    if (types.Contains(Main.tile[i, j].TileType))
                    {
                        WorldGen.KillTile(i, j);
                        if (killLiquid)
                        {
                            Main.tile[i, j].LiquidAmount = 0;
                        }
                    }
                }
            }
        }
        private bool BasementCheck(List<int> BaseBlock,Point tile,int halfWidth, int halfHeight)
        {
            bool canSpawn = false;
            bool l1 = false;
            bool l2 = false;
            bool center = false;
            bool r1 = false;
            bool r2 = false;
            for (int i = -halfHeight; i <= halfHeight; i++)
            {
                Tile t = Main.tile[tile.X, tile.Y + i];
                if (t.HasTile && BaseBlock.Contains(t.TileType)) center = true;
                int j = halfWidth / 2;
                Tile t1 = Main.tile[tile.X - j, tile.Y + i];
                Tile t2 = Main.tile[tile.X + j, tile.Y + i];
                if(t1.HasTile && BaseBlock.Contains(t1.TileType)) l1 = true;
                if (t2.HasTile && BaseBlock.Contains(t2.TileType)) r1 = true;
                Tile t3 = Main.tile[tile.X - halfWidth, tile.Y + i];
                Tile t4 = Main.tile[tile.X - halfWidth, tile.Y + i];
                if (t3.HasTile && BaseBlock.Contains(t3.TileType)) l2 = true;
                if (t4.HasTile && BaseBlock.Contains(t4.TileType)) r2 = true;
            }
            if (l1 && l2 && r1 && r2 && center) canSpawn = true;
            return canSpawn;
        }
        public void StructureGenCheck(float chance, List<int> BaseBlock, List<int> ProperBlock, float left = 0, float right = 1, float mindepth = 0, float maxdepth = 1, int depthOffset = 10)
        /*
         * chance: the chance of each block in world get checked. ProperBlock: a list containing the TileIDs of the blocks that can be used as its base.
         * depthOffset: the foundation thickness of the structure.
         */
        {
            int mX = Main.maxTilesX;
            int mY = Main.maxTilesY;
            int minY = (int)(mY * mindepth) + (int)StructureSize.Y + depthOffset;
            int maxY = (int)(mY * maxdepth) - (int)StructureSize.Y - depthOffset;
            //Offset the boundary of posY according to structure size, to prevent it have some part generated outside of the world.
            bool place = false;
            //Set the bool "place" to false. Upon actual generation, change this to true and jump out of loop.
            int trial = (int)(mX * mY * chance);
            //Maximum number of gen check attempts is decided by the world size and parameter"chance".
            for (int a = 0; a < trial; a++)
            {
                int tileX = WorldGen.genRand.Next((int)StructureSize.X + (int)(mX * left), (int)(mX * right) - (int)StructureSize.X);
                //This is the randomized posX which is offseted by the structure size, in order to prevent the structure part generated outside of the world.
                if (maxY > minY)
                {
                    int tileY = WorldGen.genRand.Next(minY, maxY);
                    bool canplace = true;
                    bool canplace1 = false;
                    bool canplace2 = false;
                    /*
                     * canplace: decide if the place is proper for structure gen. any requirement not met will set this to false.
                     * canplace1 & canplace2 : decide if the place left/right to the center is proper( have tiles under it and no tiles above).
                     */
                    for (int c = 2; c <= (int)(StructureSize.Y); c++)
                    //Check the place directly above the tile.
                    {
                        Tile tile = Main.tile[tileX, tileY - c];
                        if (tile.HasTile && !SafeBlock.Contains(tile.TileType))
                        {
                            canplace = false;
                            break;
                        }
                        //If there's tile above it and is not trees, this is not a proper place for generation.
                    }
                    Tile tile1 = Main.tile[tileX, tileY];
                    if (!BaseBlock.Contains(tile1.TileType) || !tile1.HasTile)
                    {
                        canplace = false;
                    }
                    if (!BasementCheck(BaseBlock, new Point(tileX, tileY), (int)StructureSize.X / 2, depthOffset)) canplace = false;
                    /*
                     * If the place has no solid tiles or the type doesn't meet the need, this is not a proper place for generation.
                     */
                    for (int b = 0; b <= (int)(StructureSize.X / 2) + 1; b += (int)Math.Min((int)((StructureSize.X + 4) / 4), 5))
                    /*
                     * Check the area right to the position.
                     */
                    {
                        bool can1 = false;
                        bool can2 = true;
                        for (int d = 0; d <= depthOffset; d += (int)Math.Min(depthOffset, 2))
                        {
                            Tile tile = Main.tile[tileX + b, tileY + d];                            if (tile.HasTile && (BaseBlock.Contains(tile.TileType) || SafeBlock.Contains(tile.TileType)))
                            {
                                can1 = true;
                                break;
                            }
                        }
                        /*
                         * Check if there's solid tiles of certain types below the right side of structure bottom. If false, this is not a proper place for it.
                         */
                        for (int e = 0; e <= depthOffset; e += (int)Math.Min(depthOffset, 1))
                        {
                            Tile tile = Main.tile[tileX + b, tileY - e];
                            if (tile.HasTile && tile.TileType != TileID.Trees)
                            {
                                can2 = false;
                                break;
                            }
                        }
                        /*
                         * Check if there's no tile blocked above the right side of structure bottom. If false, this is not a proper place for it.
                         */
                        if (can1 && can2)
                        {
                            canplace1 = true;
                            break;
                        }
                    }
                    for (int f = 0; f <= (int)(StructureSize.X / 2) + 1; f += (int)Math.Min((int)((StructureSize.X + 4) / 4), 5))
                    {
                        /*
                         * Again check the left side of the structure.
                         */
                        bool can1 = true;
                        bool can2 = true;
                        for (int d = 0; d <= depthOffset; d += (int)Math.Min(depthOffset, 2))
                        {
                            Tile tile = Main.tile[tileX - f, tileY + d];
                            if (!tile.HasTile || (!ProperBlock.Contains(tile.TileType) && !SafeBlock.Contains(tile.TileType)))
                            {
                                can1 = false;
                                break;
                            }
                        }
                        for (int e = 0; e <= depthOffset; e += (int)Math.Min(depthOffset, 2))
                        {
                            Tile tile = Main.tile[tileX - f, tileY - e];
                            if (tile.HasTile && !SafeBlock.Contains(tile.TileType))
                            {
                                can2 = false;
                                break;
                            }
                        }
                        if (can1 && can2)
                        {
                            canplace2 = true;
                            break;
                        }
                    }
                    if (!canplace1 || !canplace2)
                    {
                        canplace = false;
                    }
                    /*
                     * If both sides of the structure have met the need of tile check, generate the house.
                     */
                    if (canplace)
                    {
                        Point16 pos = new(tileX, tileY - (int)StructureSize.Y);
                        for (int x = -(int)StructureSize.X / 2; x <= (int)StructureSize.X / 2; x++)
                        {
                            for (int y = 0; y <= (int)StructureSize.Y; y++)
                            {
                                Main.tile[pos.X + x, pos.Y + y].ClearEverything();
                                Main.tile[pos.X + x, pos.Y + y].LiquidAmount = 0;
                            }
                        }
                        Point16 pos2 = pos - new Point16((int)(StructureSize.X / 2), 0);
                        /*
                         * Clear liquid in the area before actually generating.
                         */
                        GenTask(pos2);
                        place = true;
                    }
                    if (place)
                    {
                        break;
                    }
                    else
                    {
                        if (a >= trial - 2)
                        {
                            StructureGenCheckAgain(chance, BaseBlock, ProperBlock, left, right, mindepth, maxdepth, depthOffset);
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
        public void StructureGenCheckAgain(float chance, List<int> BaseBlock, List<int> ProperBlock, float left = 0, float right = 1, float mindepth = 0, float maxdepth = 1, int depthOffset = 10)
        /*
         * chance: the chance of each block in world get checked. ProperBlock: a list containing the TileIDs of the blocks that can be used as its base.
         * depthOffset: the foundation thickness of the structure.
         */
        {
            int mX = Main.maxTilesX;
            int mY = Main.maxTilesY;
            int minY = (int)(mY * mindepth) + (int)StructureSize.Y + depthOffset;
            int maxY = (int)(mY * maxdepth) - (int)StructureSize.Y - depthOffset;
            //Offset the boundary of posY according to structure size, to prevent it have some part generated outside of the world.
            bool place = false;
            //Set the bool "place" to false. Upon actual generation, change this to true and jump out of loop.
            int trial = (int)(mX * mY * chance);
            //Maximum number of gen check attempts is decided by the world size and parameter"chance".
            for (int a = 0; a < trial; a++)
            {
                int tileX = WorldGen.genRand.Next((int)StructureSize.X + (int)(mX * left), (int)(mX * right) - (int)StructureSize.X);
                //This is the randomized posX which is offseted by the structure size, in order to prevent the structure part generated outside of the world.
                if (maxY > minY)
                {
                    int tileY = WorldGen.genRand.Next(minY, maxY);
                    bool canplace = true;
                    /*
                     * canplace: decide if the place is proper for structure gen. any requirement not met will set this to false.
                     * canplace1 & canplace2 : decide if the place left/right to the center is proper( have tiles under it and no tiles above).
                     */
                    for (int c = 2; c <= (int)StructureSize.Y; c += 1)
                    //Check the place directly above the tile.
                    {
                        Tile tile = Main.tile[tileX, tileY - c];
                        if (tile.HasTile && !SafeBlock.Contains(tile.TileType))
                        {
                            canplace = false;
                            break;
                        }
                        //If there's tile above it and is not trees, this is not a proper place for generation.
                    }
                    Tile tile1 = Main.tile[tileX, tileY];
                    if (!BaseBlock.Contains(tile1.TileType) || !tile1.HasTile)
                    {
                        canplace = false;
                    }
                    /*
                     * If the place has no solid tiles or the type doesn't meet the need, this is not a proper place for generation.
                     */
                    /*
                     * If both sides of the structure have met the need of tile check, generate the house.
                     */
                    if (canplace)
                    {
                        Point16 pos = new(tileX, tileY - (int)StructureSize.Y);
                        for (int x = -(int)StructureSize.X / 2; x <= (int)StructureSize.X / 2; x++)
                        {
                            for (int y = 0; y <= (int)StructureSize.Y; y++)
                            {
                                Main.tile[pos.X + x, pos.Y + y].ClearEverything();
                                Main.tile[pos.X + x, pos.Y + y].LiquidAmount = 0;
                            }
                        }
                        Point16 pos2 = pos - new Point16((int)(StructureSize.X / 2), 0);
                        /*
                         * Clear liquid in the area before actually generating.
                         */
                        GenTask(pos2);
                        place = true;
                    }
                    if (place)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}

