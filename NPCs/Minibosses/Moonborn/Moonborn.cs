using MythosOfMoonlight.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonlight.NPCs.Minibosses.Moonborn
{
    public class Moonborn : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Zombie);
        }
        const int maxJoints = 10;
        Vector2[] joint = new Vector2[maxJoints], foot = new Vector2[maxJoints], target = new Vector2[maxJoints], targetTarget = new Vector2[maxJoints];
        const float jointLength = 68;
        float jointAngle0;
        float jointAngle1;
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < maxJoints; i++)
            {
                float angle = Helper.CircleDividedEqually(i, maxJoints) + MathHelper.PiOver4;
                Vector2 vel = new Vector2(1, 0).RotatedBy(angle);
                joint[i] = NPC.Center + vel * jointLength;
                Vector2 vel2 = new Vector2(1, 0).RotatedBy(angle);
                foot[i] = joint[i] + vel2 * jointLength;
                target[i] = TRay.Cast(NPC.Center - new Vector2(0, 100), Vector2.UnitY, 100 + jointLength * 2);
                targetTarget[i] = target[i];
            }
        }
        void UpdateJoints()
        {
            for (int i = 0; i < maxJoints; i++)
            {
                float targetLength = Vector2.Distance(NPC.Center, target[i]);

                Vector2 diff = target[i] - NPC.Center;
                float atan = MathF.Atan2(diff.Y, diff.X);

                float cosAngle0 = ((targetLength * targetLength) + (jointLength * jointLength) - (jointLength * jointLength)) / (2 * targetLength * jointLength);
                float angle0 = MathF.Acos(cosAngle0);
                float cosAngle1 = ((jointLength * jointLength) + (jointLength * jointLength) - (targetLength * targetLength)) / (2 * jointLength * jointLength);
                float angle1 = MathF.Acos(cosAngle1);

                jointAngle0 = atan - angle0;
                jointAngle1 = MathHelper.Pi - angle1;
                if (float.IsNaN(jointAngle0) || float.IsNaN(jointAngle1))
                {
                    jointAngle0 = Helper.FromAToB(NPC.Center, target[i]).ToRotation();
                    jointAngle1 = Helper.FromAToB(NPC.Center, target[i]).ToRotation();
                }
                joint[i] = NPC.Center + Helper.FromAToB(NPC.Center, joint[i].RotatedBy(jointAngle0 + MathHelper.PiOver2 + MathHelper.PiOver4)) * (i < 5 ? -1 : 1) * jointLength;
                foot[i] = joint[i] + Helper.FromAToB(joint[i], target[i]) * jointLength;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < maxJoints; i++)
            {
                spriteBatch.Draw(Helper.GetTex(Texture + "Leg0"), NPC.Center - Main.screenPosition, null, i < 2 ? Color.Blue : drawColor, Helper.FromAToB(NPC.Center, joint[i]).ToRotation() - MathHelper.PiOver2, new Vector2(Helper.GetTex(Texture + "Leg0").Width / 2, 0), 1, SpriteEffects.None, 0);
                spriteBatch.Draw(Helper.GetTex(Texture + "Leg1"), joint[i] - Main.screenPosition, null, i < 2 ? Color.Blue : drawColor, Helper.FromAToB(joint[i], foot[i]).ToRotation() - MathHelper.PiOver2, new Vector2(Helper.GetTex(Texture + "Leg1").Width / 2, 0), 1, SpriteEffects.None, 0);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        public override void AI()
        {
            NPC.Center = Main.MouseWorld;
            int movingIndex = -1;
            int movingIndex2 = -1;
            for (int i = 0; i < maxJoints; i++)
            {
                float targetLength = Vector2.Distance(joint[i], targetTarget[i]);
                //float angle = Helper.CircleDividedEqually(i, maxJoints * 6) + MathHelper.PiOver4;
                if (targetLength > jointLength * 1.25f)
                    targetTarget[i] = TRay.Cast(joint[i], Vector2.UnitY.RotatedBy(i % 2 == 0 ? MathHelper.PiOver4 * 0.5f : MathHelper.PiOver4 * -0.5f), 100 + jointLength * 3);
                if (targetLength > jointLength * 0.75f)
                {
                    if (movingIndex == -1)
                        movingIndex = i;
                    else if (movingIndex2 == -1)
                        movingIndex2 = i;
                    else
                        continue;
                    targetTarget[i] = TRay.Cast(joint[i], Vector2.UnitY.RotatedBy(i % 2 == 0 ? MathHelper.PiOver4 * 0.5f : MathHelper.PiOver4 * -0.5f), 100 + jointLength * 3);
                }
                else
                {
                    if (movingIndex == i)
                        movingIndex = -1;
                    if (movingIndex2 == i)
                        movingIndex = -1;
                }
                target[i] = targetTarget[i];//Vector2.Lerp(target[i], targetTarget[i], 0.4f);
            }
            UpdateJoints();
        }
    }
}
