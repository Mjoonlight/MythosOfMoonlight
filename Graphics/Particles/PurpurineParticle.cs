using Microsoft.Xna.Framework;
using MythosOfMoonlight.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace MythosOfMoonlight.Graphics.Particles
{
    public class PurpurineParticle : Particle
    {
        public bool Fade;
        public float Depth;
        public override void OnSpawn()
        {
            Depth = Main.rand.NextFloat(.1f, 1f);
            IsAdditive = true;
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Alpha = 0f;
            Frame = new Rectangle(0, 0, 80, 80);
            Scale = new Vector2(.1f * Depth);
            Origin = Texture.Size() / 2f;
            Color = Color.White;
        }
        public override void Update()
        {
            base.Update();
            Velocity *= .85f;
            if (!Fade)
            {
                Alpha += 0.025f;
                if (Alpha >= 0.75f) Fade = true;
            }
            else
            {
                Scale -= new Vector2(0.005f);
                Alpha -= 0.005f;
                if (Alpha <= 0f) Kill();
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
