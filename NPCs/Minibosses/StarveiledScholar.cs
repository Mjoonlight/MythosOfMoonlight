using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using MythosOfMoonlight.Common.Systems;
using MythosOfMoonlight.NPCs.Enemies.CometFlyby.CometEmber;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles;
using MythosOfMoonlight.NPCs.Minibosses.StarveiledProj;
using rail;
using SteelSeries.GameSense;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses
{
    public class StarveiledScholar : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 12;
            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            NPC.width = 68;
            NPC.height = 74;
            NPC.lifeMax = 2300;
            NPC.boss = true;
            NPC.defense = 22;
            NPC.damage = 0;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 7f;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }
        float flickerGlow;
        float riftAlpha;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*if (AIState == Comet)
            {
                Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/star_05").Value;
                spriteBatch.Reload(BlendState.Additive);
                spriteBatch.Draw(tex, cometPos - screenPos, null, Color.White, Main.GameUpdateCount * 0.0025f, tex.Size() / 2, 0.3f, SpriteEffects.None, 0f);
                spriteBatch.Reload(BlendState.AlphaBlend);
            }*/
            if ((AIState == Rift && AITimer >= 30) && !ded)
            {
                Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/thingy_transparent").Value;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
                float riftAlpha2 = MathHelper.Lerp(1, 0, riftAlpha);
                DrawData a = new(tex, NPC.Center - screenPos, null, Color.White * riftAlpha2, Main.GameUpdateCount * 0.0035f, tex.Size() / 2, 1, SpriteEffects.None, 0);
                DrawData colored = new(tex, NPC.Center - screenPos, null, Color.White * riftAlpha * (AITimer < 150 ? 1 : 0), Main.GameUpdateCount * 0.0035f, tex.Size() / 2, 1, SpriteEffects.None, 0);
                DrawData b = new(tex, NPC.Center - screenPos, null, Color.White * 0.85f * riftAlpha2, Main.GameUpdateCount * 0.0055f, tex.Size() / 2, 0.75f, SpriteEffects.None, 0);
                //int shader = ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex;
                GameShaders.Armor.GetShaderFromItemId(3978).Apply(null, colored);
                GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye).Apply(null, a);
                GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye).Apply(null, b);
                for (int i = 0; i < 3; i++)
                {
                    a.Draw(spriteBatch);
                    b.Draw(spriteBatch);
                    colored.Draw(spriteBatch);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!ded && NPC.life < NPC.lifeMax / 2)
            {

                Texture2D tex2 = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/thingy_transparent").Value;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
                DrawData a = new(tex2, NPC.Center - screenPos, null, Color.White, Main.GameUpdateCount * 0.0035f, tex2.Size() / 2, 1, SpriteEffects.None, 0);
                DrawData b = new(tex2, NPC.Center - screenPos, null, Color.White * 0.85f, Main.GameUpdateCount * 0.0055f, tex2.Size() / 2, 0.75f, SpriteEffects.None, 0);
                GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye).Apply(null, a);
                GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye).Apply(null, b);
                for (int i = 0; i < 3; i++)
                {
                    a.Draw(spriteBatch);
                    b.Draw(spriteBatch);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            }
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D glow = ModContent.Request<Texture2D>("MythosOfMoonlight/NPCs/Minibosses/StarveiledScholar_Glow").Value;
            Rectangle rect = new Rectangle(NPC.frame.X, NPC.frame.Y, 68, 74);
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int i = 1; i < 5; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(5 - i) / 5);
                var fadeMult = 1f / 5;
                spriteBatch.Draw(tex, NPC.oldPos[i] - screenPos + NPC.Size / 2, rect, Color.Pink * (1f - fadeMult * i) * 0.5f, NPC.oldRot[i], NPC.Size / 2, _scale, effects, 0f);
            }
            spriteBatch.Draw(tex, NPC.Center - screenPos, rect, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);

            if (NPC.frame.Y == 7 * 74 && NPC.frame.X == 5 * 68)
            {

                flickerGlow += Main.rand.NextFloat(-.1f, .1f);
                flickerGlow = MathHelper.Clamp(flickerGlow, 0, 1);
                DrawData data = new(tex, NPC.Center - screenPos, rect, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
                DrawData data2 = new(tex, NPC.Center - screenPos, rect, Color.White * flickerGlow, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
                GameShaders.Armor.GetShaderFromItemId(ItemID.VoidDye).Apply(NPC, data);
                GameShaders.Armor.GetShaderFromItemId(ItemID.VoidDye).Apply(NPC, data2);
                data.Draw(Main.spriteBatch);
                data2.Draw(Main.spriteBatch);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            }

            spriteBatch.Draw(glow, NPC.Center - screenPos, rect, Color.White, NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0);
            return false;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 1)
            {
                for (int num901 = 0; num901 < 10; num901++)
                {
                    int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.PurpurineDust>(), 0f, 0f, 200, default(Color), 1f);
                    Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                    Main.dust[num902].noGravity = true;
                    Dust dust2 = Main.dust[num902];
                    dust2.velocity = hitDirection * Vector2.UnitX;
                    num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, default(Color), 0.75f);
                    Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                    dust2 = Main.dust[num902];
                    dust2.velocity = hitDirection * Vector2.UnitX;
                    Main.dust[num902].noGravity = true;
                    Main.dust[num902].fadeIn = 2.5f;
                }
                for (int num903 = 0; num903 < 10; num903++)
                {
                    int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 272, 0f, 0f, 0, default(Color), 1);
                    Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                    Dust dust2 = Main.dust[num904];
                    dust2.velocity = hitDirection * Vector2.UnitX;
                }
            }
        }
        void DeathAnim(int height, int width)
        {
            NPC.frameCounter++;
            if (Main.rand.NextBool(3))
            {
                int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 272, 0f, 0f, 0, default(Color), 0.45f);
                Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                Main.dust[num904].noGravity = true;
                Dust dust2 = Main.dust[num904];
            }
            if (NPC.frameCounter % 5 == 0)
            {
                if (NPC.frame.Y == 1 * height && NPC.frame.X == 7 * width)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CometEmberProj>(), 20, .1f, Main.myPlayer);
                    for (int num901 = 0; num901 < 10; num901++)
                    {
                        int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.PurpurineDust>(), 0f, 0f, 200, default(Color), 1f);
                        Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                        Main.dust[num902].noGravity = true;
                        Dust dust2 = Main.dust[num902];
                        dust2.velocity *= 3f;
                        num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, default(Color), 0.75f);
                        Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                        dust2 = Main.dust[num902];
                        dust2.velocity *= 2f;
                        Main.dust[num902].noGravity = true;
                        Main.dust[num902].fadeIn = 2.5f;
                    }
                    for (int num903 = 0; num903 < 10; num903++)
                    {
                        int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 272, 0f, 0f, 0, default(Color), 1);
                        Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                        Dust dust2 = Main.dust[num904];
                        dust2.velocity *= 3f;
                    }
                }
                if (AITimer >= 20 && NPC.frame.Y == 7 * height && NPC.frame.X == 5 * width)
                {
                    NPC.frame.X += width;
                    NPC.frame.Y = 0;
                }

                bool fell = (TRay.CastLength(NPC.Center, Vector2.UnitY, 1000) < NPC.height || TRay.CastLength(NPC.Left, Vector2.UnitY, 1000) < NPC.height || TRay.CastLength(NPC.Right, Vector2.UnitY, 1000) < NPC.height) || NPC.collideY;
                if (NPC.frame.Y <= 3 * height && fell && NPC.frame.X == 5 * width)
                    CameraSystem.ChangeCameraPos(NPC.Center, 300, 1);
                if (NPC.frame.Y < 3 * height && !fell && NPC.frame.X == 5 * width)
                {
                    NPC.frame.X = 5 * width;
                    NPC.frame.Y += height;
                }
                else if (NPC.frame.Y >= 3 * height && fell && NPC.frame.Y < 7 * height && NPC.frame.X == 5 * width)
                {
                    NPC.frame.X = 5 * width;
                    NPC.frame.Y += height;
                }
                else if (NPC.frame.Y == 7 * height && NPC.frame.X == 5 * width)
                {
                    AITimer++;
                }
                else if (NPC.frame.Y < 11 * height && NPC.frame.X == 6 * width)
                {
                    NPC.frame.Y += height;
                    CameraSystem.ChangeCameraPos(NPC.Center, 50);
                }
                else if (NPC.frame.Y == 11 * height && NPC.frame.X == 6 * width)
                {
                    NPC.frame.Y = 0;
                    NPC.frame.X += width;
                }
                else if (NPC.frame.Y < 6 * height && NPC.frame.X == 7 * width)
                {
                    NPC.frame.Y += height;
                }
                else if (NPC.frame.Y == 6 * height && NPC.frame.X == 7 * width)
                {
                    NPC.immortal = false;
                    NPC.life = 0;
                    NPC.checkDead();
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 68;
            NPC.frame.Height = 74;
            int height = 74;
            int width = 68;

            if (AIState == Death)
            {
                DeathAnim(height, width);
            }
            else
            {
                if (AIState == Orb || AIState == Serpents)
                {
                    NPC.frame.X = 3 * width;
                    NPC.frameCounter++;
                    if (NPC.frameCounter % 5 == 0)
                    {
                        if (NPC.frame.Y < height * 3)
                            NPC.frame.Y += height;
                        else
                            NPC.frame.Y = 0;
                    }
                }
                if (AIState == Idle || doIdleAnimation)
                {
                    NPC.frame.X = 2 * width;
                    NPC.frameCounter++;
                    if (NPC.frameCounter % 5 == 0)
                    {
                        if (NPC.frame.Y < height * 4)
                            NPC.frame.Y += height;
                        else
                            NPC.frame.Y = 0;
                    }
                }
                if (AIState == Rift && !doIdleAnimation)
                {
                    NPC.frame.X = width * 4;
                    NPC.frameCounter++;
                    if (NPC.frameCounter % 5 == 0)
                    {
                        if (NPC.frame.Y < height * 10)
                            NPC.frame.Y += height;
                        else
                        {
                            NPC.frame.Y = 0;
                            doIdleAnimation = true;
                        }
                    }
                }
            }
        }
        bool doIdleAnimation;
        public float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float AITimer3
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        const int Death = -1, Intro = 0, Idle = 1, Orb = 2, Rift = 3, Serpents = 4, P2Transition = 5, Comet = 6;
        bool ded;
        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !ded)
            {
                NPC.life = 1;
                riftAlpha = 0;
                AIState = Death;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                NPC.noGravity = false;
                doIdleAnimation = false;
                NPC.noTileCollide = false;
                NPC.dontTakeDamage = true;
                ded = true;
                NPC.frame.X = 5 * 68;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
                NPC.life = 1;
                return false;
            }
            return true;
        }
        int NextAttack = Orb;
        bool p2;
        public override void AI()
        {
            if (NPC.life < NPC.lifeMax / 2 && !p2)
            {
                AIState = P2Transition;
                AITimer = AITimer2 = AITimer3 = 0;
                p2 = true;
            }
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);
            NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
            if (player.dead || !PurpleCometEvent.PurpleComet)
            {
                NPC.active = false;

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<CometEmberProj>(), 20, .1f, Main.myPlayer);
            }
            if (AIState == P2Transition)
            {
                p2 = true;
                AIState = Idle;
                NextAttack = Comet;
            }
            if (AIState == Intro)
            {
                //do intro later!
                AIState = Idle;
            }
            else if (AIState == Idle)
            {
                AITimer++;
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.015f);
                if (AITimer >= 120)
                {
                    AIState = NextAttack;
                    NPC.frame.Y = 0;
                    AITimer = 0;
                }
            }
            else if (AIState == Orb)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float angle = Helper.CircleDividedEqually(i, 2);
                        Projectile.NewProjectile(NPC.InheritSource(NPC), NPC.Center, Vector2.Zero, ModContent.ProjectileType<ScholarOrb>(), 10, 0, player.whoAmI, angle, NPC.whoAmI);
                    }
                }
                if (AITimer == 60 || AITimer == 120 || AITimer == 180)
                {
                    Vector2 rand = Main.rand.NextVector2FromRectangle(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight));
                    int attempts = 0;
                    while ((Main.tile[rand.ToTileCoordinates()].HasTile || rand.Distance(player.Center) < 100) && attempts < 250)
                    {
                        attempts++;
                        rand = Main.rand.NextVector2FromRectangle(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight));
                    }
                    NPC.Center = rand;
                    for (int num901 = 0; num901 < 10; num901++)
                    {
                        int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.PurpurineDust>(), 0f, 0f, 200, default(Color), 1f);
                        Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                        Main.dust[num902].noGravity = true;
                        Dust dust2 = Main.dust[num902];
                        dust2.velocity *= 3f;
                        num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, default(Color), 0.75f);
                        Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                        dust2 = Main.dust[num902];
                        dust2.velocity *= 2f;
                        Main.dust[num902].noGravity = true;
                        Main.dust[num902].fadeIn = 2.5f;
                    }
                    for (int num903 = 0; num903 < 10; num903++)
                    {
                        int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 272, 0f, 0f, 0, default(Color), 1);
                        Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                        Dust dust2 = Main.dust[num904];
                        dust2.velocity *= 3f;
                    }
                }
                if (AITimer >= 270)
                {
                    AITimer = 0;
                    NextAttack = Rift;
                    AIState = Idle;
                }
            }
            else if (AIState == Rift)
            {
                AITimer++;
                if (AITimer < 150 && riftAlpha > 0)
                    riftAlpha -= 0.025f;
                if (AITimer > 150 && riftAlpha < 1)
                    riftAlpha += 0.025f;
                if (AITimer == 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        float angle = Helper.CircleDividedEqually(i, 5);
                        Vector2 vel = angle.ToRotationVector2() * 15;
                        Projectile.NewProjectile(NPC.InheritSource(NPC), NPC.Center, vel, ModContent.ProjectileType<ScholarBolt_Telegraph>(), 0, 0, player.whoAmI, -3);
                    }
                }
                if (AITimer == 30)
                {
                    riftAlpha = 1f;
                    for (int i = 0; i < 5; i++)
                    {
                        int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y) + Main.rand.NextVector2CircularEdge(250, 250), NPC.width, NPC.height, 272, 0f, 0f, 0, default(Color), 1);
                        Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                        Dust dust2 = Main.dust[num904];
                        dust2.velocity = Helper.FromAToB(dust2.position, NPC.position);

                        float angle = Helper.CircleDividedEqually(i, 5);
                        Vector2 vel = angle.ToRotationVector2() * 15;
                        Projectile.NewProjectile(NPC.InheritSource(NPC), NPC.Center, vel, ModContent.ProjectileType<ScholarBolt3>(), 10, 0);
                    }


                    for (int i = 0; i < 10; i++)
                    {
                        float angle = Helper.CircleDividedEqually(i, 10);
                        Vector2 vel = angle.ToRotationVector2() * 20;
                        Projectile.NewProjectile(NPC.InheritSource(NPC), NPC.Center, vel, ModContent.ProjectileType<ScholarBolt_Telegraph>(), 0, 0, player.whoAmI, 3);
                    }
                }
                if (AITimer == 60)
                {

                    for (int i = 0; i < 10; i++)
                    {
                        float angle = Helper.CircleDividedEqually(i, 10);
                        Vector2 vel = angle.ToRotationVector2() * 20;
                        Projectile.NewProjectile(NPC.InheritSource(NPC), NPC.Center, vel, ModContent.ProjectileType<ScholarBolt2>(), 10, 0);
                    }
                }
                if (AITimer >= 300)
                {
                    AITimer = 0;
                    AIState = Serpents;
                    NextAttack = Serpents;
                    NPC.frame.Y = 0;
                    doIdleAnimation = false;
                }
            }
            else if (AIState == Serpents)
            {
                AITimer++;
                if (AITimer == 120)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y) + Main.rand.NextVector2CircularEdge(250, 250), NPC.width, NPC.height, 272, 0f, 0f, 0, default(Color), 1);
                        Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                        Dust dust2 = Main.dust[num904];
                        dust2.velocity = Helper.FromAToB(dust2.position, NPC.position);

                        float angle = Helper.CircleDividedEqually(i, 3);
                        Vector2 vel = angle.ToRotationVector2() * 15;
                        for (int j = -1; j < 2; j++)
                        {
                            if (j == 0)
                                continue;
                            Projectile a = Projectile.NewProjectileDirect(NPC.InheritSource(NPC), NPC.Center, vel, ModContent.ProjectileType<ScholarSerpent>(), 10, 0, player.whoAmI, j * 0.2f);
                            a.localAI[0] = NPC.whoAmI;
                        }
                    }
                }
                if (AITimer % 100 == 0 && AITimer < 600)
                {
                    Vector2 rand = player.Center - (NPC.Center - player.Center) * 0.75f;
                    /*Vector2 rand = Main.rand.NextVector2FromRectangle(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight));
                    int attempts = 0;
                    while ((Main.tile[rand.ToTileCoordinates()].HasTile || rand.Distance(player.Center) > 400) && attempts < 250)
                    {
                        attempts++;
                        rand = Main.rand.NextVector2FromRectangle(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight));
                    }*/
                    NPC.Center = rand;
                    for (int num901 = 0; num901 < 10; num901++)
                    {
                        int num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<Dusts.PurpurineDust>(), 0f, 0f, 200, default(Color), 1f);
                        Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                        Main.dust[num902].noGravity = true;
                        Dust dust2 = Main.dust[num902];
                        dust2.velocity *= 3f;
                        num902 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.PurpleTorch, 0f, 0f, 100, default(Color), 0.75f);
                        Main.dust[num902].position = NPC.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * NPC.width / 2f;
                        dust2 = Main.dust[num902];
                        dust2.velocity *= 2f;
                        Main.dust[num902].noGravity = true;
                        Main.dust[num902].fadeIn = 2.5f;
                    }
                    for (int num903 = 0; num903 < 10; num903++)
                    {
                        int num904 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 272, 0f, 0f, 0, default(Color), 1);
                        Main.dust[num904].position = NPC.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(NPC.velocity.ToRotation()) * NPC.width / 2f;
                        Dust dust2 = Main.dust[num904];
                        dust2.velocity *= 3f;
                    }
                }
                if (AITimer >= 630)
                {
                    AITimer = 0;
                    NextAttack = p2 ? Comet : Orb;
                    AIState = Idle;
                }
            }
            else if (AIState == Comet)
            {
                AITimer++;
                //if (AITimer < 60)
                //    cometPos = player.Center + new Vector2(player.velocity.X * 15, 0);
                //Vector2 pos = (cometPos - Vector2.UnitY * Main.screenHeight) - new Vector2(Main.rand.NextFloat(-100, 100), 0);
                /*if (AITimer % 5 == 0 && AITimer > 60)
                {
                    Projectile.NewProjectile(default, pos, (Helper.FromAToB(pos, cometPos) + new Vector2(Main.rand.NextFloat(-.1f, .1f), 0)) * 1.5f, ModContent.ProjectileType<ScholarCometBig>(), 10, 0);
                }*/
                if (AITimer % 10 == 0)
                {
                    Vector2 pos2 = new Vector2(Main.screenPosition.X + Main.rand.NextFloat(Main.screenWidth), Main.screenPosition.Y);
                    Projectile.NewProjectile(default, pos2, Vector2.UnitY * 1.75f, ModContent.ProjectileType<ScholarCometBig>(), 10, 0);
                }
                if (AITimer % 15 == 0)
                {
                    Vector2 pos2 = new Vector2(Main.screenPosition.X + Main.rand.NextFloat(Main.screenWidth), Main.screenPosition.Y);
                    Projectile.NewProjectile(default, pos2, Vector2.UnitY * 2, ModContent.ProjectileType<ScholarCometBigger>(), 10, 0);
                }
                /*if (AITimer % 20 == 0 && AITimer > 60)
                {
                    Projectile.NewProjectile(default, pos, (Helper.FromAToB(pos, cometPos) + new Vector2(Main.rand.NextFloat(-.1f, .1f), 0)) * 3, ModContent.ProjectileType<ScholarCometBigger>(), 10, 0);
                }
                */
                if (AITimer >= 300)
                {
                    //AITimer = 0;
                    //if (++AITimer3 < 3)
                    //    AIState = Comet;
                    //else
                    //{
                    //cometPos = Vector2.Zero;
                    AIState = Idle;
                    AITimer = 0;
                    AITimer3 = 0;
                    NextAttack = Orb;
                    //}
                }
            }
        }
    }
}
