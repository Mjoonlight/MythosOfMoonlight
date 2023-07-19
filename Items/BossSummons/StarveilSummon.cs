using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MythosOfMoonlight.Items.BossSummons
{
    public class StarveilSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Study of Astralmancy Vol I");
            // Tooltip.SetDefault("Not Consumable\nThe pages are written in a language you don't understand. The cape isn't for some reason.\nThis probably doesn't belong to you...\nThe ink becomes invisible away from the comet...");
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13;
        }


        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine a in tooltips)
            {
                if (a.Text == Language.GetTextValue("Mods.MythosOfMoonlight.Items.StarveilSummon.TooltipSpecial") || a.Text == Language.GetTextValue("Mods.MythosOfMoonlight.Items.StarveilSummon.TooltipSpecial2"))
                {
                    a.OverrideColor = Colors.RarityDarkPurple;
                    if (PurpleCometEvent.PurpleComet)
                    {
                        a.Text = Language.GetTextValue("Mods.MythosOfMoonlight.Items.StarveilSummon.TooltipSpecial");
                    }
                    else
                    {
                        a.Text = Language.GetTextValue("Mods.MythosOfMoonlight.Items.StarveilSummon.TooltipSpecial2");
                    }
                }
            }
        }
        public override bool CanUseItem(Player player)
        {
            return PurpleCometEvent.PurpleComet && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Minibosses.StarveiledScholar>());
        }

        public override bool? UseItem(Player player)
        {
            NPC.NewNPCDirect(player.GetSource_ItemUse(Item), player.Center - (Vector2.UnitY * 200), ModContent.NPCType<NPCs.Minibosses.StarveiledScholar>());
            return true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/BossSummons/StarveilSummon").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/BossSummons/StarveilSummon_Alt").Value;
            if (!PurpleCometEvent.PurpleComet)
                spriteBatch.Draw(tex, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(tex2, position, new Rectangle(0, 0, tex2.Width, tex2.Height), drawColor, 0, tex2.Size() / 2, scale, SpriteEffects.None, 0);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/BossSummons/StarveilSummon").Value;
            if (PurpleCometEvent.PurpleComet)
                tex = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/BossSummons/StarveilSummon_Alt").Value;
            spriteBatch.Draw(tex, Item.Center - Main.screenPosition, null, lightColor, 0, tex.Size() / 2, scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
