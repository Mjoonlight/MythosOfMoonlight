﻿using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;

namespace MythosOfMoonlight.Common.Systems
{
    public class EventShaderSystem : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override bool IsSceneEffectActive(Player player)
        {
            return PurpleCometEvent.PurpleComet;
        }
        public override int Music
        {
            get
            {
                if (PurpleCometEvent.PurpleComet && (Main.LocalPlayer.ZoneOverworldHeight || Main.LocalPlayer.ZoneSkyHeight)) return MusicLoader.GetMusicSlot(Mod, "Assets/Music/PurpleComet");
                else
                {
                    return base.Music;
                }
            }
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (PurpleCometEvent.PurpleComet && (player.ZoneOverworldHeight || player.ZoneSkyHeight))
            {
                if (!SkyManager.Instance["PurpleComet"].IsActive())
                {
                    SkyManager.Instance.Activate("PurpleComet");
                }
                player.ManageSpecialBiomeVisuals("PurpleComet", isActive);
            }
            else
            {
                if (SkyManager.Instance["PurpleComet"].IsActive())
                {
                    SkyManager.Instance.Deactivate("PurpleComet");
                }
                player.ManageSpecialBiomeVisuals("PurpleComet", false);
            }
        }
    }
}
