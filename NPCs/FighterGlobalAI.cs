using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight;
using MythosOfMoonlight.Dusts;

public class FighterGlobalAI : GlobalNPC
{
    public void FighterAI(NPC npc, float jumpHeight, float strideSpeed, bool firstFrame)
    {
		npc.TargetClosest(false);
		Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, false, 0);
		var player = Main.player[npc.target];
		// var sqrDistance = player.DistanceSQ(npc.position);

		if (npc.velocity.X == 0 && npc.velocity.Y == 0 && !firstFrame)
		{
			npc.velocity.Y = -jumpHeight;
		}
		npc.spriteDirection = npc.direction;

		npc.velocity.X = MathHelper.Lerp(npc.velocity.X, strideSpeed * npc.direction, 0.1f);
		var horizontalDistance = Math.Abs(npc.Center.X - player.Center.X);
		if (horizontalDistance >= 35.78f)
		{
			npc.FaceTarget();
		}
	}
}