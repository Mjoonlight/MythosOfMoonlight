using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Dusts
{
	public class PurpurineStoneDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = false;
			dust.noLight = false;
			// dust.scale *= 2f;
		}
		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.15f;
			}

			dust.rotation += 0.1f;
			dust.scale -= 0.05f;
			dust.position += dust.velocity;
			if (dust.scale <= 0)
				dust.active = false;

			return false;
		}
	}
}