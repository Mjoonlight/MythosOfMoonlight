using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using MythosOfMoonlight.Projectiles;
using Terraria.GameContent;
using Terraria.Audio;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Linq;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace MythosOfMoonlight.Items.Galactite
{
    public class Estrella : ModItem
    {
        public override void SetDefaults()
        {
            Item.knockBack = 10f;
            Item.width = Item.height = 66;
            Item.crit = 5;
            Item.damage = 24;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.channel = true;
            //Item.reuseDelay = 45;
            Item.DamageType = DamageClass.Melee;
            //Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<EstrellaP>();
        }
        int dir = 1;
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D tex = Helper.GetTex(Texture + "_Glow");
            spriteBatch.Reload(BlendState.Additive);
            spriteBatch.Draw(tex, Item.Center - Main.screenPosition, null, Color.White, rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);
            spriteBatch.Reload(BlendState.AlphaBlend);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            dir = -dir;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, dir);
            return false;
        }
    }
    public class EstrellaP : HeldSword
    {
        public override string Texture => "MythosOfMoonlight/Items/Galactite/Estrella";
        public override string GlowTexture => "MythosOfMoonlight/Items/Galactite/Estrella_Glow";
        public override void SetStaticDefaults()
        {
            Projectile.AddElement(CrossModHelper.Celestial);
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetExtraDefaults()
        {
            swingTime = 250;
            Projectile.Size = new(66);
            glowAlpha = 1f;
            BlendState _blendState = new BlendState();
            _blendState.AlphaSourceBlend = Blend.SourceAlpha;
            _blendState.AlphaDestinationBlend = Blend.InverseSourceAlpha;

            _blendState.ColorSourceBlend = Blend.SourceAlpha;
            _blendState.ColorDestinationBlend = Blend.InverseSourceAlpha;
            glowBlend = _blendState;
            Projectile.extraUpdates = 4;
        }
        public override float Ease(float x)
        {
            return (float)(x == 0
  ? 0
  : x == 1
  ? 1
  : x < 0.5 ? Math.Pow(2, 20 * x - 10) / 2
  : (2 - Math.Pow(2, -20 * x + 10)) / 2);
        }
        public override void ExtraAI()
        {
            Player player = Main.player[Projectile.owner];
            float rot = Projectile.rotation - MathHelper.PiOver4;
            Vector2 start = player.Center;
            Vector2 end = player.Center + rot.ToRotationVector2() * (Projectile.height + holdOffset * 0.25f);
            Vector2 offset = (Projectile.Size / 2) + ((Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * holdOffset * 0.25f);
            if (Projectile.ai[2].CloseTo(0.5f, 0.35f))
            {
                if (Projectile.timeLeft % 4 == 0)
                    for (int i = 0; i < 4; i++)
                        Dust.NewDustPerfect(Vector2.Lerp(start, end, Main.rand.NextFloat()), ModContent.DustType<PurpurineDust>(), Vector2.Zero).noGravity = true;

                for (float i = 0.1f; i < 4; i += 0.1f)
                {
                    Vector2 pos = Vector2.Lerp(start, end, i / 4);
                    Dust.NewDustPerfect(pos, ModContent.DustType<Starry>(), Helper.FromAToB(pos, player.Center + Helper.FromAToB(player.Center, pos, false).RotatedBy(-Projectile.ai[1] * 0.5f)) * 5, newColor: Color.Lerp(Color.Gray * 0.5f, Color.White, i / 4), Scale: (i / 3f) * 0.11f).noGravity = true;
                }
            }
        }
        public override void PreExtraDraw(float progress)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D tex = Helper.GetTex(Texture + "_Glow2");
            Main.spriteBatch.Reload(BlendState.Additive);


            float s = 1;
            if (Projectile.oldPos.Length > 2)
            {
                Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Textures/Extra/Extra_209");
                Texture2D tex3 = Helper.GetTex("MythosOfMoonlight/Textures/Extra/seamlessNoise");
                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[(Projectile.oldPos.Length - 1) * 6];
                //VertexPositionColorTexture[] vertices2 = new VertexPositionColorTexture[(Projectile.oldPos.Length - 1) * 6];
                if (Projectile.ai[2].CloseTo(0.5f, 0.3f))
                    for (int i = 0; i < Projectile.oldPos.Length - 1; i++)
                    {
                        if (Projectile.oldPos[i] != Vector2.Zero && Projectile.oldPos[i + 1] != Vector2.Zero)
                        {
                            Vector2 start = Projectile.oldPos[i];
                            Vector2 end = Projectile.oldPos[i + 1];
                            float num = Vector2.Distance(Projectile.oldPos[i], Projectile.oldPos[i + 1]);
                            Vector2 vector = (end - start) / num;

                            Color color = Color.Plum * s;
                            float off = 18.5f;
                            Vector2 offset = (Projectile.Size / 2) + ((Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * off);
                            Vector2 pos1 = Projectile.oldPos[i] + offset - Main.screenPosition;
                            Vector2 pos2 = Projectile.oldPos[i + 1] + offset - Main.screenPosition;
                            Vector2 dir1 = Helper.GetRotation(Projectile.oldPos.ToList(), i) * off * s;
                            Vector2 dir2 = Helper.GetRotation(Projectile.oldPos.ToList(), i + 1) * off * (s + i / (float)Projectile.oldPos.Length * 0.03f);
                            Vector2 v1 = pos1 + dir1;
                            Vector2 v2 = pos1 - dir1;
                            Vector2 v3 = pos2 + dir2;
                            Vector2 v4 = pos2 - dir2;
                            float p1 = i / (float)Projectile.oldPos.Length;
                            float p2 = (i + 1) / (float)Projectile.oldPos.Length;
                            vertices[i * 6] = Helper.AsVertex(v1, color, new Vector2(p1, Projectile.ai[1] != 1 ? 1 : 0));
                            vertices[i * 6 + 1] = Helper.AsVertex(v3, color, new Vector2(p2, Projectile.ai[1] != 1 ? 1 : 0));
                            vertices[i * 6 + 2] = Helper.AsVertex(v4, color, new Vector2(p2, Projectile.ai[1] == 1 ? 1 : 0));

                            vertices[i * 6 + 3] = Helper.AsVertex(v4, color, new Vector2(p2, Projectile.ai[1] == 1 ? 1 : 0));
                            vertices[i * 6 + 4] = Helper.AsVertex(v2, color, new Vector2(p1, Projectile.ai[1] == 1 ? 1 : 0));
                            vertices[i * 6 + 5] = Helper.AsVertex(v1, color, new Vector2(p1, Projectile.ai[1] != 1 ? 1 : 0));

                            //s -= i / (float)Projectile.oldPos.Length * 0.03f;
                        }
                    }
                //Helper.DrawTexturedPrimitives(vertices2, PrimitiveType.TriangleList, tex3);
                Helper.DrawTexturedPrimitives(vertices, PrimitiveType.TriangleList, tex2);
            }
            //DrawData data = new DrawData(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            //Helper.DrawWithDye(Main.spriteBatch, data, ItemID.TwilightDye, Projectile);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item1);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 25; i++)
                Helper.SpawnDust(Projectile.Center, Projectile.Size, ModContent.DustType<PurpurineDust>(), Projectile.velocity);
            if (Projectile.ai[0] < 3)
            {
                Projectile.ai[0]++;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + Main.rand.NextVector2Circular(target.width / 2, target.height / 2), Helper.FromAToB(Projectile.Center, target.Center), ModContent.ProjectileType<EstrellaPImpact>(), 0, 0, Projectile.owner, target.whoAmI);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center - Vector2.UnitY * 500, Helper.FromAToB(target.Center - Vector2.UnitY * 500, target.Center) * Main.rand.NextFloat(15, 25f), ModContent.ProjectileType<EstrellaP2>(), Projectile.damage, 0, Projectile.owner, target.whoAmI);
            }
        }
    }
    public class EstrellaP2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Type] = 4;
            Projectile.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 64;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 7; i++)
                Helper.SpawnDust(Projectile.Center, Projectile.Size, ModContent.DustType<PurpurineDust>(), Projectile.velocity * 0.3f);
            for (int i = 0; i < 7; i++)
                Helper.SpawnDust(Projectile.Center, Projectile.Size, ModContent.DustType<PurpurineDust>(), Projectile.velocity.Length() * 0.3f * Main.rand.NextVector2Unit());
            for (int i = 0; i < 7; i++)
                Helper.SpawnDust(Projectile.Center, Projectile.Size, 58, Projectile.velocity.Length() * 0.3f * Main.rand.NextVector2Unit(), dustModification: new Action<Dust>((d) => { d.color = Color.Violet; }));

        }
        float alpha = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex2 = TextureAssets.Projectile[Type].Value;
            Texture2D tex = Helper.GetTex(Texture + "Star");
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 66, 40, 66), Color.White * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0f);
            /*for (int i = 1; i < 5; i++)
            {
                float _scale = MathHelper.Lerp(1f, 0.95f, (float)(5 - i) / 5);
                var fadeMult = 1f / 5;
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, null, Color.White * (1f - fadeMult * i) * alpha, Projectile.oldRot[i], tex.Size() / 2, _scale, SpriteEffects.None, 0f);
            }*/

            Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(0, 12) - Main.screenPosition, null, Color.White * alpha * 0.75f, Projectile.rotation - MathHelper.PiOver2, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * alpha;
        }
        public override void AI()
        {
            Helper.SpawnDust(Projectile.position, Projectile.Size, ModContent.DustType<PurpurineDust>(), Projectile.velocity * 0.5f);
            if (Projectile.frameCounter++ % 3 == 0)
            {
                if (Projectile.frame < 3)
                    Projectile.frame++;
                else
                    Projectile.frame = 0;
            }
            if (Projectile.timeLeft > 100)
                Projectile.velocity = Projectile.velocity.Length() * Helper.FromAToB(Projectile.Center, Main.npc[(int)Projectile.ai[0]].Center);
            Projectile.rotation += MathHelper.ToRadians(3);
            if (Projectile.timeLeft < 20)
                alpha -= 0.05f;
        }
    }
    public class EstrellaPImpact : ModProjectile
    {
        public override string Texture => "MythosOfMoonlight/Textures/Extra/blank";
        public override void SetStaticDefaults()
        {
            Projectile.AddElement(CrossModHelper.Celestial);
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Helper.GetTex("MythosOfMoonlight/Textures/Extra/flare");
            Texture2D tex2 = Helper.GetTex("MythosOfMoonlight/Textures/Extra/star_05");
            Main.spriteBatch.Reload(BlendState.Additive);
            if (lightColor == Color.Transparent)
            {
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenLastPosition, null, Color.White * Projectile.scale * 2, Projectile.velocity.ToRotation(), tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0); ;
                Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenLastPosition, null, Color.White * Projectile.scale * 2, Main.GameUpdateCount * -0.025f, tex2.Size() / 2, Projectile.scale * 0.5f, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            float progress = Utils.GetLerpValue(0, 20, Projectile.timeLeft);
            Projectile.scale = MathHelper.Clamp((float)Math.Sin(progress * MathHelper.Pi) * 0.5f, 0, 0.5f);
        }
    }
}