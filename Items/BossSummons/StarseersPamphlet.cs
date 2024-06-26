﻿using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim;
using MythosOfMoonlight.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.BossSummons
{
    public class StarseersPamphlet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 30;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        bool hasChecked;
        public override bool? UseItem(Player player)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Starine_Symbol>()))
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
                                    npc.homeTileX = i - 2;
                                    npc.homeTileY = j - 1;
                                    if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.SyncNPC);
                                }
                                break;
                            }
                        }
                    }
                    hasChecked = true;
                }
                else
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<Starine_Symbol>()))
                    {
                        NPC npc = NPC.NewNPCDirect(null, player.Center, ModContent.NPCType<Starine_Symbol>());
                        npc.homeTileX = player.Center.ToTileCoordinates().X;
                        npc.homeTileY = player.Center.ToTileCoordinates().Y;
                    }
                }
            }
            return base.UseItem(player);
        }
    }
}
