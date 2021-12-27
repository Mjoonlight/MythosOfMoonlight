using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using MythosOfMoonlight;
using MythosOfMoonlight.NPCs.Bosses.Mortiflora.Projectiles;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora
{
	[AutoloadBossHead]
	public class Mortiflora : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mortiflora");
			Main.npcFrameCount[npc.type] = 16;
			NPCID.Sets.TrailingMode[npc.type] = 7;
			NPCID.Sets.TrailCacheLength[npc.type] = 16;
		}
		public override void SetDefaults()
		{
			npc.width = 54;
			npc.height = 74;
			npc.damage = 25;
			npc.lifeMax = 2800;
			npc.defense = 10;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 2, 50, 0);
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.noGravity = true;
			npc.boss = true;
			npc.netAlways = true;
			npc.noTileCollide = true;
			npc.lavaImmune = true;
			music = MusicID.Boss5;
			npc.buffImmune[BuffID.Poisoned] = true;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Mortiflora");
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var texture = Main.npcTexture[npc.type];
			var drawPos = npc.Center - Main.screenPosition;
			var orig = npc.frame.Size() / 2f;
			var effects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Main.spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, orig, npc.scale, effects, 0f);
			texture = mod.GetTexture("NPCs/Bosses/Mortiflora/Mortiflora_Glowmask");
			// very bright and very transparent
			var glowMaskColor = new Color(250, 250, 250, 0);
			Main.spriteBatch.Draw(texture, drawPos, npc.frame, glowMaskColor, npc.rotation, orig, npc.scale, effects, 0f);

			int trailLength = NPCID.Sets.TrailCacheLength[npc.type];
			var offset = new Vector2(npc.width / 2f, npc.height / 2f);
			int colorMult = (int)(1f / trailLength);
			for (int i = 0; i < trailLength; i++)
			{
				drawPos = npc.oldPos[i] + offset;
				var imageColor = glowMaskColor * (1f - colorMult * i);
				Main.spriteBatch.Draw(texture, drawPos, npc.frame, imageColor, npc.oldRot[i], orig, npc.scale, effects, 0f);
			}
			return false;
		}

		enum MortState
		{
			Default,
			Attack1,
			Attack1Expert,
			Attack2,
			Dead
		};
		MortState pstate = MortState.Default;
		MortState state
        {
			get => pstate;
			set
            {
				attackCounter = 0;
				pstate = value;
            }
        }

		const float frameInterval = 5f;
		int lastFrame = 0;
		public void Animate()
        {
			var getFrame = (int)(npc.frameCounter / (frameInterval / speedMod));
			if (npc.justHit)
			{
				lastFrame = getFrame;
			}

			int GetFrame(int frameAmount, int frameStart) => ((getFrame + lastFrame) % frameAmount + frameStart) * npc.height;
			switch (state)
            {
				case MortState.Default:
					npc.frame.Y = GetFrame(6, 0);
					break;
				case MortState.Attack1:
					if (Phase == 1)
					{
						if (InRange(attackCounter, 0, 90))
                        {
							goto case MortState.Default;
                        }

						else if (InRange(attackCounter, 90, 127))
						{
							npc.frame.Y = GetFrame(5, 7); // ((getFrame + lastFrame) % 5 + 7) * npc.height;
						}
					}

					else
                    {
						
                    }
					break;
				case MortState.Attack2:
					npc.frame.Y = GetFrame(4, 12); // ((getFrame + lastFrame) % 4 + 13) * npc.height;
					break;
            }
        }

		float speedMod;
		bool InRange(float number, float min, float max)
        {
			return min <= number && max >= number;
        }

		bool InRange(int number, int min, int max)
		{
			return min <= number && max >= number;
		}

		int Phase
        {
			get => (int)npc.ai[0];
			set => npc.ai[0] = value;
        }

		float attackCounter
        {
			get => npc.ai[1];
			set => npc.ai[1] = value;
        }

		int dashCounter
        {
			get => (int)npc.ai[2];
			set => npc.ai[2] = value;
        }

		public override void AI()
		{
			if (Main.netMode != NetmodeID.Server) // This all needs to happen client-side!
			{
				Filters.Scene.Activate("PurpleComet");

				Filters.Scene["PurpleComet"].Deactivate();
			}

			npc.TargetClosest(true);
			var target = Main.player[npc.target];

			npc.frameCounter++;
			attackCounter++;
			speedMod = (npc.lifeMax - npc.life * .5f) / (npc.lifeMax * .77f);
			Animate();

			switch (state)
			{
				case MortState.Default:
					npc.spriteDirection = npc.direction;
					npc.velocity.X = MathHelper.Lerp(npc.velocity.X, (target.Center.X > npc.Center.X ? 1 : -1) * 3 * speedMod, 0.05f);
					npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, (target.Center.Y > npc.Center.Y ? 5 : -5) + (float)Math.Sin(attackCounter / 33d) * 2, 0.04f * (speedMod * .5f));
					if (Main.rand.NextFloat() < .02f && attackCounter > 120)
                    {
						state = MortState.Attack2;
					}
					break;
				case MortState.Attack1:
					if (InRange(attackCounter, 0, 90))
					{
						// place dust
						npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.01f);
					}

					else if (InRange(attackCounter, 90, 90))
                    {
						npc.velocity = (target.Center - npc.Center).SafeNormalize(Vector2.UnitX) * speedMod * 8;
						var projectile = Projectile.NewProjectileDirect(npc.Center + npc.velocity, npc.velocity * 1.5f, ModContent.ProjectileType<MortifloraRedWave>(), 6, 5);
						Main.NewText(projectile.position);
						Main.NewText(npc.position);
					}

					else if (InRange(attackCounter, 120, 120))
                    {
						state = MortState.Default;
                    }
					break;
				case MortState.Attack1Expert:
					int delay = 45, dashLength = 75, dashMax = 4;
					delay = (int)(delay / speedMod);
					dashLength = (int)(dashLength / speedMod);
					if (InRange(attackCounter, 0, delay))
                    {
						npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.01f);
					}

					else if (attackCounter > delay && (int)(attackCounter / dashLength) < dashMax)
                    {
						var realCounter = attackCounter % dashLength;
						if (InRange(realCounter, delay, delay))
						{
							var direction = (target.Center - npc.Center).SafeNormalize(Vector2.UnitX) * 8;
							npc.velocity = direction.RotatedByRandom(MathHelper.Pi/6f) * speedMod;
							var projectile = Projectile.NewProjectileDirect(npc.Center + npc.velocity, direction, ModContent.ProjectileType<MortifloraRedWave>(), 6, 5);
						}

						else if (InRange(realCounter, delay, dashLength))
                        {
							npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);
						}

						else if (realCounter > dashLength)
                        {
							attackCounter += dashLength - 1;
                        }
					}

					else
                    {
						state = MortState.Default;
                    }
					break;
				case MortState.Attack2:
					if (attackCounter == 1)
					{
						npc.velocity = Vector2.Zero;
						Helper.FireProjectilesInArc(npc.Center + Vector2.UnitY * 16, -Vector2.UnitY, MathHelper.Pi, ModContent.ProjectileType<MortifloraBones>(), 6, 6, 1, ++fireTime);
					}

					else if (attackCounter > 30)
                    {
						state = MortState.Default;
                    }
					break;
			}
		}

		int fireTime = 0;
	}
}