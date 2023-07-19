using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Projectiles
{
    public class Starine_Strobulb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }
        float brightness = 0;
        Vector2 scale = new(.4f, .2f);
        public override void AI()
        {
            Player Player = Main.player[Projectile.owner];
            if (Projectile.ai[0] >= 26f)
            {
                if (Projectile.ai[0] == 26f) SoundEngine.PlaySound(SoundID.Item25, Player.Center);
                Projectile.ai[0] = 27f;
                bool released = false;
                if (!Player.channel || Player.noItems || Player.CCed) released = true;
                if (released)
                {
                    Projectile.ai[1]++;
                    if (Projectile.ai[1] <= 5)
                    {
                        if (Projectile.ai[1] == 3f)
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(0, 100f).RotatedBy(Projectile.rotation), Vector2.Zero, ModContent.ProjectileType<Starine_Flash>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);

                        brightness += .05f;
                        scale += new Vector2(.2f, .2f);
                    }
                    else
                    {
                        if (Projectile.ai[1] == 10) Projectile.Kill();
                        brightness -= .25f;
                        scale -= new Vector2(.3f, .4f);
                    }
                }
            }
            else
            {
                if (Projectile.ai[0] % 5 == 0) brightness += .03f;
                if (!Player.channel || Player.noItems || Player.CCed) Projectile.Kill();
            }
            Projectile.ai[0]++;
            brightness += (float)Math.Sin(Projectile.ai[0] / 10f) / 1000f;
            if (Main.MouseWorld.X > Player.Center.X) Player.ChangeDir(1);
            else if (Main.MouseWorld.X < Player.Center.X) Player.ChangeDir(-1);
            Projectile.rotation = (Player.Center - Main.MouseWorld).ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = Player.direction;
            Projectile.Center = Player.MountedCenter;
            Projectile.position = Projectile.Center;
            Player.itemAnimation = 2;
            Player.itemTime = 2;
            Player.heldProj = Projectile.whoAmI;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Effect").Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(0, -10f).RotatedBy(Projectile.rotation) - Main.screenPosition, null, new Color(brightness, brightness, brightness, brightness),
                Projectile.rotation, new Vector2(84f, 0), new Vector2(1f + (.5f * Math.Abs((float)Math.Sin(Main.GameUpdateCount / 10f))), 1f + (.15f * Math.Abs((float)Math.Sin(Main.GameUpdateCount / 10f)))) * scale, SpriteEffects.None, 1);
            Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(0, -10f).RotatedBy(Projectile.rotation) - Main.screenPosition, null, new Color(brightness, brightness, brightness, brightness),
                Projectile.rotation, new Vector2(84f, 0), new Vector2(1f + (.5f * Math.Abs((float)Math.Cos(Main.GameUpdateCount / 10f))), 1f + (.15f * Math.Abs((float)Math.Cos(Main.GameUpdateCount / 10f)))) * scale, SpriteEffects.None, 1);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return true;
        }
    }
    //Only using a different Projectile to avoid collision bs
    public class Starine_Flash : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_164";
        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}