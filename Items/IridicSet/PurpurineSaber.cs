using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MythosOfMoonlight.Projectiles.IridicProjectiles;
using MythosOfMoonlight.Items.Materials;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Items.IridicSet
{
    public class PurpurineSaber : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iridic Saber");
            Tooltip.SetDefault("Every full swing creates a short energy slice forward.\n" +
                "Is that a lightsaber?\n" +
                "Nope, Even better!");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Melee;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 1f;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.channel = true;
            Item.value = Item.buyPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<PurpurineSaberSlice>();
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<PurpurineSaberSlice>()] < 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PurpurineQuartz>(), 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
