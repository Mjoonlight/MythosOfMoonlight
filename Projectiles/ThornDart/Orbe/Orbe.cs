using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Projectiles.ThornDart.Orbe
{
    public class Orbe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orbe");
            Main.projFrames[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
        }
        public override void SetDefaults()
        {
            projectile.width = projectile.height = 22;
            projectile.timeLeft = 120;
            projectile.maxPenetrate = -1;
            projectile.tileCollide = false;
            projectile.damage = 3;
            projectile.knockBack = 0;
            projectile.aiStyle = -1;
        }

        Vector2 distance;
        public override void AI()
        {
            var target = Main.player[projectile.owner];
            distance = projectile.position - (target.position + target.oldVelocity);
            var unsqDist = distance.Length();
            var speed = MathHelper.Max(3, distance.Length() / 20);
            projectile.velocity = Vector2.Lerp(projectile.velocity, -distance.SafeNormalize(Vector2.UnitX) * speed, 0.1f);

            if (distance.LengthSquared() < 256)
            {
                var heal = (int)projectile.ai[0];
                target.statLife += heal;
                target.HealEffect(heal);
                projectile.timeLeft = 0;
            }


            // For some apparent reason, 255 is supposed to be invisible and yet it displays the projectile if it is of a value 255
            // I don't understand why this one projectile has so many problems but if it works, it works
            projectile.alpha = (int)MathHelper.Lerp(0, 255, Utils.Clamp(projectile.timeLeft, 0, 60) / 60f);
            Main.NewText(projectile.alpha);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture("MythosOfMoonlight/Projectiles/ThornDart/Orbe/Orbe_Glowy");
            Rectangle rect = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, rect, Color.Lerp(Color.Transparent, new Color(5, 240, 0), Utils.Clamp(projectile.timeLeft, 0, 60) / 60f), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = projectile.Center - Main.screenPosition;
            var off = new Vector2(projectile.width / 2f, projectile.height / 2f);
            var origTexture = Main.projectileTexture[projectile.type];
            texture = mod.GetTexture("Projectiles/ThornDart/Orbe/Orbe");
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            var frame = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight - 2);
            var orig = frame.Size() / 2f;

            Main.spriteBatch.Draw(origTexture, drawPos, frame, clr, projectile.rotation, orig, projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawPos, frame, clr, projectile.rotation, orig, projectile.scale, SpriteEffects.None, 0f);

            var green = new Color(0, 255, 0, 155);
            var trailLength = ProjectileID.Sets.TrailCacheLength[projectile.type];
            var fadeMult = 1f / trailLength;
            for (int i = 1; i < trailLength; i++)
            {
                Main.spriteBatch.Draw(origTexture, projectile.oldPos[i] - Main.screenPosition + off, frame, Color.Lerp(Color.Transparent, green * (1f - fadeMult * i), Utils.Clamp(projectile.timeLeft, 0, 60) / 60f), projectile.oldRot[i], orig, projectile.scale * (trailLength - i) / trailLength, SpriteEffects.None, 0f);
            }


            return true;
        }


        public override Color? GetAlpha(Color lightColor) => Color.White * projectile.alpha;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {

            target.statLife += 3;
            Kill(0);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

        }
    }
}