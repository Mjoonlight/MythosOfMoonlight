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
            NPCID.Sets.TrailCacheLength[npc.type] = 9;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //3hi31mg
            var off = new Vector2(npc.width / 2, npc.height / 2 + 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = npc.Center - Main.screenPosition;
            var texture = mod.GetTexture("NPCs/Enemies/Starine/Starine_Skipper_Trail");
            var frame = new Rectangle(0, npc.frame.Y, npc.width, npc.height);
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[npc.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(1f, 0.95f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = npc.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, npc.oldPos[i] - Main.screenPosition + off, frame, clr * (1f - fadeMult * i), npc.oldRot[i], orig, scale, flipType, 0f);
            }
            return true;
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
                        if (npc.velocity.Y > 0)
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
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1.5f;
            }
            if (npc.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                    Main.dust[dust].scale = 2f;
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Gore.NewGore(npc.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, mod.GetGoreSlot("Gores/Enemies/Starine"));
                }
            }
        }
        float NumZeroes
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
                            if (npc.velocity.X == 0)
                            {
                                if (NumZeroes++ < 1)
                                {
                                    State = 1;
                                }
                                else
                                {
                                    State = 3;
                                    NumZeroes = 0;
                                }
                            }
                            else State = (JumpsElapsed % 4f == 0) ? 2f : 1f;
                            Timer = (JumpsElapsed % 4f == 0) ? 30f : (8f * ((npc.life < npc.lifeMax / 2) ? (npc.life / npc.lifeMax) : 1f));
                            npc.velocity.X = 0;
                        } else {
                            npc.velocity.X = 4f * npc.direction;
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
                        npc.velocity = new Vector2((Main.rand.NextFloat() - .5f + 4f) * npc.direction, -7f);
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
            NPCID.Sets.TrailCacheLength[npc.type] = 10;
            NPCID.Sets.TrailingMode[npc.type] = 1;
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
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //3hi31mg
            var off = new Vector2(npc.width / 2, npc.height / 2 + 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = npc.Center - Main.screenPosition;
            var texture = mod.GetTexture("NPCs/Enemies/Starine/Starine_Sightseer_Trail");
            var frame = new Rectangle(0, npc.frame.Y, npc.width, npc.height);
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[npc.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.95f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = npc.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, npc.oldPos[i] - Main.screenPosition + off, frame, clr * (1f - fadeMult * i), npc.oldRot[i], orig, scale, flipType, 0f);
            }
            return true;
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
                Main.dust[dust].noGravity = true;
                          Main.dust[dust].scale = 1.5f;
            }
            if (npc.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                    Main.dust[dust].scale = 2f;
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
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
    public class Starine_Scatterer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Scatterer");
            Main.npcFrameCount[npc.type] = 12;
            NPCID.Sets.TrailCacheLength[npc.type] = 10;
            NPCID.Sets.TrailingMode[npc.type] = 3;
        }
        public override void SetDefaults()
        {
            npc.width = 36;
            npc.height = 30;
            npc.aiStyle = -1;
            npc.defense = 4;
            npc.lifeMax = 90;
            npc.knockBackResist = .5f;
            npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
        }
        public override void DrawEffects(ref Color drawColor)
        {
            drawColor = Color.White;
        }
        readonly int aggrorange = 240;
        readonly float ClimbSpeed = 2f;
        void NormalStateAnimation()
        {
            if (npc.velocity.Y == 0 || npc.collideX)
            {
                if (npc.frameCounter > 4) npc.frameCounter = 0;
                else if (npc.ai[1] % 4 == 0) npc.frameCounter++;
            }
            else npc.frameCounter = 4;
        }
        void AngryStateAnimation()
        {
            if (SqrDistanceFromPlayer <= aggrorange)
            {
                if (npc.frameCounter == 11 && (npc.ai[1]) % 4 == 0) npc.ai[0] = 0;
                if (npc.ai[0] < 50) npc.frameCounter = 5;
                else if (npc.ai[1] % 4 == 0) npc.frameCounter++;
            }
            else
            {
                NormalStateAnimation();
            }
        }
        public override void FindFrame(int frameHeight)
        {
            switch (AIState)
            {
                case GiveUp:
                case Normal:
                    NormalStateAnimation();
                    break;
                case Angry:
                    AngryStateAnimation();
                    break;
            }
            npc.frame.Y = (int)npc.frameCounter * frameHeight;
        }

        int AIState
        {
            get => (int)npc.ai[3];
            set => npc.ai[3] = value;
        }
        float SqrDistanceFromPlayer => Math.Abs(npc.PlayerTarget().Center.X - npc.Center.X);
        float targetRotation;
        const int Normal = 1, Angry = 2, GiveUp = 3;
        const float MinimumPlayerHeightDistance = 10f;
        void WalkAndCrawlLogic()
        {
            if (npc.collideX) // if colliding with wall
            {
                if (npc.collideY && npc.velocity.Y != 0)
                {
                    AIState = GiveUp;
                }
                npc.velocity.Y = -ClimbSpeed; // climb upwards
                npc.velocity.X = npc.direction; // ensure collision along x direction on next frame; if a tile is infront, will allow to keep crawling. if a tile isn't infront, hurray
                targetRotation = -npc.direction * MathHelper.PiOver2; // rotate
            }
            else // if not colliding anymore
            {
                targetRotation = 0; // fix rotation to 0
            }
            npc.GetGlobalNPC<FighterGlobalAI>().FighterAI(npc, 0, 1.75f, false); // hustle ass
            npc.ai[0] = 0; 
        }
        void WalkAndCrawlLogicGivenUp()
        {
            if (npc.collideX) // if colliding with wall
            {
                npc.velocity.Y = -ClimbSpeed; // climb upwards
                npc.velocity.X = -npc.direction; // ensure collision along x direction on next frame; if a tile is infront, will allow to keep crawling. if a tile isn't infront, hurray
                targetRotation = npc.direction * MathHelper.PiOver2; // rotate
            }
            else // if not colliding anymore
            {
                targetRotation = 0; // fix rotation to 0
            }
            npc.GetGlobalNPC<FighterGlobalAI>().FighterAI(npc, 0, -1.75f, false); // hustle ass
            npc.ai[0] = 0;
        }
        void GiveUpState()
        {
            WalkAndCrawlLogicGivenUp();
            if (Collision.CanHitLine(npc.position, npc.width, npc.height, npc.PlayerTarget().position, npc.PlayerTarget().width, npc.PlayerTarget().height))
            {
                AIState = Normal;
            }
        }
        void NormalState()
        {
            if (SqrDistanceFromPlayer >= aggrorange || npc.PlayerTarget().Center.Y - npc.Center.Y >= MinimumPlayerHeightDistance || targetRotation != 0 || !Collision.CanHitLine(npc.position, npc.width, npc.height, npc.PlayerTarget().position, npc.PlayerTarget().width, npc.PlayerTarget().height)) // third check means if not rotated; means not climbing
            {
                WalkAndCrawlLogic();
            }
            else // finished crawling
            {
                AIState = Angry;
            }
        }
        void AngryState()
        {
            var playerDistance = SqrDistanceFromPlayer;
            if (playerDistance >= aggrorange * 2) // double aggro range; he's fuckin pissed 
            {
                AIState = Normal;
            }
            else
            {
                if (playerDistance <= aggrorange && npc.PlayerTarget().Center.Y - npc.Center.Y <= MinimumPlayerHeightDistance)
                {
                    if (!npc.justHit && npc.velocity.Y == 0)
                    {
                        npc.velocity.X = 0;
                    }
                    npc.FaceTarget();
                    npc.spriteDirection = npc.direction;
                    npc.ai[0]++;
                    if (npc.ai[0] % 60 <= 0)// && npc.collideY)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(1f, 6f) * npc.direction, -Main.rand.NextFloat(2f, 5f)), ModContent.ProjectileType<Starine_Sparkle>(), 10, 1f);
                        }
                        Main.PlaySound(SoundID.Item9, npc.Center);
                    }
                }
                else
                {
                    WalkAndCrawlLogic();
                }
            }
        }
        public override void AI()
        {
            npc.ai[1]++;
            if (npc.ai[0] == 52) npc.ai[1] = 0;
            npc.TargetClosest(false);
            npc.rotation = MathHelper.Lerp(npc.rotation, targetRotation, 0.35f);
            switch (AIState)
            {
                default:
                    AIState = Normal; // only during first frame of life
                    break;
                case Normal:
                    NormalState();
                    break;
                case Angry:
                    AngryState();
                    break;
                case GiveUp:
                    GiveUpState();
                    break;
            }  
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1.5f;
            }
            if (npc.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>(), 2 * hitDirection, -1.5f);
                    Main.dust[dust].scale = 2f;
                    Main.dust[dust].noGravity = true;
                }
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Gore.NewGore(npc.Center + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), Vector2.Zero, mod.GetGoreSlot("Gores/Enemies/Starine"));
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //3hi31mg
            var off = new Vector2(npc.width / 2, npc.height / 2 + 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = npc.Center - Main.screenPosition;
            var texture = mod.GetTexture("NPCs/Enemies/Starine/Starine_Scatterer_Trail");
            var frame = new Rectangle(0, npc.frame.Y, npc.width, npc.height);
            var orig = frame.Size() / 2f;
            var trailLength = NPCID.Sets.TrailCacheLength[npc.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(1f, 0.95f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = npc.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, npc.oldPos[i] - Main.screenPosition + off, frame, clr * (1f - fadeMult * i), npc.oldRot[i], orig, scale, flipType, 0f);
            }
            return true;
        }
    }
}