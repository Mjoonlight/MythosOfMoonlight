using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythosOfMoonlight.Gores.Enemies
{
	public class Starine : ModGore
	{
		public override void OnSpawn(Gore gore)
		{
			gore.behindTiles = true;
			gore.timeLeft = Gore.goreTime * 3;
		}
		public override bool Update(Gore gore)
		{
			gore.GetAlpha(Color.White);
			gore.rotation += 0.5f;
			gore.position += gore.velocity;
			gore.velocity.Y += 0.04f;
			gore.scale -= 0.007f;
			if (gore.scale < 0.1)
			{
				gore.scale = 0.1f;
				gore.alpha = 255;
			}
			return false;
		}
		 public override Color? GetAlpha(Gore gore, Color lightColor) => new Color(255, 255, 255, 255);
	}
}