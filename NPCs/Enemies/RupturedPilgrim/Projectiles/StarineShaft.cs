using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameContent;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles
{
    public class StarineShaft : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starine Shaft");
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 60;
            projectile.aiStyle = 1;
            projectile.friendly = false;
            projectile.tileCollide = true;
            projectile.hostile = true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].velocity.Y = -1.5f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D drawTexture = Main.projectileTexture[projectile.type];
			Rectangle sourceRectangle = new Rectangle(0, 0, drawTexture.Width, drawTexture.Height);
			spriteBatch.Draw(drawTexture, projectile.Center - Main.screenPosition, sourceRectangle, Color.White, projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);

            //3hi31mg
            var off = new Vector2(projectile.width / 2, projectile.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = Main.projectileTexture[projectile.type];
            var frame = new Rectangle(0, projectile.frame, projectile.width, projectile.height);
            var orig = frame.Size() / 2f;
            var trailLength = ProjectileID.Sets.TrailCacheLength[projectile.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.70f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = projectile.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, projectile.oldPos[i] - Main.screenPosition + off, frame, clr * (1f - fadeMult * i), projectile.oldRot[i], orig, scale, flipType, 0f);
            }
            return false;
        }
    }
}