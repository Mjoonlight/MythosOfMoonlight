using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Items.Galactite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Projectiles;
using static tModPorter.ProgressUpdate;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;

namespace MythosOfMoonlight.Items.Weapons.Melee
{
    public class Kristellation : ModItem
    {
        public override void SetDefaults()
        {
            Item.knockBack = 5f;
            Item.width = Item.height = 66;
            Item.crit = 5;
            Item.damage = 7;
            Item.useAnimation = 2;
            Item.useTime = 2;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<KristellationP>();
        }
        int dir = 1;
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D tex = Helper.GetTex(Texture + "P_Glow");
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
    public class KristellationP : HeldSword
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.DontCancelChannelOnKill[Type] = true;
        }
        private List<float> rots;

        public int len;
        public Color drawColor;
        public Vector2 pos;
        public float rot;
        public override void SetExtraDefaults()
        {
            swingTime = 28;
            Projectile.extraUpdates = 2;
            Projectile.Size = new(48);
            swingArcModifier = 0f;
            holdOffset = 20;
            startRotOffset = MathHelper.ToRadians(Main.rand.NextFloat(-6, 6));
            rots = new List<float>();
            len = 0;
            swirlinessMod = Main.rand.NextFloat(0.05f, 0.15f) * (Main.rand.NextFloatDirection() > 0 ? 1 : -1);
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
        float value;
        float totalLength = 30, swirliness = 2f, swirlinessMod = 0.1f;
        public float TentacleCounter
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D glow = Helper.GetTex(Texture + "_Glow");
            Texture2D tex = Helper.GetTex(Texture);

            Helper.SaveCurrent(Main.spriteBatch);
            List<VertexInfo2> bars = new List<VertexInfo2>();
            Vector2 p = Projectile.Center - new Vector2(16, -14).RotatedBy(Projectile.rotation);
            for (int i = 1; i < len; i++)
            {
                float factor = (float)i / (float)len;
                Vector2 v0 = p + Utils.RotatedBy(new Vector2((float)(5 * (i - 1)), 0f), rots[i - 1]);
                Vector2 v1 = p + Utils.RotatedBy(new Vector2((float)(5 * i), 0f), rots[i]);
                Vector2 normaldir = v1 - v0;
                normaldir = new Vector2(normaldir.Y, 0f - normaldir.X);
                ((Vector2)(normaldir)).Normalize();
                float w = 10 * MathHelper.SmoothStep(0.8f, 0.1f, factor);
                bars.Add(new VertexInfo2(v1 + w * normaldir, new Vector3(factor, 0f, 0f), drawColor));
                bars.Add(new VertexInfo2(v1 - w * normaldir, new Vector3(factor, 1f, 0f), drawColor));
            }
            if (bars.Count > 2)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                Matrix projection = Matrix.CreateOrthographicOffCenter(0f, (float)Main.screenWidth, (float)Main.screenHeight, 0f, 0f, 1f);
                Matrix model = Matrix.CreateTranslation(new Vector3(0f - Main.screenPosition.X, 0f - Main.screenPosition.Y, 0f)) * Main.GameViewMatrix.ZoomMatrix;
                MythosOfMoonlight.Tentacle.Parameters[0].SetValue(model * projection);
                MythosOfMoonlight.Tentacle.CurrentTechnique.Passes[0]
                    .Apply();
                ((Game)Main.instance).GraphicsDevice.Textures[0] = (Texture)(object)ModContent.Request<Texture2D>("MythosOfMoonlight/Textures/Extra/Ex2", (AssetRequestMode)2).Value;
                ((Game)Main.instance).GraphicsDevice.DrawUserPrimitives<VertexInfo2>((PrimitiveType)1, bars.ToArray(), 0, bars.Count - 2);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
            }
            Helper.ApplySaved(Main.spriteBatch);

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            //if (TentacleCounter > -1)
            //  Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (TentacleCounter > -1)
            {
                TentacleCounter = 0;
                Projectile.localAI[1] = 1;
            }
        }
        void UpdateTentacle()
        {
            int timeLeft = (TentacleCounter == -1 ? 30 : 14);
            drawColor = new Color(0, 230, 230) * (Projectile.timeLeft > 7 ? 1 : (float)(Projectile.timeLeft / 7f));
            for (int i = 0; i < 3; i++)
            {
                value += swirliness;
                if (Projectile.timeLeft % 1 == 0)
                {
                    float factor = 1f;
                    Vector2 velocity = Projectile.velocity * factor * 4f;
                    rot = swirlinessMod * (float)Math.Sin((double)(value / 60f)) + velocity.ToRotation();
                    rots.Insert(0, rot);
                    while (rots.Count > totalLength)
                    {
                        rots.RemoveAt(rots.Count - 1);
                    }
                }
                if (len < totalLength && Projectile.timeLeft > timeLeft)
                {
                    len++;
                }
                if (len >= 0 && Projectile.timeLeft <= timeLeft)
                {
                    len--;
                }
            }
        }
        public override void ExtraAI()
        {
            if (TentacleCounter == -1)
            {
                swirliness = Main.rand.NextFloat(1.5f, 3f);
                totalLength = 70;
            }
            else
            {
                swirliness = Main.rand.NextFloat(3f, 6f);
                totalLength = 30;
            }
            UpdateTentacle();
            if (swingTime < Projectile.timeLeft)
            {
                swingArcModifier = 0.01f;
                swingTime = Projectile.timeLeft;
            }
            float swingProgress = Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft);
            if (swingProgress < 0.5f)
                holdOffset = MathHelper.Lerp(14, 30, swingProgress * 2);
            else
                holdOffset = MathHelper.Lerp(30, 14, (swingProgress - 0.5f) * 2);
            if (holdOffset > 23)
                stretch = Player.CompositeArmStretchAmount.Full;
            else if (holdOffset <= 23 && holdOffset > 12)
                stretch = Player.CompositeArmStretchAmount.Quarter;
            else
                stretch = Player.CompositeArmStretchAmount.None;
            Player player = Main.player[Projectile.owner];

            if (Projectile.timeLeft < 5)
            {
                if (player.active && player.channel && !player.dead && !player.CCed && !player.noItems)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center);
                        Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, dir, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, 0, (-Projectile.ai[1]));
                        proj.rotation = Projectile.rotation;
                        proj.Center = Projectile.Center;
                        if (++TentacleCounter > 10 || Projectile.localAI[1] == 1)
                            TentacleCounter = -1;
                        if (TentacleCounter == -1)
                        {
                            proj.timeLeft = 60;
                        }
                        proj.localAI[0] = TentacleCounter;
                        proj.localAI[1] = 0;
                        Projectile.active = false;
                    }
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 1; i < len - 5; i++)
            {
                float factor = (float)i / (float)len;
                float w = 10 * MathHelper.SmoothStep(0.8f, 0.1f, factor);
                if (Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - new Vector2(w, w) + Utils.RotatedBy(new Vector2((float)(5 * i), 0f), rots[i]), new Vector2(w, w) * 2f))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
