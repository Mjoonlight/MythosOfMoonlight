using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Assets.Effects
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct StarTrail
    {
        private static VertexStrip _vertexStrip = new VertexStrip();

        public void Draw(Projectile proj, float alpha)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(6f * alpha);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        public Color StripColors(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Cyan, Color.White, progressOnStrip);
            result.A = 0;
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(50, 0, num);
        }
    }
}