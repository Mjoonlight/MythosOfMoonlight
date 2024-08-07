using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using static System.Net.Mime.MediaTypeNames;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.Common.Globals;

namespace MythosOfMoonlight.Items.Accessories
{
    [AutoloadEquip(EquipType.Front)]
    public class ColdwindPendant : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MoMPlayer>().Coldwind = true;
        }
    }
    public class ColdwindPendantEffect : ModProjectile
    {
        public override string Texture => Helper.Placeholder;
        public override void SetDefaults()
        {
            Projectile.Size = Vector2.One;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile projectile = Projectile;
            Texture2D tex = Helper.GetExtraTex("Extra/polarConvertedLineGradation");
            Texture2D tex2 = Helper.GetExtraTex("Extra/circlething");
            Texture2D tex3 = ModContent.Request<Texture2D>("MythosOfMoonlight/Assets/Textures/Extra/crosslight").Value;
            Main.spriteBatch.Reload(BlendState.Additive);
            //Main.spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.White * (projectile.scale * projectile.scale * 2f), projectile.rotation, tex2.Size() / 2, 0.5f, SpriteEffects.None, 0);
            //Main.spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.White * (projectile.scale * projectile.scale * 2f), -projectile.rotation, tex2.Size() / 2, 0.5f, SpriteEffects.None, 0);

            float t = MathHelper.SmoothStep(0, 1, (MathF.Sin(Projectile.ai[0] * 0.05f + 4f) + 1) * 0.5f);
            //Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * (projectile.scale * t), -projectile.rotation * 0.5f * t, tex2.Size() / 2, 0.5f, SpriteEffects.None, 0);
            //Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * (projectile.scale * t), projectile.rotation * 0.5f * t, tex2.Size() / 2, 0.5f, SpriteEffects.None, 0);

            for (int i = 0; i < sparkles.Count; i++)
            {
                Sparkle d = sparkles[i];
                Main.spriteBatch.Draw(tex3, Projectile.Center + d.pos - Main.screenPosition, null, d.color * (d.scale * 10), 0, tex3.Size() / 2, d.scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        class Sparkle
        {
            public Vector2 pos;
            public Vector2 vel;
            public Color color;
            public float scale;
            public Sparkle(Vector2 _pos, Vector2 _vel, Color _color, float _scale)
            {
                pos = _pos;
                vel = _vel;
                color = _color;
                scale = _scale;
            }
        }
        List<Sparkle> sparkles = new List<Sparkle>(300);
        public static void DrawAll(SpriteBatch spriteBatch)
        {
            Texture2D tex = Helper.GetExtraTex("Extra/vortex_Inverse");
            Texture2D tex2 = Helper.GetExtraTex("Extra/circlething");
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.type == ModContent.ProjectileType<ColdwindPendantEffect>())
                {
                    //spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * (projectile.scale * projectile.scale), projectile.rotation, tex.Size() / 2, 0.35f, SpriteEffects.None, 0);
                    //spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * (projectile.scale * projectile.scale), -projectile.rotation, tex.Size() / 2, 0.15f, SpriteEffects.None, 0);

                    //spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.White * (projectile.scale * projectile.scale * 2), projectile.rotation, tex2.Size() / 2, 0.5f, SpriteEffects.None, 0);
                    //spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.White * (projectile.scale * projectile.scale * 2), -projectile.rotation, tex2.Size() / 2, 0.5f, SpriteEffects.None, 0);
                }
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player == null || !player.active || player.dead || ++Projectile.ai[0] > 5 * 60) return;
            Projectile.timeLeft = 2;
            Projectile.Center = player.Center;
            if (Projectile.ai[0] % 2 == 0)
            {
                sparkles.Add(new Sparkle(Main.rand.NextVector2Circular(100, 100), Main.rand.NextVector2Circular(2, 2), Color.Lerp(Color.LightCyan, Color.White, Main.rand.NextFloat()) * Main.rand.NextFloat(0.5f, 1), Main.rand.NextFloat(0.01f, 0.15f)));
                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(100, 100), ModContent.DustType<SparkleDust>(), Main.rand.NextVector2Circular(2, 2), 0, Color.Lerp(Color.LightCyan, Color.White, Main.rand.NextFloat()) * Main.rand.NextFloat(0.5f, 1), Main.rand.NextFloat(0.01f, 0.15f));

                for (int i = 0; i < 4; i++)
                {
                    Vector2 pos = Main.rand.NextVector2Circular(30, 30);
                    Dust d = Dust.NewDustPerfect(Projectile.Center + pos, ModContent.DustType<ColdwindDust>(), Helper.FromAToB(Projectile.Center + pos, Projectile.Center + pos.RotatedBy(MathHelper.Pi + -MathHelper.ToRadians(25))).RotatedByRandom(MathHelper.PiOver4 * 0.7f) * Main.rand.NextFloat(5, 8), 0, Color.Lerp(Color.LightCyan, Color.White, Main.rand.NextFloat()), Main.rand.NextFloat(0.25f, 0.7f));
                    d.customData = player.whoAmI;
                }
            }
            for (int i = 0; i < sparkles.Count; i++)
            {
                sparkles[i].pos += sparkles[i].vel;
                sparkles[i].scale -= 0.005f;
                sparkles[i].vel *= 0.95f;
                if (sparkles[i].scale <= 0)
                    sparkles.RemoveAt(i);
            }
            Projectile.rotation += MathHelper.ToRadians(Projectile.scale * 3);
            float progress = Utils.GetLerpValue(5 * 60, 0, Projectile.ai[0]);
            float t = MathHelper.SmoothStep(0, 1, (MathF.Sin(Projectile.ai[0] * 0.1f) + 1) * 0.025f);
            Projectile.scale = MathHelper.Clamp(MathF.Sin(progress * MathHelper.Pi) * 2f, 0, 0.5f + t);

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.CanBeChasedBy(this))
                {
                    if (npc.Center.Distance(Projectile.Center) < 150)
                    {
                        npc.GetGlobalNPC<MoMGlobalNPC>().coldwindCD += 2;
                        if (npc.GetGlobalNPC<MoMGlobalNPC>().coldwindCD > 0 && Projectile.ai[0] % 5 - (npc.GetGlobalNPC<MoMGlobalNPC>().coldwindCD < 100 ? 2 : 0) == 0)
                        {
                            Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<ColdwindDust>(), npc.velocity.X * Main.rand.NextFloat(), Main.rand.NextFloat(-5, -1), 0, default, Main.rand.NextFloat(0.3f, 0.6f));
                            if (Main.rand.NextBool())
                                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Frost, npc.velocity.X * Main.rand.NextFloat(), Main.rand.NextFloat(-5, -1), 0, default, Main.rand.NextFloat(.25f, .75f));
                        }
                    }
                }
            }
        }
    }
}
