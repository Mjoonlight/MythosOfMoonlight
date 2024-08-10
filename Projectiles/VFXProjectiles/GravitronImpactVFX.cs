using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Common.Crossmod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace MythosOfMoonlight.Projectiles.VFXProjectiles
{
    public class GravitronImpactVFX : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Assets/Textures/Extra/blank";
        public override void SetStaticDefaults()
        {
            Projectile.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 50;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 50;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y - 100, Projectile.width, Projectile.height);
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        int seed;
        public override void OnSpawn(IEntitySource source)
        {
            seed = Main.rand.Next(int.MaxValue);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Assets/Textures/Extra/cone4");
            Main.spriteBatch.Reload(BlendState.Additive);
            UnifiedRandom rand = new UnifiedRandom(seed);
            float max = 40;
            if (lightColor == Color.Transparent)
            {
                float alpha = MathHelper.Lerp(0.5f, 0, Projectile.ai[1]) * 2;
                Main.spriteBatch.Reload(MythosOfMoonlight.SpriteRotation);
                MythosOfMoonlight.SpriteRotation.Parameters["rotation"].SetValue(rand.NextFloat(MathHelper.Pi, MathHelper.TwoPi) * (rand.NextFloatDirection() > 0 ? 1 : -1) + Projectile.ai[1]);
                MythosOfMoonlight.SpriteRotation.Parameters["scale"].SetValue(new Vector2(1, rand.NextFloat(0.2f, 0.8f)));
                MythosOfMoonlight.SpriteRotation.Parameters["uColor"].SetValue(Color.DarkViolet.ToVector4() * alpha * 0.5f);
                for (float i = 0; i < max; i++)
                {
                    float angle = Helper.CircleDividedEqually(i, max * 2) + MathHelper.Pi;
                    float scale = rand.NextFloat(0.1f, .4f);
                    Vector2 offset = new Vector2(Main.rand.NextFloat(-10, 20) * Projectile.ai[1] * scale, 0).RotatedBy(angle);
                    for (float j = 0; j < 2; j++)
                        Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.DarkViolet * alpha * 0.5f, angle, new Vector2(0, tex.Height / 2), new Vector2(Projectile.ai[1], alpha) * scale, SpriteEffects.None, 0);
                }
                Main.spriteBatch.Reload(effect: null);
                for (float i = 0; i < max; i++)
                {
                    float angle = Helper.CircleDividedEqually(i, max * 2) + MathHelper.Pi;
                    float scale = rand.NextFloat(0.2f, .6f);
                    Vector2 offset = new Vector2(Main.rand.NextFloat(-10, 20) * Projectile.ai[1] * scale, 0).RotatedBy(angle);
                    for (float j = 0; j < 2; j++)
                        Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.DarkViolet * alpha * 0.5f, angle, new Vector2(0, tex.Height / 2), new Vector2(Projectile.ai[1], alpha) * scale, SpriteEffects.None, 0);
                }
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.ai[1] = MathHelper.Lerp(Projectile.ai[1], 1, 0.1f);
            if (Projectile.ai[1] > 1)
                Projectile.Kill();
        }
    }
}
