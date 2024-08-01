using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Weapons.Ranged
{
    public class Shrimpy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pistol Shrimp");
            /* Tooltip.SetDefault("'Not as loud as an actual pistol shrimp'"
            + "\nConverts musket balls into water bullets"); */
            //            + $"\nUses Bullets [i:{ItemID.MusketBall}]"

            Item.ResearchUnlockCount = 1;

        }

        public override void SetDefaults()
        {
            Item.damage = 3;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.32f;
            Item.value = Item.sellPrice(silver: 35);
            Item.UseSound = SoundID.Item54;

            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.noMelee = true;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileID.Bullet;

        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<ShrimpleBullet>();
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 10f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            if (Main.rand.NextBool(5))
            {
                //Helper.SpawnGore(position, "MythosOfMoonlight/TommyGunPellets");
                //Gore.NewGore(source, player.Center + muzzleOffset * 1, new Vector2(player.direction * -1, -0.5f) * 2, , 1f);

            }

            for (int i = 0; i < 14; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(0.3f, 0.3f);
                var d = Dust.NewDustPerfect(Main.LocalPlayer.Center, DustID.Water, speed * 5, Scale: 1f);
                d.noGravity = true;
            }

            Projectile a = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);

            return false;
        }
        //#TO DO: Water Bullets
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.20f;
        }

        public override Vector2? HoldoutOffset()
        {
            var offset = new Vector2(-5, 0);
            return offset;
        }
    }
    class ShrimpleBullet : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WaterBolt);
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 100;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Math.Abs(Projectile.velocity.X - Projectile.oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -Projectile.oldVelocity.X;
            }

            if (Math.Abs(Projectile.velocity.Y - Projectile.oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -Projectile.oldVelocity.Y;
            }
            return false;
        }
        public override void AI()
        {
            Projectile.direction = Projectile.velocity.X < 0 ? -1 : 1;
            Dust.NewDustPerfect(Projectile.Center, DustID.DungeonWater, Projectile.velocity, Scale: 1).noGravity = true;
        }
    }
}
