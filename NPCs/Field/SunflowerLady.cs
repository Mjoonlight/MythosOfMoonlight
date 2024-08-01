using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Common.Crossmod;
using MythosOfMoonlight.Common.Systems;
using MythosOfMoonlight.Items.Armor;
using MythosOfMoonlight.Items.Food;
using MythosOfMoonlight.Items.Misc;
using MythosOfMoonlight.Items.Weapons.Magic;
using MythosOfMoonlight.Items.Weapons.Melee;
using MythosOfMoonlight.Items.Weapons.Ranged;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
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
            NPC.immortal = true;
        }
        public override bool CanBeHitByNPC(NPC attacker) => false;
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Melissa is a sunflower Floirad that has a knack for collecting strange trinkets and herbs she finds on her travels to sell.")
            });
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //3hi31mg
            var clr = Color.White; // full white
            var drawPos = NPC.Center - screenPos;
            var texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;
            var origTexture = TextureAssets.Npc[NPC.type].Value;
            var frame = new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height);
            var orig = frame.Size() / 2f - new Vector2(0, 3);
            Main.spriteBatch.Draw(origTexture, drawPos, frame, drawColor, NPC.rotation, orig, NPC.scale, NPC.direction < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            Main.spriteBatch.Draw(texture, drawPos, frame, clr, NPC.rotation, orig, NPC.scale, NPC.direction < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melissa");
            Main.npcFrameCount[Type] = 9;
            NPCID.Sets.NoTownNPCHappiness[Type] = true;
            NPC.AddNPCElementList("Plantlike");
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.sunflower && !NPC.AnyNPCs(Type) ? 0.25f : 0;
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
        public override void AddShops()
        {
            Condition firstStep = new Condition(this.GetLocalization("MelissaQuest0"), () => GenericSystem.melissaQuest[0]);
            NPCShop npcShop = new NPCShop(Type)
                .Add<FieldSnack>()
                .Add<SunHat>()
                .Add<Hoe>(new Condition(this.GetLocalization("MelissaQuest1"), () => GenericSystem.melissaQuest[1]))
                .Add<Borer>(new Condition(this.GetLocalization("MelissaQuest2"), () => GenericSystem.melissaQuest[2] && Condition.DownedEyeOfCthulhu.IsMet()))
                .Add(ItemID.Blinkroot, firstStep)
                .Add(ItemID.Daybloom, firstStep)
                .Add(ItemID.Fireblossom, firstStep)
                .Add(ItemID.Moonglow, firstStep)
                .Add(ItemID.Shiverthorn, firstStep)
                .Add(ItemID.Waterleaf, firstStep);
            npcShop.Register();
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (Main.npcChatText != "" && questChat.Contains(Main.npcChatText))
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
        List<string> chat = new List<string>
            {
                "Well, aren't ya short? Welcome to my humble little abode, "+ (GenericSystem.melissaQuest[0] ? "I got potion-brewing herbs if ya like." : "I'd sell ya potion-brewing herbs if I could just get them to grow..."),
                "Ya know, as fashionable as hats are, they are an active detriment for us plants. Do ya want one?",
                "I always try avoid the " + (WorldGen.crimson ? "crimson" : "corruption") + ", those creatures are sticky and gross. There are fine potion ingredients that grow there, though.",
                "Have ya been to the desert before? Ya probably agree it is unbearably dry, then. It's incredible some plants still thrive there, despite all odds!",
                "Ya sure like chatting, do ya? I do appreciate it well enough."
            };

        List<string> questChat = new List<string>
        {
            "Ya know, I've been trying to grow some of those medicinal herbs, but I don't think the soil I use is quite fertile enough...\nHmm, say, would you happen to have something that could help fertilize my little horticulture?",
            "I need something that could fertilize soil... Any luck?",
            "My, that looks like a good amount of fertilizer...\nI'm sure my little herbs are gonna love this! Thank ya very much, short friend.",

            "Thanks for your help earlier, short friend. But... My horticulture's still losing herbs by the hour, ya know?\nSometimes when I pluck a herb, it just disintegrates in my hands!\nMaybe if I had a tool specifically to harvest herbs...",
            "Hmm... I've heard of such a tool before... A staff of some kind?",
            "Of course!\nThat's it, a Staff of Regrowth, right? It should do the trick!\nThank you, short friend, I don't think I even need this hoe anymore, have it!",

            "Hey, short friend, I have a gift for ya!\n...Well, I would've had one if I knew how to fix it. I know I've already asked a lot from ya before, but would ya mind helping me one last time? You'll love it, promise!\nI just need some... Gun parts, alrighty?",
            "I promise you'll love this, I just need some gun parts!",
            "Aaand... Done, here you go! I found this while I was digging around my little horticulture...\nCleaned it up, polished and now switched some broken parts, I hope ya enjoy it!",

            "Thanks for all your help, short one. I hope you enjoyed my gift. You've just been a wonderful boon to my little abode, short friend!"
        };
        public override string GetChat()
        {
            shouldMusic = true;
            WeightedRandom<string> _chat = new WeightedRandom<string>();
            for (int i = 0; i < chat.Count; i++)
                _chat.Add(chat[i]);

            if (!heardIntroDialogue)
            {
                heardIntroDialogue = true;
                return chat[0];
            }

            return _chat;
        }
        public bool[] heardQuestDialogue = new bool[3];//index 0 = Fertilizer 1 = SoR 2 = IGP
        public bool despawnReady = false;
        public bool heardIntroDialogue;
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = "";
            if (!GenericSystem.melissaQuest[0] || !GenericSystem.melissaQuest[1] || !GenericSystem.melissaQuest[2])
                button2 = "Need anything?";
            if (!GenericSystem.melissaQuest[0] && Main.LocalPlayer.HasItem(ItemID.Fertilizer) && heardQuestDialogue[0])
                button2 = "How's this?";
            if (GenericSystem.melissaQuest[0] && !GenericSystem.melissaQuest[1] && Main.LocalPlayer.HasItem(ItemID.StaffofRegrowth) && heardQuestDialogue[1])
                button2 = "How's this?";
            if (GenericSystem.melissaQuest[0] && GenericSystem.melissaQuest[1] && !GenericSystem.melissaQuest[2] && Main.LocalPlayer.HasItem(ItemID.IllegalGunParts) && heardQuestDialogue[2])
                button2 = "How's this?";
            if (GenericSystem.melissaQuest[0] && GenericSystem.melissaQuest[1] && GenericSystem.melissaQuest[2])
                button2 = "Need anything else?";

            if (despawnReady)
            {
                button = "";
                button2 = "";
            }
        }
        void Celebrate()
        {
            string a = "";
            SetChatButtons(ref a, ref a);
            SoundEngine.PlaySound(SoundID.ResearchComplete);
            for (int i = 0; i < 10; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(8, 8);
                Gore.NewGorePerfect(null, Main.rand.NextVector2FromRectangle(NPC.getRect()), Main.rand.NextVector2Unit(), Main.rand.Next(276, 283));
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Confetti, vel.X, vel.Y, 150, default(Color), Main.rand.NextFloat(0.5f, 2));
            }
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            Player player = Main.LocalPlayer;
            if (firstButton)
                shopName = "Shop";
            else
            {
                SoundEngine.PlaySound(SoundID.Chat);
                if (NPC.frameCounter > 6 && NPC.frame.Y != 2 * 78)
                {
                    NPC.frame.Y = 78;
                    NPC.frameCounter = 0;
                }
                if (!GenericSystem.melissaQuest[0])
                {
                    if (Main.LocalPlayer.HasItem(ItemID.Fertilizer) && heardQuestDialogue[0])
                    {
                        Main.npcChatCornerItem = 0;
                        Main.npcChatText = questChat[2];
                        Celebrate();
                        for (int j = 0; j < 54; j++)
                        {
                            if (player.inventory[j].type == ItemID.Fertilizer && player.inventory[j].stack > 0)
                            {
                                player.inventory[j].stack--;
                                despawnReady = true;
                                Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<ThePebble>());
                                GenericSystem.melissaQuest[0] = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Main.npcChatCornerItem = ItemID.Fertilizer;
                        if (!heardQuestDialogue[0])
                        {
                            Main.npcChatText = questChat[0];
                            heardQuestDialogue[0] = true;
                        }
                        else
                            Main.npcChatText = questChat[1];
                    }
                }
                else if (GenericSystem.melissaQuest[0] && !GenericSystem.melissaQuest[1])
                {
                    if (Main.LocalPlayer.HasItem(ItemID.StaffofRegrowth) && heardQuestDialogue[1])
                    {
                        Main.npcChatCornerItem = 0;
                        Main.npcChatText = questChat[5];
                        Celebrate();
                        for (int j = 0; j < 54; j++)
                        {
                            if (player.inventory[j].type == ItemID.Fertilizer && player.inventory[j].stack > 0)
                            {
                                player.inventory[j].stack--;
                                despawnReady = true;
                                Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<Hoe>());
                                GenericSystem.melissaQuest[1] = true;
                                break;
                            }
                        }

                    }
                    else
                    {
                        Main.npcChatCornerItem = ItemID.StaffofRegrowth;
                        if (!heardQuestDialogue[1])
                        {
                            Main.npcChatText = questChat[3];
                            heardQuestDialogue[1] = true;
                        }
                        else
                            Main.npcChatText = questChat[4];
                    }
                }
                else if (GenericSystem.melissaQuest[0] && GenericSystem.melissaQuest[1] && !GenericSystem.melissaQuest[2])
                {
                    if (Main.LocalPlayer.HasItem(ItemID.IllegalGunParts) && heardQuestDialogue[2])
                    {
                        Main.npcChatCornerItem = 0;
                        Main.npcChatText = questChat[8];
                        Celebrate();
                        for (int j = 0; j < 54; j++)
                        {
                            if (player.inventory[j].type == ItemID.Fertilizer && player.inventory[j].stack > 0)
                            {
                                player.inventory[j].stack--;
                                despawnReady = true;
                                Main.LocalPlayer.QuickSpawnItem(NPC.GetSource_FromThis(), ModContent.ItemType<Borer>());
                                GenericSystem.melissaQuest[2] = true;
                                break;
                            }
                        }

                    }
                    else
                    {
                        Main.npcChatCornerItem = ItemID.IllegalGunParts;
                        if (!heardQuestDialogue[2])
                        {
                            Main.npcChatText = questChat[6];
                            heardQuestDialogue[2] = true;
                        }
                        else
                            Main.npcChatText = questChat[7];
                    }
                }
                else if (GenericSystem.melissaQuest[0] && GenericSystem.melissaQuest[1] && GenericSystem.melissaQuest[2])
                {
                    Main.npcChatText = questChat[9];
                }
            }
        }
        public override void _AI()
        {
            Lighting.AddLight(NPC.Center, TorchID.Yellow);
            if (Main.npcChatText != "")
            {
                NPC.velocity.X = 0;
                NPC.direction = NPC.spriteDirection = NPC.Center.X > Main.LocalPlayer.Center.X ? -1 : 1;
            }
            else
            {
                NPC.direction = NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
                if (despawnReady)
                {

                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 vel = Main.rand.NextVector2Circular(8, 8);
                        Gore.NewGorePerfect(null, Main.rand.NextVector2FromRectangle(NPC.getRect()), Main.rand.NextVector2Unit(), GoreID.Smoke1 + Main.rand.Next(2));
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, vel.X, vel.Y, 150, default(Color), Main.rand.NextFloat(0.5f, 2));
                    }
                    Main.NewText("Melissa has departed.", 50, 255, 130);
                    NPC.active = false;
                }
            }
            if (shouldMusic)
                FieldSpawnRateNPC.activeNPC = 0;
            NPC.homeless = true;
            if (Main.LocalPlayer.Distance(NPC.Center) > 2000)
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
        public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("MythosOfMoonlight/Assets/Textures/Extra/blank");
    }
}
