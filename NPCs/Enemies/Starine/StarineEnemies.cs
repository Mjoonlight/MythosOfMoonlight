using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Enemies.Starine
{
    public class Starine_Skipper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Skipper");
            Main.npcFrameCount[npc.type] = 8;
        }
        public override void SetDefaults()
        {
            npc.width = 46;
            npc.height = 38;
            npc.aiStyle = -1;
            npc.damage = 15;
            npc.defense = 2;
            npc.lifeMax = 80;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
        }
        public override void DrawEffects(ref Color drawColor)
        {
            drawColor = Color.White;
        }
        float State = 0;
        float JumpsElapsed = 0;
        float Timer = 0;
        public override void FindFrame(int frameHeight)
        {
            switch (State)
            {
                case 0:
                    {
                        if (npc.velocity.Y >= 0)
                        {
                            npc.frameCounter = 7;
                        }
                        else
                        {
                            if (Timer < 8)
                            {
                                npc.frameCounter = 5;
                            }
                            else
                            {
                                npc.frameCounter = 4;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        if (Timer % 4 == 0) npc.frameCounter ++;
                        if (npc.frameCounter > 3)
                        {
                            npc.frameCounter = 0;
                        }
                        break;
                    }
                case 2:
                    {
                        if (Timer % 4 == 0) npc.frameCounter ++;
                        if (npc.frameCounter > 3)
                        {
                            npc.frameCounter = 0;
                        }
                        break;
                    }
            }
            npc.frame.Y = (int)npc.frameCounter * frameHeight;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNight.Chance * .05f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
            }
            if (npc.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                    Main.dust[dust].scale = 2f;
                }
                for (int i = 0; i < 4; i++)
                {
                    Gore.NewGore(npc.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, mod.GetGoreSlot("Gores/Enemies/Starine"));
                }
            }
        }
        float LastXValue
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }
        public override void AI()
        {
            Lighting.AddLight(npc.Center, new Vector3(.25f, .3f, .4f));
            Player target = Main.player[npc.target];
            npc.TargetClosest(false);
            switch (State)
            {
                //Falling waiting to be grounded
                case 0:
                    {
                        Timer++;
                        if (npc.velocity.Y == 0)
                        {
                            npc.velocity.X = 0;
                            if (npc.oldVelocity.X == 0) State = 3;
                            else State = (JumpsElapsed % 4f == 0) ? 2f : 1f;
                            Timer = (JumpsElapsed % 4f == 0) ? 30f : (8f * ((npc.life < npc.lifeMax / 2) ? (npc.life / npc.lifeMax) : 1f));
                        }
                        break;
                    }
                //Jumping for joy
                case 1:
                    {
                        Timer--;
                        if (Timer <= 0)
                        {
                            npc.direction = npc.spriteDirection = (target.position.X > npc.position.X) ? 1 : -1;
                            npc.velocity = new Vector2(4f * npc.direction, -7f);
                            JumpsElapsed++;
                            State = 0;
                        }
                        break;
                    }
                //ZZZ...
                case 2:
                    {
                        Timer--;
                        if (Timer <= 0)
                        {
                            JumpsElapsed++;
                            State = 1;
                        }
                        break;
                    }
                //Jump backwards to fix psotion
                case 3:
                    Timer--;
                    if (Timer <= 0)
                    {
                        npc.direction = npc.spriteDirection = (target.position.X > npc.position.X) ? -1 : 1;
                        npc.velocity += new Vector2((Main.rand.NextFloat() - .5f + 4f) * npc.direction, -7f);
                        JumpsElapsed++;
                        State = 0;
                    }
                    break;
            }
        }
    }
    public class Starine_Sightseer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Sightseer");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 38;
            npc.aiStyle = -1;
            npc.damage = 0;
            npc.defense = 3;
            npc.lifeMax = 140;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
        }
        float TargetY;
        float sineTime = 10f;
        int Timer = 0;
        public override void AI()
        {
            Lighting.AddLight(npc.Center, new Vector3(.25f, .3f, .4f));
            //SpawnNPC() wasn't working as intended, so this equates to something happening as soon as the npc is spawned, I think
            if (Timer == 0)
            {
                TargetY = npc.position.Y;
            }
            else npc.position.Y = MathHelper.Lerp(npc.position.Y, (float)(TargetY + Math.Sin(Timer / 20f) * sineTime), 0.05f);
            Timer++;
            if (npc.life < npc.lifeMax)
            {
                npc.damage = 19;
                npc.TargetClosest(false);
            }

            int minDist = 128;
            bool exceedMin = Helper.HorizontalDistance(npc.Center, Main.player[npc.target].Center) <= minDist;
            if (Main.player[npc.target].active)
            {
                TargetY = Main.player[npc.target].position.Y;
                if (!exceedMin || npc.direction == 0)
                {
                    npc.direction = (Main.player[npc.target].position.X > npc.position.X) ? 1 : -1;
                }
                npc.spriteDirection = (Main.player[npc.target].position.X > npc.position.X) ? 1 : -1;
                sineTime = MathHelper.Lerp(sineTime, 110f, 0.01f);
            }

            npc.velocity.X = MathHelper.Lerp(npc.velocity.X, npc.direction * 2f, 0.05f);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
            }
            if (npc.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                    Main.dust[dust].scale = 2f;
                }
                for (int i = 0; i < 4; i++)
                {
                    Gore.NewGore(npc.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, mod.GetGoreSlot("Gores/Enemies/Starine"));
                }
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNight.Chance * .05f;
        }
        public override void DrawEffects(ref Color drawColor)
        {
            drawColor = Color.White;
        }
        public override void FindFrame(int frameHeight)
        {
            if (Timer % (npc.life < npc.lifeMax ? 4 : 6) == 0) npc.frameCounter++;
            if (npc.frameCounter > 3)
            {
                npc.frameCounter = 0;
            }
            npc.frame.Y = (int)npc.frameCounter * frameHeight;
        }
    }

}