using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim;
using MythosOfMoonlight.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Common.Systems
{
    public class GenericSystem : ModSystem
    {
        bool hasChecked;
        public override void PostUpdateEverything()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Starine_Symbol>()))
            {
                if (Main.dayTime)
                {
                    if (!hasChecked)
                    {
                        for (int i = 1; i < Main.maxTilesX; i++)
                        {
                            for (int j = 1; j < Main.maxTilesY; j++)
                            {
                                Tile tile = Main.tile[i, j];
                                if (tile.TileType == ModContent.TileType<SymbolPointTile>())
                                {
                                    if (!NPC.AnyNPCs(ModContent.NPCType<Starine_Symbol>()))
                                    {
                                        NPC npc = NPC.NewNPCDirect(null, (i - 1) * 16, (j - 2) * 16, ModContent.NPCType<Starine_Symbol>());
                                        npc.homeTileX = i;
                                        npc.homeTileY = j - 1;
                                        if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.SyncNPC);
                                    }
                                    break;
                                }
                            }
                        }
                        hasChecked = true;
                    }
                }
                else
                    hasChecked = false;
            }
        }
    }
}

