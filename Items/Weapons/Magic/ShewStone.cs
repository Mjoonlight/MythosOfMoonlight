using MythosOfMoonlight.Projectiles.SoulCandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Projectiles;

namespace MythosOfMoonlight.Items.Weapons.Magic
{
    public class ShewStone : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Magic;
            Item.width = 26;
            Item.height = 24;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.knockBack = 3;
            Item.value = 30000;
            Item.rare = 3;
            Item.mana = 60;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.shoot = ModContent.ProjectileType<ShewStoneCrystal>();
            Item.shootSpeed = 1f;
            Item.noMelee = true;
            Item.UseSound = SoundID.DD2_EtherianPortalSpawnEnemy;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Diamond, 15)
                .AddIngredient(ItemID.Geode)
                .AddRecipeGroup(RecipeGroup.RegisterGroup("GoldAndPlatinumBar", new RecipeGroup(() => $"{Lang.GetItemNameValue(ItemID.GoldBar)} / {Lang.GetItemNameValue(ItemID.PlatinumBar)}", ItemID.PlatinumBar, ItemID.GoldBar)), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
