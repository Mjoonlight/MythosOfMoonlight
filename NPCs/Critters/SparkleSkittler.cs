using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Critters
{
    public class SparkleSkittler : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sparkle Skittler");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.friendly = true;
            npc.aiStyle = -1;
            npc.lifeMax = 5;
            npc.width = 30;
            npc.height = 22;
            npc.defense = 0;
        }
        const float SPEED = 3.5f;
        const int TRANSITION_CHANCE = 99;
        int State
        {
            get => (int)npc.ai[0];
            set => npc.ai[0] = value;
        }
        public override void AI()
        {
            switch (State)
            {
                case 0:
                    if (npc.direction == 0)
                    {
                        npc.direction = Main.rand.NextBool(2) ? 1 : -1;
                    }
                    if (Main.rand.NextBool(TRANSITION_CHANCE))
                    {
                        npc.velocity.X = 0;
                        State = 1;
                    } else if (npc.velocity.X == 0)
                    {
                        npc.direction = -npc.direction;
                    }
                    npc.velocity.X = npc.direction * SPEED;
                    break;
                case 1:
                    npc.velocity.X = 0;
                    if (Main.rand.NextBool(TRANSITION_CHANCE))
                    {
                        npc.direction = 0;
                        State = 0;
                    }
                    break;
            }
            npc.spriteDirection = npc.direction;
        }
        const int FRAME_RATE = 3;
        public override void FindFrame(int frameHeight)
        {
            switch (State)
            {
                case 0:
                    if (npc.frameCounter + 1 < FRAME_RATE * 4)
                    {
                        npc.frameCounter++;
                        if (npc.frameCounter < FRAME_RATE)
                        {
                            npc.frameCounter = FRAME_RATE;
                        }
                    } 
                    else
                    {
                        npc.frameCounter = FRAME_RATE;
                    }
                    break;
                case 1:
                    npc.frameCounter = 0;
                    break;
            }
            npc.frame.Y = (int)npc.frameCounter / FRAME_RATE * frameHeight;
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !PurpleCometEvent.PurpleComet ? 0 : 0.17f;
        }
    }
}