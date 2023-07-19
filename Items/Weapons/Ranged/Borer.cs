using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Weapons.Ranged
{
    public class Borer : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("4-Bore");
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 20;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.shoot = ModContent.ProjectileType<BorerP>();
            Item.shootSpeed = 25f;
            Item.rare = ItemRarityID.Green;
            Item.knockBack = 5f;
            Item.useStyle = 5;
            Item.UseSound = SoundID.Item38;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 8, 0, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Helper.SpawnGore(position + velocity, "MythosOfMoonlight/BorerG", vel: -Vector2.UnitY);
            Helper.SpawnDust(position + velocity * 2, Vector2.One, DustID.Torch, velocity, 10);
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-19, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += new Vector2(-44, -6).RotatedBy(velocity.ToRotation()) * player.direction;
            type = ModContent.ProjectileType<BorerP>();
        }
    }
    public class BorerP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("4-Bore");
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 500;
            Projectile.Size = new(12, 12);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center - new Vector2(6, 0).RotatedBy(Projectile.rotation - MathHelper.PiOver2), DustID.Torch, Projectile.velocity).noGravity = true;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}
