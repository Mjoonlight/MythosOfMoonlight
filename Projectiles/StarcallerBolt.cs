using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Projectiles.VFXProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles
{
    public class StarcallerBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 60;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 4000;
            Projectile.Size = new Vector2(18, 30);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            Projectile.ai[2] -= 0.01f;
            Projectile.tileCollide = false;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[2] >= 0 && hit.Crit)
            {
                Projectile.NewProjectile(null, target.Center - new Vector2(0, target.height / 2 + 40), Vector2.UnitY, ModContent.ProjectileType<StarcallerPortal>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            Projectile.ai[2] -= 0.01f;
        }
        public override bool? CanDamage() => Projectile.ai[2] >= (Projectile.ai[1] == 0 ? -0.5f : -0.1f);
        public override void AI()
        {
            if (Projectile.timeLeft < 100 || Projectile.ai[2] < 0)
            {
                if (Projectile.ai[1] == 0)
                    Projectile.ai[2] -= 0.025f;
                else
                    Projectile.ai[2] -= 0.005f;
            }
            if (Projectile.ai[2] < -1)
                Projectile.Kill();
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.velocity.Y = MathHelper.Clamp(Projectile.velocity.Y + 0.002f, -2, 2);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex(Texture);

            Main.spriteBatch.Reload(BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float mult = (1f - fadeMult * i);
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.White * 0.5f * mult * (1 + Projectile.ai[2]), Projectile.rotation, Projectile.Size / 2, Projectile.scale * MathHelper.Clamp(mult, 0.75f, 1f), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);

            lightColor = Color.White * (1 + Projectile.ai[2]);
            return true;
        }
    }
    public class StarcallerPortal : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetStaticDefaults()
        {
            MythosOfMoonlight.projectileFinalDrawList.Add(Type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? CanDamage() => false;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.35f, 0.35f, 0.45f);
            Projectile.velocity.SafeNormalize(Vector2.UnitX);
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 30)
                Projectile.ai[2] = MathHelper.SmoothStep(0, 1, Projectile.ai[0] / 30);
            else if (Projectile.ai[0] > 70)
                Projectile.ai[2] = MathHelper.SmoothStep(1, 0, (Projectile.ai[0] - (70)) / 30);

            if (Projectile.ai[0] >= 30 && Projectile.ai[0] <= 70 && Projectile.ai[0] % 15 == 0)
            {
                Vector2 offset = Main.rand.NextVector2Circular(40, 10);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + offset, Projectile.velocity * 3.5f, ModContent.ProjectileType<StarcallerBolt>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 1, -0.01f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + offset, Projectile.velocity, ModContent.ProjectileType<StarcallerShotVFX>(), 0, 0, Projectile.owner, ai2: 1);
            }

            if (Projectile.ai[0] == 12)
                SoundEngine.PlaySound(new SoundStyle("MythosOfMoonlight/Assets/Sounds/miscMagicPulse") { PitchVariance = 0.25f, Pitch = -0.1f, MaxInstances = 0 }, Projectile.Center);

            Projectile.ai[1] = MathHelper.Lerp(Projectile.ai[1], MathF.Sin(Projectile.ai[0] * .05f) * 0.05f, 0.25f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetExtraTex("Extra/star_02");
            Vector2 scale = new Vector2(1f + Projectile.ai[1], 0.25f - Projectile.ai[1] * 0.5f);
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.spriteBatch.Reload(MythosOfMoonlight.SpriteRotation);
            MythosOfMoonlight.SpriteRotation.Parameters["rotation"].SetValue(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 125));
            MythosOfMoonlight.SpriteRotation.Parameters["scale"].SetValue(scale * 0.2f * Projectile.ai[2]);
            Vector4 col = (new Color(44, 137, 215) * Projectile.ai[2]).ToVector4();
            col.W = Projectile.ai[2] * 0.15f;
            MythosOfMoonlight.SpriteRotation.Parameters["uColor"].SetValue(col * 0.75f);
            for (int i = 0; i < 60; i++)
            {
                float s = MathHelper.SmoothStep(Projectile.ai[2], 0, (float)i / 60);
                Vector2 pos = Projectile.Center - new Vector2(i * s * 0.5f, 0).RotatedBy(Projectile.velocity.ToRotation());
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, Color.White, Projectile.velocity.ToRotation() + MathHelper.PiOver2, tex.Size() / 2, s, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, Color.White, Projectile.velocity.ToRotation() + MathHelper.PiOver2, tex.Size() / 2, s, SpriteEffects.FlipHorizontally, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Main.spriteBatch.Reload(effect: null);
            return false;
        }
    }
}
