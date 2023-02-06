using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;

namespace MythosOfMoonlight.Projectiles.IridicProjectiles
{
    public class MOCIrisProj : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public float ExistingTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public float DustTimer = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MOC-Iris");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 30;
            Projectile.netUpdate = true;
            Projectile.netUpdate2 = true;
            Projectile.netImportant = true;
            Projectile.ownerHitCheck = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            ExistingTime = 0;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (player.active && player.channel && !player.dead && !player.CCed && !player.noItems)
            {
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0], Projectile.ai[1]);
            }
        }
        public override void AI()
        {
            ExistingTime++;
            DustTimer++;
            Lighting.AddLight(Projectile.Center, .5f, .5f, .5f);
            foreach (Player player in Main.player)
            {
                if (player == Main.player[Projectile.owner])
                {
                    if (player == Main.LocalPlayer)
                    {
                        Projectile.timeLeft++;
                        player.direction = Main.MouseWorld.X >= player.Center.X ? 1 : -1;
                        player.itemTime = 2;
                        player.itemAnimation = 2;
                        player.heldProj = Projectile.whoAmI;
                        Projectile.Center = player.Center + Utils.SafeNormalize(Main.MouseWorld - player.Center, Vector2.UnitX);
                        Projectile.rotation = (Main.MouseWorld - player.Center).ToRotation();
                        if (ExistingTime > 100) Projectile.Kill();
                        if (!player.channel || player.statMana <= 0) Projectile.Kill();
                    }
                }
            }
            for (int i = 0; i <= 20; i += 5)
            {
                if (ExistingTime == 90 + i)
                {
                    Vector2 shoot = Projectile.rotation.ToRotationVector2().RotatedBy(Main.rand.NextFloat(-.1f, .1f)) * 18f;
                    for (int j = 0; i < 15; i++)
                    {
                        Dust dust;
                        Vector2 position = Projectile.Center;
                        dust = Terraria.Dust.NewDustDirect(position, 0, 0, 71, shoot.X, shoot.Y, 0, new Color(255, 255, 255), 1f);
                    }
                    SoundEngine.PlaySound(SoundID.Item68, Projectile.Center + shoot * 2f);
                    Projectile star = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + shoot * 2f, shoot, ModContent.ProjectileType<IrisStar>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    star.DamageType = DamageClass.Magic;
                }
            }
            if (Main.netMode != NetmodeID.Server)
            {
                if (ExistingTime < 90)
                {
                    int DustCooldown = Math.Max((int)((190 - ExistingTime) / 10f), 2);
                    if (DustTimer >= DustCooldown)
                    {

                        DustTimer = 0;
                        for (int i = 1; i <= 3; i++)
                        {
                            Vector2 Center = Projectile.Center + Projectile.rotation.ToRotationVector2() * 36f;
                            Vector2 randPos = Main.rand.NextVector2CircularEdge(24, 24);
                            Dust dust = Dust.NewDustDirect(Center + randPos, 1, 1, ModContent.DustType<PurpurineDust>());
                            dust.noGravity = true;
                            dust.velocity = -randPos / 20f;
                        }
                    }
                }
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/IridicSet/MOCIris").Value;
            Vector2 ori = new(0, 8);
            float rot = Projectile.rotation + MathHelper.Pi;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            Color color = Color.White;
            //Math.Abs(rot) <= MathHelper.PiOver2
            Main.EntitySpriteDraw(tex, pos, null, color, rot, ori, -1, Main.player[Projectile.owner].direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            return true;
        }
    }
}
