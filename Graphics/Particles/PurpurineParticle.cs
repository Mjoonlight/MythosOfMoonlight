using Microsoft.Xna.Framework;
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
            Frame = new Rectangle(0, 0, 38, 38);
            Scale = new Vector2(2f*Depth);
            Origin = Texture.Size() / 2f;
            Color = Color.White;
        }
        public override void Update()
        {
            base.Update();
            Velocity *= .985f;
            if (Main.netMode != NetmodeID.Server)
            {
                Position -= Main.LocalPlayer.velocity * Depth;
            }
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
        }
    }
}
