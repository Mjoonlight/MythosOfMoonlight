using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using MythosOfMoonlight.Dusts;
using Terraria.Audio;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    internal class StarineSlush : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Projectiles/PilgrimExplosion_Extra";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Goop");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 200;
            Projectile.hostile = true;
            Projectile.scale = 0.01f;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].velocity.Y = -1.5f;
                Main.dust[dust].noGravity = true;
            }
            for (int i = -1; i < 2; i++)
            {
                if (i == 0)
                    continue;
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), TRay.Cast(Projectile.Center - Vector2.UnitY * 100, Vector2.UnitY, 500, true) - 30 * Vector2.UnitY, Vector2.UnitX * i, ModContent.ProjectileType<StarineShockwave>(), 12, .1f, Main.myPlayer).ai[1] = i;
            }
            //SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
        public override void AI()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 pos = Projectile.Center + Main.rand.NextVector2Circular(50, 50);
                Vector2 dVel = Helper.FromAToB(pos, Projectile.Center) * 6f;
                Dust dust = Dust.NewDustDirect(pos, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
            }

            if (TRay.CastLength(Projectile.Center, Vector2.UnitY, 200, true) < 25 && Projectile.Center.Y > Main.LocalPlayer.Center.Y - 20)
                Projectile.Kill();
            if (Projectile.scale < 0.15f)
                Projectile.scale += 0.0025f;
            //float progress = Utils.GetLerpValue(0, 200, Projectile.timeLeft);
            //Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * Math.PI) * 0.1f, 0, 0.15f);
            //if (Projectile.timeLeft > 150)
            //     Projectile.velocity = (Main.LocalPlayer.Center - Vector2.UnitY * 300 - Projectile.Center) / 20f;
            if (Projectile.timeLeft == 150)
                Projectile.velocity = -Vector2.UnitY * 5f;
            if (Projectile.timeLeft < 130 && Projectile.timeLeft > 120)
                Projectile.velocity *= 0.98f;
            if (Projectile.timeLeft < 120)
            {
                Projectile.aiStyle = 2;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Reload(BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Main.spriteBatch.Draw(drawTexture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.LightSkyBlue * 0.65f * (1f - fadeMult * i), 0, drawTexture.Size() / 2, Projectile.scale * (1f - fadeMult * i), SpriteEffects.None, 0);
                //Main.spriteBatch.Draw(drawTexture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.White * (1f - fadeMult * i), 0, Projectile.Size / 2, Projectile.scale * (1f - fadeMult * i), SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, null, Color.LightSkyBlue * 0.65f, 0f, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, null, Color.White * 0.7f, 0f, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
    }
    internal class StarineSlushSmall : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Projectiles/PilgrimExplosion_Extra";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Goop");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 488;
            Projectile.height = 486;
            Projectile.aiStyle = 2;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 200;
            Projectile.hostile = true;
            Projectile.scale = 0;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return true;
        }
        public override void Kill(int timeLeft)
        {
            SoundStyle style = SoundID.DD2_BetsyFireballImpact;
            style.Volume = 0.5f;
            SoundEngine.PlaySound(style, Projectile.Center);
            for (int i = 0; i < 16; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].velocity.Y = -1.5f;
                Main.dust[dust].noGravity = true;
            }
            for (int i = -1; i < 2; i++)
            {
                if (i == 0)
                    continue;
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), TRay.Cast(Projectile.Center - Vector2.UnitY * 50, Vector2.UnitY, 500, true) - 30 * Vector2.UnitY, Vector2.UnitX * i, ModContent.ProjectileType<StarineShockwave>(), 12, .1f, Main.myPlayer).ai[0] = 6;
            }
            // SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.scale >= 0.075f;
        }
        public override void AI()
        {
            if (TRay.CastLength(Projectile.Center, Vector2.UnitY, 200, true) < 25 && Projectile.Center.Y > Main.LocalPlayer.Center.Y - 20)
                Projectile.Kill();
            for (int i = 0; i < 2; i++)
            {
                Vector2 pos = Projectile.Center + Main.rand.NextVector2Circular(10, 10);
                Vector2 dVel = Helper.FromAToB(pos, Projectile.Center) * 6f;
                Dust dust = Dust.NewDustDirect(pos, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
            }
            if (Projectile.scale < 0.075f)
                Projectile.scale += 0.001f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Reload(BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Main.spriteBatch.Draw(drawTexture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.LightSkyBlue * 0.65f * (1f - fadeMult * i), 0, drawTexture.Size() / 2, Projectile.scale * (1f - fadeMult * i), SpriteEffects.None, 0);
                //Main.spriteBatch.Draw(drawTexture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.White * (1f - fadeMult * i), 0, Projectile.Size / 2, Projectile.scale * (1f - fadeMult * i), SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, null, Color.LightSkyBlue * 0.65f, 0f, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, null, Color.White * 0.7f, 0f, drawTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
    }
}
