using log4net.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Items;
using Terraria.ModLoader;
using Terraria.UI;

namespace MythosOfMoonlight.Misc
{
    public static class Lenikya
    {
        public static Texture2D Alphabet;
        public class LenikyaString
        {
            public string text = "";
            public Vector2 position;
            public int timeLeft;
            public float scale = 1f;
        }
        public static LenikyaString[] strings = new LenikyaString[32];
        public static void DrawAll(SpriteBatch spriteBatch)
        {

        }
        public static void DrawCustomLenikyaText(this SpriteBatch spriteBatch, string text, Vector2 pos, float scale)
        {

        }
    }
    public class LenikyaSystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int textIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            layers.Insert(textIndex, new LegacyGameInterfaceLayer("MythosOfMoonlight: Lenikya", () =>
            {
                Lenikya.DrawAll(Main.spriteBatch);
                return true;
            }, InterfaceScaleType.UI));
        }
        public override void Load()
        {
            Lenikya.Alphabet = Helper.GetTex("MythosOfMoonlight/Textures/LenikyaLanguage");
        }
        public override void Unload()
        {
            Lenikya.Alphabet = null;
        }
    }
}
