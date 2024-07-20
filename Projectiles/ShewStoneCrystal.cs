using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using MythosOfMoonlight.Projectiles.VFXProjectiles;

namespace MythosOfMoonlight.Projectiles
{
    public class ShewStoneCrystal : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new(44);
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.timeLeft;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = -oldVelocity;
            return false;
        }
        bool hasHit = false;
        public override bool? CanDamage() => !hasHit;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hasHit = true;
            Projectile.timeLeft = 8;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item101, Projectile.Center);
            for (int i = 0; i < 30; i++)
                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(15, 15), DustID.GemDiamond, Main.rand.NextVector2Circular(15, 15)).noGravity = true;
            Projectile.NewProjectile(null, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShewStoneRain>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            int off = Main.rand.Next(3, 5);
            for (int i = 0; i < off; i++)
            {
                Projectile.NewProjectile(null, Projectile.Center, Main.rand.NextVector2Circular(4, 4), ModContent.ProjectileType<ShewStoneArrow>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex(Texture);
            Texture2D glow = Helper.GetTex(Texture + "_Glow");
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, null, Color.White * 0.5f, Projectile.rotation, glow.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0]++;
            Projectile.ai[1] = MathHelper.Lerp(Projectile.ai[1], 0.4f, 0.01f);
            Projectile.scale = MathHelper.Lerp(Projectile.scale, 1 + MathF.Sin(Projectile.ai[0] * 0.5f) * Projectile.ai[1], 0.1f);
            Projectile.rotation += MathHelper.ToRadians(3 * Projectile.scale);
            if (player.whoAmI == Main.myPlayer && Projectile.timeLeft > 7)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Helper.FromAToB(Projectile.Center, Main.MouseWorld) * 4.5f, 0.025f);
                if (Projectile.timeLeft < 500 && Main.mouseLeft)
                    Projectile.timeLeft = 7;
            }
            if (Projectile.timeLeft == 7)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.NewProjectile(null, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShewExplosion>(), 0, 0, Projectile.owner);
            }
        }
    }
    public class ShewStoneRain : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetDefaults()
        {
            Projectile.Size = new(2);
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 160;
            Projectile.aiStyle = -1;
        }
        public override bool? CanDamage() => false;
        public override bool ShouldUpdatePosition() => false;
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(15, 15), DustID.GemDiamond, Main.rand.NextVector2Circular(2, 2)).noGravity = true;
            if (Projectile.timeLeft % 10 == 0)
                Projectile.NewProjectile(null, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShewRainExplosion>(), 0, 0, Projectile.owner);
            if (Projectile.timeLeft % 20 == 0)
            {
                SoundEngine.PlaySound(new SoundStyle("MythosOfMoonlight/Assets/Sounds/crystalArrowForm") { PitchVariance = .25f, MaxInstances = 3 }, Projectile.Center);
                SoundStyle shatter = SoundID.Shatter.WithPitchOffset(0.1f).WithVolumeScale(0.5f);
                shatter.PitchVariance = 0.25f;
                SoundEngine.PlaySound(shatter, Projectile.Center);
                for (int i = 0; i < 5; i++)
                    Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(15, 15), DustID.GemDiamond, Main.rand.NextVector2Circular(15, 15)).noGravity = true;

                Vector2 velocity = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-5, -2));
                for (int i = 0; i < 15; i++)
                    Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(15, 15), DustID.GemDiamond, velocity.RotatedByRandom(MathHelper.PiOver4) * 2).noGravity = true;
                Projectile.NewProjectile(null, Projectile.Center, velocity, ModContent.ProjectileType<ShewStoneArrow>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }
    public class ShewStoneArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new(18, 38);
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 500;
            Projectile.aiStyle = 1;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.timeLeft;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, Projectile.Center);
            for (int i = 0; i < 20; i++)
                Dust.NewDustPerfect(Projectile.Center, DustID.GemDiamond, Main.rand.NextVector2Circular(15, 15)).noGravity = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex(Texture);

            Main.spriteBatch.Reload(BlendState.Additive);
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float mult = (1f - fadeMult * i);
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.White * 0.5f * mult * (1 + Projectile.ai[2]), Projectile.rotation, Projectile.Size / 2, Projectile.scale * MathHelper.Clamp(mult, 0.1f, 0.75f), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            Texture2D glow = Helper.GetTex(Texture + "_Glow");
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, null, Color.White * 0.5f, Projectile.rotation, glow.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
    }
}
