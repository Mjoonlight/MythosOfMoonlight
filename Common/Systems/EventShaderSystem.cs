using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
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
                if (PurpleCometEvent.PurpleComet) return MusicLoader.GetMusicSlot(Mod, "Assets/Music/PurpleComet");
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
