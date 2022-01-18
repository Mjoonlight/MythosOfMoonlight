using System;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Items.PurpleComet.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Critters.PurpleComet
{
    public class CometPeep : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Peep");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.width = npc.height = 22;
            npc.friendly = true;
            npc.aiStyle = -1;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.noGravity = true;
            npc.noTileCollide = false;
        }
        public bool isCasual = true;
        public float SineCounter
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }
        public float InitialPositionY
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }
        public const float SPEED = 2, SINE_SPEED = .5f;
        public float offset = 4;
        public int state = 0;
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public float LogicFrameCounter
        {
            get => npc.ai[2];
            set => npc.ai[2] = value;
        }
        public override void AI()
        {
            LogicFrameCounter++;
            switch (state)
            {
                case -1:
                    npc.velocity = Vector2.Zero;
                    if (LogicFrameCounter++ > 60)
                    {
                        SineCounter = state = 0;
                        LogicFrameCounter = 0;
                        npc.direction = -npc.direction;
                    }
                    break;
                case 0:
                    Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, false, 0);
                    if (npc.direction == 0)
                    {
                        npc.position.Y -= offset * 16f;
                    }
                    if (LogicFrameCounter == 1)
                    {
                        npc.direction = Main.rand.NextBool(2) ? 1 : -1;
                    }
                    else if (npc.velocity.X == 0)
                    {
                        // npc.position.Y -= SINE_SPEED * 4;
                        InitialPositionY = npc.position.Y;
                        state = 1;
                    }
                    if (Main.rand.Next(120) == 0 && isCasual)
                    {
                        state = -1;
                    }
                    npc.velocity.X = npc.direction * SPEED;

                    SineCounter += 0.1f;
                    npc.position.Y += (float)Math.Sin(SineCounter) * SINE_SPEED;
                    break;
                case 1:
                    if (npc.position.X != npc.oldPosition.X)
                    {
                        state = 2;
                    }
                    npc.velocity.X = npc.direction * SPEED;
                    npc.velocity.Y = -SPEED;
                    break;
                case 2:
                    npc.frameCounter++;
                    if (npc.position.X == npc.oldPosition.X && npc.frameCounter > 5)
                    {
                        state = 1;
                        npc.frameCounter = 0;
                    }
                    else if (npc.velocity.Y != 0 && npc.frameCounter > 15)
                    {
                        state = 3;
                        npc.noGravity = false;
                        npc.frameCounter = 0;
                    }
                    npc.velocity.X = npc.direction * SPEED;
                    npc.velocity.Y = SPEED;
                    break;
                case 3:
                    if (npc.velocity.Y == 0)
                    {
                        npc.velocity.Y = -npc.oldVelocity.Y / 2;
                        state = 4;
                    }
                    else
                    {
                        npc.velocity.X = npc.direction * SPEED;
                    }
                    break;
                case 4:
                    npc.frameCounter++;
                    npc.velocity.Y *= 1.1f;
                    if (npc.frameCounter > 15)
                    {
                        SineCounter = 0;
                        npc.frameCounter = 0;
                        npc.noGravity = true;
                        npc.velocity.Y = 0;
                        state = 0;
                    }
                    break;
            }

        }

        /*
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0.2f;
        }
        */
    }
}
