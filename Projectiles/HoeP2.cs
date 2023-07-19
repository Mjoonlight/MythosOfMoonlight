using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles
{
    public class HoeP2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
            Projectile.AddElement(CrossModHelper.Nature);
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 45;
            Projectile.scale = 0;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void Kill(int timeLeft)
        {

            int type = DustID.Grass;
            switch (Projectile.frame)
            {
                case 0:
                    type = DustID.BrownMoss; break;
                case 5:
                    type = DustID.CorruptionThorns; break;
                case 4:
                    type = DustID.Torch; break;
                case 2:
                    type = DustID.Ice; break;
            }
            Helper.SpawnDust(Projectile.Center, Vector2.One, type, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-1, -2)), 10, new Action<Dust>((target) => { target.noGravity = true; target.scale = Main.rand.NextFloat(0.5f, 0.75f); }
            ));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 22, 16, 22), lightColor, Projectile.rotation, new Vector2(Projectile.width / 2, 20), new Vector2(Projectile.scale, MathHelper.Clamp(Projectile.scale * 2, 0, 1)), Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {

            if (Main.LocalPlayer.ZoneDesert)
                Projectile.frame = 1;
            else if (Main.LocalPlayer.ZoneSnow)
                Projectile.frame = 2;
            else if (Main.LocalPlayer.ZoneJungle)
                Projectile.frame = 3;
            else if (Main.LocalPlayer.ZoneUnderworldHeight)
                Projectile.frame = 4;
            else if (Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneCrimson)
                Projectile.frame = 5;
            else if (Main.LocalPlayer.ZoneForest)
                Projectile.frame = 6;


            int type = DustID.Grass;
            switch (Projectile.frame)
            {
                case 0:
                    type = DustID.BrownMoss; break;
                case 5:
                    type = DustID.CorruptionThorns; break;
                case 4:
                    type = DustID.Torch; break;
                case 2:
                    type = DustID.Ice; break;
            }
            Helper.SpawnDust(Projectile.Center, Vector2.One, type, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-1, -2)), 10, new Action<Dust>((target) => { target.noGravity = true; target.scale = Main.rand.NextFloat(0.4f, 0.6f); }
            ));
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override void AI()
        {
            if (Projectile.timeLeft == 35)
            {
                if (Projectile.ai[0] < 10)
                {
                    SoundStyle style = SoundID.Grass;
                    style.Volume = 0.5f;
                    SoundEngine.PlaySound(style, Projectile.Center);
                    Projectile a = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), 16 * Projectile.velocity + (TRay.Cast(Projectile.Center - Vector2.UnitY * 20, Vector2.UnitY, 500, true)), Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0] + 1, Projectile.ai[1]);
                    a.ai[0] = Projectile.ai[0] + 1;
                    a.ai[1] = Projectile.ai[1];
                }
            }
            if (Projectile.scale < 1)
                Projectile.scale += 0.1f;
        }
    }
}
