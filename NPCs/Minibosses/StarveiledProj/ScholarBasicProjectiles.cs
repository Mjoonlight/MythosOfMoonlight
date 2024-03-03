using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Dusts;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses.StarveiledProj
{
    public class ScholarOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 7;
            ProjectileID.Sets.TrailingMode[Type] = 0;
            Projectile.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.timeLeft = 500;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        float alpha;
        float pinkAlpha;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            for (int i = 1; i < 7; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(7 - i) / 7);
                var fadeMult = 1f / 7;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, null, Color.HotPink * alpha * (1f - fadeMult * i) * 0.5f, Projectile.oldRot[i], tex.Size() / 2, _scale, SpriteEffects.None, 0);
            }
            DrawData a = new(tex, Projectile.Center - Main.screenPosition, null, Color.White * alpha, Projectile.rotation, tex.Size() / 2, 1, SpriteEffects.None, 0);
            GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye).Apply(Projectile, a);
            a.Draw(Main.spriteBatch);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            Main.spriteBatch.Reload(effect: null);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * 0.5f * alpha, Projectile.rotation, tex.Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.HotPink * 0.5f * pinkAlpha, Projectile.rotation, tex.Size() / 2, 1, SpriteEffects.None, 0);
            return false;
        }
        int a;
        public override void Kill(int timeLeft)
        {
            for (int num901 = 0; num901 < 10; num901++)
            {
                int num902 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.PurpurineDust>(), 0f, 0f, 200, default(Color), 1f);
                Main.dust[num902].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                Main.dust[num902].noGravity = true;
                Dust dust2 = Main.dust[num902];
                dust2.velocity *= 3f;
                num902 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 100, default(Color), 0.75f);
                Main.dust[num902].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                dust2 = Main.dust[num902];
                dust2.velocity *= 2f;
                Main.dust[num902].noGravity = true;
                Main.dust[num902].fadeIn = 2.5f;
            }
            for (int num903 = 0; num903 < 10; num903++)
            {
                int num904 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 0, default(Color), 1);
                Main.dust[num904].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                Dust dust2 = Main.dust[num904];
                dust2.velocity *= 3f;
            }
        }
        public override void AI()
        {
            NPC owner = Main.npc[(int)Projectile.ai[1]];
            if (pinkAlpha > 0)
                pinkAlpha -= 0.05f;
            if (alpha < 1)
                alpha += 0.05f;
            if (Projectile.timeLeft == 440 || Projectile.timeLeft == 380)
            {
                pinkAlpha = 1;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Helper.FromAToB(Projectile.Center, Main.player[owner.target].Center) * 2.5f, ModContent.ProjectileType<ScholarBolt>(), 10, 0);
            }
            if (Projectile.timeLeft < 45)
            {
                Projectile.velocity *= 0.95f;
            }
            if (!(!owner.active || owner.type != ModContent.NPCType<StarveiledScholar>() || owner.ai[0] == -1))
            {
                if (Main.rand.NextBool(3))
                {
                    int num904 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 0, default(Color), 0.45f);
                    Main.dust[num904].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                    Main.dust[num904].noGravity = true;
                    Dust dust2 = Main.dust[num904];
                }
                if (Projectile.timeLeft > 320)
                {
                    Projectile.ai[0] += 2f * (float)Math.PI / 600f * 4;
                    Projectile.ai[0] %= 2f * (float)Math.PI;
                    Projectile.Center = owner.Center + 50 * new Vector2((float)Math.Cos(Projectile.ai[0]), (float)Math.Sin(Projectile.ai[0]));
                }
                else
                    Projectile.velocity *= 1.025f;
                if (Projectile.timeLeft == 320)
                {
                    Projectile.velocity = Helper.FromAToB(Projectile.Center, Main.player[owner.target].Center) * 5;
                }

                Vector2 move = Vector2.Zero;
                float distance = 5050f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.player[k].active)
                    {
                        Vector2 newMove = Main.player[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                        }
                    }
                }
                if (++a % 2 == 0 && target && Projectile.timeLeft > 45 && Projectile.timeLeft < 300)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (11f * Projectile.velocity + move) / 11f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 11f)
            {
                vector *= 11f / magnitude;
            }
        }
    }
    public class ScholarBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AI()
        {
            Projectile.velocity *= 1.025f;
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
        }
    }
    public class ScholarBolt2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }
        float alpha = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < 5; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(5 - i) / 5);
                var fadeMult = 1f / 5;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, null, Color.Pink * (1f - fadeMult * i) * 0.5f * alpha, Projectile.oldRot[i], Projectile.Size / 2, _scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * alpha;
        }
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(3));
            if (Projectile.timeLeft < 20)
                alpha -= 0.05f;
        }
    }
    public class ScholarBolt3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 33;
            Projectile.height = 33;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < 10; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(10 - i) / 10);
                var fadeMult = 1f / 10;
                for (int j = -1; j < 2; j++)
                {
                    if (j == 0)
                        continue;
                    Vector2 offset = new Vector2((float)Math.Sin(3 * sinething[i]) * 15 * j * i * 0.1f, 0).RotatedBy(Projectile.oldRot[i]);
                    Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2 + offset, null, Color.Pink * (1f - fadeMult * i) * 0.5f * alpha, Projectile.oldRot[i], Projectile.Size / 2, (1f - fadeMult * i), SpriteEffects.None, 0f);
                }
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * alpha;
        }
        float[] sinething = new float[10];
        float alpha = 1;
        public override void AI()
        {

            for (int num25 = sinething.Length - 1; num25 > 0; num25--)
            {
                sinething[num25] = sinething[num25 - 1];
            }
            sinething[0]++;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-3));
            if (Projectile.timeLeft < 20)
                alpha -= 0.05f;
        }
    }
    public class ScholarBolt_Telegraph : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Minibosses/StarveiledProj/ScholarBolt2";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }
        float alpha = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < 50; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(50 - i) / 50);
                var fadeMult = 1f / 50;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, null, Color.Pink * (1f - fadeMult * i) * 0.15f * alpha, Projectile.oldRot[i], Projectile.Size / 2, _scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Transparent;
        }
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
            //          if (Projectile.timeLeft < 20)
            //                alpha -= 0.05f;
            Projectile.damage = 0;
            if (Projectile.timeLeft < 180)
            {
                if (alpha > 0)
                    alpha -= 0.05f;
                Projectile.velocity = Vector2.Zero;
            }
        }
    }
    public class ScholarSerpent : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
        }
        float alpha = 1;
        public override Color? GetAlpha(Color lightColor) => Color.White * alpha;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < Math.Clamp((int)50 * alpha, 0, 50); i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(50 - i) / 50);
                var fadeMult = 1f / 50;
                for (int j = -1; j < 2; j++)
                {
                    if (j == 0)
                        continue;
                    Vector2 offset = new Vector2((float)Math.Sin(3 * sinething[i]) * 15 * j * i * 0.1f, 0).RotatedBy(Projectile.oldRot[i]);
                    Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2 + offset, null, Color.Pink * (1f - fadeMult * i) * 0.5f * alpha, Projectile.oldRot[i], Projectile.Size / 2, (1f - fadeMult * i) * 0.65f, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
        float[] sinething = new float[50];
        float acceleration;
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.localAI[0]];
            for (int num25 = sinething.Length - 1; num25 > 0; num25--)
            {
                sinething[num25] = sinething[num25 - 1];
            }
            acceleration = MathHelper.Clamp(acceleration, 1, 3);
            if (Projectile.Center.Distance(npc.Center) > 500)
                acceleration = MathHelper.SmoothStep(acceleration, 3, 0.02f);
            else
                acceleration = MathHelper.SmoothStep(acceleration, 1, 0.02f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            sinething[0]++;
            if (Projectile.timeLeft < 20)
                alpha -= 0.075f;
            if (Projectile.timeLeft == 120)
                Projectile.velocity = Helper.FromAToB(Projectile.Center, Main.player[npc.target].Center) * 10;
            if (Projectile.timeLeft < 120)
                Projectile.velocity *= 1.005f;
            if (!npc.active || Projectile.timeLeft < 120 || npc.type != ModContent.NPCType<StarveiledScholar>())
                return;
            Vector2 move = npc.Center - Projectile.Center;
            AdjustMagnitude(ref move);
            Projectile.velocity = (((11f * (acceleration)) * Projectile.velocity + move) / (11f * (acceleration)));
            AdjustMagnitude(ref Projectile.velocity);
        }
        private void AdjustMagnitude(ref Vector2 vector)
        {
            NPC npc = Main.npc[(int)Projectile.localAI[0]];
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > (11f * (acceleration)))
            {
                vector *= (11f * (acceleration)) / magnitude;
            }
        }
    }
    public class ScholarCometBig : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 26;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < 5; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(5 - i) / 5);
                var fadeMult = 1f / 5;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, new Rectangle(0, Projectile.frame * 26, 28, 26), Color.LightPink * (1f - fadeMult * i) * 0.5f, Projectile.oldRot[i], Projectile.Size / 2, _scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CometEmberProj2>(), 0, .1f, Main.myPlayer);
            Helper.SpawnGore(Projectile.Center, "MythosOfMoonlight/PurpChunk" + (Main.rand.Next(4) + 1), 3, -1, Main.rand.NextVector2Unit());
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(3);
            Projectile.velocity *= 1.035f;
            if (Main.rand.NextBool(3))
            {
                int num904 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 0, default(Color), 0.45f);
                Main.dust[num904].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                Main.dust[num904].noGravity = true;
                Dust dust2 = Main.dust[num904];
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(3);
        }
    }
    public class ScholarCometBigger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 40;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < 5; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(5 - i) / 5);
                var fadeMult = 1f / 5;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, null, Color.LightPink * (1f - fadeMult * i) * 0.5f, Projectile.oldRot[i], Projectile.Size / 2, _scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft > 40)
            {
                Projectile.rotation += MathHelper.ToRadians(3);
                Projectile.velocity *= 1.05f;
            }
            if (Main.rand.NextBool(3))
            {
                int num904 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 0, default(Color), 0.45f);
                Main.dust[num904].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                Main.dust[num904].noGravity = true;
                Dust dust2 = Main.dust[num904];
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft > 40)
                Projectile.timeLeft = 40;
            Projectile.velocity = Vector2.UnitY * 0.5f;
            return false;
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CometEmberProj2>(), 0, .1f, Main.myPlayer);
            Helper.SpawnGore(Projectile.Center, "MythosOfMoonlight/PurpChunk" + (Main.rand.Next(4) + 1), 5, -1, Main.rand.NextVector2Unit());
        }
    }
    /*public class ScholarCometSmall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }
    }
    public class ScholarCometSmall2 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Minibosses/StarveiledProj/ScholarCometSmall";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }
    }

    */
    public class CometEmberProj2 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Explosion");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
        }
        public float Scale
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public float Alpha
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public override void AI()
        {
            Alpha = Projectile.timeLeft / 5f;
            Scale = 5f * (2f * (5 - Projectile.timeLeft) + 1f);
            if (Projectile.timeLeft == 4)
            {
                for (int i = 1; i <= 10; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(7.5f, 7.5f);
                    Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, ModContent.DustType<PurpurineDust>());
                    dust.velocity = vel;
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(1f, 4f);
                }
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Projectiles/PilgrimExplosion_Extra", AssetRequestMode.ImmediateLoad).Value;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            BlendState blend = BlendState.Additive;
            float sc = 10 * Scale / tex.Width;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, blend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(tex, pos, null, Color.White * Alpha, 0f, tex.Size() / 2f, sc, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Vector2.Distance(new Vector2(projHitbox.Center.X, projHitbox.Center.Y), new Vector2(targetHitbox.Center.X, targetHitbox.Center.Y)) <= Scale + targetHitbox.Width / 2f;
        }
    }
}
