using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class EntropicBody : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.Size = new Vector2(36, 18);
            Item.rare = ItemRarityID.Green;
            Item.defense = 3;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 1, 50, 0);
        }
    }
    public class EntropicPlayerLayer : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, "EntropicBody", EquipType.Body);
        }

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Torso);
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            Texture2D texture = ModContent.Request<Texture2D>("MythosOfMoonlight/Items/Armor/EntropicBody_Cloth").Value;

            Vector2 legsPos = drawInfo.legsOffset + new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.legFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.legFrame.Height + 4f) - 20) + drawInfo.legVect;


            Vector2 origin = drawInfo.headVect;

            Rectangle headFrame = drawPlayer.bodyFrame;

            DrawData data = new(texture, legsPos, headFrame, drawInfo.colorArmorBody, drawInfo.drawPlayer.headRotation, origin, 1f, drawInfo.playerEffect, 0)
            {
                shader = drawInfo.cBody
            };

            drawInfo.DrawDataCache.Add(data);
        }
    }
}
