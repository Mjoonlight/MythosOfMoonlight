using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace MythosOfMoonlight.NPCs.Bosses.Mortiflora
{
    [AutoloadBossHead]
    public class Mortiflora : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mortiflora");
            Main.npcFrameCount[NPC.type] = 16;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 16;

            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCDebuffImmunityData debuffData = new()
            {
                SpecificallyImmuneTo = new int[] { BuffID.Poisoned }
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1 };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

        }
        public override void SetDefaults()
        {
            NPC.width = 54;
            NPC.height = 74;
            NPC.damage = 25;
            NPC.lifeMax = 2800;
            NPC.defense = 10;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.buyPrice(0, 2, 50, 0);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.SpawnWithHigherTime(30);
            NPC.netAlways = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Mortiflora");
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            var drawPos = NPC.Center - screenPos;
            var orig = NPC.frame.Size() / 2f;
            var effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, orig, NPC.scale, effects, 0f);
            texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glowmask").Value;
            // very bright and very transparent
            var glowMaskColor = new Color(250, 250, 250, 0);
            Main.spriteBatch.Draw(texture, drawPos, NPC.frame, glowMaskColor, NPC.rotation, orig, NPC.scale, effects, 0f);

            int trailLength = NPCID.Sets.TrailCacheLength[NPC.type];
            var offset = new Vector2(NPC.width / 2f, NPC.height / 2f);
            int colorMult = (int)(1f / trailLength);
            for (int i = 0; i < trailLength; i++)
            {
                drawPos = NPC.oldPos[i] + offset;
                var imageColor = glowMaskColor * (1f - colorMult * i);
                Main.spriteBatch.Draw(texture, drawPos, NPC.frame, imageColor, NPC.oldRot[i], orig, NPC.scale, effects, 0f);
            }
            return false;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.675f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
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
        MortState State
        {
            get => pstate;
            set
            {
                AttackCounter = 0;
                pstate = value;
            }
        }

        const float frameInterval = 5f;
        int lastFrame = 0;
        public void Animate()
        {
            var getFrame = (int)(NPC.frameCounter / (frameInterval / speedMod));
            if (NPC.justHit)
                lastFrame = getFrame;

            int GetFrame(int frameAmount, int frameStart) => ((getFrame + lastFrame) % frameAmount + frameStart) * NPC.height;
            switch (State)
            {
                case MortState.Default:
                    NPC.frame.Y = GetFrame(6, 0);
                    break;
                case MortState.Attack1:
                    if (Phase == 1)
                    {
                        if (InRange(AttackCounter, 0, 90))
                        {
                            goto case MortState.Default;
                        }

                        else if (InRange(AttackCounter, 90, 127))
                        {
                            NPC.frame.Y = GetFrame(5, 7); // ((getFrame + lastFrame) % 5 + 7) * NPC.height;
                        }
                    }
                    else
                    {

                    }
                    break;
                case MortState.Attack2:
                    NPC.frame.Y = GetFrame(4, 12); // ((getFrame + lastFrame) % 4 + 13) * NPC.height;
                    break;
            }
        }

        float speedMod;

        static bool InRange(float number, float min, float max)
        {
            return min <= number && max >= number;
        }

        static bool InRange(int number, int min, int max)
        {
            return min <= number && max >= number;
        }

        int Phase
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        float AttackCounter
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        int DashCounter
        {
            get => (int)NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            var target = Main.player[NPC.target];

            NPC.frameCounter++;
            AttackCounter++;
            speedMod = (NPC.lifeMax - NPC.life * .5f) / (NPC.lifeMax * .77f);
            Animate();

            if (NPC.frameCounter % 60 == 0)
            {
                Helper.WarpAroundPlayer(NPC, NPC.PlayerTarget().Center, 10000, 300);
            }
            /*
			switch (state)
			{
				case MortState.Default:
					NPC.spriteDirection = NPC.direction;
					NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, (target.Center.X > NPC.Center.X ? 1 : -1) * 3 * speedMod, 0.05f);
					NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, (target.Center.Y > NPC.Center.Y ? 5 : -5) + (float)Math.Sin(attackCounter / 33d) * 2, 0.04f * (speedMod * .5f));
					if (Main.rand.NextFloat() < .02f && attackCounter > 120)
                    {
						state = MortState.Attack2;
					}
					break;
				case MortState.Attack1:
					if (InRange(attackCounter, 0, 90))
					{
						// place dust
						NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.01f);
					}

					else if (InRange(attackCounter, 90, 90))
                    {
						NPC.velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * speedMod * 8;
						var Projectile = Projectile.NewProjectileDirect(NPC.Center + NPC.velocity, NPC.velocity * 1.5f, ModContent.ProjectileType<MortifloraRedWave>(), 6, 5);
						Main.NewText(Projectile.position);
						Main.NewText(NPC.position);
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
						NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.01f);
					}

					else if (attackCounter > delay && (int)(attackCounter / dashLength) < dashMax)
                    {
						var realCounter = attackCounter % dashLength;
						if (InRange(realCounter, delay, delay))
						{
							var direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 8;
							NPC.velocity = direction.RotatedByRandom(MathHelper.Pi/6f) * speedMod;
							var Projectile = Projectile.NewProjectileDirect(NPC.Center + NPC.velocity, direction, ModContent.ProjectileType<MortifloraRedWave>(), 6, 5);
						}

						else if (InRange(realCounter, delay, dashLength))
                        {
							NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.1f);
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
						NPC.velocity = Vector2.Zero;
						Helper.FireProjectilesInArc(NPC.Center + Vector2.UnitY * 16, -Vector2.UnitY, MathHelper.Pi, ModContent.ProjectileType<MortifloraBones>(), 6, 6, 1, ++fireTime);
					}

					else if (attackCounter > 30)
                    {
						state = MortState.Default;
                    }
					break;
			}
			*/
        }

        int fireTime = 0;
    }
}