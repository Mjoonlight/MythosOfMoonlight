using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class StarineShaft : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Shaft");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 400;
            Projectile.hostile = true;
        }

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
            //SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.aiStyle == 1 || Projectile.ai[0] > 0;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[2] == 0 || Projectile.timeLeft < 370;
        }
        public override void AI()
        {
            if (Projectile.ai[2] == 1)
                Projectile.aiStyle = 0;
            if (Projectile.aiStyle == 0)
            {
                if (Projectile.ai[0] == -1)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                        Main.dust[dust].scale = 2f;
                        Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                        Main.dust[dust].velocity.Y = -1.5f;
                        Main.dust[dust].noGravity = true;
                    }
                }

                if (++Projectile.ai[0] >= 0)
                {
                    if (Projectile.timeLeft > 370)
                        Projectile.velocity *= 0.99f;
                    else
                    {
                        if (Projectile.velocity.Length() < 25f)
                            Projectile.velocity *= 1.05f;
                    }
                }
                else Projectile.timeLeft++;

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);

            //3hi31mg
            var off = new Vector2(Projectile.width / 2, Projectile.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = TextureAssets.Projectile[Projectile.type].Value;
            var trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.70f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = SpriteEffects.None;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] - Main.screenPosition + off, null, clr * (1f - fadeMult * i), Projectile.oldRot[i], drawTexture.Size() / 2, 1, flipType, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}