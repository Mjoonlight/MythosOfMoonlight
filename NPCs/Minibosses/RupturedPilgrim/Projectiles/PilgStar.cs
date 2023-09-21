using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using MythosOfMoonlight.Dusts;
using Terraria.Audio;
using System.IO;
using Terraria.DataStructures;

namespace MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim.Projectiles
{
    internal class PilgStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starine Shaft");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
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
            Projectile.tileCollide = false;
            Projectile.hostile = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //Projectile.velocity = -oldVelocity;
            return false;
        }
        //public static NPC Sym => Starine_Symbol.symbol;
        //public NPC Sym = null;
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.localAI[0] = 1;
        }
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
                    Projectile.Kill();
                }
            }
            NPC Sym = Main.npc[sym];
            if (!Sym.active || Sym.ai[3] < 350)
            {
                Projectile.active = false;
                return;
            }
            //if (Projectile.velocity.Length() < 1 && Projectile.timeLeft < 250)
            //  Projectile.velocity = Helper.FromAToB(Projectile.Center, Sym.Center) * 5f;
            if (Vector2.Distance(Sym.Center, Projectile.Center) > 420)
            {
                if (Projectile.ai[1] == 0)
                {
                    if (Projectile.ai[0] == 0)
                    {
                        Projectile.ai[0] = 1;
                        Projectile.velocity = -Projectile.velocity.RotatedBy((MathHelper.ToRadians(18)));
                    }
                    else
                        Projectile.velocity = -Projectile.velocity.RotatedBy((MathHelper.ToRadians(36)));
                    Projectile.ai[1] = 1;
                    Projectile.ai[2]++;
                }
            }
            else
                Projectile.ai[1] = 0;
            if (Projectile.ai[2] >= 5)
                Projectile.Kill();
            if (Projectile.timeLeft < 100 || Projectile.ai[2] >= 4)
                Projectile.localAI[0] -= 0.05f;
            //if (Vector2.Distance(Sym.Center, Projectile.Center) > 425)
            //{
            //    Projectile.velocity = Helper.FromAToB(Projectile.Center, Sym.Center) * Projectile.velocity.Length();
            //}
            Projectile.rotation += MathHelper.ToRadians(3);
            if (Projectile.velocity.Length() < 20f)
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
                Main.dust[dust].velocity = Main.rand.NextVector2Unit() * 1.2f;
                Main.dust[dust].velocity.Y = -1.5f;
                Main.dust[dust].noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D drawTexture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle sourceRectangle = new(0, 0, drawTexture.Width, drawTexture.Height);
            Main.EntitySpriteDraw(drawTexture, Projectile.Center - Main.screenPosition, sourceRectangle, Color.White, Projectile.rotation, drawTexture.Size() / 2, 1, SpriteEffects.None, 0);

            //3hi31mg
            var off = new Vector2(Projectile.width / 2, Projectile.height / 2);
            var clr = new Color(255, 255, 255, 255); // full white
            var texture = TextureAssets.Projectile[Projectile.type].Value;
            //var frame = new Rectangle(0, Projectile.frame, Projectile.width, Projectile.height);
            var orig = Projectile.Size / 2f;
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
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}