using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Projectiles
{
    public class CometEmber : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Ember");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.hostile = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 180;
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
        }
        private enum ProjState
        {
            Straight,
            Stomp
        }
        private ProjState State
        {
            get { return (ProjState)(int)projectile.ai[1]; }
            set { projectile.ai[1] = (int)value; }
        }
        private void SwitchTo(ProjState state)
        {
            State = state;
        }
        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 1, 1, 1);
            if (projectile.timeLeft < 179)
            {
                switch(State)
                {
                    case ProjState.Straight:
                        {
                            projectile.rotation += MathHelper.ToRadians(projectile.ai[0] * 9f);
                            if (projectile.timeLeft <= 60 || Math.Abs(projectile.Center.X-Main.player[projectile.owner].Center.X)<=30)
                            {
                                projectile.velocity.Y = 0;
                                SwitchTo(ProjState.Stomp);
                            }
                            break;
                        }
                    case ProjState.Stomp:
                        {
                            projectile.rotation = MathHelper.Lerp(projectile.rotation, MathHelper.ToRadians(180), .05f);
                            projectile.velocity.X = 0;
                            projectile.velocity.Y += .25f;
                            break;
                        }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<CometEmberExplode>(), projectile.damage, projectile.knockBack);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            Vector2 ori = new Vector2(9, 9);
            for (int i = 0; i <= 4; i += 1)
            {
                Vector2 pos = projectile.oldPos[i] + ori - Main.screenPosition;
                float scale = (float)Math.Sqrt(((float)projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                Color color = Color.White * (float)((float)((float)projectile.oldPos.Length - i) / (float)projectile.oldPos.Length) * ((float)(255 - (float)projectile.alpha) / 255f);
                spriteBatch.Draw(tex, pos, null, color, projectile.oldRot[i], ori, 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, lightColor);
        }
    }
}
