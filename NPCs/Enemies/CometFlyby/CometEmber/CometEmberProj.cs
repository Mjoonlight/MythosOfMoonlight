using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Enemies.CometFlyby.CometEmber
{
    public class CometEmberProj : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Explosion");
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Fire);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
        }
        public float Scale
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public float Alpha
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public override void AI()
        {
            Alpha = Projectile.timeLeft / 5f;
            Scale = 10f * (2f * (5 - Projectile.timeLeft) + 1f);
            if (Projectile.timeLeft == 4)
            {
                for (int i = 1; i <= 100; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(7.5f, 7.5f);
                    Dust dust = Dust.NewDustDirect(Projectile.Center, 1, 1, ModContent.DustType<PurpurineDust>());
                    dust.velocity = vel;
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(1f, 4f);
                }
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Projectiles/PilgrimExplosion_Extra", AssetRequestMode.ImmediateLoad).Value;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            BlendState blend = BlendState.Additive;
            float sc = 10 * Scale / tex.Width;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, blend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(tex, pos, null, Color.White * Alpha, 0f, tex.Size() / 2f, sc, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Vector2.Distance(new Vector2(projHitbox.Center.X, projHitbox.Center.Y), new Vector2(targetHitbox.Center.X, targetHitbox.Center.Y)) <= Scale + targetHitbox.Width / 2f;
        }
    }
}
