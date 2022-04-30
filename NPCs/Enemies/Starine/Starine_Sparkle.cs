using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Dusts;
using Terraria.Audio;
using Terraria.GameContent;

namespace MythosOfMoonlight.NPCs.Enemies.Starine
{
    public class Starine_Sparkle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Sparkle");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public const int MAX_TIMELEFT = 240;
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 18;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.damage = 30;
            Projectile.tileCollide = true;
            Projectile.timeLeft = MAX_TIMELEFT;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            var dustType = ModContent.DustType<StarineDust>();
            var dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity.Y = -1.5f;
            if (HasCollided)
                Projectile.rotation += 0.05f;
            else
                Projectile.rotation += 0.2f;

            if (Projectile.velocity.Y == 0)
            {
                Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, 0, 0.35f);
                Projectile.velocity.Y = -Projectile.oldVelocity.Y / 5;
            }
            else
                Projectile.velocity.Y += 0.1f;
        }
        bool HasCollided = false;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (HasCollided == false)
            {
                SoundEngine.PlaySound(SoundID.Item10.WithVolume(0.8f), Projectile.Center);
                HasCollided = true;
            }
            // Projectile.velocity.Y = -Projectile.oldVelocity.Y / 5;
            // if (Projectile.velocity.X == 0)
            // {
            //    Projectile.velocity.X = -Projectile.oldVelocity.X / 5;
            // }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //3hi31mg
            var off = new Vector2(Projectile.width / 2, Projectile.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = TextureAssets.Projectile[Projectile.type].Value;
            var frame = new Rectangle(0, Projectile.frame, Projectile.width, Projectile.height);
            var orig = frame.Size() / 2f;
            var trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.1f, 0.5f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = Projectile.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] - Main.screenPosition + off, frame, clr * (1f - fadeMult * i), Projectile.oldRot[i], orig, scale, flipType, 0f);
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
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
