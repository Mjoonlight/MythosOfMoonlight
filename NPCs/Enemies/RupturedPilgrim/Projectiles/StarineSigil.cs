using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameContent;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles
{
    public class StarineSigil : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Sigil");
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            projectile.rotation += .1f;
            projectile.alpha += 50;
            Texture2D drawTexture = Main.projectileTexture[projectile.type];
            Texture2D drawTextureGlow = ModContent.GetTexture("MythosOfMoonlight/NPCs/Enemies/RupturedPilgrim/Projectiles/StarineSigil_Glow");
            Texture2D drawTexture1 = ModContent.GetTexture("MythosOfMoonlight/NPCs/Enemies/RupturedPilgrim/Projectiles/StarineSigil_Extra1");
            Texture2D drawTexture2 = ModContent.GetTexture("MythosOfMoonlight/NPCs/Enemies/RupturedPilgrim/Projectiles/StarineSigil_Extra2");
            Rectangle sourceRectangle = new Rectangle(0, 0, drawTexture.Width, drawTexture.Height);
            spriteBatch.Draw(drawTexture, projectile.Center - Main.screenPosition, sourceRectangle, new Color(projectile.alpha, projectile.alpha, projectile.alpha, projectile.alpha), projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(drawTexture1, projectile.Center - Main.screenPosition, sourceRectangle, new Color(projectile.alpha, projectile.alpha, projectile.alpha, projectile.alpha), -projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(drawTexture2, projectile.Center - Main.screenPosition, sourceRectangle, new Color(projectile.alpha, projectile.alpha, projectile.alpha, projectile.alpha), -projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            if (!Main.dayTime)
            {
                spriteBatch.Draw(drawTextureGlow, projectile.Center - Main.screenPosition, sourceRectangle, new Color(projectile.alpha, projectile.alpha, projectile.alpha, projectile.alpha), projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public override void SetDefaults()
        {
            projectile.width = 5;
            projectile.height = 5;
            projectile.aiStyle = 0;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.hostile = true;
            projectile.timeLeft = 45;
        }
        int projDamage = 8;
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(projectile.Center, Main.rand.NextVector2Unit() * 5, ModContent.ProjectileType<StarineShaft>(), 10, 0);
            }
            Main.PlaySound(SoundID.Item9, projectile.Center);
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].noGravity = true;
            }
            for (int i = 0; i < 60; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 4f;
                Main.dust[dust].noGravity = true;
            }
        }
        int ProjectileTimer
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }
        double RandRadian => Main.rand.NextDouble() * (MathHelper.PiOver2 / 3f) - (MathHelper.PiOver2 / 6f);
        public override void AI()
        {
            if (!Main.expertMode)
            {
                if (++ProjectileTimer % 20 == 0)
                {
                    Projectile.NewProjectile(projectile.Center, 10.5f * Utils.RotatedBy(projectile.DirectionTo(Main.player[Main.myPlayer].Center), RandRadian * .5f), ModContent.ProjectileType<StarineShaft>(), projDamage, 0);
                    Main.PlaySound(SoundID.Item9, projectile.Center);
                    if (ProjectileTimer == 60)
                    {
                        projectile.Kill();
                    }
                }
            }
            else
            {
                if (++ProjectileTimer >= 15)
                {
                    if (ProjectileTimer == 55)
                    {
                        projectile.Kill();
                    }
                    else if (ProjectileTimer % 15 == 0)
                    {
                        Projectile.NewProjectile(projectile.Center, 10.5f * Utils.RotatedBy(projectile.DirectionTo(Main.player[Main.myPlayer].Center), RandRadian * .5f), ModContent.ProjectileType<StarineShaft>(), projDamage, 0);
                        Main.PlaySound(SoundID.Item9, projectile.Center);
                    }
                }
            }
        }
    }
}