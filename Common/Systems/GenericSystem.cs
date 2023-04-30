using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim;
using MythosOfMoonlight.Tiles;
using System;
using System.Collections.Generic;
using System.IO;
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
        public override void NetReceive(BinaryReader reader)
        {
            byte type = reader.ReadByte();
            switch (type)
            {
                case 0:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = Mod.GetPacket();
                        packet.WriteVector2(reader.ReadVector2()); // idk if this method still exists
                        packet.Send();
                    }
                    Vector2 pos = reader.ReadVector2();
                    int pil = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)pos.X, (int)pos.Y, ModContent.NPCType<RupturedPilgrim>()); // add params i forgor
                    Main.npc[pil].ai[0] = 6;
                    break;
            }
        }
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

