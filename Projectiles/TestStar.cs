using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles
{
    public class TestStar : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(26, 28);
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(2);
        }
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.Draw(Helper.GetTex(Texture + "_Extra"), Projectile.Center - Main.screenPosition, null, Color.White, -Projectile.rotation, Helper.GetTex(Texture + "_Extra").Size() / 2, 1, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
        }
    }
}
