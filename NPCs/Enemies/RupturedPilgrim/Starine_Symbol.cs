using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles;
using MythosOfMoonlight.Projectiles;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim
{
    public class Starine_Symbol : ModNPC
    {
		public static NPC symbol = null;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starine Symbol");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
			npc.townNPC = true;
			npc.width = 54;
            npc.height = 62;
            npc.aiStyle = -1;
            npc.damage = 1;
            npc.defense = 2;
            npc.lifeMax = 1000;
			npc.friendly = true;
			npc.rarity = 4;
			npc.HitSound = SoundID.NPCHit19;
            npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
			npc.noGravity = true;
			for (int i = 1; i <= 205; i += 1)
			{
				npc.buffImmune[i] = true;
			}
		}
		private enum NState
		{
			Normal,
			Invulerable,
			Laser,
			Death
		}
		private NState State
		{
			get { return (NState)(int)npc.ai[0]; }
			set { npc.ai[0] = (int)value; }
		}
		private void SwitchTo(NState state)
		{
			State = state;
		}
		public float SymbolTimer
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}
		public float StateTimer
		{
			get => npc.ai[2];
			set => npc.ai[2] = value;
		}
		public float Radius
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}
		public float FloatTimer;
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(FloatTimer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			FloatTimer = reader.ReadSingle();
        }
		public Vector2 CircleCenter;
        public override void AI()
        {
			if (symbol == null || !symbol.active)
            {
				symbol = npc;
            }
			Main.npcChatText = "I will never leave you alone";
			Lighting.AddLight(npc.Center, 1f, 1f, 1f);
			FloatTimer++;
			if (CircleCenter != Vector2.Zero)
            {
				if (State != NState.Laser)
				{
					npc.velocity = (CircleCenter + new Vector2(0, 20f * (float)Math.Sin(MathHelper.ToRadians(FloatTimer))) - npc.Center) / 15f;
				}
            }
			if (State != NState.Normal)
			{
				SymbolTimer++;
			}
			if (CircleCenter == Vector2.Zero)
            {
				CircleCenter = npc.Center - new Vector2(0, 48);
            }
            switch (State) 
			{
				case NState.Normal:
                    {
						break;
                    }
				case NState.Invulerable:
					{
						npc.dontTakeDamage = true;
						Radius = Math.Min(SymbolTimer * 3.5f, 420f);
						break;
                    }
				case NState.Laser:
                    {
						StateTimer++;
						npc.velocity = (CircleCenter - npc.Center) / 10f;
						if (StateTimer == 30)
						{
							Main.PlaySound(SoundID.NPCHit5, npc.Center);
							for (int i = 4; i <= 360; i += 4)
							{
								Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
								Dust dust = Dust.NewDustDirect(npc.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
								dust.noGravity = true;
							}
						}
						if (StateTimer == 60)
						{
							for (int i = 90; i <= 360; i += 90)
							{
								Vector2 shoot = MathHelper.ToRadians(i).ToRotationVector2();
								Projectile.NewProjectile(npc.Center + new Vector2(0, 15), shoot, ModContent.ProjectileType<Projectiles.TestTentacleProj>(), 8, .1f, Main.myPlayer);
							}
						}
						if (StateTimer == 90)
						{
							Main.PlaySound(SoundID.NPCHit5, npc.Center);
							for (int i = 4; i <= 360; i += 4)
							{
								Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
								Dust dust = Dust.NewDustDirect(npc.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
								dust.noGravity = true;
							}
						}
						if (StateTimer == 120)
						{
							for (int i = 90; i <= 360; i += 90)
							{
								Vector2 shoot = MathHelper.ToRadians(i + 45).ToRotationVector2();
								Projectile.NewProjectile(npc.Center + new Vector2(0, 15), shoot, ModContent.ProjectileType<Projectiles.TestTentacleProj>(), 8, .1f, Main.myPlayer);
							}
							StateTimer = 0;
							SwitchTo(NState.Invulerable);
						}
						break;
                    }
				case NState.Death:
                    {
						Radius -= 3.5f;
						if (Radius <= 0f)
                        {
							StateTimer++;
                        }
						if (StateTimer > 0)
                        {
							npc.velocity = CircleCenter + new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6)) - npc.Center;
                        }
						if (StateTimer == 120)
                        {
							npc.life = 0;
							npc.checkDead();
                        }
						break;
                    }
			}
			if (Vector2.Distance(npc.Center, Main.LocalPlayer.Center) <= 1000f)
            {
				if (Radius >= 400 && Vector2.Distance(CircleCenter, Main.LocalPlayer.Center) > 420)
                {
					Vector2 vel = Utils.SafeNormalize(CircleCenter - Main.LocalPlayer.Center, Vector2.Zero);
					Main.LocalPlayer.Center += vel * 9f;
					Main.LocalPlayer.velocity = vel * 3f;
					Main.LocalPlayer.itemTime = Main.LocalPlayer.HeldItem.useTime - 2;
					Main.LocalPlayer.gravity = 0f;
					Main.LocalPlayer.controlMount = false;
					Main.LocalPlayer.controlHook = false;
					for (int i = 1; i <= 10; i++)
                    {
						Dust dust = Dust.NewDustDirect(Main.LocalPlayer.position, Main.LocalPlayer.width, Main.LocalPlayer.height, ModContent.DustType<StarineDust>());
						dust.noGravity = true;
						dust.velocity = vel * -5;
                    }
					Main.LocalPlayer.statLife -= 1;
					if (Main.LocalPlayer.statLife <= 0)
					{
						string text = " tried to escape.";
						Main.LocalPlayer.KillMe(PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + text), 9999, 0, false);
					}
				}

			}
        }
        public override void FindFrame(int frameHeight)
        {
			npc.frameCounter++;
			if (npc.frameCounter >= 19)
            {
				npc.frameCounter = 0;
            }
			npc.frame.Y = (int)(npc.frameCounter / 5) * npc.height;
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			npc.townNPC = false;
			NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 200, ModContent.NPCType<RupturedPilgrim>());
			SwitchTo(NState.Invulerable);
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
			SwitchTo(NState.Invulerable);
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = "Challenge";
		}
		public override string GetChat()
		{
			return "I will never leave you alone.";
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			if (State != NState.Normal)
            {
				float scale = Radius / 420f;
				float rotate = MathHelper.ToRadians(SymbolTimer * 2f);
				Vector2 orig = new Vector2(420, 422);
				Color color = Color.White * scale;
				Texture2D tex = ModContent.GetTexture("MythosOfMoonlight/NPCs/Enemies/RupturedPilgrim/Starine_Barrier");
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
				spriteBatch.Draw(tex, CircleCenter - Main.screenPosition, null, color, rotate, orig, scale, SpriteEffects.None, 0f);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
			return true;
        }
        public override bool CheckDead()
        {
			Main.PlaySound(SoundID.Item62, npc.Center);
			for (int i = 0; i < 80; i++)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>());
				Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 4f;
				Main.dust[dust].scale = 1f * Main.rand.Next(2, 3);
				Main.dust[dust].noGravity = true;
			}
			return base.CheckDead();
        }
        public override void NPCLoot()
        {
			symbol = null;
			Item.NewItem(npc.Hitbox, ItemID.FallenStar, 33);
        }
    }
}
