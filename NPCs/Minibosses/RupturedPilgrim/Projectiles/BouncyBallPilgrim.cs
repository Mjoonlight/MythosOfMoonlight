using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.ComponentModel.DataAnnotations.Schema;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    public class BouncyBallPilgrim : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/NPCs/Minibosses/RupturedPilgrim/Projectiles/PilgStar";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Shaft");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Projectile.AddElement(CrossModHelper.Celestial);
            Projectile.AddElement(CrossModHelper.Arcane);
        }
        public override void SetDefaults()
        {
            Projectile.damage = 10;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 360;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.hostile = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Math.Abs(Projectile.velocity.X - Projectile.oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -Projectile.oldVelocity.X;
            }

            if (Math.Abs(Projectile.velocity.Y - Projectile.oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -Projectile.oldVelocity.Y;
            }
            _scaleFactor = 1;
            swingBack = 30;

            SoundEngine.PlaySound(SoundID.Item154, Projectile.Center);
            /*if (Projectile.ai[0] == 1 && Projectile.ai[1] == 0)
                for (int i = 0; i < 3; i++)
                {
                    float angle = Helper.CircleDividedEqually(i, 3) + MathHelper.PiOver4;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.RotatedBy(angle) * 3, ModContent.ProjectileType<PilgStar3>(), 20, 0, ai2: 1);
                }*/
            Projectile.ai[2]++;
            return false;
        }
        //public static NPC Sym => Starine_Symbol.symbol;
        //public NPC Sym = null;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.localAI[0] = 1;
        }
        bool hasBounced;
        float Ease(float x)
        {
            float n1 = 7.5625f;
            float d1 = 2.75f;

            if (x < 1 / d1)
            {
                return n1 * x * x;
            }
            else if (x < 2 / d1)
            {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            }
            else if (x < 2.5 / d1)
            {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            }
            else
            {
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
            }
        }
        float swingBack;
        public override void AI()
        {
            /*if (Projectile.ai[0] == 1)
            {*/
            if (Projectile.timeLeft == 599)
                Projectile.localAI[0] = 1;
            int sym = 0;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == ModContent.NPCType<Starine_Symbol>())
                    sym = npc.whoAmI;

                if (npc.active && npc.type == ModContent.NPCType<RupturedPilgrim>() && npc.immortal)
                {
                    //     Projectile.Kill();
                }
            }
            NPC Sym = Main.npc[sym];
            if (!Sym.active || Sym.ai[3] < 350)
            {
                //   Projectile.active = false;
                return;
            }
            //if (Projectile.velocity.Length() < 1 && Projectile.timeLeft < 250)
            //  Projectile.velocity = Helper.FromAToB(Projectile.Center, Sym.Center) * 5f;
            /*if (swingBack > 0)
                swingBack--;
            float progress = Ease(Utils.GetLerpValue(0f, 30, swingBack));
            */
            scaleFactor = Vector2.Lerp(Vector2.One, Vector2.Lerp(new Vector2(1, 0.5f), new Vector2(0.5f, 1f), (MathF.Sin(Main.GlobalTimeWrappedHourly * 10) + 1) * 0.5f), _scaleFactor);
            _scaleFactor = MathHelper.Lerp(_scaleFactor, 0, 0.025f);
            if (Starine_Symbol._CircleCenter != Vector2.Zero)
                if (Vector2.Distance(Starine_Symbol._CircleCenter, Projectile.Center) > 370)
                {
                    if (Projectile.ai[1] == 0)
                    {
                        _scaleFactor = 1;
                        swingBack = 30;
                        Projectile.velocity = -Projectile.oldVelocity;

                        SoundEngine.PlaySound(SoundID.Item154, Projectile.Center);
                        /*if (Projectile.ai[0] == 1 && Projectile.ai[1] == 0)
                            for (int i = 0; i < 3; i++)
                            {
                                float angle = Helper.CircleDividedEqually(i, 3) + MathHelper.PiOver4;
                                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.RotatedBy(angle) * 3, ModContent.ProjectileType<PilgStar3>(), 20, 0, ai2: 1);
                            }*/
                        Projectile.ai[1] = 1;
                        Projectile.ai[2]++;
                    }
                }
                else
                    Projectile.ai[1] = 0;
            if (Projectile.ai[2] >= 4 + (Projectile.ai[0] != 1 ? 2 : 0))
                Projectile.Kill();
            if (Projectile.timeLeft < 100 || Projectile.ai[2] >= 4)
                Projectile.localAI[0] -= 0.05f;
            //if (Vector2.Distance(Sym.Center, Projectile.Center) > 425)
            //{
            //    Projectile.velocity = Helper.FromAToB(Projectile.Center, Sym.Center) * Projectile.velocity.Length();
            //}
            Projectile.rotation += MathHelper.ToRadians(3);
            if (Projectile.velocity.Length() < 5.5f)
                Projectile.velocity *= 1.1f;
            /*}
                else
                {
                    foreach (Projectile projectile in Main.projectile)
                    {
                        if (projectile.active && projectile.type == Projectile.type && projectile.whoAmI != Projectile.whoAmI && projectile.ai[0] == 1)
                        {
                            Projectile.velocity = projectile.oldPos[(int)Projectile.ai[0] * 5];
                        }
                    }
                }*/
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarineDust>(), 2f);
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
                Main.dust[dust].noGravity = true;
            }
            float offset = MathHelper.Pi;
            for (int i = 0; i < 3; i++)
            {
                float angle = Helper.CircleDividedEqually(i, 3) + offset + MathHelper.PiOver4;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitX.RotatedBy(angle) * 3, ModContent.ProjectileType<PilgStar3>(), 10, 0, ai2: 1);
            }
            SoundEngine.PlaySound(SoundID.Item167, Projectile.Center);
        }
        Vector2 scaleFactor = Vector2.One;
        float _scaleFactor;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D drawTexture2 = Helper.GetTex("MythosOfMoonlight/Textures/Extra/circle_01");
            Rectangle sourceRectangle = new(0, 0, drawTexture.Width, drawTexture.Height);
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, sourceRectangle, Color.White * 0.8f, Projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);

            //3hi31mg
            var off = new Vector2(drawTexture.Width / 2, drawTexture.Height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = TextureAssets.Projectile[Projectile.type].Value;
            //var frame = new Rectangle(0, Projectile.frame, Projectile.width, Projectile.height);
            var orig = drawTexture.Size() / 2f;
            var trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 1; i < trailLength; i++)
            {
                float scale = MathHelper.Lerp(0.70f, 1f, (float)(trailLength - i) / trailLength);
                var fadeMult = 1f / trailLength;
                SpriteEffects flipType = Projectile.spriteDirection == -1 /* or 1, idfk */ ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] - Main.screenPosition + off, null, clr * (1f - fadeMult * i) * Projectile.localAI[0] * 0.75f, Projectile.oldRot[i], orig, scale, flipType, 0f);
            }

            Main.EntitySpriteDraw(drawTexture2, Projectile.Center - Main.screenPosition, null, Color.Cyan, 0, drawTexture2.Size() / 2, scaleFactor * 0.2f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
