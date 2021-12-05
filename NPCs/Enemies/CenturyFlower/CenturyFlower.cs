using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Enemies.CenturyFlower
{
	public class CenturyFlower : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Century Flower");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 30;
			npc.height = 70;
			npc.damage = 12;
			npc.lifeMax = 150;
			npc.defense = 5;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath32;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.aiStyle = 3;
			npc.modNPC.aiType = NPCID.GoblinScout;
			npc.netAlways = true;
		}

		void SetState(int newState)
        {
			npc.ai[0] = newState;
			npc.frameCounter = 0;
        }
		const float strideSpeed = 1f, jumpHeight = 7f;
		public override bool PreAI()
		{
			npc.frameCounter++;
			if (Main.rand.NextFloat() <= .05f && npc.frameCounter > 150 && npc.ai[0] == 0)
            {
				SetState(1);
            }

			switch (npc.ai[0])
            {
				case 1:
					npc.velocity.X = 0;
					if (npc.frameCounter <= 75)
                    {
						
                    } else if (npc.frameCounter % 10 == 0) {
						Projectile.NewProjectileDirect(npc.Center - new Vector2(-20, 21), Main.rand.NextVector2Unit() * 2, ModContent.ProjectileType<CenturyFlowerSpore.CenturyFlowerSpore>(), 0, 0);
						if (npc.frameCounter >= 150)
                        {
							SetState(0);
                        }
					}
                    break;
				default:
					ManageMovement();
					break;
			}
			return false;
		}

		void ManageMovement()
        {
			npc.TargetClosest(false);
			var player = Main.player[npc.target];
			// var sqrDistance = player.DistanceSQ(npc.position);
			if (npc.velocity == Vector2.Zero)
			{
				npc.velocity.Y = -jumpHeight;
			}

			FitVelocityXToTarget(strideSpeed * npc.direction);
			var horizontalDistance = Math.Abs(npc.Center.X - player.Center.X);
			if (horizontalDistance >= 35.78f)
			{
				npc.FaceTarget();
			}
			npc.spriteDirection = npc.direction;
		}

		public void OpenPetals()
        {

        }

		void FitVelocityToTarget(Vector2 newVelocity) => npc.velocity = Vector2.Lerp(npc.velocity, newVelocity, .1f);
		void FitVelocityXToTarget(float newX) => npc.velocity.X = MathHelper.Lerp(npc.velocity.X, newX, 0.1f);
		void FitVelocityYToTarget(float newY) => npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, newY, 0.1f);
	}
}