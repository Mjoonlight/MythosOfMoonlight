using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace MythosOfMoonlight.Projectiles.ThornDart.Orbe
{
    public class Orbe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Orbe");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.AddElement(CrossModHelper.Nature);
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 22;
            Projectile.timeLeft = 120;
            Projectile.maxPenetrate = -1;
            Projectile.tileCollide = false;
            Projectile.damage = 3;
            Projectile.knockBack = 0;
            Projectile.aiStyle = -1;
        }

        Vector2 distance;
        public override void AI()
        {
            var target = Main.player[Projectile.owner];
            distance = Projectile.position - (target.position + target.oldVelocity);
            var unsqDist = distance.Length();
            var speed = MathHelper.Max(3, distance.Length() / 20);
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, -distance.SafeNormalize(Vector2.UnitX) * speed, 0.1f);

            if (distance.LengthSquared() < 256)
            {
                var heal = (int)Projectile.ai[0];
                target.statLife += heal;
                target.HealEffect(heal);
                Projectile.timeLeft = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("MythosOfMoonlight/Projectiles/ThornDart/Orbe/Orbe_Glowy").Value;
            Rectangle rect = new(0, 0, texture.Width, texture.Height);
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Color.Lerp(Color.Transparent, new Color(5, 240, 0), Utils.Clamp(Projectile.timeLeft, 0, 60) / 60f), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = Projectile.Center - Main.screenPosition;
            var off = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
            var origTexture = TextureAssets.Projectile[Projectile.type].Value;
            texture = TextureAssets.Projectile[Type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            var frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight - 2);
            var orig = frame.Size() / 2f;

            var green = new Color(0, 255, 0, 155);
            var trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];
            var fadeMult = 1f / trailLength;
            for (int i = 1; i < trailLength; i++)
            {
                Main.EntitySpriteDraw(texture, Projectile.oldPos[i] - Main.screenPosition + off, frame, Color.Lerp(Color.Transparent, green * (1f - fadeMult * i), Utils.Clamp(Projectile.timeLeft, 0, 60) / 60f), Projectile.oldRot[i], orig, Projectile.scale * (trailLength - i) / trailLength, SpriteEffects.None, 0);
            }
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle rect = new(0, 0, texture.Width, texture.Height);
            Vector2 drawOrigin = new(texture.Width / 2, texture.Height / 2);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect, Color.Lerp(Color.Transparent, Color.White, Utils.Clamp(Projectile.timeLeft, 0, 60) / 60f), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.statLife += 3;
            Kill(0);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
    }
}