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
    public class Starine_Sparkle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Sparkle");
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
        }
        public const int MAX_TIMELEFT = 240;
        public override void SetDefaults()
        {
            projectile.height = projectile.width = 18;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.damage = 30;
            projectile.tileCollide = true;
            projectile.timeLeft = MAX_TIMELEFT;
            projectile.penetrate = -1;
        }
        public override void AI()
        {
            var dustType = ModContent.DustType<StarineDust>();  
            var dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType);
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity.Y = -1.5f;
            if (HasCollided)
            {
                projectile.rotation += 0.05f;
            }
            else
            {
                projectile.rotation += 0.2f;
            }
            if (projectile.velocity.Y == 0)
            {
                projectile.velocity.X = MathHelper.Lerp(projectile.velocity.X, 0, 0.35f);
                projectile.velocity.Y = -projectile.oldVelocity.Y / 5;
            }
            else
            {
                projectile.velocity.Y += 0.1f;
            }
        }
        bool HasCollided = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (HasCollided == false)
            {
                Main.PlaySound(SoundID.Item10.WithVolume(0.8f), projectile.Center);
                HasCollided = true;
            }
            // projectile.velocity.Y = -projectile.oldVelocity.Y / 5;
            // if (projectile.velocity.X == 0)
            // {
            //    projectile.velocity.X = -projectile.oldVelocity.X / 5;
            // }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //3hi31mg
            var off = new Vector2(projectile.width / 2, projectile.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = Main.projectileTexture[projectile.type];
            var frame = new Rectangle(0, projectile.frame, projectile.width, projectile.height);
            var orig = frame.Size() / 2f;
            var trailLength = ProjectileID.Sets.TrailCacheLength[projectile.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.1f, 0.5f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = projectile.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, projectile.oldPos[i] - Main.screenPosition + off, frame, clr * (1f - fadeMult * i), projectile.oldRot[i], orig, scale, flipType, 0f);
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
