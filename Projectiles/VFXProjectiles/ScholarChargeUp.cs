using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using MythosOfMoonlight.NPCs.Minibosses;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.Projectiles.VFXProjectiles
{
    public class ScholarChargeUp : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 1;
            Projectile.width = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 50;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? CanDamage() => false;
        int seed;
        public override bool PreDraw(ref Color lightColor)
        {

            if (seed == 0) seed = Main.rand.Next(int.MaxValue / 2);
            Texture2D tex = Helper.GetExtraTex("Extra/cone5");
            Texture2D tex2 = Helper.GetExtraTex("Extra/flare_01");
            float max = 35;
            Main.spriteBatch.Reload(BlendState.Additive);
            UnifiedRandom rand = new UnifiedRandom(seed);
            float ringScale = MathHelper.Lerp(1, 0, MathHelper.Clamp(Projectile.ai[2] * 3.5f, 0, 1));
            if (ringScale > 0.01f)
            {
                for (float i = 0; i < max; i++)
                {
                    UnifiedRandom rand2 = new UnifiedRandom(seed + (int)i);
                    float angle = Helper.CircleDividedEqually(i, max) + Main.GameUpdateCount * 0.03f * rand.NextFloat(0.1f, 2f);
                    float scale = rand.NextFloat(0.1f, .5f);
                    Vector2 offset = new Vector2(rand2.NextFloat(300, 400) * (ringScale + rand2.NextFloat(-0.2f, 0.5f)) * scale, 0).RotatedBy(angle);
                    Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.Lerp(Color.Indigo, Color.Magenta, Projectile.ai[2] * 2) * ringScale, angle, tex.Size() / 2, new Vector2(MathHelper.Clamp(Projectile.ai[2] * 6.5f, 0, 1), ringScale) * scale * 0.2f, SpriteEffects.None, 0);

                    Main.spriteBatch.Draw(tex2, Projectile.Center - angle.ToRotationVector2() * 20 + offset - Main.screenPosition, null, Color.Lerp(Color.White, Color.Thistle, Projectile.ai[2] * 2) * ringScale, angle, tex2.Size() / 2, MathHelper.Clamp(Projectile.ai[2] * 6.5f, 0, 1.2f) * scale, SpriteEffects.None, 0);
                }
            }
            rand = new UnifiedRandom(seed + 1);
            ringScale = MathHelper.Lerp(1, 0, MathHelper.Clamp(Projectile.ai[1] * 3.5f, 0, 1));
            if (ringScale > 0.01f)
            {
                for (float i = 0; i < max; i++)
                {
                    UnifiedRandom rand2 = new UnifiedRandom(seed + (int)i);
                    float angle = Helper.CircleDividedEqually(i, max) + Main.GameUpdateCount * 0.06f * rand.NextFloat(0.1f, 2f);
                    float scale = rand.NextFloat(0.1f, .5f);
                    Vector2 offset = new Vector2(rand2.NextFloat(400, 600) * (ringScale + rand2.NextFloat(-0.2f, 0.5f)) * scale, 0).RotatedBy(angle);
                    for (float j = 0; j < 3; j++)
                        Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.Lerp(Color.Purple, Color.Violet, Projectile.ai[1] * 2) * (ringScale * 0.7f), angle, tex.Size() / 2, new Vector2(MathHelper.Clamp(Projectile.ai[1] * 6.5f, 0, 1.1f), ringScale) * scale * 0.3f, SpriteEffects.None, 0);

                    Main.spriteBatch.Draw(tex2, Projectile.Center - angle.ToRotationVector2() * 20 + offset - Main.screenPosition, null, Color.Lerp(Color.White, Color.Thistle, Projectile.ai[1] * 2) * (ringScale * 0.7f), angle, tex2.Size() / 2, MathHelper.Clamp(Projectile.ai[1] * 6.5f, 0, 1.2f) * scale, SpriteEffects.None, 0);
                }
            }
            rand = new UnifiedRandom(seed + 1);
            ringScale = MathHelper.Lerp(1.2f, 0, MathHelper.Clamp(Projectile.localAI[0] * 3.5f, 0, 1));
            if (ringScale > 0.01f)
            {
                for (float i = 0; i < max; i++)
                {
                    UnifiedRandom rand2 = new UnifiedRandom(seed + (int)i);
                    float angle = Helper.CircleDividedEqually(i, max) + Main.GameUpdateCount * 0.09f * rand.NextFloat(0.1f, 2f);
                    float scale = rand.NextFloat(0.1f, .5f);
                    Vector2 offset = new Vector2(rand2.NextFloat(350, 500) * (ringScale + rand2.NextFloat(-0.2f, 0.5f)) * scale, 0).RotatedBy(angle);
                    for (float j = 0; j < 4; j++)
                        Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.Lerp(Color.Purple, Color.DeepPink, Projectile.localAI[0] * 2) * ringScale, angle, tex.Size() / 2, new Vector2(MathHelper.Clamp(Projectile.localAI[0] * 6.5f, 0, 1.2f), ringScale) * scale * 0.3f, SpriteEffects.None, 0);

                    Main.spriteBatch.Draw(tex2, Projectile.Center - angle.ToRotationVector2() * 20 + offset - Main.screenPosition, null, Color.Lerp(Color.White, Color.Thistle, Projectile.localAI[0] * 2) * ringScale, angle, tex2.Size() / 2, MathHelper.Clamp(Projectile.localAI[0] * 6.5f, 0, 1.2f) * scale, SpriteEffects.None, 0);
                }
            }
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 50)
                Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 1f, 0.007f);
            if (Projectile.timeLeft < 40)
                Projectile.ai[1] = MathHelper.Lerp(Projectile.ai[1], 1f, 0.011f);
            if (Projectile.timeLeft < 30)
                Projectile.localAI[0] = MathHelper.Lerp(Projectile.localAI[0], 1f, 0.015f);

            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (npc.type != ModContent.NPCType<StarveiledScholar>() || !npc.active)
                Projectile.Kill();

            Projectile.Center = npc.Center;
        }
    }
    public class ScholarChargeUp2 : ModProjectile
    {
        public override string Texture => Helper.Empty;
        public override void SetDefaults()
        {
            Projectile.height = 1;
            Projectile.width = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 50;
        }
        public override bool ShouldUpdatePosition() => false;
        public override bool? CanDamage() => false;
        int seed;
        public override bool PreDraw(ref Color lightColor)
        {

            if (seed == 0) { seed = Main.rand.Next(int.MaxValue / 2); return false; }
            Texture2D tex = Helper.GetExtraTex("Extra/cone5");
            Texture2D tex2 = Helper.GetExtraTex("Extra/flare_01");
            Texture2D tex3 = Helper.GetExtraTex("Extra/star_02");
            Texture2D tex4 = Helper.GetExtraTex("Extra/spotlight");
            Texture2D tex5 = Helper.GetExtraTex("Extra/circlething");
            float max = 33;
            Main.spriteBatch.Reload(BlendState.Additive);
            UnifiedRandom rand = new UnifiedRandom(seed);
            rand = new UnifiedRandom(seed + 1);
            float ringScale = MathHelper.Clamp(MathF.Sin(Projectile.ai[1] * 4 * MathHelper.Pi) * 0.5f, 0, 0.3f);
            if (ringScale > 0.01f)
            {
                for (float i = 0; i < max; i++)
                {
                    UnifiedRandom rand2 = new UnifiedRandom(seed + (int)i);
                    float angle = Helper.CircleDividedEqually(i, max) + Main.GameUpdateCount * 0.04f * rand.NextFloat(-2f, 2f);
                    float scale = rand.NextFloat(0.1f, .5f) * 3;
                    Vector2 offset = new Vector2(rand2.NextFloat(200, 400) * (ringScale + rand2.NextFloat(-0.2f, 0.5f)) * (scale / 3), 0).RotatedBy(angle);
                    for (float j = 0; j < 2; j++)
                        Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.White * (ringScale * 0.3f), angle, tex.Size() / 2, new Vector2(MathHelper.Clamp(Projectile.ai[1] * 6.5f, 0, 1.1f), ringScale) * scale * 0.3f, SpriteEffects.None, 0);

                    Main.spriteBatch.Draw(tex2, Projectile.Center - angle.ToRotationVector2() * 20 + offset - Main.screenPosition, null, Color.Lerp(Color.White, Color.Thistle, Projectile.ai[1] * 2) * (ringScale * 0.7f), angle, tex2.Size() / 2, MathHelper.Clamp(Projectile.ai[1] * 6.5f, 0, 1.2f) * scale, SpriteEffects.None, 0);
                }
            }
            tex = Helper.GetExtraTex("Extra/slash");
            rand = new UnifiedRandom(seed + 1);
            ringScale = MathHelper.Lerp(1.2f, 0, MathHelper.Clamp(Projectile.localAI[0] * 3.5f, 0, 1));
            if (ringScale > 0.01f)
            {
                for (float i = 0; i < max - 5; i++)
                {
                    UnifiedRandom rand2 = new UnifiedRandom(seed + (int)i);
                    float angle = Helper.CircleDividedEqually(i, max - 5) + Main.GameUpdateCount * 0.02f * rand.NextFloat(-2f, 2f);
                    float scale = rand.NextFloat(0.1f, .7f) * 3;
                    Vector2 offset = new Vector2(rand2.NextFloat(250, 400) * (ringScale + rand2.NextFloat(-0.2f, 0.5f)) * (scale / 4), 0).RotatedBy(angle);
                    Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.White * ringScale * 0.5f, angle, tex.Size() / 2, new Vector2(MathHelper.Clamp(Projectile.localAI[0] * 6.5f, 0, 1.2f), ringScale) * scale * 0.6f, SpriteEffects.None, 0);

                    //Main.spriteBatch.Draw(tex2, Projectile.Center - angle.ToRotationVector2() * 20 + offset - Main.screenPosition, null, Color.Lerp(Color.White, Color.Thistle, Projectile.localAI[0] * 2) * ringScale * 0.5f, angle, tex2.Size() / 2, MathHelper.Clamp(Projectile.localAI[0] * 6.5f, 0, 1.2f) * scale, SpriteEffects.None, 0);
                }
            }

            if (ringScale > 0.01f)
            {
                for (float i = 0; i < max / 2; i++)
                {
                    UnifiedRandom rand2 = new UnifiedRandom(seed + (int)i);
                    float angle = Helper.CircleDividedEqually(i, max / 2) + Main.GameUpdateCount * 0.01f * rand.NextFloat(-2f, 2f);
                    float scale = rand.NextFloat(0.1f, .5f) * 3;
                    Vector2 offset = new Vector2(rand2.NextFloat(250, 500) * (ringScale + rand2.NextFloat(-0.2f, 0.5f)) * (scale / 1.5f), 0).RotatedBy(angle);
                    for (float j = 0; j < 2; j++)
                        Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, Color.Lerp(Color.Indigo, Color.White, Projectile.localAI[0] * 1.5f) * ringScale * 0.6f, angle, tex.Size() / 2, new Vector2(MathHelper.Clamp(Projectile.localAI[0] * 6.5f, 0, 1.2f), ringScale) * scale * 0.6f, SpriteEffects.None, 0);
                }
            }
            rand = new UnifiedRandom(seed + 1);
            Main.spriteBatch.Draw(tex3, Projectile.Center - Main.screenPosition, null, Color.Indigo * MathHelper.Clamp(MathF.Sin(Projectile.ai[2] * 3.5f * MathHelper.Pi) * 0.5f, 0, 0.2f), Main.GameUpdateCount * 0.03f + rand.NextFloat(MathHelper.TwoPi), tex3.Size() / 2, MathHelper.Clamp(MathHelper.SmoothStep(rand.NextFloat(2f, 0.7f), -2, Projectile.ai[2] * 1.6f), 0, 2) / 1.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex4, Projectile.Center - Main.screenPosition, null, Color.Violet * MathHelper.Clamp(MathF.Sin(Projectile.ai[2] * 3.5f * MathHelper.Pi) * 0.75f, 0, 0.5f), Main.GameUpdateCount * 0.03f + rand.NextFloat(MathHelper.TwoPi), tex4.Size() / 2, MathHelper.Clamp(MathHelper.SmoothStep(rand.NextFloat(2f, 0.7f), -2, Projectile.ai[2] * 1.6f), 0, 2) / 1.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex5, Projectile.Center - Main.screenPosition, null, Color.Indigo * MathHelper.Clamp(MathF.Sin(Projectile.ai[2] * 3.5f * MathHelper.Pi) * 0.5f, 0, 0.3f), Main.GameUpdateCount * 0.03f + rand.NextFloat(MathHelper.TwoPi), tex5.Size() / 2, MathHelper.Clamp(MathHelper.SmoothStep(rand.NextFloat(2f, 0.7f), -2, Projectile.ai[2] * 1.6f), 0, 2) / 1.5f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return true;
        }
        public override void AI()
        {
            if (Projectile.timeLeft < 50)
                Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 1f, 0.0077f);
            if (Projectile.timeLeft < 40)
                Projectile.ai[1] = MathHelper.Lerp(Projectile.ai[1], 1f, 0.0131f);
            if (Projectile.timeLeft < 30)
                Projectile.localAI[0] = MathHelper.Lerp(Projectile.localAI[0], 1f, 0.0165f);
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (npc.type != ModContent.NPCType<StarveiledScholar>() || !npc.active)
                Projectile.Kill();
            Vector2 pos = Projectile.Center + Main.rand.NextVector2CircularEdge(300, 300) * Main.rand.NextFloat(0.9f, 1.1f);
            if (Projectile.timeLeft % 10 == 0)
                Dust.NewDustPerfect(pos, ModContent.DustType<SparkleDust>(), Velocity: Helper.FromAToB(pos, Projectile.Center).RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(5, 15), newColor: Color.Lerp(Color.Thistle, Color.Violet, Main.rand.NextFloat()) * 0.5f, Scale: Main.rand.NextFloat(0.06f, 0.17f));
            Projectile.Center = npc.Center;
        }
    }
}
