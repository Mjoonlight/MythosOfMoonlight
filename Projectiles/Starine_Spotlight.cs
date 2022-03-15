using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Projectiles
{
    public class Starine_Spotlight : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 0;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.magic = true;
        }
        public override void AI()
        {
            Player Player = Main.player[projectile.owner];
            projectile.ai[0]++;
            if (!Player.channel || Player.noItems || Player.CCed)
            {
                projectile.Kill();
            }
            Player.heldProj = projectile.whoAmI;
            Player.itemTime = 2;
            Player.itemAnimation = 2;
            if (Main.MouseWorld.X > Player.Center.X) Player.ChangeDir(1);
            else if (Main.MouseWorld.X < Player.Center.X) Player.ChangeDir(-1);
            projectile.rotation = (Player.Center - Main.MouseWorld).ToRotation() + MathHelper.PiOver2;
            projectile.position = Player.itemLocation;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 rectpos = projectile.Center + new Vector2(0, 60f).RotatedBy(projectile.rotation) - new Vector2(50f, 50f); 
            return targetHitbox.Intersects(new Rectangle((int)rectpos.X, (int)rectpos.Y, 100, 100));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("MythosOfMoonlight/Projectiles/Starine_Spotlight_Effect");
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            spriteBatch.Draw(texture, projectile.Center + new Vector2(0, -10f).RotatedBy(projectile.rotation) - Main.screenPosition, null, new Color(100, 100, 100, 100), projectile.rotation, new Vector2(84f, 0), new Vector2(1f + (.2f * (float)Math.Sin(Main.GameUpdateCount / 2)), 1f + (.2f * (float)Math.Cos(Main.GameUpdateCount / 2))), SpriteEffects.None, 1f);
            spriteBatch.Draw(texture, projectile.Center + new Vector2(0, -10f).RotatedBy(projectile.rotation) - Main.screenPosition, null, new Color(100, 100, 100, 100), projectile.rotation, new Vector2(84f, 0), new Vector2(1f + (.2f * (float)Math.Cos(Main.GameUpdateCount / 2)), 1f + (.2f * (float)Math.Sin(Main.GameUpdateCount / 2))), SpriteEffects.None, 1f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return true;
        }
    }
}