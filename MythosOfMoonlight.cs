using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace MythosOfMoonlight
{
    public static class Helper
    {
        public const float ONEDEG = .01745329252f;
        public struct Range
        {
            float min, max;
            public Range(float min, float max)
            {
                this.min = min;
                this.max = max;
            }
            public static implicit operator float(Range range) => range.FValue;
            public float FValue => Main.rand.NextFloat(min, max);
            public int IValue => Main.rand.Next((int)min, (int)max);
        }
        public static void FireProjectilesInArc(Vector2 origin, Vector2 centerDirection, float radians, int type, float speed, int damage, float knockback, int amount)
        {
            var centeredDir = centerDirection.RotatedBy(-radians/2f); //-MathHelper.ToRadians(degrees / 2f));
            for (float i = 1; i <= amount; i++)
            {
                var direction = centeredDir.RotatedBy(i/amount * radians);
                Projectile.NewProjectile(origin, direction * speed, type, damage, knockback);
            }

            centeredDir = centerDirection.RotatedBy(-radians/1.5f); //-MathHelper.ToRadians(degrees / 2f));
            for (float i = 1; i <= amount; i++)
            {
                var direction = centeredDir.RotatedBy(i / amount * radians);
                Projectile.NewProjectile(origin, direction * speed, type, damage, knockback);
            }

            centeredDir = centerDirection.RotatedBy(-radians); //-MathHelper.ToRadians(degrees / 2f));
            for (float i = 1; i <= amount; i++)
            {
                var direction = centeredDir.RotatedBy(i / amount * radians);
                Projectile.NewProjectile(origin, direction * speed, type, damage, knockback);
            }
        }
    }
    public class MythosOfMoonlight : Mod
	{
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Ref<Effect> filterRef = new Ref<Effect>(GetEffect("Effects/PurpleComet"));
                Filters.Scene["PurpleComet"] = new Filter(new ScreenShaderData(filterRef, "ScreenBasic"), EffectPriority.Medium);
            }
        }
    }
}