using MythosOfMoonlight.BaseClasses.BaseProj;
using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Items.Weapons;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace MythosOfMoonlight.Projectiles
{
    public class FireflyMinion : BaseMinion
    {
        public float fireflyTimer = 0;
        public override void SetStaticDefaults()
        {
            StaticDefaults(2, 12, 2);
        }
        public override void SetDefaults()
        {
            Defaults(16, 16, 16, -1, false, 2, 0);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            rotateRadian = float.NaN;
            fireflyTimer = 0;
        }
        public float rotateRadian;
        public Vector2 desirePos;
        public Vector2 targetPos;
        public float dashTimer;
        public int drawLayer
        {
            get
            {
                return (int)(Math.Sin(rotateRadian) * 3);
            }
        }
        public enum State
        {
            rotate,
            dash,
            back
        }
        public State state
        {
            get => (State)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }
        Vector2 baseMousePos;
        bool yes;
        public override void AI()
        {
            if (!yes)
            {
                baseMousePos = Main.MouseWorld;
                yes = true;
            }
            fireflyTimer++;
            if (fireflyTimer >= 99999) fireflyTimer = 0;
            Projectile.frame = fireflyTimer % 20 < 10 ? 0 : 1;
            Glow(new Vector3(.45f, .45f, .15f));
            Player owner = Main.player[Projectile.owner];
            OwnerCheckMinions(3, (float)ModContent.ItemType<FireflyStick>());
            MinionSort();
            if (owner.ownedProjectileCounts[Type] > 0)
            {
                if (fireflyTimer > 0)
                {
                    if (rotateRadian is float.NaN)
                    {
                        rotateRadian = MathHelper.ToRadians((360f / owner.ownedProjectileCounts[Type]) * MinionOrderNum);
                    }
                    else rotateRadian += MathHelper.ToRadians(-owner.direction);
                    switch (state)
                    {
                        case State.rotate:
                            {
                                baseMousePos = Main.MouseWorld;
                                desirePos = owner.Center;
                                desirePos += rotateRadian.ToRotationVector2() * new Vector2(70, 15);
                                Projectile.velocity = desirePos - Projectile.Center;
                                break;
                            }
                        case State.dash:
                            {
                                targetPos = baseMousePos;
                                if (Vector2.Distance(targetPos, owner.Center) > 600f)
                                {
                                    targetPos = Vector2.Normalize(targetPos - owner.Center) * 600f + owner.Center;
                                }
                                dashTimer++;
                                Vector2 dashProgress = ((MathHelper.ToRadians(dashTimer * 3).ToRotationVector2() * (Vector2.Distance(targetPos, desirePos) / 2) - new Vector2(Vector2.Distance(targetPos, desirePos) / 2, 0)) * new Vector2(1, .33f)).RotatedBy((desirePos - targetPos).ToRotation());
                                Projectile.velocity = desirePos + dashProgress - Projectile.Center;
                                if (dashTimer > 60)
                                {
                                    dashTimer = 0;
                                    state = State.back;
                                }
                                break;
                            }
                        case State.back:
                            {
                                desirePos = owner.Center;
                                desirePos += rotateRadian.ToRotationVector2() * new Vector2(70, 15);
                                dashTimer++;
                                Vector2 dashProgress = ((MathHelper.ToRadians(dashTimer * 3 + 180).ToRotationVector2() * (Vector2.Distance(targetPos, desirePos) / 2) + new Vector2(Vector2.Distance(targetPos, desirePos) / 2, 0)) * new Vector2(1, .33f)).RotatedBy((desirePos - targetPos).ToRotation());
                                Projectile.velocity = targetPos + dashProgress - Projectile.Center;
                                if (dashTimer > 60 || Vector2.Distance(Projectile.Center, desirePos) <= 32)
                                {
                                    dashTimer = 0;
                                    state = State.rotate;
                                }
                                break;
                            }
                    }
                }
            }
        }
        public override bool? CanDamage()
        {
            return state != State.rotate;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (state == State.dash)
            {
                targetPos = Projectile.Center;
                dashTimer = 0;
                state = State.back;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float light = Math.Max(0, -Math.Abs(60 - fireflyTimer % 120) + 20) / 15f;
            Texture2D tex = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture).Value;
            Texture2D glow = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Glow").Value;
            SpriteEffects effect = Projectile.velocity.X >= 0 ? SpriteEffects.None : SpriteEffects.FlipVertically; List<VertexInfo2> vertices = new();
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 ori = Projectile.Size / 2;
            Texture2D trail = ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/White").Value;
            if (state != State.rotate)
            {
                for (int j = 0; j <= Math.Min(16 - Projectile.timeLeft, 11); j += 1)
                {
                    if (Projectile.oldPos[j] != Vector2.Zero)
                    {
                        vertices.Add(new VertexInfo2(
                            Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] + MathHelper.PiOver2).ToRotationVector2() * 5f,
                            new Vector3(j / (float)(Math.Min(16 - Projectile.timeLeft, 11) + 1), 0, 1 - (j / (float)(Math.Min(300 - Projectile.timeLeft, 11) + 1))),
                            new Color(.1f, .1f, .033f) * (1 - (j / (float)(Math.Min(16 - Projectile.timeLeft, 11) + 1)))));
                        vertices.Add(new VertexInfo2(
                            Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] - MathHelper.PiOver2).ToRotationVector2() * 5f,
                            new Vector3(j / (float)(Math.Min(16 - Projectile.timeLeft, 11) + 1), .25f, 1 - (j / (float)(Math.Min(16 - Projectile.timeLeft, 11) + 1))),
                            new Color(.66f, .66f, .22f) * (1 - (j / (float)(Math.Min(16 - Projectile.timeLeft, 11) + 1)))));
                    }
                }
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Main.graphics.GraphicsDevice.Textures[0] = trail;
                if (vertices.Count >= 3)
                {
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
                }
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 16, 16, 16), lightColor, Projectile.rotation, new Vector2(8), 1f, effect, 0);
            Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 16, 16, 16), Color.White * light, Projectile.rotation, new Vector2(8), 1f, effect, 0);
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            switch (drawLayer)
            {
                case -2:
                    {
                        behindNPCsAndTiles.Add(index);
                        break;
                    }
                case -1:
                    {
                        behindNPCs.Add(index);
                        break;
                    }
                case 0:
                    {
                        behindProjectiles.Add(index);
                        break;
                    }
                case 1:
                    {
                        overPlayers.Add(index);
                        break;
                    }
                case 2:
                    {
                        overWiresUI.Add(index);
                        break;
                    }
            }
        }
    }
}
