using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Events;
using Terraria.ID;
using MythosOfMoonlight.Graphics.Particles;
using MythosOfMoonlight.Graphics;

namespace MythosOfMoonlight.BiomeManager
{
    public class PurpleCometBiome : ModBiome
    {
        public static PurpleCometBiome Instance { get; set; }
        public PurpleCometBiome()
        {
            Instance = this;
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Comet Flyby");
        }
        public override string BestiaryIcon
        {
            get
            {
                return base.BestiaryIcon;
            }
        }
        public override string BackgroundPath
        {
            get
            {
                return base.BackgroundPath;
            }
        }
        public override string MapBackground
        {
            get
            {
                return BackgroundPath;
            }
        }
        public override int Music
        {
            get
            {
                return (Main.LocalPlayer.ZoneOverworldHeight || Main.LocalPlayer.ZoneSkyHeight) ? MusicLoader.GetMusicSlot(Mod, "Assets/Music/PurpleComet") : base.Music;
            }
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override bool IsBiomeActive(Player player)
        {
            return PurpleCometEvent.PurpleComet;
        }
        public override void OnInBiome(Player player)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                if (Main.rand.NextBool(2) && (player.ZoneOverworldHeight || player.ZoneSkyHeight))
                {
                    Particle.Spawn<PurpurineParticle>(Main.screenPosition + Main.rand.NextVector2FromRectangle(new Rectangle(0, 0, Main.screenWidth, Main.screenHeight)), Main.rand.NextVector2Circular(3, 3), Color.White);
                }
                player.gravity = .2f;
                player.maxFallSpeed *= .75f;
            }
        }
    }
}
