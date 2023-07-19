using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace MythosOfMoonlight.Items.Mortiflora.MortMask
{
    [AutoloadEquip(EquipType.Head)]
    public class MortMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mortiflora Mask");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 32;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
        }
    }
}