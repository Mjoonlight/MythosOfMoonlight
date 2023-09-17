using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class TinySightseer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 7;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
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
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.aiStyle = -1;
            Projectile.width = 24;
            Projectile.tileCollide = false;
            Projectile.height = 20;
        }
        public override bool? CanDamage() => false;
        public override void AI()
        {
            if (Projectile.ai[0] == 1)
                Projectile.timeLeft = Main.rand.Next(300, 600);
            Projectile.frameCounter++;
            if (Projectile.frameCounter == 5)
                Projectile.frame = 0;
            if (Projectile.frameCounter == 10)
            {
                Projectile.frame = 1;
                Projectile.frameCounter = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 15 == 0)
                Projectile.ai[1] = MathHelper.ToRadians(Main.rand.NextFloat(-3, 3));
            Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], Projectile.ai[1], 0.1f);
            Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[2]);
            if (Projectile.velocity.Length() < 1.5f)
                Projectile.velocity *= 1.1f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            var trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.70f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, new Rectangle(0, Projectile.frame * 22, 24, 22), Color.White * (1f - fadeMult * i), Projectile.oldRot[i], Projectile.Size / 2, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
