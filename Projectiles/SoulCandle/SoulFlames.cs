using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles.SoulCandle
{
    public class SoulFlames : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[Type] = 35;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 36;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.timeLeft = 200;
            Projectile.scale = 1f;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.timeLeft;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !target.friendly;
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[2] > 0.5f;
        }
        Vector2 startP;
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White * Projectile.ai[2];
            Texture2D tex = Helper.GetExtraTex("Extra/Ex3");
            Main.spriteBatch.Reload(BlendState.Additive);
            for (int i = -1; i < 2; i++)
            {
                for (int j = 0; j < Projectile.oldPos.Length; j++)
                {
                    float f = MathHelper.Lerp(1, 0, (float)j / 35);
                    Vector2 offset = Projectile.Size / 2 + new Vector2(0, 9) + new Vector2(4 * i * f * Projectile.ai[2], 0).RotatedBy(Helper.FromAToB(Projectile.Center, startP).ToRotation()) - Main.screenPosition;

                    Color color = Color.Lerp(new Color(96, 164, 255) * 0.8f, new Color(231, 152, 255) * 0.4f, MathHelper.Clamp((float)j, 0, 25) / 25) * MathHelper.Clamp(Projectile.ai[2] - 0.5f, 0, 0.5f);
                    if (i == 0)
                        color *= 0.35f;

                    if (j > 0)
                        for (float k = 0; k < 5; k++)
                        {
                            Vector2 position = Vector2.Lerp(Projectile.oldPos[j - 1], Projectile.oldPos[j], k / 5) + offset;
                            Main.spriteBatch.Draw(tex, position, null, color, Helper.FromAToB(Projectile.oldPos[j - 1], Projectile.oldPos[j]).ToRotation(), tex.Size() / 2, new Vector2(1, MathHelper.Lerp(0.2f * (i == 0 ? 3 : 1) * Projectile.ai[2], 0, MathHelper.Clamp(j, 0, (i == 0 ? 25 : 35)) / (i == 0 ? 25 : 35))), SpriteEffects.None, 0);
                        }
                }
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return base.PreDraw(ref lightColor);
        }
        public override bool PreAI()
        {
            if (startP == Vector2.Zero && Projectile.owner == Main.LocalPlayer.whoAmI)
                startP = Main.MouseWorld;
            return base.PreAI();
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[2];
            if (Projectile.ai[2] > Main.rand.NextFloat(0.25f))
                if (++Projectile.frameCounter % 5 == 0)
                {
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                        Projectile.frame = 0;
                }
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(5 * (Projectile.ai[0] % 2 == 0 ? 1 : -1))) * 0.86f;
            if (Projectile.velocity.Length() < 0.01f)
            {
                Projectile.ai[2] = MathHelper.Max(Projectile.ai[2] - 0.1f, 0);
                if (Projectile.ai[2] <= 0)
                    Projectile.Kill();
            }
            else
                Projectile.ai[2] = MathHelper.Min(Projectile.ai[2] + 0.1f, 1);
        }
    }
}
