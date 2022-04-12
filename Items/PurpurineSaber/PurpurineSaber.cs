using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MythosOfMoonLight.Projectiles.PurpurineSaberSlice;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Items.Materials;

namespace MythosOfMoonLight.Items.PurpurineSaber
{
    public class PurpurineSaber : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purpurine Saber");
            Tooltip.SetDefault("Every full swing creates a short energy slice forward.\n"+
                "Is that a lightsaber?\n"+
                "Nope, Even better!");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.damage = 9;
            item.melee = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.knockBack = 1f;
            item.width = 48;
            item.height = 48;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = false;
            item.channel = true;
            item.value = Item.buyPrice(0, 0, 0, 1);
            item.rare = ItemRarityID.Green;
            item.shoot = ModContent.ProjectileType<PurpurineSaberSlice>();
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<PurpurineSaberSlice>()] < 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<PurpurineQuartz>(), 25);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
