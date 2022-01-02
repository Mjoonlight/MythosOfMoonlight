using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Dusts
{
	public class StarineDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = false;
			dust.noLight = true;
			// dust.scale *= 2f;
		}
		public override bool Update(Dust dust)
		{
			dust.velocity.Y += 0.1f;
			dust.rotation += 1f;
			dust.scale -= 0.05f;
			dust.position += dust.velocity;
			if (dust.scale <= 0)
				dust.active = false;

			Lighting.AddLight(dust.position, 0.5f * dust.scale, 0.7f * dust.scale, 1f * dust.scale);
			return false;
		}
	}
}