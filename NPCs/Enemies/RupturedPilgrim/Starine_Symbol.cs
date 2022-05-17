using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.NPCs.Enemies.RupturedPilgrim.Projectiles;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
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
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
            {
                ImmuneToAllBuffsThatAreNotWhips = true
            });

            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1f, };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.width = 54;
            NPC.height = 62;
            NPC.aiStyle = -1;
            NPC.damage = 1;
            NPC.defense = 2;
            NPC.lifeMax = 1000;
            NPC.friendly = true;
            NPC.rarity = 4;
            NPC.HitSound = SoundID.NPCHit19;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
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
            get { return (NState)(int)NPC.ai[0]; }
            set { NPC.ai[0] = (int)value; }
        }
        private void SwitchTo(NState state)
        {
            State = state;
        }
        public float SymbolTimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float StateTimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float Radius
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
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
            Lighting.AddLight(NPC.Center, 1f, 1f, 1f);
            FloatTimer++;
            if (CircleCenter != Vector2.Zero)
            {
                if (State != NState.Laser)
                    NPC.velocity = (CircleCenter + new Vector2(0, 20f * (float)Math.Sin(MathHelper.ToRadians(FloatTimer))) - NPC.Center) / 15f;
            }
            if (State != NState.Normal)
                SymbolTimer++;

            if (CircleCenter == Vector2.Zero)
                CircleCenter = NPC.Center - new Vector2(0, 36);

            switch (State)
            {
                case NState.Normal:
                    break;
                case NState.Invulerable:
                    NPC.dontTakeDamage = true;
                    Radius = Math.Min(SymbolTimer * 3.5f, 420f);
                    break;
                case NState.Laser:
                    StateTimer++;
                    NPC.velocity = (CircleCenter - NPC.Center) / 10f;
                    if (StateTimer == 30)
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                        for (int i = 4; i <= 360; i += 4)
                        {
                            Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                            Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                            dust.noGravity = true;
                        }
                    }
                    if (StateTimer == 60)
                    {
                        for (int i = 90; i <= 360; i += 90)
                        {
                            Vector2 shoot = MathHelper.ToRadians(i).ToRotationVector2();
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 15), shoot, ModContent.ProjectileType<TestTentacleProj>(), 8, .1f, Main.myPlayer);
                        }
                    }
                    if (StateTimer == 90)
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit5, NPC.Center);
                        for (int i = 4; i <= 360; i += 4)
                        {
                            Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                            Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                            dust.noGravity = true;
                        }
                    }
                    if (StateTimer == 120)
                    {
                        for (int i = 90; i <= 360; i += 90)
                        {
                            Vector2 shoot = MathHelper.ToRadians(i + 45).ToRotationVector2();
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 15), shoot, ModContent.ProjectileType<TestTentacleProj>(), 8, .1f, Main.myPlayer);
                        }
                        StateTimer = 0;
                        SwitchTo(NState.Invulerable);
                    }
                    break;
                case NState.Death:
                    Radius -= 3.5f;
                    if (Radius <= 0f)
                        StateTimer++;

                    if (StateTimer > 0)
                        NPC.velocity = CircleCenter + new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6)) - NPC.Center;

                    if (StateTimer == 120)
                    {
                        NPC.life = 0;
                        NPC.checkDead();
                    }
                    break;
            }
            if (Vector2.Distance(NPC.Center, Main.LocalPlayer.Center) <= 1000f)
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
            NPC.frameCounter++;
            if (NPC.frameCounter >= 19)
                NPC.frameCounter = 0;

            NPC.frame.Y = (int)(NPC.frameCounter / 5) * NPC.height;
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (!Main.dayTime)
            {
                NPC.townNPC = false;
                int pil = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 200, ModContent.NPCType<RupturedPilgrim>());
                Main.npc[pil].ai[0] = 6;
                if (symbol == null || !symbol.active)
                    symbol = NPC;

                SwitchTo(NState.Invulerable);
            }
            else
            {
                Utils.DrawBorderString(Main.spriteBatch, "Please come at nighttime!", NPC.Center - new Vector2(0, 30), Color.Cyan, 2f);
            }
        }
        public override void OnHitByProjectile(Projectile Projectile, int damage, float knockback, bool crit)
        {
            SwitchTo(NState.Invulerable);
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (!Main.dayTime)
            {
                button = "Challenge";
            }
        }
        public override string GetChat()
        {
            return "I will never leave you alone.";
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (State != NState.Normal)
            {
                float scale = Radius / 420f;
                float rotate = MathHelper.ToRadians(SymbolTimer * 2f);
                Vector2 orig = new(420, 422);
                Color color = Color.White * scale;
                Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/NPCs/Enemies/RupturedPilgrim/Starine_Barrier").Value;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                spriteBatch.Draw(tex, CircleCenter - screenPos, null, color, rotate, orig, scale, SpriteEffects.None, 0f);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            return true;
        }
        public override bool CheckDead()
        {
            SoundEngine.PlaySound(SoundID.Item62, NPC.Center);
            for (int i = 0; i < 80; i++)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<StarineDust>());
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 4f;
                Main.dust[dust].scale = 1f * Main.rand.Next(2, 3);
                Main.dust[dust].noGravity = true;
            }
            return base.CheckDead();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.FallenStar, 1, 33, 33));
        }
    }
}
