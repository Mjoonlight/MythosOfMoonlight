using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;
using Terraria.ModLoader.Utilities;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using MythosOfMoonlight.BiomeManager;
using Steamworks;

namespace MythosOfMoonlight.NPCs.Enemies.Overworld.Starine
{
    public class Starine_Skipper : ModNPC
    {
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale);
        }
        public int f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Skipper");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.TrailCacheLength[NPC.type] = 9;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                //Frame = (int)(NPC.frameCounter / 5),
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("Powered by magic derived from the stars, it uses its well-developed back legs to hop and skip about the night, usually kicking off unsuspecting prey to cripple them.")
            });
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //3hi31mg
            var off = new Vector2(NPC.width / 2, NPC.height / 2 + 2);
            var clr = new Color(255, 255, 255, 255); // full white
            Texture2D texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Trail").Value;
            var frame = new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height);
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[NPC.type];
            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(1f, 0.95f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = NPC.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off, frame, clr * (1f - fadeMult * i), NPC.oldRot[i], orig, scale, flipType, 0f);
            }
            return true;
        }
        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 40;
            NPC.aiStyle = -1;
            NPC.damage = 15;
            NPC.defense = 2;
            NPC.lifeMax = 80;
            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath1;
            SpawnModBiomes = new int[]
            {
                ModContent.GetInstance<PurpleCometBiome>().Type
            };
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
            NPC.frameCounter++;
            if (NPC.IsABestiaryIconDummy)
            {
                if (NPC.frameCounter % 4 == 0)
                {
                    f++;
                }
                if (f > 3)
                    f = 0;
            }
            else
                switch (State)
                {
                    case 0:
                        /*if (Timer % 5 == 0)
                        {
                            if (f < 7)
                                f++;
                        }*/
                        if (NPC.velocity.Y > -1)
                        {
                            if (NPC.frameCounter % 5 == 0) f++;
                            if (f > 3)
                                f = 0;
                        }
                        else
                        {
                            if (NPC.frameCounter % 5 == 0)
                            {
                                if (f < 4)
                                    f = 4;

                                else if (f < 7)
                                    f++;
                            }
                        }
                        break;
                    case 1:
                        if (NPC.frameCounter % 5 == 0)
                        {
                            if (f < 7)
                                f++;
                            if (f < 4)
                                f = 4;
                        }
                        break;
                    case 2:
                        if (NPC.frameCounter % 5 == 0) f++;
                        if (f > 3)
                            f = 0;
                        break;
                }
            NPC.frame.Y = f * frameHeight;
        }
        public bool HasStarineEnemies = NPC.AnyNPCs(ModContent.NPCType<Starine_Scatterer>()) || NPC.AnyNPCs(ModContent.NPCType<Starine_Skipper>());
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return HasStarineEnemies ? 0 : SpawnCondition.OverworldNight.Chance * .05f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1.5f;
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                    Main.dust[dust].scale = 2f;
                    Main.dust[dust].noGravity = true;
                }
                if (Main.netMode == NetmodeID.Server)
                    return;
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                    Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, ModContent.Find<ModGore>("MythosOfMoonlight/Starine").Type);
            }
        }
        float NumZeroes
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public override void AI()
        {
            //NPC.frameCounter++;
            //if (NPC.frameCounter >= 39) NPC.frameCounter = 0;
            Lighting.AddLight(NPC.Center, new Vector3(.25f, .3f, .4f));
            Player target = Main.player[NPC.target];
            NPC.TargetClosest(false);
            if (NPC.Grounded())
            {
                switch (State)
                {
                    //Falling waiting to be grounded
                    case 0:
                        {
                            Timer++;
                            {
                                /*if (NPC.velocity.X == 0)
                                {
                                    if (NumZeroes++ < 1)
                                        State = 1;
                                    else
                                    {
                                        State = 3;
                                        NumZeroes = 0;
                                    }
                                }
                                else */
                                State = JumpsElapsed % 4f == 0 ? 2f : 1f;
                                if (JumpsElapsed % 4f != 0)
                                    f = 3;
                                Timer = JumpsElapsed % 4f == 0 ? 100f : 8f * (NPC.life < NPC.lifeMax / 2 ? NPC.life / NPC.lifeMax : 1f);
                                NPC.velocity.X *= JumpsElapsed % 4f == 0 ? 0f : 0.9f;
                                if (JumpsElapsed % 4f == 0)
                                    NPC.velocity.Y = 0;
                            }
                            break;
                        }
                    //Jumping for joy
                    case 1:
                        {
                            Timer--;
                            if (Timer <= 0)
                            {
                                NPC.direction = NPC.spriteDirection = target.position.X > NPC.position.X ? 1 : -1;
                                NPC.velocity = new Vector2(4 * NPC.direction, -7f);
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
                                NPC.velocity.X = 0;
                                JumpsElapsed++;
                                State = 1;
                                f = 4;
                            }
                            break;
                        }
                        //Jump backwards to fix psotion
                        /*case 3:
                            Timer--;
                            if (Timer <= 0)
                            {
                                NPC.direction = NPC.spriteDirection = target.position.X > NPC.position.X ? -1 : 1;
                                NPC.velocity = new Vector2((Main.rand.NextFloat() - .5f + 4f) * NPC.direction, -7f);
                                JumpsElapsed++;
                                State = 0;
                            }
                            break;*/
                }
            }
            else
                NPC.velocity.X = 4 * NPC.direction;

        }
    }
    public class Starine_Sightseer : ModNPC
    {
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Sightseer");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                Velocity = 1f,
                Position = new Vector2(0, 0),
                PortraitPositionYOverride = 0,
                Direction = 1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 38;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.defense = 3;
            NPC.lifeMax = 140;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath1;
            SpawnModBiomes = new int[]
            {
                ModContent.GetInstance<PurpleCometBiome>().Type
            };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("Brilliant and peaceful. Hovering around certain spots, they seem to study the environment. Although they may seem helpless, they will defend themselves if necessary.")
            });
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //3hi31mg
            var off = new Vector2(NPC.width / 2, NPC.height / 2 + 2);
            var clr = new Color(255, 255, 255, 255); // full white
            Texture2D texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Trail").Value;
            var frame = new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height);
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[NPC.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.95f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = NPC.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off, frame, clr * (1f - fadeMult * i), NPC.oldRot[i], orig, scale, flipType, 0f);
            }
            return true;
        }

        float TargetY;
        float sineTime = 10f;
        int Timer = 0;
        public override void AI()
        {
            Lighting.AddLight(NPC.Center, new Vector3(.25f, .3f, .4f));
            //SpawnNPC() wasn't working as intended, so this equates to something happening as soon as the NPC is spawned, I think
            if (Timer == 0)
            {
                TargetY = NPC.position.Y;
            }
            else NPC.position.Y = MathHelper.Lerp(NPC.position.Y, (float)(TargetY + Math.Sin(Timer / 20f) * sineTime), 0.05f);
            Timer++;
            if (NPC.life < NPC.lifeMax)
            {
                NPC.damage = 19;
                NPC.TargetClosest(false);
            }

            int minDist = 128;
            bool exceedMin = Helper.HorizontalDistance(NPC.Center, Main.player[NPC.target].Center) <= minDist;
            if (Main.player[NPC.target].active)
            {
                TargetY = Main.player[NPC.target].position.Y;
                if (!exceedMin || NPC.direction == 0)
                    NPC.direction = Main.player[NPC.target].position.X > NPC.position.X ? 1 : -1;

                NPC.spriteDirection = Main.player[NPC.target].position.X > NPC.position.X ? 1 : -1;
                sineTime = MathHelper.Lerp(sineTime, 110f, 0.01f);
            }

            NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, NPC.direction * 2f, 0.05f);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1.5f;
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                    Main.dust[dust].scale = 2f;
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, ModContent.Find<ModGore>("MythosOfMoonlight/Starine").Type);
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
            NPC.frameCounter++;
            /*if (NPC.frameCounter >= (NPC.life >= NPC.lifeMax / 2 ? 17 : 11)) NPC.frameCounter = 0;
            NPC.frame.Y = (int)(NPC.frameCounter / (NPC.life >= NPC.lifeMax / 2 ? 6 : 4)) * frameHeight;*/
            if (NPC.frameCounter % 5 == 0)
            {
                if (NPC.frame.Y < 3 * frameHeight)
                    NPC.frame.Y += frameHeight;
                else
                    NPC.frame.Y = 0;

            }
        }
    }
    public class Starine_Scatterer : ModNPC
    {
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale);
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Scatterer");
            Main.npcFrameCount[NPC.type] = 12;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1f };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 30;
            NPC.aiStyle = NPCAIStyleID.Snail;
            NPC.defense = 4;
            NPC.lifeMax = 135;
            NPC.knockBackResist = .5f;
            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath1;
            SpawnModBiomes = new int[]
            {
                ModContent.GetInstance<PurpleCometBiome>().Type
            };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("All three of its legs are made up of a strange, jelly-like substance, which gives it grippy, sticky capabilities, climbing over most obstacles.")
            });
        }
        public override void DrawEffects(ref Color drawColor)
        {
            drawColor = Color.White;
        }
        public override void FindFrame(int frameHeight)
        {
            frame.Width = NPC.width;
            frame.Height = frameHeight;
            switch (AIState)
            {
                case 0:
                    if (++frameCounter % 5 == 0)
                    {
                        if (frame.Y < frameHeight * 5)
                            frame.Y += frameHeight;
                        else
                            frame.Y = 0;
                    }
                    break;
                case 1:
                    if (++frameCounter % 5 == 0)
                    {
                        if (frame.Y < frameHeight * 5)
                            frame.Y = frameHeight * 5;
                        else if (frame.Y < frameHeight * 11)
                            frame.Y += frameHeight;
                        else
                            frame.Y = frameHeight * 5;
                    }
                    break;
            }
        }
        public float AIState;
        public float AITimer;
        const int Walk = 0, Shoot = 1;
        float targetRotation;
        int frameCounter;
        Rectangle frame;
        public override void AI()
        {
            //NPC.rotation = MathHelper.Lerp(NPC.rotation, targetRotation, 0.35f);
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(false);
            /*if (AIState == Walk)
            {
                if (!NPC.collideX)
                {
                    targetRotation = 0;
                    NPC.GetGlobalNPC<FighterGlobalAI>().FighterAI(NPC, 0, 1, false);
                }
                else if (NPC.collideX && !NPC.collideY)
                {
                    NPC.velocity.Y = Helper.FromAToB(NPC.Center, player.Center - Vector2.UnitY * 30).Y * 3;
                    /*if (TRay.CastLength(NPC.Center + new Vector2(0, NPC.height / 2).RotatedBy(NPC.rotation), -Vector2.UnitY, 200) < NPC.height / 2)
                    {
                        targetRotation = MathHelper.ToRadians(180);
                        NPC.GetGlobalNPC<FighterGlobalAI>().FighterAI(NPC, 0, 1, false);
                    }
                    else *//*
                    NPC.velocity.X = -NPC.direction;
                    targetRotation = -NPC.direction * MathHelper.Pi;
                }
            }
            else
            {

            }*/
            if (AIState == Walk)
            {
                AITimer++;
                if (NPC.collideX || NPC.collideY)
                    NPC.velocity *= 6;
                if (AITimer >= 300)
                {
                    AITimer = 0;
                    AIState = Shoot;
                }
            }
            else
            {
                NPC.velocity *= 0.1f;
                AITimer++;
                if (AITimer == 15)
                {
                    for (int i = 0; i < 3; i++)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Helper.FromAToB(NPC.Center, player.Center) * 5, ModContent.ProjectileType<Starine_Sparkle>(), 10, 1f);

                }
                if (AITimer >= 30)
                {
                    AIState = 0;
                    AITimer = 0;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //3hi31mg
            var off = new Vector2(NPC.width / 2, NPC.height / 2 + 2);
            var clr = new Color(255, 255, 255, 255); // full white
            Texture2D texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Trail").Value;
            var framee = new Rectangle(0, frame.Y, NPC.width, NPC.height);
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[NPC.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(1f, 0.95f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = NPC.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off, framee, clr * (1f - fadeMult * i), NPC.oldRot[i], orig, scale, flipType, 0f);
            }
            NPC.frame = framee;
            return true;
        }
    }
}