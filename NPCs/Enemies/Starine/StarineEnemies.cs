using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

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
        public override void AI()
        {
            Player target = Main.player[npc.target];
            npc.TargetClosest();
            switch (State)
            {
                //Falling waiting to be grounded
                case 0:
                    {
                        Timer++;
                        if (npc.velocity.Y == 0)
                        {
                            npc.velocity.X = 0;
                            State = (JumpsElapsed % 4f == 0) ? 2f : 1f;
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
                            npc.velocity += new Vector2(4f * ((target.position.X > npc.position.X) ? 1f : -1f), -7f);
                            npc.direction = (target.position.X > npc.position.X) ? 1 : -1;
                            npc.spriteDirection = (target.position.X > npc.position.X) ? 1 : -1;
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
                            State = 0;
                        }
                        break;
                    }
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
        }
        float TargetY;
        public override int SpawnNPC(int tileX, int tileY)
        {
            Vector2 spawnvect = new Vector2(tileX, tileY);
            TargetY = spawnvect.ToWorldCoordinates().Y;
            return base.SpawnNPC(tileX, tileY);
        }
        public override void AI()
        {
            if (npc.life < npc.lifeMax)
            {
                npc.TargetClosest();
            }
            if (Main.player[npc.target].active)
            {
                TargetY = Main.player[npc.target].position.Y;
                npc.direction = (Main.player[npc.target].position.X > npc.position.X) ? 1 : -1;
                npc.spriteDirection = (Main.player[npc.target].position.X > npc.position.X) ? 1 : -1;
            }
            switch (npc.direction)
            {
                case 1:
                    {
                        if (npc.velocity.X < 3f) npc.velocity.X++;
                        break;
                    }
                case -1:
                    {
                        if (npc.velocity.X > -3f) npc.velocity.X--;
                        break;
                    }
            }
            if (npc.position.Y <= TargetY - 50f)
            {
                if (npc.velocity.Y < 5f) npc.velocity.Y++;
            }
            else if (npc.position.Y >= TargetY + 50f)
            {
                if (npc.velocity.Y > -5f) npc.velocity.Y--;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1)
            {
                return SpawnCondition.OverworldNight.Chance * .05f;
            }
            return 0;
        }
        public override void DrawEffects(ref Color drawColor)
        {
            drawColor = Color.White;
        }
        int Timer = 0;
        public override void FindFrame(int frameHeight)
        {
            Timer++;
            if (Timer % 4 == 0) npc.frameCounter++;
            if (npc.frameCounter > 3)
            {
                npc.frameCounter = 0;
            }
            npc.frame.Y = (int)npc.frameCounter * frameHeight;
        }
    }

}