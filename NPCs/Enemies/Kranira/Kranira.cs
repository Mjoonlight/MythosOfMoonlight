using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Enemies.Kranira
{
	public class Kranira : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kranira");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 44;
			npc.damage = 12;
			npc.lifeMax = 120;
			npc.defense = 2;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.aiStyle = -1;
			npc.noGravity = true;
			npc.netAlways = true;
			npc.noTileCollide = true;
		}

		float increment = 0.0075f, currentSpeed = 0;
		float sineIncrement = 0;
		public override void AI()
		{
			UpdateFrame();
			npc.TargetClosest(true);
			var target = Main.player[npc.target];

			npc.DirectionTo(target.Center);
			npc.spriteDirection = npc.direction;
			MoveTowardsPlayer(target);

			IncreaseSine(.05f);
		}

		void MoveTowardsPlayer(Player target)
        {
			Vector2 direction = -GetNormalized(npc.position - target.position);
			npc.velocity = direction * currentSpeed;
			currentSpeed += currentSpeed <= 2.4976f ? increment : 0;
		}

		void IncreaseSine(float value)
        {
			sineIncrement += value;
			npc.velocity += new Vector2(0, (float)Math.Sin(sineIncrement) * 0.3f);
		}

		Vector2 GetNormalized(Vector2 direction)
        {
			Vector2 returnDir = direction;
			returnDir.Normalize();
			return returnDir;
        }

		Vector2 GetNormalized(float x, float y)
        {
			Vector2 returnDir = new Vector2(x, y);
			returnDir.Normalize();
			return returnDir;
        }

		float trueAnimationFrame = 0;
		void UpdateFrame()
        {
			trueAnimationFrame = (trueAnimationFrame + .125f) % 4;
			int animationFrame = (int)trueAnimationFrame;
			npc.frame.Y = animationFrame * npc.height;
        }
	}
}