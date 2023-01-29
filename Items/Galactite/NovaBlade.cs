using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Projectiles;
using Terraria.DataStructures;
using Terraria.ID;
using MythosOfMoonlight.Dusts;
using static Humanizer.In;
using static tModPorter.ProgressUpdate;

namespace MythosOfMoonlight.Items.Galactite
{
    public class NovaBlade : ModItem
    {
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 76;
            Item.crit = 45;
            Item.damage = 34;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.channel = false;
            //Item.reuseDelay = 45;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<NovaBladeP>();
        }
        int dir = 1;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch (dir)
            {
                case 0: dir = 1; break;
                case 1: dir = -1; break;
                case -1: dir = 0; break;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, dir);
            return false;
        }
    }
    public class NovaBladeP : HeldSword
    {
        public override string Texture => "MythosOfMoonlight/Items/Galactite/NovaBlade";
        public override string GlowTexture => "MythosOfMoonlight/Items/Galactite/NovaBlade_Glow";
        public override void SetExtraDefaults()
        {
            swingTime = 30;
            Projectile.Size = new(46, 50);
            glowAlpha = 1f;
            glowBlend = BlendState.AlphaBlend;
        }
        public override void ExtraAI()
        {
            if (Projectile.ai[1] == 0 && Projectile.timeLeft < 20)
            {
                Vector2 vel = Projectile.velocity;
                vel.Normalize();
                Projectile a = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center, vel * 10, ModContent.ProjectileType<NovaBladeP2>(), Projectile.damage, 0, Projectile.owner);
                a.timeLeft = 20;
                a.ai[0] = Main.rand.NextFloat(-10f, 10f) * 0.07f;
                a.ai[1] = Main.rand.NextFloat(-10f, 10f) * 0.07f;
            }
        }
    }
    public class NovaBladeP2 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(496);
            Projectile.aiStyle = -1;
        }
        int time;
        public override void OnSpawn(IEntitySource source)
        {
            time = Projectile.timeLeft;
        }
        public override void AI()
        {
            Vector2 center15 = Projectile.Center;
            Projectile.scale = 1f - Projectile.localAI[0];
            Projectile.width = (int)(20f * Projectile.scale);
            Projectile.height = Projectile.width;
            Projectile.position.X = center15.X - (float)(Projectile.width / 2);
            Projectile.position.Y = center15.Y - (float)(Projectile.height / 2);
            if ((double)Projectile.localAI[0] < 0.1)
            {
                Projectile.localAI[0] += 0.01f;
            }
            else
            {
                Projectile.localAI[0] += 0.025f;
            }
            if (Projectile.localAI[0] >= 0.95f)
            {
                Projectile.Kill();
            }
            Projectile.velocity.X += Projectile.ai[0] * 1.5f;
            Projectile.velocity.Y += Projectile.ai[1] * 1.5f;
            if (Projectile.velocity.Length() > 16f)
            {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 16f;
            }
            Projectile.ai[0] *= 1.05f;
            Projectile.ai[1] *= 1.05f;
            if (Projectile.scale < 1f)
            {
                for (float num614 = 0f; num614 < 1f; num614 += 0.25f)
                {
                    Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<FireDust>(), Vector2.UnitY.RotatedBy(Projectile.velocity.ToRotation()), 150, Color.DarkViolet, Main.rand.NextFloat(1, 1.75f) * 0.05f).noGravity = true;
                }
            }
        }
    }
    public class NovaBladeP3 : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        int time;
        public override void OnSpawn(IEntitySource source)
        {
            time = Projectile.timeLeft;
        }
        public override void AI()
        {
            float progresss = Utils.GetLerpValue(0, time, Projectile.timeLeft);
            float progress = MathHelper.Clamp((float)(Math.Sin(progresss * MathHelper.Pi)) * 3, 0, 3);
            for (float num614 = 0f; num614 < 1f; num614 += 0.5f)
            {
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<FireDust>(), Vector2.UnitY.RotatedBy(Projectile.velocity.ToRotation()) * progress, 150, Color.DarkViolet, Main.rand.NextFloat(1, 1.75f) * 0.075f).noGravity = true;
            }
        }
    }
}
