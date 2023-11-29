using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Common.Systems;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles;
using MythosOfMoonlight.Projectiles.VFXProjectiles;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim
{
    public class Starine_Symbol : ModNPC
    {
        public static NPC symbol = null;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Symbol");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true, });
            //NPCID.Sets.NPCBestiaryDrawModifiers value = new(0) { Velocity = 1f, };
            // NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
        }
        public override Color? GetAlpha(Color drawColor) => Color.White;
        public override void SetDefaults()
        {
            NPC.width = 54;
            NPC.height = 64;
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
            NPC.noTileCollide = true;
        }
        private enum NState
        {
            Normal,
            Invulerable,
            Laser,
            Death,
            Despawn,
            Spawn
        }
        private NState State
        {
            get { return (NState)(int)NPC.ai[0]; }
            set { NPC.ai[0] = (int)value; }
        }
        public override bool NeedSaving()
        {
            return true;
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
        public static Vector2 _CircleCenter;
        float AnimSpeed = 1f;
        SoundStyle tp = new SoundStyle("MythosOfMoonlight/Assets/Sounds/pilgTP")
        {
            PitchVariance = 0.15f,
        };
        SoundStyle push = new SoundStyle("MythosOfMoonlight/Assets/Sounds/push")
        {
            PitchVariance = 0.15f,
        };
        public override void AI()
        {
            if (CircleCenter == Vector2.Zero)
                CircleCenter = TRay.Cast(NPC.Center - Vector2.UnitY * 70, Vector2.UnitY, 200) - Vector2.UnitY * 120;
            _CircleCenter = CircleCenter;
            symbol = NPC;
            /*if (Main.netMode == NetmodeID.Server)
            {
                NPC.netUpdate = true;
            }*/
            //Main.NewText(NPC.Center.Distance(Main.LocalPlayer.Center));
            if (Main.dayTime && State == NState.Normal)
                AnimSpeed = MathHelper.Lerp(AnimSpeed, 0.5f, 0.1f);
            else if (!Main.dayTime && State == NState.Normal)
                AnimSpeed = MathHelper.Lerp(AnimSpeed, 0.5f, 0.1f);

            if (State != NState.Despawn)
            {
                Lighting.AddLight(NPC.Center, 1f, 1f, 1f);
                FloatTimer++;
                if (CircleCenter != Vector2.Zero)
                {
                    if (State != NState.Laser)
                        NPC.velocity = (CircleCenter + new Vector2(0, 14 + 7f * (float)Math.Sin(MathHelper.ToRadians(FloatTimer * AnimSpeed))) - NPC.Center) / 15f;
                }
                if (State != NState.Normal && State != NState.Death && State != NState.Spawn)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<RupturedPilgrim>()) && SymbolTimer > 3)
                    {
                        StateTimer = 0;
                        SymbolTimer = 0;
                        State = NState.Despawn;
                        //Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<QuickFlare>(), 0, 0);
                        //NPC.active = false;
                    }
                }
                if (State != NState.Normal)
                    SymbolTimer++;


                switch (State)
                {
                    case NState.Normal:
                        NPC.dontTakeDamage = true;
                        break;
                    case NState.Spawn:
                        {
                            if (SymbolTimer == 1)
                            {
                                SoundEngine.PlaySound(push, NPC.Center);
                                CameraSystem.ChangeCameraPos(CircleCenter, 70, 1.25f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 7), Vector2.Zero, ModContent.ProjectileType<BigQuickFlare>(), 0, 0);
                                for (int i = 0; i < 15; i++)
                                {
                                    Dust.NewDustPerfect(NPC.Center, ModContent.DustType<StarineDust>(), Main.rand.NextVector2Circular(15, 15), Scale: Main.rand.NextFloat(0.7f, 1.2f));
                                    Dust.NewDustPerfect(NPC.Center, ModContent.DustType<StarineDustAlt>(), Main.rand.NextVector2Circular(25, 25), Scale: Main.rand.NextFloat(0.7f, 1f));
                                }
                                CameraSystem.ScreenShakeAmount = 10f;
                                foreach (Player player in Main.player)
                                {
                                    if (player.active)
                                        if (player.Center.Distance(CircleCenter) < 420f && player.velocity.Y > -1f)
                                        {
                                            player.Center -= new Vector2(0, 5);
                                            player.velocity += new Vector2(Helper.FromAToB(NPC.Center, player.Center).X * 10, -15);
                                        }
                                }
                            }
                            AnimSpeed = MathHelper.Lerp(AnimSpeed, 1.5f, 0.1f);
                            if (SymbolTimer == 60)
                            {
                                Vector2 pos = NPC.Center - new Vector2(0, Main.screenHeight);

                                for (int i = 0; i < Main.screenHeight / 7; i++)
                                {
                                    for (int j = 0; j < 2; j++)
                                        Dust.NewDustPerfect(pos, ModContent.DustType<StarineDustAlt>(), Main.rand.NextVector2Circular(5, 5), Scale: Main.rand.NextFloat(1.5f, 2)).noGravity = true;
                                    pos += Helper.FromAToB(pos, NPC.Center + new Vector2(0, -100), false) * 0.05f;
                                }
                                int pil = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 100, ModContent.NPCType<RupturedPilgrim>());
                                Main.npc[pil].ai[0] = 1;
                                Main.npc[pil].netUpdate = true;
                                SoundEngine.PlaySound(tp, NPC.Center);
                                CameraSystem.ScreenShakeAmount = 5f;
                                //if (GenericSystem.BeenThereDoneThatPilgrim)
                                {
                                    //SymbolTimer = 0;
                                    //State = NState.Invulerable;
                                }
                            }
                            //if (!GenericSystem.BeenThereDoneThatPilgrim)
                            {
                                if (SymbolTimer >= 360)
                                {
                                    SymbolTimer = 0;
                                    State = NState.Invulerable;
                                }
                            }
                        }
                        break;
                    case NState.Invulerable:
                        NPC.dontTakeDamage = true;
                        Radius = Math.Min(SymbolTimer * 3.5f, 420f);
                        break;
                    case NState.Laser:
                        StateTimer++;
                        NPC.velocity = (CircleCenter - NPC.Center) / 10f;
                        if (StateTimer == 10)
                        {
                            SoundEngine.PlaySound(SoundID.Item60, NPC.Center);
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                        }
                        SoundStyle style = SoundID.Item82;
                        style.Volume = 0.5f;
                        if (StateTimer == 40)
                        {
                            SoundEngine.PlaySound(style, NPC.Center);
                            float offset = 0;
                            for (int i = 0; i < (Main.expertMode ? 10 : 5); i++)
                            {
                                float angle = Helper.CircleDividedEqually(i, (Main.expertMode ? 10 : 5)) + offset;
                                Vector2 pos = NPC.Center + new Vector2(500, 0).RotatedBy(angle);
                                Vector2 vel = Helper.FromAToB(pos, CircleCenter) * 0.1f;
                                Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), pos, vel, ModContent.ProjectileType<StarineShaft>(), 10, 0);
                                a.tileCollide = false;
                                a.aiStyle = 0;
                                a.timeLeft = 110;
                            }
                        }
                        if (StateTimer == 50)
                        {
                            SoundEngine.PlaySound(style, NPC.Center);
                            float offset = MathHelper.PiOver4 / 2;
                            for (int i = 0; i < (Main.expertMode ? 10 : 5); i++)
                            {
                                float angle = Helper.CircleDividedEqually(i, (Main.expertMode ? 10 : 5)) + offset;
                                Vector2 pos = NPC.Center + new Vector2(600, 0).RotatedBy(angle);
                                Vector2 vel = Helper.FromAToB(pos, CircleCenter) * 0.1f;
                                Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), pos, vel, ModContent.ProjectileType<StarineShaft>(), 10, 0);
                                a.tileCollide = false;
                                a.aiStyle = 0;
                                a.ai[0] = -60;
                                a.timeLeft = 115;
                            }
                        }
                        if (StateTimer == 60)
                        {
                            SoundEngine.PlaySound(style, NPC.Center);
                            float offset = MathHelper.PiOver4;
                            for (int i = 0; i < (Main.expertMode ? 10 : 5); i++)
                            {
                                float angle = Helper.CircleDividedEqually(i, (Main.expertMode ? 10 : 5)) + offset;
                                Vector2 pos = NPC.Center + new Vector2(700, 0).RotatedBy(angle);
                                Vector2 vel = Helper.FromAToB(pos, CircleCenter) * 0.1f;
                                Projectile a = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), pos, vel, ModContent.ProjectileType<StarineShaft>(), 10, 0);
                                a.tileCollide = false;
                                a.aiStyle = 0;
                                a.ai[0] = -120;
                                a.timeLeft = 120;
                            }
                        }
                        if (StateTimer == 70)
                        {
                            //SoundEngine.PlaySound(SoundID., NPC.Center);
                            for (int i = 4; i <= 360; i += 4)
                            {
                                Vector2 dVel = MathHelper.ToRadians(i).ToRotationVector2() * 6f;
                                Dust dust = Dust.NewDustDirect(NPC.Center, 1, 1, ModContent.DustType<StarineDust>(), dVel.X, dVel.Y);
                                dust.noGravity = true;
                            }
                            StateTimer = 0;
                            SwitchTo(NState.Invulerable);
                        }
                        /*if (StateTimer == 120)
                        {
                            for (int i = 90; i <= 360; i += 90)
                            {
                                Vector2 shoot = MathHelper.ToRadians(i + 45).ToRotationVector2();
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 15), shoot, ModContent.ProjectileType<TestTentacleProj>(), 8, .1f, Main.myPlayer);
                            }
                            StateTimer = 0;
                        }*/
                        break;
                    case NState.Death:
                        Radius -= 3.5f;
                        if (Radius <= 0f)
                            StateTimer++;

                        if (StateTimer > 0)
                            NPC.velocity = Vector2.Lerp(NPC.velocity, CircleCenter + new Vector2(Main.rand.NextFloat(-25 + (Radius * 0.025f), 25 - (Radius * 0.025f)), Main.rand.NextFloat(-25 + (Radius * 0.025f), 25 - (Radius * 0.025f))) - NPC.Center, 0.2f);

                        if (StateTimer == 120)
                        {
                            NPC.life = 0;
                            NPC.checkDead();
                        }
                        break;
                }
                if (Radius >= 400)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && !npc.friendly && !npc.boss)
                        {
                            if (npc.Distance(NPC.Center) < 450)
                            {
                                npc.Center += Helper.FromAToB(NPC.Center, npc.Center) * 10;
                                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<StarineDust>());
                                dust.noGravity = true;
                                dust.velocity = Helper.FromAToB(NPC.Center, npc.Center) * 10;
                            }
                        }
                    }
                }
                if (Vector2.Distance(NPC.Center, Main.LocalPlayer.Center) <= 1000f)
                {
                    if (Radius >= 400 && Vector2.Distance(CircleCenter, Main.LocalPlayer.Center) > 420)
                    {
                        Vector2 vel = Utils.SafeNormalize(CircleCenter - Main.LocalPlayer.Center, Vector2.Zero);
                        Main.LocalPlayer.Center += vel * 9f;
                        Main.LocalPlayer.velocity = vel * 1.25f;
                        //Main.LocalPlayer.itemTime = Main.LocalPlayer.HeldItem.useTime - 2;
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
            else
            {
                Star.starfallBoost = 0;
                Radius -= 7.5f;
                if (Radius < 10 && StateTimer == 0)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<QuickFlare>(), 0, 0);
                    StateTimer = 1;
                }
                if (StateTimer != 0)
                    StateTimer++;
                if (StateTimer > 10)
                    NPC.active = false;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 19)
                NPC.frameCounter = 0;

            NPC.frame.Y = (int)(NPC.frameCounter / 5) * NPC.height;
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (!Main.dayTime)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<RupturedPilgrim>()) && State != NState.Death)
                {
                    NPC.townNPC = false;
                    SwitchTo(NState.Spawn);
                }
            }
            else
            {
                //Utils.DrawBorderString(Main.spriteBatch, "Please come at nighttime!", NPC.Center - new Vector2(0, 30), Color.Cyan, 2f);
            }
        }
        public override bool CanChat()
        {
            return State == NState.Normal;
        }
        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (!Main.dayTime)
            {
                button = "Challenge";
            }
            Main.LocalPlayer.currentShoppingSettings.HappinessReport = "";
        }
        public override string GetChat()
        {
            return "It shimmers intensely.\n" +
                "Something may come if you disturb it.";
        }
        public override bool UsesPartyHat()
        {
            return false;
        }
        float rotate;
        float rotateSpeed;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (State != NState.Normal)
            {
                float scale = Radius / 280f;

                if (State != NState.Despawn)
                {
                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && npc.type == ModContent.NPCType<RupturedPilgrim>() && npc.ai[0] != 11)
                        {
                            rotateSpeed = (float)((float)npc.life / (float)npc.lifeMax);
                            //rotate = MathHelper.Lerp(rotate, MathHelper.ToRadians(SymbolTimer * (0.95f + Utils.GetLerpValue(npc.lifeMax, 0, npc.life) * 0.25f)), 0.1f);
                        }
                    }
                    //rotate = MathHelper.Lerp(rotate, MathHelper.ToRadians(SymbolTimer * 0.95f), 0.1f);
                }
                rotate += MathHelper.ToRadians(3 - rotateSpeed * 2);
                Vector2 orig = new(420, 420);
                Color color = Color.White * scale;
                Texture2D tex = ModContent.Request<Texture2D>("MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Starine_Barrier").Value;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                spriteBatch.Draw(tex, CircleCenter - screenPos, null, color * MathHelper.Clamp(AnimSpeed, 0, 1f), rotate, orig, scale, SpriteEffects.None, 0f);
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
