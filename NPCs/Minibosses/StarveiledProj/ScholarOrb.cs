using log4net.Util;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
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
                int num904 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 272, 0f, 0f, 0, default(Color), 1);
                Main.dust[num904].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                Dust dust2 = Main.dust[num904];
                dust2.velocity *= 3f;
            }
        }
        public override void AI()
        {
            NPC owner = Main.npc[(int)Projectile.ai[1]];
            if (!owner.active || owner.type != ModContent.NPCType<StarveiledScholar>() || owner.ai[0] == -1)
                Projectile.Kill();
            if (Main.rand.NextBool(3))
            {
                int num904 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 272, 0f, 0f, 0, default(Color), 0.45f);
                Main.dust[num904].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 2f;
                Main.dust[num904].noGravity = true;
                Dust dust2 = Main.dust[num904];
            }
            if (pinkAlpha > 0)
                pinkAlpha -= 0.05f;
            if (alpha < 1)
                alpha += 0.05f;
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
            if (Projectile.timeLeft == 440 || Projectile.timeLeft == 380)
            {
                pinkAlpha = 1;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Helper.FromAToB(Projectile.Center, Main.player[owner.target].Center) * 2.5f, ModContent.ProjectileType<ScholarBolt>(), 10, 0);
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
            if (++a % 5 == 0 && target && Projectile.timeLeft > 45 && Projectile.timeLeft < 300)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (6.2f * Projectile.velocity + move) / 6.2f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            if (Projectile.timeLeft < 45)
            {
                Projectile.velocity *= 0.95f;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6.2f)
            {
                vector *= 6.2f / magnitude;
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
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            for (int i = 1; i < 5; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(5 - i) / 5);
                var fadeMult = 1f / 5;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, null, Color.Pink * (1f - fadeMult * i) * 0.5f, Projectile.oldRot[i], Projectile.Size / 2, _scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(3)) * (Projectile.timeLeft < 20 ? 0.98f : 1);
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
                    Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2 + offset, null, Color.Pink * (1f - fadeMult * i) * 0.5f, Projectile.oldRot[i], Projectile.Size / 2, _scale, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        float[] sinething = new float[10];
        public override void AI()
        {

            for (int num25 = sinething.Length - 1; num25 > 0; num25--)
            {
                sinething[num25] = sinething[num25 - 1];
            }
            sinething[0]++;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-3)) * (Projectile.timeLeft < 20 ? 0.98f : 1);
        }
    }
}
