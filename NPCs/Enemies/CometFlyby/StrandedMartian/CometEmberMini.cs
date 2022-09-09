using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace MythosOfMoonlight.NPCs.Enemies.CometFlyby.StrandedMartian
{
    public class CometEmberMini : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Ember Mini");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.netUpdate = true;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.Kill();
        }
        private enum ProjState
        {
            Straight,
            Stomp
        }
        private ProjState State
        {
            get { return (ProjState)(int)Projectile.ai[1]; }
            set { Projectile.ai[1] = (int)value; }
        }
        private void SwitchTo(ProjState state)
        {
            State = state;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1, 1, 1);
            var dustType = ModContent.DustType<PurpurineDust>();
            var dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity.Y = -1f;
            if (Projectile.timeLeft < 179)
            {
                if (Projectile.timeLeft == 178)
                {
                }
                switch (State)
                {
                    case ProjState.Straight:
                        {
                            Projectile.rotation += MathHelper.ToRadians(-Projectile.timeLeft * 6f * Projectile.ai[0]);
                            Projectile.velocity.Y += .15f;
                            if (Projectile.timeLeft <= 60 || Math.Abs(Projectile.Center.X - Main.player[Projectile.owner].Center.X) <= 76)
                            {
                                SwitchTo(ProjState.Stomp);
                            }
                            break;
                        }
                    case ProjState.Stomp:
                        {
                            Projectile.rotation = MathHelper.Lerp(Projectile.rotation, MathHelper.ToRadians(180), .05f);
                            Projectile.velocity.X *= .9f;
                            Projectile.velocity.Y += .3f;
                            break;
                        }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CometEmberExplode>(), Projectile.damage, Projectile.knockBack);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 ori = new(9, 9);
            for (int i = 0; i <= 4; i += 1)
            {
                Vector2 pos = Projectile.oldPos[i] + ori - Main.screenPosition;
                float scale = (float)Math.Sqrt(((float)Projectile.oldPos.Length - i) / Projectile.oldPos.Length) * .75f;
                Color color = Color.White * (float)((float)((float)Projectile.oldPos.Length - i) / Projectile.oldPos.Length) * ((float)(255 - (float)Projectile.alpha) / 255f);
                Main.EntitySpriteDraw(tex, pos, null, color, Projectile.oldRot[i], ori, scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
