using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Items.Armor;
using MythosOfMoonlight.Items.Food;
using MythosOfMoonlight.Items.Weapons.Melee;
using MythosOfMoonlight.Items.Weapons.Ranged;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace MythosOfMoonlight.NPCs.Field
{
    public class SunflowerLady : FieldNPC
    {
        public override void Defaults()
        {
            NPC.Size = new Vector2(44, 76);
            NPC.dontTakeDamage = true;
            NPC.lifeMax = 20;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Helper.GetTex(Texture + "_Glow");
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, new Vector2(44, 78) / 2, NPC.scale, NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return true;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Melissa");
            Main.npcFrameCount[Type] = 9;
            //NPCID.Sets.NoTownNPCHappiness[Type] = true; // for 1.4.4
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>
            {
                "Melissa"
            };
        }
        public override ITownNPCProfile TownNPCProfile()
        {
            return new Melissa();
        }
        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<FieldSnack>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Hoe>());
            if (NPC.downedBoss1)
                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Borer>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SunHat>());
            shop.item[nextSlot++].SetDefaults(ItemID.Blinkroot);
            shop.item[nextSlot++].SetDefaults(ItemID.Daybloom);
            shop.item[nextSlot++].SetDefaults(ItemID.Fireblossom);
            shop.item[nextSlot++].SetDefaults(ItemID.Moonglow);
            shop.item[nextSlot++].SetDefaults(ItemID.Shiverthorn);
            shop.item[nextSlot++].SetDefaults(ItemID.Waterleaf);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (Main.npcChatText != "")
            {
                if (NPC.frameCounter > 5)
                    NPC.frame.Y = 2 * frameHeight;
            }
            else if (NPC.velocity.Length() < 0.1f)
            {
                NPC.frame.Y = 0;
            }
            else if (NPC.velocity.Length() > .1f)
            {
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < 8 * frameHeight)
                        NPC.frame.Y += frameHeight;
                    else
                        NPC.frame.Y = 3 * frameHeight;
                }
                if (NPC.frame.Y < 3 * frameHeight)
                    NPC.frame.Y = 3 * frameHeight;
            }
        }
        public override string GetChat()
        {
            if (NPC.frameCounter > 6 && NPC.frame.Y != 2 * 78)
            {
                NPC.frame.Y = 78;
                NPC.frameCounter = 0;
            }
            shouldMusic = true;
            WeightedRandom<string> chat = new WeightedRandom<string>();
            chat.Add("Well, aren't ya short? Welcome to my humble little abode, I got potion-brewing herbs if ya like.", 2);
            chat.Add("Ya know, as fashionable as hats are, they are an active detriment for us plants. Do ya want one?");
            chat.Add("I always try avoid the " + (WorldGen.crimson ? "crimson" : "corruption") + ", those creatures are sticky and gross. There are fine potion ingredient that grow there, though.");
            chat.Add("Have ya been to the desert before? Ya probably agree it is unbearably dry, then. It's incredible some plants still thrive there, despite all odds!");
            chat.Add("Ya sure like chatting, do ya? I do appreciate it well enough.");
            return chat;
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }
        public override void _AI()
        {
            Lighting.AddLight(NPC.Center, TorchID.Yellow);
            if (Main.npcChatText == "")
                NPC.direction = NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
            else
                NPC.direction = NPC.spriteDirection = NPC.Center.X > Main.LocalPlayer.Center.X ? -1 : 1;
            if (shouldMusic)
                FieldSpawnRateNPC.activeNPC = 0;
            NPC.homeless = true;
            if (Main.LocalPlayer.Distance(NPC.Center) > 3000)
                NPC.active = false;

            //NPC.GetGlobalNPC<FighterGlobalAI>().AimlessWander(NPC, 5, 2);
        }
    }
    public class Melissa : ITownNPCProfile
    {
        public int RollVariation() => 0;
        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();
        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
        {
            return TextureAssets.Npc[npc.type];
        }
        public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("MythosOfMoonlight/Textures/Extra/blank");
    }
}
