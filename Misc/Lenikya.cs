using log4net.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Items;
using Terraria.ModLoader;
using Terraria.UI;
using static MythosOfMoonlight.Misc.Lenikya;
using static System.Net.Mime.MediaTypeNames;

namespace MythosOfMoonlight.Misc
{
    public static class Lenikya
    {
        public static Texture2D Alphabet;
        public class LenikyaString
        {
            public string text = "";
            public Vector2 position;
            public Vector2 off;
            public Color color;
            public Color outlineColor;
            public bool centered;
            public int timeLeft;
            public int maxTimeLeft;
            public float scale;
            public bool additive;
            public float maxScale = 1f;
            public bool active;
            public int index;
        }
        public static LenikyaString[] strings = new LenikyaString[6];
        public static bool IsLoaded;
        public static void DrawLenikyaCombatText(SpriteBatch spriteBatch)
        {
            if (IsLoaded)
            {
                spriteBatch.SaveCurrent();
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
                Alphabet = Helper.GetTex("MythosOfMoonlight/Textures/LenikyaLanguage");
                Texture2D Outline = Helper.GetTex("MythosOfMoonlight/Textures/LenikyaLanguage_Outline");
                List<int> SmallFrames = new List<int> { 1, 5, 9, 11, 15, 19, 23, 36, 37 }; //-10 frames
                List<int> MediumFrames = new List<int> { 2, 6, 12, 16, 20, 24, 38 }; //-8 frames
                List<int> BigFrames = new List<int> { 3, 7, 13, 17, 21, 25 }; //-2 frames
                for (int i = 0; i < strings.Length; i++)
                {
                    if (strings[i].active)
                    {
                        if (strings[i].additive)
                            spriteBatch.Reload(BlendState.Additive);
                        float len = strings[i].text.Length * 20;
                        foreach (char c in strings[i].text)
                        {
                            int frameY = 0;
                            int letter = char.ToUpper(c) - (int)'A';
                            if (letter + (int)'A' < 'A' || letter + 'A' > 'Z')
                            {

                                if (char.IsNumber(c))
                                {
                                    frameY = ((int)char.GetNumericValue(c) + 26) * 20;
                                }
                                else
                                {
                                    switch (c)
                                    {
                                        case '-':
                                            frameY = 37 * 20;
                                            break;
                                        case '.':
                                            frameY = 36 * 20;
                                            break;
                                        case ',':
                                            frameY = 36 * 20;
                                            break;
                                        case '\'':
                                            frameY = 36 * 20;
                                            break;
                                        case '"':
                                            frameY = 36 * 20;
                                            break;
                                        case '!':
                                            frameY = 38 * 20;
                                            break;
                                        case '?':
                                            frameY = 39 * 20;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                frameY = letter * 20;
                            }
                            if (SmallFrames.Contains(frameY / 20))
                                len -= 10;
                            if (MediumFrames.Contains(frameY / 20))
                                len -= 8;
                            if (BigFrames.Contains(frameY / 20))
                                len -= 2;
                        }
                        len = MathHelper.Clamp(len, 0, Main.screenWidth / 2);
                        Vector2 pos = strings[i].position - (strings[i].centered ? new Vector2(len * 0.5f * strings[i].maxScale, 10) : Vector2.Zero);
                        Vector2 startPos = strings[i].position - (strings[i].centered ? new Vector2(len * 0.5f * strings[i].maxScale, 10) : Vector2.Zero);

                        for (int j = 0; j < strings[i].text.Length; j++)
                        {
                            float rot = 0;
                            if (strings[i].text[j] == ' ')
                            {
                                pos += new Vector2(20 * strings[i].maxScale, 0);
                                if (pos.X - startPos.X > Main.screenWidth / 2)
                                {
                                    pos = new Vector2(startPos.X, pos.Y + 25 * strings[i].maxScale);
                                }
                                continue;
                            }
                            if (strings[i].text[j] == '\n')
                            {
                                pos = new Vector2(startPos.X, pos.Y + 25 * strings[i].maxScale);
                                continue;
                            }
                            int frameY = 0;
                            int letter = char.ToUpper(strings[i].text[j]) - (int)'A';
                            if (letter + (int)'A' < 'A' || letter + 'A' > 'Z')
                            {

                                if (char.IsNumber(strings[i].text[j]))
                                {
                                    frameY = ((int)char.GetNumericValue(strings[i].text[j]) + 26) * 20;
                                }
                                else
                                {
                                    switch (strings[i].text[j])
                                    {
                                        case '-':
                                            frameY = 37 * 20;
                                            break;
                                        case '.':
                                            frameY = 36 * 20;
                                            rot = MathHelper.Pi;
                                            break;
                                        case ',':
                                            frameY = 36 * 20;
                                            rot = MathHelper.Pi;
                                            break;
                                        case '\'':
                                            frameY = 36 * 20;
                                            break;
                                        case '"':
                                            frameY = 36 * 20;
                                            break;
                                        case '!':
                                            frameY = 38 * 20;
                                            break;
                                        case '?':
                                            frameY = 39 * 20;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                frameY = letter * 20;
                            }
                            if (strings[i].additive)
                            {
                                if (strings[i].outlineColor != Color.Transparent)
                                    spriteBatch.Draw(Outline, pos + strings[i].off - Main.screenPosition, new Rectangle(0, frameY, 20, 20), strings[i].outlineColor, rot, Vector2.One * 10, new Vector2(strings[i].maxScale, strings[i].scale), rot != 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                                spriteBatch.Draw(Alphabet, pos + strings[i].off - Main.screenPosition, new Rectangle(0, frameY, 20, 20), strings[i].color, rot, Vector2.One * 10, new Vector2(strings[i].maxScale, strings[i].scale), rot != 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                            }
                            if (strings[i].outlineColor != Color.Transparent)
                                spriteBatch.Draw(Outline, pos + strings[i].off - Main.screenPosition, new Rectangle(0, frameY, 20, 20), strings[i].outlineColor, rot, Vector2.One * 10, new Vector2(strings[i].maxScale, strings[i].scale), rot != 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                            spriteBatch.Draw(Alphabet, pos + strings[i].off - Main.screenPosition, new Rectangle(0, frameY, 20, 20), strings[i].color, rot, Vector2.One * 10, new Vector2(strings[i].maxScale, strings[i].scale), rot != 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                            int offset = 0;
                            if (SmallFrames.Contains(frameY / 20))
                                offset = 10;
                            if (MediumFrames.Contains(frameY / 20))
                                offset = 8;
                            if (BigFrames.Contains(frameY / 20))
                                offset = 2;
                            pos += new Vector2((20 - offset) * strings[i].maxScale, 0);
                        }
                    }
                }

                spriteBatch.ApplySaved();
            }
        }
        public static void UpdateLenikyaCombatText()
        {
            if (IsLoaded)
                for (int i = 0; i < strings.Length; i++)
                {
                    if (strings[i].active)
                    {
                        strings[i].timeLeft--;
                        if (strings[i].timeLeft < 0)
                        {
                            strings[i].active = false;
                        }
                        strings[i].off = Vector2.Lerp(strings[i].off, Main.rand.NextVector2Circular(3 * strings[i].scale, 3 * strings[i].scale), Main.rand.NextFloat(0.1f, 0.5f));
                        float progress = Utils.GetLerpValue(0, strings[i].maxTimeLeft, strings[i].timeLeft);
                        strings[i].scale = MathHelper.Clamp(MathF.Sin(progress * MathHelper.Pi) * 3 * strings[i].maxScale, 0, strings[i].maxScale);
                    }
                }
        }
        public static void DrawLenikyaText(this SpriteBatch spriteBatch, string text, Vector2 position, Color color, Color outlineColor, float scale = 1f, bool centered = true) //scale should remain constant.
        {
            Alphabet = Helper.GetTex("MythosOfMoonlight/Textures/LenikyaLanguage");
            Texture2D Outline = Helper.GetTex("MythosOfMoonlight/Textures/LenikyaLanguage_Outline");
            List<int> SmallFrames = new List<int> { 1, 5, 9, 11, 15, 19, 23, 36, 37 }; //-10 frames
            List<int> MediumFrames = new List<int> { 2, 6, 12, 16, 20, 24 }; //-8 frames
            List<int> BigFrames = new List<int> { 3, 7, 13, 17, 21, 25 }; //-2 frames
            float len = text.Length;
            if (len * 20 > Main.screenWidth / 2)
                len = Main.screenWidth / 40;
            Vector2 pos = position - (centered ? new Vector2(len * 10 * scale, 10) : Vector2.Zero);
            Vector2 startPos = position - (centered ? new Vector2(len * 10 * scale, 10) : Vector2.Zero);
            for (int j = 0; j < text.Length; j++)
            {
                float rot = 0;
                if (text[j] == ' ')
                {
                    pos += new Vector2(20 * scale, 0);
                    if (pos.X - startPos.X > Main.screenWidth / 2)
                    {
                        pos = new Vector2(startPos.X, pos.Y + 25 * scale);
                    }
                    continue;
                }
                if (text[j] == '\n')
                {
                    pos = new Vector2(startPos.X, pos.Y + 25 * scale);
                    continue;
                }
                int frameY = 0;
                int letter = char.ToUpper(text[j]) - (int)'A';
                if (letter + (int)'A' < 'A' || letter + 'A' > 'Z')
                {

                    if (char.IsNumber(text[j]))
                    {
                        frameY = ((int)char.GetNumericValue(text[j]) + 26) * 20;
                    }
                    else
                    {
                        switch (text[j])
                        {
                            case '-':
                                frameY = 37 * 20;
                                break;
                            case '.':
                                frameY = 36 * 20;
                                rot = MathHelper.Pi;
                                break;
                            case ',':
                                frameY = 36 * 20;
                                rot = MathHelper.Pi;
                                break;
                            case '\'':
                                frameY = 36 * 20;
                                break;
                            case '"':
                                frameY = 36 * 20;
                                break;
                            case '!':
                                frameY = 38 * 20;
                                break;
                            case '?':
                                frameY = 39 * 20;
                                break;
                        }
                    }
                }
                else
                {
                    frameY = letter * 20;
                }
                if (outlineColor != Color.Transparent)
                    spriteBatch.Draw(Outline, pos, new Rectangle(0, frameY, 20, 20), outlineColor, rot, Vector2.One * 10, scale, rot != 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                spriteBatch.Draw(Alphabet, pos, new Rectangle(0, frameY, 20, 20), color, rot, Vector2.One * 10, scale, rot != 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                int offset = 0;
                if (SmallFrames.Contains(frameY / 20))
                    offset = 10;
                if (MediumFrames.Contains(frameY / 20))
                    offset = 8;
                if (BigFrames.Contains(frameY / 20))
                    offset = 2;
                pos += new Vector2((20 - offset) * scale, 0);
            }
        }
        public static int NewLenikyaCombatText(string text, Vector2 position, Color color, Color outlineColor, float scale = 1f, int timeLeft = 60, bool centered = true, bool additive = false)
        {
            int index = 0;
            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i].active)
                    index++;
            }
            if (index >= 6) return -1;

            strings[index] = (new LenikyaString()
            {
                text = text,
                position = position,
                scale = scale,
                maxScale = scale,
                timeLeft = timeLeft,
                maxTimeLeft = timeLeft,
                color = color,
                additive = additive,
                outlineColor = outlineColor,
                centered = centered,
                active = true,
                index = index
            });
            return index;
        }
    }
    public class LenikyaSystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int textIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars")) + 1;
            layers.Insert(textIndex, new LegacyGameInterfaceLayer("MythosOfMoonlight: Lenikya", () =>
            {
                DrawLenikyaCombatText(Main.spriteBatch);
                return true;
            }, InterfaceScaleType.UI));
        }
        public override void PostUpdateEverything()
        {
            UpdateLenikyaCombatText();
        }
        public override void Load()
        {
            Alphabet = Helper.GetTex("MythosOfMoonlight/Textures/LenikyaLanguage");
            IsLoaded = true;
            for (int i = 0; i < 6; i++)
            {
                strings[i] = (new LenikyaString()
                {
                    text = " ",
                    position = Vector2.Zero,
                    scale = 1f,
                    maxScale = 1f,
                    timeLeft = 60,
                    maxTimeLeft = 60,
                    centered = false,
                    color = Color.White,
                    outlineColor = Color.Transparent,
                    active = false,
                    index = i
                });
            }
        }
        public override void Unload()
        {
            Alphabet = null;
            strings = null;
            IsLoaded = false;
        }
    }
}
