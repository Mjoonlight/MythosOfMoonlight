using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace MythosOfMoonlight.Projectiles
{
    public class FallingStarTiny : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 400;
            Projectile.light = 1f;
            Projectile.penetrate = -1;
            Projectile.hide = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = TextureAssets.Extra[ExtrasID.FallingStar].Value;
            if (Projectile.ai[0] == 0)
            {
                Main.spriteBatch.Reload(BlendState.Additive);
                float alpha = MathHelper.Clamp(Projectile.velocity.Length(), 0, 1f);
                Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(1).RotatedBy(Projectile.rotation * 3) - Main.screenPosition, null, Color.CornflowerBlue * 0.5f * alpha, Projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 4), 2f * 0.3f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(1).RotatedBy(-Projectile.rotation * 3) - Main.screenPosition, null, Color.CornflowerBlue * 0.5f * alpha, Projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 4), 2f * 0.3f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Cyan * ((Projectile.ai[2] - 0.5f) * 0.5f * alpha), Projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 4), Projectile.ai[2] * 0.3f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Cyan * 0.25f * alpha, Projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 4), 1.5f * 0.3f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(3).RotatedBy(Projectile.rotation) - Main.screenPosition, null, Color.DarkSlateBlue * 0.5f * alpha, Projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 4), 1f * 0.3f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(3).RotatedBy(-Projectile.rotation) - Main.screenPosition, null, Color.DarkSlateBlue * 0.5f * alpha, Projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 4), 1f * 0.3f, SpriteEffects.None, 0);
                Main.spriteBatch.Reload(BlendState.AlphaBlend);
            }
            Main.spriteBatch.Reload(BlendState.Additive);
            Texture2D tex2 = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White * ((MathF.Sin(Main.GlobalTimeWrappedHourly * 1.5f) + 1) / 2), Projectile.rotation, tex2.Size() / 2, 1f, SpriteEffects.None, 0);

            Main.spriteBatch.Reload(BlendState.AlphaBlend);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Reload(BlendState.Additive);
            for (int i = 0; i < 4; i++)
            {
                float angle = Helper.CircleDividedEqually(i, 4) + Main.GlobalTimeWrappedHourly * 3;
                Vector2 pos = Projectile.Center + new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 1.5f) * 2).RotatedBy(angle);
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, Color.CornflowerBlue * 0.6f, Projectile.rotation, tex.Size() / 2, 1f, SpriteEffects.None, 0);
            }
            for (int i = 0; i < 4; i++)
            {
                float angle = Helper.CircleDividedEqually(i, 4) - Main.GlobalTimeWrappedHourly * 3;
                Vector2 pos = Projectile.Center - new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 1.5f) * 2).RotatedBy(angle);
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, Color.CornflowerBlue * 0.6f, Projectile.rotation, tex.Size() / 2, 1f, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return true;
        }
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center, 278, Projectile.velocity);
            if (Main.dayTime) Projectile.Kill();
            Projectile.ai[2] = MathHelper.Clamp(Projectile.ai[2] + 0.05f, 1f, 1.5f);
            if (Projectile.ai[2] > 1.49f)
                Projectile.ai[2] = 1f;
            Projectile.timeLeft = 10;
            Projectile.rotation += MathHelper.ToRadians(Math.Clamp(Projectile.velocity.Length(), 0, 5));
            if (Projectile.ai[0] == 1)
            {
                Projectile.velocity *= 0.75f;
                if (TRay.CastLength(Projectile.Center, Vector2.UnitY, 60) > 40)
                    Projectile.velocity.Y = 1f;
                else
                    Projectile.velocity.Y = 0;
            }

            if (TRay.CastLength(Projectile.Center, Vector2.UnitY, 60) < 10)
            {
                Projectile.damage = 0;
                Projectile.velocity *= 0.35f;
                Projectile.tileCollide = false;
                //Projectile.Center += new Vector2(0, Projectile.height / 3);
                Projectile.Kill();
            }
            // just some homing code 
        }
        public override void OnKill(int timeLeft)
        {
            for (int num905 = 0; num905 < 3; num905++)
            {
                int num906 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), (int)Projectile.Size.X, (int)Projectile.Size.Y, 31, 0f, 0f, 0, default(Color), 2.5f);
                Main.dust[num906].position = Projectile.position + Vector2.UnitX.RotatedByRandom(3.1415927410125732) * Projectile.Size.X / 2f;
                Main.dust[num906].noGravity = true;
                Dust dust2 = Main.dust[num906];
                dust2.velocity *= 3f;
            }
            for (int num899 = 0; num899 < 2; num899++)
            {
                int num900 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), (int)Projectile.Size.X, (int)Projectile.Size.Y, 31, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num900].position = Projectile.position + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.Size.X / 2f;
            }
            Color newColor7 = Color.CornflowerBlue;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 0.8f);
            }
            for (float num614 = 0f; num614 < 1; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (3f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (3f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
            Vector2 vector52 = new Vector2(Main.screenWidth, Main.screenHeight);
            if (Projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector52 / 2f, vector52 + new Vector2(400f))))
            {
                for (int num616 = 0; num616 < 5; num616++)
                {
                    Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * Projectile.velocity.Length(), Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17));
                }
            }
        }
    }
}
