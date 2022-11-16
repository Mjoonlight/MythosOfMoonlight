/*using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Enemies.Kranira
{
    public class Kranira : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kranira");
			Main.npcFrameCount[NPC.type] = 4;
			NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned,
				}
			});

			NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 44;
			NPC.damage = 12;
			NPC.lifeMax = 120;
			NPC.defense = 2;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.netAlways = true;
			NPC.noTileCollide = true;
		}

        private readonly float increment = 0.0075f;
        private float currentSpeed = 0;
        float sineIncrement = 0;
		public override void AI()
		{
			UpdateFrame();
			NPC.TargetClosest(true);
			var target = Main.player[NPC.target];

			NPC.DirectionTo(target.Center);
			NPC.spriteDirection = NPC.direction;
			MoveTowardsPlayer(target);

			IncreaseSine(.05f);
		}

		void MoveTowardsPlayer(Player target)
        {
			Vector2 direction = -GetNormalized(NPC.position - target.position);
			NPC.velocity = direction * currentSpeed;
			currentSpeed += currentSpeed <= 2.4976f ? increment : 0;
		}

		void IncreaseSine(float value)
        {
			sineIncrement += value;
			NPC.velocity += new Vector2(0, (float)Math.Sin(sineIncrement) * 0.3f);
		}

		Vector2 GetNormalized(Vector2 direction)
        {
			Vector2 returnDir = direction;
			returnDir.Normalize();
			return returnDir;
        }

		Vector2 GetNormalized(float x, float y)
        {
			Vector2 returnDir = new(x, y);
			returnDir.Normalize();
			return returnDir;
        }

		float trueAnimationFrame = 0;
		void UpdateFrame()
        {
			trueAnimationFrame = (trueAnimationFrame + .125f) % 4;
			int animationFrame = (int)trueAnimationFrame;
			NPC.frame.Y = animationFrame * NPC.height;
        }
	}
}*/