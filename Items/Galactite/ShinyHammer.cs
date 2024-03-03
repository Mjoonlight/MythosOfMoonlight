using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.GameContent;
using MythosOfMoonlight.Misc;
using MythosOfMoonlight.Tiles.Furniture.Pilgrim;
using MythosOfMoonlight.Tiles;
using System.Collections.Generic;

namespace MythosOfMoonlight.Items.Galactite
{
    public class ShinyHammer : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.PaladinsHammer);
            Item.damage = 20;
        }
    }
    public class GravitronOrb : ModProjectile
    {
        public override string Texture => Helper.ExtraDir + "purpVortex";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.aiStyle = -1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D tex2 = Helper.GetTex(Helper.ExtraDir + "purpCircle");
            Texture2D tex3 = Helper.GetTex(Helper.ExtraDir + "darkPurpCircle");
            Main.spriteBatch.Draw(tex3, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex2.Size() / 2, Projectile.scale * 0.075f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.Additive);
            Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex2.Size() / 2, Projectile.scale * 0.075f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(effect: MythosOfMoonlight.PullingForce);
            MythosOfMoonlight.PullingForce.Parameters["uOpacity"].SetValue(1f);
            MythosOfMoonlight.PullingForce.Parameters["uIntensity"].SetValue(1f);
            MythosOfMoonlight.PullingForce.Parameters["uOffset"].SetValue(0.5f);
            MythosOfMoonlight.PullingForce.Parameters["uSpeed"].SetValue(4f);
            MythosOfMoonlight.PullingForce.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 4f);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.025f, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(effect: null);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(2);
            Projectile.timeLeft = 10;
        }
    }
}
