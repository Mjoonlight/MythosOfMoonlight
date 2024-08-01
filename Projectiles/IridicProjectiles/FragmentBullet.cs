using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Linq;
using Terraria.GameContent;
using Terraria.ID;
using MythosOfMoonlight.Common.Crossmod;

namespace MythosOfMoonlight.Projectiles.IridicProjectiles
{
    public class FragmentBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fragment Bullet");
            Projectile.AddElement(CrossModHelper.Celestial);
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 100000;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 3;
            Projectile.height = 3;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = int.MaxValue;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 15;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 200;
        }
        Vector2 startPos;
        public override bool PreAI()
        {
            if (startPos == Vector2.Zero)
                startPos = Projectile.Center;
            return true;
        }
        public override void AI()
        {
            /*if (Main.player[Projectile.owner].ownedProjectileCounts[Projectile.type] > 3)
                Projectile.Kill();*/
            if (Projectile.ai[1] == 0)
                if (startPos.Distance(Projectile.Center) > 1100)
                {
                    Projectile.ai[1] = 1;
                    Projectile.velocity = Vector2.Zero;
                }
            if (Projectile.ai[1] == 1)
                Projectile.scale -= (0.05f / 200);
            if (Projectile.scale <= 0)
                Projectile.Kill();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[1] == 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Reload(BlendState.Additive);
            if (startPos != Vector2.Zero)
            {
                Vector2 start = startPos;
                Vector2 end = Projectile.Center;
                float num = Vector2.Distance(start, end);
                Vector2 vector = (end - start) / num;
                Vector2 vector2 = start;
                Vector2 screenPosition = Main.screenPosition;
                for (float num2 = 0f; num2 <= num; num2++)
                {
                    Main.spriteBatch.Draw(Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/Ex3"), vector2 - screenPosition, null, Color.Lerp(Color.White, Color.Purple, Projectile.scale) * Projectile.scale, Helper.FromAToB(start, end).ToRotation(), Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/Ex3").Size() / 2, new Vector2(1, .1f), SpriteEffects.None, 0f);
                    vector2 = start + num2 * vector;
                }
            }

            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[1] == 0;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[1] == 0)
            {
                if (target.life <= 0)
                    Projectile.Kill();
                Projectile.velocity = Vector2.Zero;
                for (int i = 1; i <= 3; i++)
                {
                    Vector2 vel = -Utils.SafeNormalize(Projectile.oldVelocity, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-.66f, .66f)) * Main.rand.NextFloat(1f, 2f);
                    Dust dust = Dust.NewDustDirect(target.Center, Projectile.width, Projectile.height, ModContent.DustType<PurpurineDust>(), vel.X, vel.Y, 0, default, Main.rand.NextFloat(.6f, 1.2f));
                    dust.noGravity = true;
                    dust.velocity = vel;
                }
            }

            Projectile.ai[1] = 1;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            for (int i = 1; i <= 3; i++)
            {
                Vector2 vel = -Utils.SafeNormalize(Projectile.oldVelocity, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-.66f, .66f)) * Main.rand.NextFloat(1f, 2f);
                Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<PurpurineDust>(), vel.X, vel.Y, 0, default, Main.rand.NextFloat(.6f, 1.2f));
                dust.noGravity = true;
                dust.velocity = vel;
            }
            Projectile.ai[1] = 1;
            return false;
        }
    }
}
