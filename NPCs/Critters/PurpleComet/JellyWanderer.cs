using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Items.Critters;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using MythosOfMoonlight.Common.BiomeManager;
using MythosOfMoonlight.Common.Utilities;

namespace MythosOfMoonlight.NPCs.Critters.PurpleComet
{
    public class JellyWanderer : ModNPC
    {
        int[] frameTimer = new int[2];
        int mRIndex;
        float[] meanRotation = new float[8];
        float moveY;
        float[] jumpVal = new float[2] { 2, 1 };
        MathFunctions.Parabola parabola;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jelly Wanderer");
            Main.npcFrameCount[NPC.type] = 11;
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 5;
            NPC.width = 24;
            NPC.height = 36;
            NPC.defense = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.catchItem = (short)ModContent.ItemType<JellyWandererItem>();
            NPC.dontCountMe = true;
            NPC.npcSlots = 0;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamageFromHostiles = false;
            SpawnModBiomes = new int[]
            {
                ModContent.GetInstance<PurpleCometBiome>().Type
            };
        }
        public override void OnKill()
        {
            PurpleCometEvent.CritterDeath(NPC.Center);
        }
        public override void FindFrame(int frameHeight)
        {
            bool Relative(float percent) => jumpVal[0] < (int)Math.Round(jumpVal[1] * percent);
            if (Relative(0.1f))
            {
                if (Relative(0.025f))
                    NPC.frame.Y = NPC.height * 7;
                else if (Relative(0.05f))
                    NPC.frame.Y = NPC.height * 8;
                else if (Relative(0.075f))
                    NPC.frame.Y = NPC.height * 9;
                else
                    NPC.frame.Y = NPC.height * 10;
            }
            else if (Relative(0.85f))
            {
                if (frameTimer[0]++ > (int)Math.Round(jumpVal[1] * 0.025f))
                {
                    frameTimer[0] = 0;
                    if (frameTimer[1]++ > 2)
                        frameTimer[1] = 0;
                }
                NPC.frame.Y = NPC.height * frameTimer[1];
            }
            else
            {
                if (Relative(0.875f))
                    NPC.frame.Y = NPC.height * 3;
                else if (Relative(0.9f))
                    NPC.frame.Y = NPC.height * 4;
                else if (Relative(0.95f))
                    NPC.frame.Y = NPC.height * 5;
                else
                    NPC.frame.Y = NPC.height * 6;
            }

        }
        public override void AI()
        {
            // Lighting.AddLight(NPC.Center, new Vector3(255, 125, 255) * 0.01f);
            Lighting.AddLight(NPC.Center, 0.5f, 0.7f, 1f);
            int check = (int)Math.Round(NPC.velocity.Length() * 60);
            //Main.NewText(check);
            if (check > 1 && Main.rand.NextBool(check))
                Dust.NewDustDirect(NPC.Center + (new Vector2(Main.rand.Next(-NPC.width, NPC.width), Main.rand.Next(-NPC.height, NPC.height)) / 2), 0, 0, ModContent.DustType<PurpurineDust>()).velocity = Vector2.Zero;
            NPC.spriteDirection = NPC.direction = -1;
            if (jumpVal[0]++ > jumpVal[1])
            {
                Vector2 targ = new Vector2(Main.rand.Next(100, 125) * (Main.rand.NextBool(2) ? -1 : 1), Main.rand.Next(75, 125));
                parabola = MathFunctions.Parabola.ParabolaFromCoords(NPC.Center, NPC.Center + targ);
                //Main.NewText(targ);
                parabola.SetIncrement(360);
                jumpVal[0] = 0;
                float multiply = (float)Math.Sqrt(Math.Pow(targ.X, 2) + Math.Pow(targ.Y, 2));
                jumpVal[1] = (int)Math.Round(3.5f * multiply);
                NPC.velocity.Y = 0;
                moveY = 0;
                //frameState = 1;
            }
            else
            {
                if (NPC.velocity.Y < 0.5f || NPC.velocity.Y == 0)
                {
                    moveY += parabola.increment[0];
                    NPC.velocity = new Vector2(parabola.increment[0] * (parabola.backwards ? -1 : 1), parabola.GetY(moveY - parabola.increment[0]) - parabola.GetY(moveY));
                }
            }
            #region Rotation shenanigans
            meanRotation[mRIndex] = (float)((float)(NPC.velocity.X / Math.PI) + (NPC.velocity.Y / ((NPC.velocity.X > 0 ? -2 : 2) * Math.PI)));
            if (mRIndex++ > meanRotation.Length - 2)
                mRIndex = 0;
            float rotation = 0;
            for (int a = 0; a < meanRotation.Length; a++)
                rotation += meanRotation[a];
            //Main.NewText(rotation / meanRotation.Length);
            NPC.rotation = rotation / meanRotation.Length;
            #endregion
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<PurpleCometBiome>().ModBiomeBestiaryInfoElement),
                new FlavorTextBestiaryInfoElement("Jelly Wanderers are constantly on the move, and passively absorb energy from the sun, resulting in them glowing brilliantly at night. Many species of wildlife take advantage of this by using their illuminance for wayfinding.")
            });
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return !(PurpleCometEvent.PurpleComet && spawnInfo.Player.ZoneOverworldHeight) ? 0 : 0.37f;
        }
    }
}