using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MythosOfMoonlight.Projectiles.VFXProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Items.Weapons
{
    public class MilkywayReaper : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 15;
            Item.height = 15;
            Item.useTime = 2;
            Item.useAnimation = 16;
            Item.reuseDelay = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 7f;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<AnimeSlash>();
        }
        int a = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundStyle sound = SoundID.Item1;
            sound.MaxInstances = 0;
            a++;
            int max2 = Main.rand.Next(2, 5);
            for (int i = 0; i < max2; i++)
            {
                Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(40, 40), velocity.RotatedByRandom(MathHelper.Pi * 2) * Main.rand.NextFloat(1.2f, 2f), type, damage, knockback, player.whoAmI, 0, Main.rand.NextFloat(-0.155f, 0.155f), Main.rand.NextFloat(0.1f, 0.25f));
            }

            SoundEngine.PlaySound(sound, position);

            int max = Main.rand.Next(6, 11);
            if (a % 2 == 0)
            {
                for (int i = 0; i < max; i++)
                {
                    float angle = Helper.CircleDividedEqually(i, max);
                    Projectile.NewProjectile(source, position + new Vector2(Main.rand.NextFloat(60, 100), 0).RotatedBy(angle + MathHelper.PiOver4 * 0.3f), new Vector2(Main.rand.NextFloat(15, 20), 0).RotatedBy(angle), type, damage, knockback, player.whoAmI, 0, -Main.rand.NextFloat(0.1f, 0.15f), Main.rand.NextFloat(0.1f, 0.25f));
                }
            }
            return false;
        }
        public override bool IsLoadingEnabled(Mod mod) => false;
    }
}
