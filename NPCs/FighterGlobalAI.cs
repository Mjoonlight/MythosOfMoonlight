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
	public override bool InstancePerEntity => true;
	const float StrideLimit = 70.78f;
    public void FighterAI(NPC npc, float jumpHeight, float strideSpeed, bool canJump)
    {
		// var sqrDistance = player.DistanceSQ(npc.position);
		if (npc.velocity.Y < 0)
		{
			npc.spriteDirection = npc.direction;
			npc.velocity.X = npc.direction;
		}
		if (!npc.collideY)
		{
			return;
		}
		var player = Main.player[npc.target];
		Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, false, 0);
		if (npc.collideX && canJump)
		{
			npc.velocity -= new Vector2(0, jumpHeight);
		}
		npc.velocity.X = MathHelper.Lerp(npc.velocity.X, strideSpeed * npc.direction, 0.35f);
		var horizontalDistance = Math.Abs(npc.Center.X - player.Center.X);
		if (horizontalDistance >= StrideLimit)
		{
			npc.FaceTarget();
		}
		npc.spriteDirection = npc.direction;
	}
}