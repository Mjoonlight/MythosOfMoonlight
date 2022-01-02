using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.Items.CenturySet;
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
			npc.HitSound = SoundID.NPCHit1;
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
        public override void FindFrame(int frameHeight)
        {
			npc.frame.Y = GetFrame() * frameHeight;
		}
        void SetFrame(int frame)
		{
			npc.frame.Y = npc.height * frame;
		}
		const float strideSpeed = 1f, jumpHeight = 7f;
		public override bool PreAI()
		{
			npc.frameCounter++;
			if (Main.rand.NextFloat() <= .05f && npc.frameCounter > 150 && npc.ai[0] == 0)
			{
				RealFrame = ScaleFrame(5);
				SetState(1);
			}

			switch (npc.ai[0])
			{
				case 1:
					OpenPetals();
					OpenPetalsAnimation();
					break;
				default:
                    ManageMovement();
					ManageMovementAnimation();
					break;
			}
			return false;
		}
		const float animationSpdOffset = 4f;
		int GetFrame() => (int)(RealFrame / strideSpeed / animationSpdOffset);
		int ScaleFrame(int frame) => (int)(animationSpdOffset * frame * strideSpeed); // returns value necessary for real frame to set animation frame to target frame frame
		float RealFrame {
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}
		void ManageMovement()
		{
			npc.TargetClosest(false);
			Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY, 1, false, 0);
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

		void ManageMovementAnimation()
		{
			RealFrame++;
			if (npc.velocity.Y != 0 || npc.oldVelocity == Vector2.Zero || GetFrame() > 4 || GetFrame() < 0)
            {
				RealFrame = 3;
            }
        }

		public void OpenPetals()
        {
			if (npc.frameCounter == 1)
			{
				npc.velocity.X = 0;
			}
			else if (npc.frameCounter > 75 && npc.frameCounter % 10 == 0)
			{

				if (npc.frameCounter >= 150)
				{
					RealFrame = ScaleFrame(4);
					SetState(0);
				}

				else
				{
					Projectile.NewProjectileDirect(npc.Center - new Vector2(1, 19), Main.rand.NextVector2Unit() * 2, ModContent.ProjectileType<CenturyFlowerSpore.CenturyFlowerSpore>(), 0, 0);
				}
			}
			if (npc.velocity.Y == 0)
			{
				npc.velocity.X *= 0.9f;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GrassBlades, 2 * hitDirection, -1.5f);
			}
			if (npc.life <= 0)
            {
				Helper.SpawnGore(npc, "Gores/Enemies/Century", 4, 1);
				Helper.SpawnGore(npc, "Gores/Enemies/Century", 2, 2);
				Helper.SpawnGore(npc, "Gores/Enemies/Century", 2, 3);
				Helper.SpawnGore(npc, "Gores/Enemies/Century", 2, 4);
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Grass, 2 * hitDirection, -1.5f);
				}
			}
		}
		public void OpenPetalsAnimation()
		{
			if (npc.frameCounter <= 75 && GetFrame() < 7)
			{
				RealFrame++;
			}

			else if (Helper.InRange(npc.frameCounter, 140, 150) && GetFrame() > 5)
            {
				RealFrame--;
            }
        }

		void FitVelocityToTarget(Vector2 newVelocity) => npc.velocity = Vector2.Lerp(npc.velocity, newVelocity, .1f);
		void FitVelocityXToTarget(float newX) => npc.velocity.X = MathHelper.Lerp(npc.velocity.X, newX, 0.1f);
		void FitVelocityYToTarget(float newY) => npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, newY, 0.1f);
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OverworldDay.Chance * 0.2f;
		}

        public override void NPCLoot()
		{
			if (Main.rand.NextBool(20))
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CenturySprayer>());
			}
		}
    }
}