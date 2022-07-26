using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using MythosOfMoonlight.NPCs.Critters.PurpleComet;
using MythosOfMoonlight.NPCs.Enemies.Starine;
using MythosOfMoonlight.NPCs.Enemies.StrandedMartian;
using Terraria.Chat;

namespace MythosOfMoonlight //Every comment is a guess lmao
{
    public class PurpleCometEvent : ModSystem
    {
        private static bool dayTimeLast;
        public static bool testedEvents;

        public static bool PurpleComet = false;
        public static bool downedPurpleComet = false;
        public static PurpleCometEvent Instance { get; set; }
        public PurpleCometEvent()
        {
            Instance = this;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            var downed = new List<string>();

            if (downedPurpleComet)
                downed.Add("downedPurpleComet");
            if (PurpleComet)
                downed.Add("PurpleComet");

            tag["downed"] = downed;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            var downed = tag.GetList<string>("lists");
            downedPurpleComet = downed.Contains("downedPurpleComet");
            PurpleComet = downed.Contains("PurpleComet");
        }
        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = downedPurpleComet;
            flags[1] = PurpleComet;
            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedPurpleComet = flags[0];
            PurpleComet = flags[1];
        }

        public override void OnWorldLoad() // This and OnWorldUnload replaces Initilize
        {
            PurpleComet = false;
            downedPurpleComet = false;
        }
        public override void OnWorldUnload()
        {
            PurpleComet = false;
            downedPurpleComet = false;
        }

        public static int[] PurpleCometCritters => new[]
         {
            NPCType<SparkleSkittler>(),
            NPCType<CometPeep>(),
            NPCID.EnchantedNightcrawler,
            NPCID.Firefly
        };
        public static int[] StarineEntities => new[]
        {
            NPCType<Starine_Sightseer>(),
            NPCType<Starine_Skipper>(),
            NPCType<Starine_Scatterer>()
        };
        public static int[] RarePurpleCometEnemies => new[]
        {
            NPCType<StrandedMartian>()
        };
        public override void PreUpdateWorld()
        {
            if (!PurpleComet && !testedEvents && !Main.fastForwardTime && !Main.bloodMoon && !Main.dayTime && WorldGen.spawnHardBoss == 0)
            {
                if ((Main.rand.NextBool(8) && !downedPurpleComet) || (Main.rand.NextBool(16) && downedPurpleComet))
                {
                    string status = "You feel like you're levitating...";
                    if (Main.netMode == NetmodeID.Server)
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(status), new Color(179, 0, 255));
                    else if (Main.netMode == NetmodeID.SinglePlayer)
                        Main.NewText(Language.GetTextValue(status), new Color(179, 0, 255));

                    PurpleComet = true;
                    downedPurpleComet = true;

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.WorldData);
                }
                testedEvents = true;
            }
            else if (PurpleComet && Main.dayTime)
            {
                string status = "The purple shine fades as the sun rises.";
                if (Main.netMode == NetmodeID.Server)
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(status), new Color(179, 0, 255));
                else if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(Language.GetTextValue(status), new Color(179, 0, 255));

                PurpleComet = false;
                testedEvents = false;

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void PostUpdateWorld()
        {
            if (PurpleComet && !downedPurpleComet)
                downedPurpleComet = true;
        }
    }
}