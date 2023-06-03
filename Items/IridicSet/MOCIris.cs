using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MythosOfMoonlight.Projectiles.IridicProjectiles;
using MythosOfMoonlight.Items.Materials;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Projectiles.IridicProjectiles;

namespace MythosOfMoonlight.Items.IridicSet
{
    public class MOCIris : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MOC-Iris");
            Tooltip.SetDefault("Mars Originated Cannon, still slightly radioactive.\n" +
                " Fires homing Embers after charging up. ");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 2;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 3f;
            Item.width = 60;
            Item.height = 20;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.channel = true;
            Item.value = Item.buyPrice(0, 0, 0, 1);
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<MOCIrisProj>();
        }
        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<MOCIrisProj>()] < 1 && player.statMana > 5;
        }
        public override bool MagicPrefix()
        {
            return true;
        }
    }
}
