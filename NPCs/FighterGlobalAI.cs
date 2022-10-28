using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs
{
    public class FighterGlobalAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        const float StrideLimit = 70.78f;
        public bool Jump = false;
        public void FighterAI(NPC NPC, float jumpHeight, float strideSpeed, bool canJump, int jumpFrame = 1, int jumpOffset = 4, float turningVel = .06f)
        {
            // var sqrDistance = player.DistanceSQ(NPC.position);
            NPC.TargetClosest(false);

            if (strideSpeed * NPC.direction >= 0)
            {
                if (NPC.velocity.X < strideSpeed * NPC.direction) NPC.velocity.X += turningVel;
            }
            else
            {
                if (NPC.velocity.X > strideSpeed * NPC.direction) NPC.velocity.X -= turningVel;
            }
            var player = Main.player[NPC.target];
            if (NPC.collideX && canJump && !Jump)
            {
                NPC.velocity.Y = -jumpHeight;
                Jump = true;
            }
            if (NPC.collideY && NPC.velocity.Y >= 0) Jump = false;
            if (Jump)
            {
                if (jumpFrame >= 0) NPC.frame = new Rectangle(0, NPC.height * jumpFrame + jumpOffset, NPC.width, NPC.height + jumpOffset);
            }
            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, false, 0);

            var horizontalDistance = Math.Abs(NPC.Center.X - player.Center.X);
            if (horizontalDistance >= StrideLimit)
            {
                NPC.FaceTarget();
            }
            NPC.spriteDirection = NPC.direction;
        }
    }
}