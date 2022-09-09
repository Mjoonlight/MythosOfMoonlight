using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Items.PurpleComet
{
    public class PurpleCometSpawn : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purple Comet's Offering");
            Tooltip.SetDefault("Use at nighttime to call upon the Purple Comet.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 18;
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = Item.useAnimation = 10;
            Item.noMelee = true;
            Item.consumable = true;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item43;
        }

        public override bool CanUseItem(Player player)
        {
            if (Main.dayTime)
            {
                Main.NewText("With light already shining in the skies, there is no possible way for the Purple Comet to illuminate for all to witness.", 179, 0, 255);
                return false;
            }
            if (PurpleCometEvent.PurpleComet)
                return false;

            return true;
        }

        public override bool? UseItem(Player player)
        {
            Main.NewText("You start to feel like levitating...", 179, 0, 255);
            SoundEngine.PlaySound(SoundID.Roar, new Vector2((int)player.position.X, (int)player.position.Y));
            PurpleCometEvent.PurpleComet = true;

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
            return true;
        }
    }
}
