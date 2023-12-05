using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles.SoulCandle
{
    public class SoulCandleHeld : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.knockBack = 4;
        }

        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player == null || !player.active || player.dead || !player.channel || player.CCed)
            {
                Projectile.Kill();
                return;
            }
            Projectile.rotation = Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation() + MathHelper.PiOver2;
            player.ChangeDir(Helper.FromAToB(player.Center, Main.MouseWorld).X < 0 ? -1 : 1);
            player.itemRotation = (Projectile.rotation - MathHelper.PiOver2) * player.direction;
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.Pi);

            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            pos += (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 20;
            Projectile.Center = pos;
            player.itemTime = 2;
            player.itemAnimation = 2;
            Projectile.timeLeft = 2;

            Color col = Color.Lerp(new Color(201, 122, 255), new Color(191, 210, 255) * 0.5f, Projectile.ai[0] / 100);
            Dust dust = Dust.NewDustPerfect(Projectile.Center - new Vector2(-4, Projectile.height / 2 * player.direction).RotatedBy(Projectile.rotation - MathHelper.PiOver2), DustID.SilverFlame, new Vector2(Main.rand.NextFloat(-1, 1f), -1), newColor: col, Scale: Main.rand.NextFloat(0.75f, 1.5f));
            dust.noGravity = true;
            dust.noLight = false;

            Projectile.ai[0]++;
            if (Projectile.ai[0] == 60)
            {
                for (int i = 0; i < 30; i++)
                {
                    Dust dustt = Dust.NewDustPerfect(Projectile.Center - new Vector2(-4, Projectile.height / 2 * player.direction).RotatedBy(Projectile.rotation - MathHelper.PiOver2), DustID.SilverFlame, Main.rand.NextVector2Circular(5, 5), newColor: col, Scale: Main.rand.NextFloat(0.75f, 1.5f));
                    dustt.noGravity = true;
                    dustt.noLight = false;
                }
                SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, Projectile.Center);
                float off = Main.rand.NextFloat(MathHelper.Pi * 2);
                for (int i = 0; i < 30; i++)
                {
                    float angle = Helper.CircleDividedEqually(i, 6) + off;
                    Vector2 position = Main.MouseWorld + Vector2.UnitX.RotatedBy(angle) * 50;
                    Dust dustt = Dust.NewDustPerfect(position, DustID.SilverFlame, Main.rand.NextVector2Circular(5, 5), newColor: col, Scale: Main.rand.NextFloat(0.75f, 1.5f));
                    dustt.noGravity = true;
                    dustt.noLight = false;
                }
                for (int i = 0; i < 6; i++)
                {
                    float angle = Helper.CircleDividedEqually(i, 6) + off;
                    Vector2 position = Main.MouseWorld + Vector2.UnitX.RotatedBy(angle) * 50;
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), position, Helper.FromAToB(position, Main.MouseWorld), ModContent.ProjectileType<SoulFlames>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
            if (Projectile.ai[0] > 100)
            {
                Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center);
                Projectile proj = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, dir, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                proj.rotation = Projectile.rotation;
                proj.Center = Projectile.Center;
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation - MathHelper.PiOver2, tex.Size() / 2, Projectile.scale, player.direction < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            return false;
        }
    }
}
