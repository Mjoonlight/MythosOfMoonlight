using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MythosOfMoonLight.Projectiles.IridicProjectiles;
using MythosOfMoonlight.Items.Materials;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Projectiles.IridicProjectiles;
using Terraria.Audio;

namespace MythosOfMoonlight.Items.IridicSet
{
    public class CDGIris : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CDG-Iris");
            Tooltip.SetDefault("Cosmos Derived Gun? Cosmos Derived Fun.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.DamageType = DamageClass.Ranged;
            Item.noUseGraphic = false;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.width = 38;
            Item.height = 16;
            Item.useTime = 5;
            SoundStyle style = SoundID.Item31;
            style.Volume = .5f;
            Item.UseSound = style;
            Item.useAnimation = 15;
            Item.reuseDelay = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<FragmentBullet>();
            Item.shootSpeed = 16f;
        }
        public override bool RangedPrefix()
        {
            return true;
        }
        public override bool AllowPrefix(int pre)
        {
            if (pre == PrefixID.Unreal) return true;
            return base.AllowPrefix(pre);
        }
        public override void UseAnimation(Player player)
        {
            Item.useAnimation = Item.useTime * 3;
            Main.NewText(Item.useAnimation);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.Bullet) type = ModContent.ProjectileType<FragmentBullet>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Type)
                .AddIngredient(ModContent.ItemType<PurpurineQuartz>(), 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
