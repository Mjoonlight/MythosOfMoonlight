using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Gores.Enemies
{
	public class CenturyLeaf : ModGore
	{
		public override void OnSpawn(Gore gore)
		{
			gore.numFrames = 4;
			gore.behindTiles = true;
			gore.timeLeft = Gore.goreTime * 3;
		}

		public override bool Update(Gore gore)
		{
			gore.position += gore.velocity * (Main.windSpeed * 1.2f) * 2f;
			gore.velocity.Y += 0.04f;
			gore.rotation = gore.velocity.ToRotation();
			
			gore.frameCounter++;
			if (gore.frameCounter > 3)
			{
				gore.frameCounter = 0;
				gore.frame++;
				if (gore.frame > 3) gore.frame = 0;
			}

			if (Collision.SolidCollision(gore.position, 2, 2))
			{
				gore.active = false;
				return false;
			}
			return false;
		}
	}
}
