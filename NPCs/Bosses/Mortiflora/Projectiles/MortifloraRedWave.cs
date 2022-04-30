using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora.Projectiles
{
    public class MortifloraRedWave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Wave");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.Bullet;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 720;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = Projectile.Center - Main.screenPosition;
            var off = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
            var texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            var frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight - 2);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            var orig = frame.Size() / 2f;
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + off - Main.screenPosition, frame, clr * (1f - fadeMult * i), Projectile.oldRot[i], orig, Projectile.scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, drawPos, frame, clr, Projectile.rotation, orig, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void AI()
        {
            Projectile.velocity *= 1.01f;
        }
    }
}