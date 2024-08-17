using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using static MythosOfMoonlight.Projectiles.FireflyMinion;
using MythosOfMoonlight.Common.BiomeManager;
using MythosOfMoonlight.Common.Crossmod;
using Terraria.Utilities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using Terraria.Audio;

namespace MythosOfMoonlight.NPCs.Enemies.Overworld.Starine
{
    public class Starine_Skipper : ModNPC
    {
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossAdjustment * balance);
        }
        public int f;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Skipper");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.TrailCacheLength[NPC.type] = 9;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                //Frame = (int)(NPC.frameCounter / 5),
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPC.AddElement(CrossModHelper.Celestial);
            NPC.AddNPCElementList("Inorganic");
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
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>(), 2 * hit.HitDirection, -1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1.5f;
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>(), 2 * hit.HitDirection, -1.5f);
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
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossAdjustment * balance);
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Sightseer");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.TrailCacheLength[NPC.type] = 25;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new()
            {
                Velocity = 1f,
                Position = new Vector2(0, 0),
                PortraitPositionYOverride = 0,
                Direction = 1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPC.AddElement(CrossModHelper.Celestial);
            NPC.AddNPCElementList("Inorganic");
        }
        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 40;
            NPC.aiStyle = -1;
            NPC.damage = 0;
            NPC.defense = 3;
            NPC.lifeMax = 140;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.knockBackResist = 0.2f;
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
            var off = new Vector2(NPC.width / 2, NPC.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            Texture2D texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Trail").Value;
            var frame = new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height);
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[NPC.type] / 2;

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.9f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / (trailLength * 2);
                SpriteEffects flipType = NPC.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, NPC.oldPos[i] - screenPos + off, frame, clr * (1f - fadeMult * i) * 0.05f, NPC.rotation, orig, scale, flipType, 0f);
            }
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Helper.GetExtraTex("Extra/solidCone");
            spriteBatch.Reload(BlendState.Additive);
            for (int i = -1; i < 2; i++)
            {
                float scale = MathHelper.SmoothStep(0.7f, 1, SolidConeAlpha);
                float offset = MathHelper.SmoothStep(-20, 30, SolidConeAlpha);
                float angle = (MathHelper.PiOver4 + i * (0.35f)) * NPC.direction;
                Vector2 off = new Vector2(0, -NPC.height + offset).RotatedBy(angle);
                for (int j = 0; j < 4; j++) 
                spriteBatch.Draw(tex, NPC.Center + off - Main.screenPosition, null, new Color(0, 230, 230) * SolidConeAlpha, angle + MathHelper.PiOver2 * NPC.direction, new Vector2(0, tex.Height / 2), new Vector2(scale * 0.35f, 0.2f), NPC.direction == 1 ?SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            spriteBatch.Reload(BlendState.AlphaBlend);
        }

        public float AIState;
        public float[] AITimer = new float[6];
        public float InterestMeter;
        public float SolidConeAlpha = 0;
        public const int WanderAround = 0, Interested = 1, Angry = 2;
        float TargetY;
        float sineTime = 10f;
        int Timer = 0;
        public override bool PreAI()
        {
            NPC.oldVelocity = NPC.velocity;
            return base.PreAI();
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
        }
        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            Lighting.AddLight(NPC.Center, new Vector3(.25f, .3f, .4f)); 
            if (player.Distance(NPC.Center) < 400)
                InterestMeter += MathHelper.SmoothStep(7, 1.5f, player.Distance(NPC.Center) / 400f) + MathHelper.Clamp(MathHelper.Lerp(0, 5, player.velocity.Length() / 15), 0, 1);

            if (InterestMeter > 0)
            {
                InterestMeter--;
                if (player.direction != NPC.direction)
                    InterestMeter -= 3;
                if (player.Distance(NPC.Center) > 600)
                    InterestMeter -= 15;
            }
            AITimer[0]++;
            if (AITimer[1] == 0 || AITimer[2] == 0)
                AITimer[1] = Main.rand.Next(int.MaxValue);
            UnifiedRandom rand = new UnifiedRandom((int)AITimer[1]);
            AITimer[2]++;
            if (NPC.life < NPC.lifeMax)
            {
                AIState = Angry;
            }
            if (SolidConeAlpha > 0)
                SolidConeAlpha = MathHelper.Lerp(SolidConeAlpha, 0, 0.1f);


            switch (AIState)
            {
                case WanderAround:
                    NPC.rotation = Helper.LerpAngle(NPC.rotation, NPC.velocity.ToRotation() + (NPC.direction == -1 ? MathHelper.Pi : 0), 0.025f);
                    if (NPC.direction == 0)
                    {
                        NPC.direction = Main.rand.NextBool(2) ? 1 : -1;
                    }
                    float threshold = rand.Next(100, 200);
                    if (AITimer[0] > threshold)
                    {
                        if (rand.NextBool(2))
                        {
                            AITimer[1] = Main.rand.Next(int.MaxValue);
                            AITimer[0] = 0;
                        }
                        NPC.velocity *= 0.98f;
                        if (NPC.velocity.Length() < 0.25f)
                        {
                            NPC.direction = -NPC.direction;
                            AITimer[1] = Main.rand.Next(int.MaxValue);
                            AITimer[0] = 0;
                        }
                    }
                    else
                    {
                        NPC.spriteDirection = NPC.direction;
                        NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, NPC.direction * 1.5f, 0.05f);
                        Vector2 Target = TRay.Cast(NPC.Center, Vector2.UnitY, 400) - new Vector2(0, 220);
                        NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, NPC.Center.FromAToB(new Vector2(NPC.Center.X, (float)(Target.Y + Math.Cos(AITimer[2] / rand.NextFloat(50, 100)) * rand.NextFloat(5, 10)))).Y, rand.NextFloat(0.005f, 0.02f));
                    }
                    
                    if (InterestMeter > 300)
                    {
                        AIState = Interested;
                        NPC.direction = player.direction;
                        AITimer[1] = Main.rand.Next(int.MaxValue);
                        InterestMeter = 0;
                        AITimer[2] = 0;
                        AITimer[3] = 0;
                    }
                    break;
                case Interested:
                    {
                        int fac = rand.Next(60, 200) + (int)MathHelper.Clamp(MathF.Floor(InterestMeter), 100, 1000);
                        if (AITimer[2] < fac)
                        {
                            if (NPC.Center.X > player.Center.X)
                                NPC.direction = -1;
                            else if (NPC.Center.X < player.Center.X)
                                NPC.direction = 1;
                            NPC.spriteDirection = NPC.direction;
                            NPC.rotation = Helper.LerpAngle(NPC.rotation, NPC.FromAToB(player).ToRotation() + (NPC.direction == -1 ? MathHelper.Pi : 0), 0.025f);
                            NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.Center.FromAToB(player.Center- new Vector2(0, 85)+ new Vector2(100 * -player.direction, 0)).RotatedBy(MathHelper.ToRadians(MathF.Sin(AITimer[2] / rand.NextFloat(15, 30)) * rand.NextFloat(40, 60)) * player.direction) * 1.5f, 0.02f);
                        }
                        else
                        {
                            AIState = WanderAround;
                            AITimer[1] = Main.rand.Next(int.MaxValue);
                            InterestMeter = -300;
                            AITimer[2] = 0;
                        }
                    }
                    break;
                case Angry:
                    {
                        switch (AITimer[5])
                        {
                            case 0:
                                AITimer[3]++;
                                NPC.damage = 0;
                                if (AITimer[3] < 200)
                                {
                                    if (NPC.Center.X > player.Center.X)
                                        NPC.direction = -1;
                                    else if (NPC.Center.X < player.Center.X)
                                        NPC.direction = 1;
                                    NPC.spriteDirection = NPC.direction;
                                    NPC.rotation = Helper.LerpAngle(NPC.rotation, NPC.FromAToB(player).ToRotation() + (NPC.direction == -1 ? MathHelper.Pi : 0), 0.025f);
                                    Vector2 pos = player.Center + new Vector2(rand.NextFloat(175, 300) * (int)(rand.NextFloatDirection() > 0 ? 1 : -1), -rand.NextFloat(-50, 100));

                                    NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.Center.FromAToB(pos) * 4.5f, 0.02f) ;
                                    if (NPC.Center.Distance(pos) < 30 && AITimer[3] < 150)
                                        AITimer[3] = 150;   
                                }
                                if (AITimer[3] > 200)
                                    NPC.velocity *= 0.9f;
                                if (AITimer[3] >= 230)
                                {
                                    AITimer[4] = Main.rand.NextBool() ? 9: Main.rand.Next(4,10 - (Main.rand.NextBool(20) ? 4: 0));
                                    AITimer[3] = 0;
                                    AITimer[1] = Main.rand.Next(int.MaxValue);
                                    AITimer[5]++;
                                }
                                break;
                            case 1:
                                NPC.damage = 10 + (int)AITimer[4]*2;
                                AITimer[3]++;
                                if (AITimer[3] == 3)
                                {
                                    savedP = player.Center;
                                    NPC.velocity = Helper.FromAToB(NPC.Center, player.Center) * 5;
                                }
                                if (AITimer[3] < 10)
                                {
                                    if (NPC.Center.X > player.Center.X)
                                        NPC.direction = -1;
                                    else if (NPC.Center.X < player.Center.X)
                                        NPC.direction = 1;
                                    NPC.spriteDirection = NPC.direction;
                                    NPC.rotation = Helper.LerpAngle(NPC.rotation, Helper.FromAToB(NPC.Center, savedP).ToRotation() + (NPC.direction == -1 ? MathHelper.Pi : 0), 0.5f);
                                }
                                if (AITimer[3] == 10)
                                    NPC.velocity = Helper.FromAToB(NPC.Center, player.Center) * 28;
                                if (AITimer[3] > 15)
                                {
                                    AITimer[3] = 0;
                                    AITimer[5]++;
                                }
                                break;
                            case 2:
                                if (NPC.Center.X > player.Center.X)
                                    NPC.direction = -1;
                                else if (NPC.Center.X < player.Center.X)
                                    NPC.direction = 1;
                                NPC.spriteDirection = NPC.direction;
                                NPC.rotation = Helper.LerpAngle(NPC.rotation, Helper.FromAToB(NPC.Center, player.Center).ToRotation() + (NPC.direction == -1 ? MathHelper.Pi : 0), 0.05f);
                                NPC.velocity *= 0.95f;
                                AITimer[3]++;
                                if (AITimer[3] > 60)
                                {
                                    AITimer[4]++;
                                    if (AITimer[4] < 10)
                                        AITimer[5] = 1;
                                    else
                                        AITimer[5] = 0;
                                    AITimer[3] = 0;
                                    AITimer[1] = Main.rand.Next(int.MaxValue);
                                }
                                break;
                        }
                        break;
                    }
            }
        }Vector2 savedP;
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (AIState != Angry)
            {
                NPC.velocity = Vector2.Zero;
                SolidConeAlpha = 1f;
                AIState = Angry;
                AITimer[1] = Main.rand.Next(int.MaxValue);
                InterestMeter = 0;
                AITimer[2] = 0;
                AITimer[3] = 0;
            }
            else
            {
                NPC.damage = 0;
                AITimer[5] = 0;
                AITimer[1] = Main.rand.Next(int.MaxValue);
            }
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(NPC.getRect()), ModContent.DustType<StarineDust>(), Main.rand.NextVector2Circular(3, 3));
                dust.noGravity = true;
                dust.scale = 1.5f;
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(NPC.getRect()), ModContent.DustType<StarineDust>(), Main.rand.NextVector2Circular(3,3));
                    dust.scale = 2f;
                    dust.noGravity = true;
                }
                for (int i = 0; i < Main.rand.Next(20, 30); i++)
                {
                    Gore.NewGore(NPC.GetSource_OnHit(NPC), Main.rand.NextVector2FromRectangle(NPC.getRect()), Main.rand.NextVector2Circular(1,1), ModContent.Find<ModGore>("MythosOfMoonlight/Starine").Type, Main.rand.NextFloat(0.8f, 1.2f));
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
            if (NPC.frameCounter % (6-AIState) == 0)
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
        public override bool IsLoadingEnabled(Mod mod) => false;
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossAdjustment * balance);
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Scatterer");
            Main.npcFrameCount[NPC.type] = 12;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1f };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPC.AddElement(CrossModHelper.Celestial);
            NPC.AddNPCElementList("Inorganic");
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