using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Projectiles
{
    public class Starine_Strobulb : ModProjectile
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
        float brightness = 0;
        Vector2 scale = new Vector2(.4f, .2f);
        public override void AI()
        {
            Player Player = Main.player[projectile.owner];
            if (projectile.ai[0] >= 26f)
			{
                if (projectile.ai[0] == 26f) Main.PlaySound(SoundID.Item25, Player.Center);
                projectile.ai[0] = 27f;
                bool released = false;
                if (!Player.channel || Player.noItems || Player.CCed) released = true;
                if (released)
                {
					projectile.ai[1]++;
                    if (projectile.ai[1] <= 5)
                    {
                        if (projectile.ai[1] == 3f)
                        {
                            Projectile.NewProjectile(projectile.Center + new Vector2(0, 100f).RotatedBy(projectile.rotation), Vector2.Zero, ModContent.ProjectileType<Starine_Flash>(), projectile.damage, projectile.knockBack,Main.myPlayer);
                        }
                        brightness += .05f;
                        scale += new Vector2(.2f, .2f);
                    }
                    else
					{
                        if (projectile.ai[1] == 10) projectile.Kill();
                        brightness -= .25f;
                        scale -= new Vector2(.3f, .4f);
                    }
				}
            }
			else
			{
                if (projectile.ai[0] % 5 == 0) brightness += .03f;
                if (!Player.channel || Player.noItems || Player.CCed) projectile.Kill();
			}
            projectile.ai[0]++;
            brightness += (float)Math.Sin(projectile.ai[0] / 10f) / 1000f;
            if (Main.MouseWorld.X > Player.Center.X) Player.ChangeDir(1);
            else if (Main.MouseWorld.X < Player.Center.X) Player.ChangeDir(-1);
            projectile.rotation = (Player.Center - Main.MouseWorld).ToRotation() + MathHelper.PiOver2;
            projectile.spriteDirection = Player.direction;
            projectile.Center = Player.MountedCenter;
            projectile.position = projectile.Center;
            Player.itemAnimation = 2;
            Player.itemTime = 2;
            Player.heldProj = projectile.whoAmI;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("MythosOfMoonlight/Projectiles/Starine_Strobulb_Effect");
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            spriteBatch.Draw(texture, projectile.Center + new Vector2(0, -10f).RotatedBy(projectile.rotation) - Main.screenPosition, null, new Color(brightness, brightness, brightness, brightness), 
                projectile.rotation, new Vector2(84f, 0), new Vector2(1f + (.5f * Math.Abs((float)Math.Sin(Main.GameUpdateCount / 10f))), 1f + (.15f * Math.Abs((float)Math.Sin(Main.GameUpdateCount / 10f)))) * scale, SpriteEffects.None, 1f);
            spriteBatch.Draw(texture, projectile.Center + new Vector2(0, -10f).RotatedBy(projectile.rotation) - Main.screenPosition, null, new Color(brightness, brightness, brightness, brightness), 
                projectile.rotation, new Vector2(84f, 0), new Vector2(1f + (.5f * Math.Abs((float)Math.Cos(Main.GameUpdateCount / 10f))), 1f + (.15f * Math.Abs((float)Math.Cos(Main.GameUpdateCount / 10f)))) * scale, SpriteEffects.None, 1f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return true;
        }
    }
    //Only using a different projectile to avoid collision bs
    public class Starine_Flash : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_164";
        public override void SetDefaults()
        {
            projectile.width = 200;
            projectile.height = 200;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.timeLeft = 2;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}