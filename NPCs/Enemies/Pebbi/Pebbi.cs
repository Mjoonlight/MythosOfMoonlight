using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace MythosOfMoonlight.NPCs.Enemies.Pebbi
{
    public class Pebbi1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pebbi");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 22;
            npc.height = 20;
            npc.damage = 12;
            npc.lifeMax = 30;
            npc.defense = 2;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath43;
            npc.buffImmune[BuffID.Confused] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.aiStyle = 3;
            npc.modNPC.aiType = NPCID.GoblinScout;
            npc.netAlways = true;
        }
        public override void AI()
        {
            npc.spriteDirection = npc.direction;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2 * hitDirection, -1.5f);
            }
            if (npc.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2 * hitDirection, -1.5f);
                }
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Cavern.Chance * 0.07f;
        }
        public override void NPCLoot()
        {
            int ammount = Main.rand.Next(0, 3) + 2;
            Item.NewItem(npc.getRect(), ItemID.StoneBlock, ammount);
        }
        //
        //
        //

        public class Pebbi2 : ModNPC
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Pebbi");
                Main.npcFrameCount[npc.type] = 4;
            }

            public override void SetDefaults()
            {
                npc.width = 20;
                npc.height = 26;
                npc.damage = 12;
                npc.lifeMax = 30;
                npc.defense = 8;
                npc.HitSound = SoundID.NPCHit7;
                npc.DeathSound = SoundID.NPCDeath43;
                npc.buffImmune[BuffID.Confused] = true;
                npc.buffImmune[BuffID.Poisoned] = true;
                npc.buffImmune[BuffID.OnFire] = true;
                npc.buffImmune[BuffID.Venom] = true;
                npc.knockBackResist = 0.5f;
                npc.aiStyle = 3;
                npc.modNPC.aiType = NPCID.SnowFlinx;
                npc.netAlways = true;
            }
            public override void AI()
            {
                npc.spriteDirection = npc.direction;
            }
            public override void HitEffect(int hitDirection, double damage)
            {
                for (int i = 0; i < 4; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2 * hitDirection, -1.5f);
                }
                if (npc.life <= 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2 * hitDirection, -1.5f);
                    }
                }
            }
            public override float SpawnChance(NPCSpawnInfo spawnInfo)
            {
                return SpawnCondition.Cavern.Chance * 0.07f;
            }

            public override void NPCLoot()
            {
                int ammount = Main.rand.Next(0, 3) + 2;
                Item.NewItem(npc.getRect(), ItemID.StoneBlock, ammount);
            }
        }

        //
        //
        //
        public class Pebbi3 : ModNPC
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Pebbi");
                Main.npcFrameCount[npc.type] = 5;
            }

            public override void SetDefaults()
            {
                npc.width = 16;
                npc.height = 16;
                npc.damage = 12;
                npc.lifeMax = 30;
                npc.defense = 2;
                npc.knockBackResist = 1.5f;
                npc.HitSound = SoundID.NPCHit7;
                npc.DeathSound = SoundID.NPCDeath43;
                npc.buffImmune[BuffID.Confused] = true;
                npc.buffImmune[BuffID.Poisoned] = true;
                npc.buffImmune[BuffID.OnFire] = true;
                npc.buffImmune[BuffID.Venom] = true;
                npc.aiStyle = 3;
                npc.modNPC.aiType = NPCID.SnowFlinx;
                npc.netAlways = true;
            }
            public override void HitEffect(int hitDirection, double damage)
            {
                for (int i = 0; i < 4; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2 * hitDirection, -1.5f);
                }
                if (npc.life <= 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2 * hitDirection, -1.5f);
                    }
                }
            }
            public override float SpawnChance(NPCSpawnInfo spawnInfo)
            {
                return SpawnCondition.Cavern.Chance * 0.07f;
            }

            public override void NPCLoot()
            {
                int ammount = Main.rand.Next(0, 3) + 2;
                Item.NewItem(npc.getRect(), ItemID.StoneBlock, ammount);
            }
        }
    }
}