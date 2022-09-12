using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using MythosOfMoonlight.Buffs;
using MythosOfMoonlight.BiomeManager;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MythosOfMoonlight
{
    public class MoMNPC : GlobalNPC
    {
        public static float AlivePlayerNum = 0;
        public static float EscapeDelay = 0;
        public static List<int> TargetedPlayer = new();
        public static void PlayerCheck(float checkradian, NPC npc)
        {
            AlivePlayerNum = 0;
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead)
                {
                    if (Vector2.Distance(npc.Center, player.Center) <= checkradian)
                    {
                        if (!TargetedPlayer.Contains(player.whoAmI)) TargetedPlayer.Add(player.whoAmI);
                        AlivePlayerNum++;
                    }
                }
                else
                {
                    if (TargetedPlayer.Contains(player.whoAmI)) TargetedPlayer.Remove(player.whoAmI);
                }
            }
            if (AlivePlayerNum > 0) npc.TargetClosest(true);
        }
        public static void EscapeCheck(float checkradian, float delaytime, NPC npc)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player target = Main.player[npc.target];
                if (target == null || !target.active || target.dead)
                {
                    PlayerCheck(checkradian, npc);
                }
                else
                {
                    if (Vector2.Distance(target.Center, npc.Center) > checkradian)
                    {
                        PlayerCheck(checkradian, npc);
                    }
                    else
                    {
                        if (!TargetedPlayer.Contains(target.whoAmI)) TargetedPlayer.Add(target.whoAmI);
                    }
                }
                if (TargetedPlayer.Count <= 0) EscapeDelay++;
                else EscapeDelay = 0;
                if (EscapeDelay >= delaytime)
                {
                    npc.active = false;
                }
            }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            // base.EditSpawnPool(pool, spawnInfo);

            if (PurpleCometEvent.PurpleComet && Main.LocalPlayer.ZoneOverworldHeight)
            {
                bool HasStarineEnemies = NPC.AnyNPCs(ModContent.NPCType<NPCs.Enemies.Starine.Starine_Scatterer>()) || NPC.AnyNPCs(ModContent.NPCType<NPCs.Enemies.Starine.Starine_Skipper>());
                pool.Clear();
                for (int i = 0; i < PurpleCometEvent.PurpleCometCritters.Length; i++)
                {
                    var type = PurpleCometEvent.PurpleCometCritters[i];
                    if (!pool.ContainsKey(type))
                    {
                        pool.Add(type, .2f);
                    }
                }
                for (int i = 0; i < PurpleCometEvent.StarineEntities.Length; i++)
                {
                    var type = PurpleCometEvent.StarineEntities[i];
                    if (!pool.ContainsKey(type))
                    {
                        pool.Add(type, HasStarineEnemies?0:.04f);
                    }
                }
                for (int i = 0; i < PurpleCometEvent.RarePurpleCometEnemies.Length; i++)
                {
                    var type = PurpleCometEvent.RarePurpleCometEnemies[i];
                    if (!pool.ContainsKey(type))
                    {
                        pool.Add(type, HasStarineEnemies ? 0 : .015f);
                    }
                }
                for (int i = 0; i < PurpleCometEvent.NotThatRareEnemies.Length; i++)
                {
                    var type = PurpleCometEvent.NotThatRareEnemies[i];
                    if (!pool.ContainsKey(type))
                    {
                        pool.Add(type, HasStarineEnemies ? 0 : .0275f);
                    }
                }
            }
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(ModContent.BuffType<NPCsuffocating>()))
            {
                damage = 5;
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (PurpleCometEvent.PurpleComet)
            {
                //spawnRate = 1;
                //maxSpawns = 50;
            }
        }
    }
}
