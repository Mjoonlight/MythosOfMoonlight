using Microsoft.Xna.Framework;
using MythosOfMoonlight.Common.Graphics;
using MythosOfMoonlight.Common.Systems;
using Terraria;
using Terraria.ID;

namespace MythosOfMoonlight.Common.Graphics.Particles
{
    public class PurpurineParticle : Particle
    {
        public bool Fade;
        public float Depth;
        public override void OnSpawn()
        {
            Alpha = 0f;
            IsAdditive = true;
            Frame = new Rectangle(0, 0, 80, 80);
            Origin = Texture.Size() / 2f;
            Color = Color.White;

            //velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.01f, 2);
            Depth = Main.rand.NextFloat(.1f, 1f);
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi); ;
            Scale = new Vector2(.1f) * Depth;
        }
        public override void Update()
        {
            base.Update();
            Velocity *= .95f;
            if (!Fade)
            {
                Alpha += 0.025f * Depth;
                if (Alpha >= 0.75f) Fade = true;
            }
            else
            {
                Scale -= new Vector2(0.005f) * Depth;
                Alpha -= 0.005f * Depth;
                if (Alpha <= 0f)
                {
                    Kill();
                }
            }
            if (Main.screenPosition == Main.screenLastPosition || CameraSystem.CameraChangeTransition > 0)
                return;
            if (Main.netMode != NetmodeID.Server)
            {
                Position -= Main.LocalPlayer.velocity * Depth;
            }
        }
    }
}
