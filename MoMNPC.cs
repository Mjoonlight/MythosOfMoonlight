using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace MythosOfMoonlight
{
    public class MoMNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            // base.EditSpawnPool(pool, spawnInfo);

            if (PurpleCometEvent.PurpleComet && Main.LocalPlayer.ZoneOverworldHeight)
            {
                pool.Clear();
                for (int i = 0; i < PurpleCometEvent.PurpleCometCritters.Length; i++)
                {
                    var type = PurpleCometEvent.PurpleCometCritters[i];
                    if (!pool.ContainsKey(type))
                    {
                        pool.Add(type, 1f);
                    }
                }
                for (int i = 0; i < PurpleCometEvent.StarineEntities.Length; i++)
                {
                    var type = PurpleCometEvent.StarineEntities[i];
                    if (!pool.ContainsKey(type))
                    {
                        pool.Add(type, 0.1f);
                    }
                }
                for (int i = 0; i < PurpleCometEvent.RarePurpleCometEnemies.Length; i++)
                {
                    var type = PurpleCometEvent.RarePurpleCometEnemies[i];
                    if (!pool.ContainsKey(type))
                    {
                        pool.Add(type, 0.05f);
                    }
                }
            }
        }
        /*
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (PurpleCometEvent.PurpleComet)
            {
                spawnRate = 50;
                maxSpawns = 255;
            }
        }
        */
    }   
}
