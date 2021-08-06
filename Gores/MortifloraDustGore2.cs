using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Gores
{
	public class MortifloraDustGore2 : ModGore
	{
		public override void OnSpawn(Gore gore) {
			gore.numFrames = 1;
			gore.behindTiles = true;
			gore.timeLeft = Gore.goreTime * 3;
		}
		public override bool Update(Gore gore)
		{
			gore.position += gore.velocity;
			gore.velocity.Y = gore.velocity.Y * 0.98f;
			gore.velocity.X = gore.velocity.X * 0.98f;
			gore.scale -= 0.007f;
			if (gore.scale < 0.1) {
				gore.scale = 0.1f;
				gore.alpha = 255;
			}
			return false;
		}
	}
}