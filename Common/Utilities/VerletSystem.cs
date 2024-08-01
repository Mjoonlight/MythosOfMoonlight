﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using XPT.Core.Audio.MP3Sharp.Decoding.Decoders.LayerIII;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace MythosOfMoonlight.Common.Utilities
{

    public class Verlet
    {

        public int stiffness { get; set; }
        public List<VerletSegment> segments { get; set; }
        public List<VerletPoint> points { get; set; }
        public VerletPoint firstP { get; set; }
        public VerletPoint lastP { get; set; }
        public float drag { get; set; }
        public float gravity { get; set; }
        public float startRot => segments[0].Rotation();
        public float endRot => segments[segments.Count - 1].Rotation();
        public Vector2 startPos => segments[0].pointA.position;
        public Vector2 endPos => segments[segments.Count - 1].pointB.position;

        public Verlet(Vector2 start, float length, int count, /*float drag = 0.9f,*/ float gravity = 0.2f, bool firstPointLocked = true, bool lastPointLocked = true, int stiffness = 6)
        {

            this.gravity = gravity;
            this.stiffness = stiffness;

            Load(start, length, count, firstPointLocked, lastPointLocked);
        }
        private void Load(Vector2 startPosition, float length, int count, bool firstPointLocked = true, bool lastPointLocked = true, Vector2 offset = default)
        {
            segments = new List<VerletSegment>();
            points = new List<VerletPoint>();


            for (int i = 0; i < count; i++)
            {
                points.Add(new VerletPoint(startPosition + (offset == default ? Vector2.Zero : offset * i), gravity/*, drag*/));
            }


            for (int i = 0; i < count - 1; i++)
            {
                segments.Add(new VerletSegment(length, points[i], points[i + 1]));
            }



            firstP = points.First();
            firstP.locked = firstPointLocked;

            lastP = points.Last();
            lastP.locked = lastPointLocked;
        }
        public void Update(Vector2 start, Vector2 end, float lerpT = 1f)
        {
            if (firstP.locked)
                firstP.position = Vector2.Lerp(firstP.position, start, lerpT);
            if (lastP.locked)
                lastP.position = Vector2.Lerp(lastP.position, end, lerpT);
            foreach (VerletPoint point in points)
            {
                point.Update();
                point.gravity = gravity;
            }
            for (int i = 0; i < stiffness; i++)
                foreach (VerletSegment segment in segments)
                    segment.Constrain();
        }

        public Vector2[] Points()
        {
            List<Vector2> verticeslist = new List<Vector2>();
            foreach (VerletPoint point in points)
                verticeslist.Add(point.position);

            return verticeslist.ToArray();
        }
        public void Draw(SpriteBatch sb, string texPath, string baseTex = null, string endTex = null, bool useColor = false, Color color = default, float scale = 1, float rot = 0, bool useRot = false, bool useRotEnd = false, bool useRotFirst = false, float endRot = 0, float firstRot = 0)
        {
            foreach (VerletSegment segment in segments)
            {
                if (baseTex != null || endTex != null ? segment != segments.First() && segment != segments.Last() : true)
                {
                    if (useColor)
                        segment.DrawSegments(sb, texPath, color, true, scale: scale, rot, useRot);
                    else
                        segment.DrawSegments(sb, texPath, scale: scale, rot: rot, useRot: useRot);
                }
                else if (endTex != null && segment == segments.Last())
                {
                    if (useColor)
                        segment.Draw(sb, endTex, color, true, scale: scale, endRot, useRotEnd);
                    else
                        segment.Draw(sb, endTex, scale: scale, rot: endRot, useRot: useRotEnd);
                }
                else if (baseTex != null && segment == segments.First())
                {
                    if (useColor)
                        segment.Draw(sb, baseTex, color, true, scale: scale, firstRot, useRotFirst);
                    else
                        segment.Draw(sb, baseTex, scale: scale, rot: firstRot, useRot: useRotFirst);

                }
            }
        }
    }
    public class VerletPoint
    {
        public Vector2 position, lastPos;
        public bool locked;
        public float gravity;
        public VerletPoint(Vector2 position, float gravity/*, float drag*/)
        {
            this.position = position;
            this.gravity = gravity;

        }

        public void Update()
        {

            lastPos = position;

            position += new Vector2(0, gravity);
        }
    }
    public class VerletSegment
    {
        public bool cut = false;
        public float len;
        public float Rotation()
        {
            return (pointA.position - pointB.position).ToRotation() - 1.57f;
        }
        public VerletPoint pointA, pointB;
        public VerletSegment(float len, VerletPoint pointA, VerletPoint pointB)
        {
            this.len = len;
            cut = false;
            this.pointA = pointA;
            this.pointB = pointB;
        }

        public void Constrain()
        {
            if (cut)
                return;
            Vector2 vel = pointB.position - pointA.position;
            float distance = vel.Length();
            float fraction = (len - distance) / Math.Max(distance, 1) / 2;
            vel *= fraction;

            if (!pointA.locked)
                pointA.position -= vel;
            if (!pointB.locked)
                pointB.position += vel;
        }
        public void Draw(SpriteBatch sb, string texPath, Color color = default, bool useColor = false, float scale = 1, float rot = 0, bool useRot = false)
        {
            if (cut)
                return;
            Texture2D tex = Helper.GetTex(texPath);
            sb.Draw(tex, pointB.position - Main.screenPosition, null, useColor ? color : Lighting.GetColor((int)pointB.position.X / 16, (int)(pointB.position.Y / 16.0)), useRot ? rot : Rotation(), tex.Size() / 2, scale, SpriteEffects.None, 0);
        }
        public void DrawSegments(SpriteBatch sb, string texPath, Color color = default, bool useColor = false, float scale = 1, float rot = 0, bool useRot = false)
        {
            if (cut)
                return;
            Texture2D tex = Helper.GetTex(texPath);
            Vector2 center = pointB.position;
            Vector2 distVector = pointA.position - pointB.position;
            float distance = distVector.Length();
            int attempts = 0;
            while (distance > tex.Height / 2 && !float.IsNaN(distance) && ++attempts < 100)
            {
                distVector.Normalize();
                distVector *= tex.Height / 2;
                center += distVector;
                distVector = pointA.position - center;
                distance = distVector.Length();
                sb.Draw(tex, center - Main.screenPosition, null, useColor ? color : Lighting.GetColor((int)pointB.position.X / 16, (int)(pointB.position.Y / 16.0)), useRot ? rot : Rotation(), tex.Size() / 2, scale, SpriteEffects.None, 0);
            }
            Draw(sb, texPath, color, useColor, scale);
        }
    }
}
