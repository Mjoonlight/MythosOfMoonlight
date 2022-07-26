using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using MythosOfMoonlight.Events;

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
            DisplayName.SetDefault("Comet Flyby");
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
                return MusicLoader.GetMusicSlot(Mod, "Assets/Music/PurpleComet");
            }
        }
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override bool IsBiomeActive(Player player)
        {
            return PurpleCometEvent.PurpleComet;
        }
    }
}
