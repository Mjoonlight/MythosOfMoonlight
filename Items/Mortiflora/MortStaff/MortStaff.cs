using MythosOfMoonlight.Projectiles.ThornDart;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace MythosOfMoonlight.Items.Mortiflora.MortStaff
{
    public class MortStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Leech Life");
            // Tooltip.SetDefault("Shoots a dart that splits into 4 healing orbs on hit.");
            Item.staff[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 11;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Green;
            Item.knockBack = 5;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;

            Item.shoot = ProjectileType<ThornDart>();
            Item.shootSpeed = 16f;
        }
        public override bool CanUseItem(Player player)
        {
            return player.statMana >= 15;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // (int)((manaCost + 100) / 100)
            int manaCost = (int)MathHelper.Max(30, player.statMana / 2);
            player.statMana -= manaCost;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, MathHelper.Clamp((manaCost + 50) / 33, 0, 3));
            return false;
        }
    }
}