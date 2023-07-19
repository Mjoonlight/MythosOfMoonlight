using Microsoft.Xna.Framework;
using MythosOfMoonlight.NPCs.Field;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim;
using MythosOfMoonlight.Tiles;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Common.Systems
{
    public class GenericSystem : ModSystem
    {
        bool hasChecked;
        public override void PostUpdateEverything()
        {
            if (FieldSpawnRateNPC.rateDecrease && !NPC.AnyNPCs(ModContent.NPCType<SunflowerLady>())) //ADD REST OF FIELD NPCS HERE (&& NOT ||)
            {
                FieldSpawnRateNPC.rateDecrease = false;
                FieldSpawnRateNPC.activeNPC = -1;
            }
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
    public class AstralShowerMusic : ModSceneEffect
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Meteor");
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override bool IsSceneEffectActive(Player player)
        {
            return !Main.dayTime && Star.starfallBoost > 2f && (player.ZoneOverworldHeight || player.ZoneSkyHeight);
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (IsSceneEffectActive(player))
            {
                if (!SkyManager.Instance["Asteroid"].IsActive())
                {
                    SkyManager.Instance.Activate("Asteroid");
                }
                if (player.ZoneOverworldHeight || player.ZoneSkyHeight)
                {
                    Filters.Scene["Asteroid"].GetShader().UseColor(Color.Blue).UseOpacity(0.1f);
                }
                player.ManageSpecialBiomeVisuals("Asteroid", isActive);
            }
            else
            {
                if (SkyManager.Instance["Asteroid"].IsActive())
                {
                    SkyManager.Instance.Deactivate("Asteroid");
                }
                player.ManageSpecialBiomeVisuals("Asteroid", false);
            }
        }
    }
}

