using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using static Terraria.ModLoader.Core.TmodFile;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace MythosOfMoonlight //Every comment is a guess lmao
{
	public class PurpleCometEvent : ModWorld
	{
		private static bool dayTimeLast;
		public static bool testedEvents;

		public static bool PurpleComet = false;
		public static bool downedPurpleComet = false;
		/*
			public override TagCompound Save()
			{
				var data = new TagCompound();
				var downed = new List<string>();
				if (downedPurpleComet)
				downed.Add("purplecomet"); //ensures that clearing the event will mark it as downed, in other words checked in boss checklist if that mod is installed (imagine not having that installed).
				data.Add("downed", downed);
				data.Add("purpleComet", PurpleComet);
				return data;
			}
			public override void Load(TagCompound tag)
			{
				var downed = tag.GetList<string>("downed");
				downedPurpleComet = downed.Contains("purplecomet"); //"downed.Add("purplecomet");" wouldn't work if this is nonexistent.
				PurpleComet = tag.GetBool("purpleComet");
			} 
		*/
		public override void NetSend(BinaryWriter writer)
		{
			BitsByte environment = new BitsByte(PurpleComet, downedPurpleComet);
		}
		public override void NetReceive(BinaryReader reader)
		{
			BitsByte environment = reader.ReadByte();
			PurpleComet = environment[0];
			downedPurpleComet = environment[1];
		}
		public override void Initialize()
		{
			PurpleComet = false;
		}
        public override void PreUpdate()
		{
			if (!PurpleComet && !testedEvents && !Main.fastForwardTime && !Main.bloodMoon && !Main.dayTime && WorldGen.spawnHardBoss == 0)
			{
				if ((Main.rand.NextBool(1) && !downedPurpleComet) || (Main.rand.NextBool(1) && downedPurpleComet))
				{
					Main.NewText("You feel like you're levitating...", 179, 0, 255); //event message in chat.
					PurpleComet = true;
					downedPurpleComet = true;
				}
				testedEvents = true;
			}
			else if (PurpleComet && Main.dayTime)
			{
				Main.NewText("The purple shine fades as the sun rises.", 179, 0, 255); //event message in chat.
				PurpleComet = false;
				testedEvents = false;
			}
		}

		public void PostUpdate()
		{
			if (PurpleComet && !downedPurpleComet)
			{
				downedPurpleComet = true;
			}
		}
	}
}