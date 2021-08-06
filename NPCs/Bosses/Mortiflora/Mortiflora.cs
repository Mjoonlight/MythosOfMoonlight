using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora
{
	[AutoloadBossHead]
	public class Mortiflora : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Mortiflora");
			Main.npcFrameCount[npc.type] = 16;
			NPCID.Sets.TrailingMode[npc.type] = 7;
			NPCID.Sets.TrailCacheLength[npc.type] = 16;
		}
        public override void SetDefaults() {
			npc.width = 48;
			npc.height = 70;
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
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
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
			for (int i = 0; i < trailLength; i++) {
				drawPos = npc.oldPos[i] + offset;
				var imageColor = glowMaskColor * (1f - colorMult * i);
				Main.spriteBatch.Draw(texture, drawPos, npc.frame, imageColor, npc.oldRot[i], orig, npc.scale, effects, 0f); 
			}
			return false;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
			npc.lifeMax = 3490 + (1120*numPlayers);
		}
		int Timer;
		int animationTimer;
		int prevAttack;
		int attack;
		int attackTimer;
		int attackanimTimer;
		int attackanimTimer2;
		float hpPercentage;
		bool attackDone = true;
		bool movement = true;
		Player target;
		public override void AI() {
			hpPercentage = npc.life/npc.lifeMax;
			npc.TargetClosest();
			Timer++;
			target = Main.player[npc.target];

			npc.DirectionTo(target.Center);
			npc.spriteDirection = npc.direction;
			if (Timer % (int)Math.Round(4 + (3f*hpPercentage)) == 0)
				animationTimer++;
			if (animationTimer > 5)
				animationTimer = 0;
			npc.frame.Y = animationTimer * 74;

			if (movement) {
				bool flag19 = false;
								bool flag20 = false;
								if (npc.justHit)
								{
									npc.ai[2] = 0f;
								}
								if (!flag20)
								{
									if (npc.ai[2] >= 0f)
									{
										int num287 = 16;
										bool flag21 = false;
										bool flag22 = false;
										if (npc.position.X > npc.ai[0] - (float)num287 && npc.position.X < npc.ai[0] + (float)num287)
										{
											flag21 = true;
										}
										else if ((npc.velocity.X < 0f && npc.direction > 0) || (npc.velocity.X > 0f && npc.direction < 0))
										{
											flag21 = true;
										}
										num287 += 24;
										if (npc.position.Y > npc.ai[1] - (float)num287 && npc.position.Y < npc.ai[1] + (float)num287)
										{
											flag22 = true;
										}
										if (flag21 & flag22)
										{
											npc.ai[2] += 1f;
											if (npc.ai[2] >= 30f && num287 == 16)
											{
												flag19 = true;
											}
											if (npc.ai[2] >= 60f)
											{
												npc.ai[2] = -200f;
												npc.direction *= -1;
												npc.velocity.X = npc.velocity.X * -1f;
												npc.collideX = false;
											}
										}
										else
										{
											npc.ai[0] = npc.position.X;
											npc.ai[1] = npc.position.Y;
											npc.ai[2] = 0f;
										}
										npc.TargetClosest(true);
									}
									else
									{
										npc.ai[2] += 1f;
										if (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) > npc.position.X + (float)(npc.width / 2))
										{
											npc.direction = -1;
										}
										else
										{
											npc.direction = 1;
										}
									}
								}
								int num288 = (int)((npc.position.X + (float)(npc.width / 2)) / 16f) + npc.direction * 2;
								int num289 = (int)((npc.position.Y + (float)npc.height) / 16f);
								bool flag23 = true;
								bool flag24 = false;
								int num290 = 3;
								int num;
								for (int num316 = num289; num316 < num289 + num290; num316 = num + 1)
								{
									if (Main.tile[num288, num316] == null)
									{
										Main.tile[num288, num316] = new Tile();
									}
									if ((Main.tile[num288, num316].nactive() && Main.tileSolid[(int)Main.tile[num288, num316].type]) || Main.tile[num288, num316].liquid > 0)
									{
										if (num316 <= num289 + 1)
										{
											flag24 = true;
										}
										flag23 = false;
										break;
									}
									num = num316;
								}
								bool flag25 = false;
								for (int num317 = num289; num317 < num289 + num290 - 2; num317 = num + 1)
								{
									if (Main.tile[num288, num317] == null)
									{
										Main.tile[num288, num317] = new Tile();
									}
									if ((Main.tile[num288, num317].nactive() && Main.tileSolid[(int)Main.tile[num288, num317].type]) || Main.tile[num288, num317].liquid > 0)
									{
										flag25 = true;
										break;
									}
									num = num317;
								}
								npc.directionY = (!flag25).ToDirectionInt();
								if (flag19)
								{
									flag24 = false;
									flag23 = false; //true
								}
								if (flag23)
								{
									npc.velocity.Y = npc.velocity.Y + 0.1f;
									if (npc.velocity.Y > 3f)
									{
										npc.velocity.Y = 3f;
									}
								}
								else
								{
									if (npc.directionY < 0 && npc.velocity.Y > 0f)
									{
										npc.velocity.Y = npc.velocity.Y - 0.1f;
									}
									if (npc.velocity.Y < -4f)
									{
										npc.velocity.Y = -4f;
									}
								}
								/*if (npc.collideX)
								{
									npc.velocity.X = npc.oldVelocity.X * -0.4f;
									if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 1f)
									{
										npc.velocity.X = 1f;
									}
									if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -1f)
									{
										npc.velocity.X = -1f;
									}
								}
								if (npc.collideY)
								{
									npc.velocity.Y = npc.oldVelocity.Y * -0.25f;
									if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
									{
										npc.velocity.Y = 1f;
									}
									if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
									{
										npc.velocity.Y = -1f;
									}
								}*/
								float num319 = 2f;
								if (npc.direction == -1 && npc.velocity.X > -num319)
								{
									npc.velocity.X = npc.velocity.X - 0.1f;
									if (npc.velocity.X > num319)
									{
										npc.velocity.X = npc.velocity.X - 0.1f;
									}
									else if (npc.velocity.X > 0f)
									{
										npc.velocity.X = npc.velocity.X + 0.05f;
									}
									if (npc.velocity.X < -num319)
									{
										npc.velocity.X = -num319;
									}
								}
								else if (npc.direction == 1 && npc.velocity.X < num319)
								{
									npc.velocity.X = npc.velocity.X + 0.1f;
									if (npc.velocity.X < -num319)
									{
										npc.velocity.X = npc.velocity.X + 0.1f;
									}
									else if (npc.velocity.X < 0f)
									{
										npc.velocity.X = npc.velocity.X - 0.05f;
									}
									if (npc.velocity.X > num319)
									{
										npc.velocity.X = num319;
									}
								}
								else
								{
									num319 = 1.5f;
								}
								if (npc.directionY == -1 && npc.velocity.Y > -num319)
								{
									npc.velocity.Y = npc.velocity.Y - 0.04f;
									if (npc.velocity.Y > num319)
									{
										npc.velocity.Y = npc.velocity.Y - 0.05f;
									}
									else if (npc.velocity.Y > 0f)
									{
										npc.velocity.Y = npc.velocity.Y + 0.03f;
									}
									if (npc.velocity.Y < -num319)
									{
										npc.velocity.Y = -num319;
									}
								}
								else if (npc.directionY == 1 && npc.velocity.Y < num319)
								{
									npc.velocity.Y = npc.velocity.Y + 0.04f;
									if (npc.velocity.Y < -num319)
									{
										npc.velocity.Y = npc.velocity.Y + 0.05f;
									}
									else if (npc.velocity.Y < 0f)
									{
										npc.velocity.Y = npc.velocity.Y - 0.03f;
									}
									if (npc.velocity.Y > num319)
									{
										npc.velocity.Y = num319;
									}
								}
								if (Timer % 6 == 0 && Math.Abs(npc.Center.X - target.Center.X) < 640) {
					if (target.Center.Y < npc.Center.Y)
										npc.velocity.Y -= 1;
					else npc.velocity.Y += 1;
				}
				//if (Math.Abs(npc.Center.X - target.Center.X) < 160 && target.Center.Y < npc.Center.Y) {
				//	if (npc.velocity.Y < 8 && Timer % 4 == 0)
				//		npc.velocity.Y -= 1;
				//}
			}

			if (attackDone) {
				attackTimer++;
				if (attackTimer >= (90 + (240 * hpPercentage))) {
					//while (attack == prevAttack) {
						//if (hpPercentage < 0.5f) attack = Main.rand.Next(1, 7);
					//	attack = Main.rand.Next(1, 5);
					//}
					attack = 6;
					prevAttack = attack;
					attackTimer = 0;
					attackDone = false;
					attackanimTimer = 0;
					attackanimTimer2 = 0;
				}
			}
			else {
				if (attack == 1) {
					if (Timer % (4 + Math.Round(3*hpPercentage)) == 0)
						attackanimTimer++;
					npc.frame.Y = (attackanimTimer + 6) * 74;
					if (npc.frame.Y == 333*2 && Timer % (4 + Math.Round(3*hpPercentage)) == 0) {
						Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.Center - target.Center) * -1f, ModContent.ProjectileType<Projectiles.MortifloraRedWave>(), (int)(npc.damage*0.3f), 0f, Main.myPlayer);
						Main.PlaySound(SoundID.Item71);
					}
					if (attackanimTimer > 5) {
						attackDone = true;
						attackTimer = 0;
						attackanimTimer = 0;
					}
				}
				if (attack == 2) {
					if (Timer % 12 == 0)
						attackanimTimer++;
					npc.frame.Y = (attackanimTimer + 11) * 74;
					if (npc.frame.Y == 1036 && Timer % 12 == 0)
						for (int i = 0; i < 4; i++) {
							Vector2 vector = Vector2.Normalize(npc.Center - (target.Center - new Vector2(0, 100))) * Main.rand.NextFloat(-7f, -5f);
							Projectile.NewProjectile(npc.Center - new Vector2(1, 34), vector.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-14, 15))), ModContent.ProjectileType<Projectiles.MortifloraBones>(), (int)(npc.damage*0.2f), 0f, Main.myPlayer);
						}
					if (attackanimTimer > 3) {
						attackDone = true;
						attackTimer = 0;
						attackanimTimer = 0;
					}
				}
				if (attack == 3) {
					if (Timer % 12 == 0)
						attackanimTimer++;
					npc.frame.Y = (attackanimTimer + 11) * 74;
					if (npc.frame.Y == 1036 && Timer % 12 == 0) {
						int damage = 11;
						if (Main.expertMode) damage = 16;
						Vector2 vector = Vector2.Normalize(npc.Center - (target.Center - new Vector2(0, 100))) * -6f;
						Projectile.NewProjectile(npc.Center - new Vector2(1, 34), vector, ModContent.ProjectileType<Projectiles.BigSap>(), damage, 0f, Main.myPlayer);
					}
					if (attackanimTimer > 3) {
						attackanimTimer2++;
						if (attackanimTimer2 > 2) {
							attackDone = true;
							attackTimer = 0;
							attackanimTimer = 0;
							attackanimTimer2 = 0;
						}
						else attackanimTimer = 0;
					}
				}
				if (attack == 4) {
					if (attackanimTimer2 == 0) {
						if (Timer % 12 == 0)
						attackanimTimer++;
						npc.frame.Y = (attackanimTimer + 11) * 74;
						if (attackanimTimer > 3 && Timer % 12 == 11) {
							attackTimer = 0;
							attackanimTimer = 0;
							attackanimTimer2 = 1;
						}
					}
					else if (attackanimTimer2 == 1) {
						attackTimer++;
						if (attackTimer == 1) {
							float num320 = 9f;
							Vector2 vector36 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
							float num321 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector36.X;
							float num322 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector36.Y;
							float num323 = (float)Math.Sqrt((double)(num321 * num321 + num322 * num322));
							num323 = num320 / num323;
							num321 *= num323;
							num322 *= num323;
							npc.velocity.X = num321 * 1f;
							npc.velocity.Y = num322 * 1f; //4
							movement = false;
						}
						if (attackTimer > (61 - ((4 + Math.Round(3*hpPercentage))*6))) {
							if (attackTimer % (4 + Math.Round(3*hpPercentage)) == 0)
								attackanimTimer++;
							npc.frame.Y = (attackanimTimer + 6) * 74;
						}
						if (attackTimer < 61)
							npc.velocity *= 0.98f;
						if (attackTimer == 61) {
							npc.velocity = new Vector2();
							attackDone = true;
							attackTimer = 0;
							attackanimTimer = 0;
							attackanimTimer2 = 0;
							movement = true;
						}
					}
				}
				if (attack == 5) {
					attackTimer++;
					if (attackTimer > (61 - ((4 + Math.Round(3*hpPercentage))*4))) {
						if (attackTimer % (4 + Math.Round(3*hpPercentage)) == 0)
							attackanimTimer++;
						npc.frame.Y = (attackanimTimer + 11) * 74;
					}
					if (attackTimer < 60) {
						movement = false;
						npc.velocity *= 0.975f;
					}
					else if (attackTimer == 61) {
						npc.velocity = new Vector2();
						for (int i = 0; i < 12; i++) {
							Gore.NewGore(npc.position + new Vector2(npc.width/2 - 20, 0), new Vector2(0, 1).RotatedByRandom(MathHelper.ToRadians(360)), mod.GetGoreSlot("Gores/MortifloraDustGore" + Main.rand.Next(3)), Main.rand.NextFloat(0.5f, 1.5f));
						}
						attackTimer = 0;
						attackanimTimer = 0;
					}
				}
				if (attack == 6) {
					if (Timer % 12 == 0)
						attackanimTimer++;
					npc.frame.Y = (attackanimTimer + 11) * 74;
					if (npc.frame.Y == 1036 && Timer % 12 == 0)
						for (int i = 0; i < 14; i++) {
							Projectile.NewProjectile(npc.Center - new Vector2(1, 34), new Vector2(0, -6).RotatedBy(MathHelper.ToRadians(((i-6)*5) + Main.rand.NextFloat(-2, 2))), ModContent.ProjectileType<Projectiles.MortifloraBones2>(), (int)(npc.damage*0.2f), 0f, Main.myPlayer);
						}
					if (attackanimTimer > 3) {
						attackDone = true;
						attackTimer = 0;
						attackanimTimer = 0;
					}
				}
			}
		}
	}
}