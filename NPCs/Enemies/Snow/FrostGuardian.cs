using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.NPCs.Enemies.Snow
{
    public class FrostGuardian : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 60;
            NPC.aiStyle = 0;
            NPC.damage = 15;
            NPC.defense = 10;
            NPC.lifeMax = 120;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.value = Item.buyPrice(silver: 5);
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;
            DisplayName.SetDefault("Boreal Blade");
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.Player.ZoneSnow && spawnInfo.Player.ZoneDirtLayerHeight) || (spawnInfo.Player.ZoneSnow && spawnInfo.Player.ZoneRain) ? 0.2f : 0;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter % 5 == 0)
            {
                switch (NPC.ai[0])
                {
                    case 0:
                        if (NPC.frame.Y < 3 * 60)
                            NPC.frame.Y += 60;
                        else
                            NPC.frame.Y = 0;
                        break;
                    case 1:
                        if (NPC.frame.Y < 19 * 60)
                            NPC.frame.Y += 60;
                        break;
                    case 2:
                        if (NPC.frame.Y < 24 * 60)
                            NPC.frame.Y += 60;
                        break;
                }
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Helper.SpawnDust(NPC.Center, NPC.Size, DustID.Ice, new Vector2(Main.rand.NextFloat(0.1f, 2.5f) * hitDirection, Main.rand.NextFloat(-2, 2)), 25, new Action<Dust>((target) => { target.noGravity = true; target.scale = Main.rand.NextFloat(0.6f, 0.9f); }
                ));
                Helper.SpawnGore(NPC, "MythosOfMoonlight/FrostGuardianGore");
                Helper.SpawnGore(NPC, "MythosOfMoonlight/FrostGuardianGore2");
                Helper.SpawnGore(NPC, "MythosOfMoonlight/FrostGuardianGore3");
                Helper.SpawnGore(NPC, "MythosOfMoonlight/FrostGuardianGore4");
            }
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            switch (NPC.ai[0])
            {
                case 0:
                    NPC.velocity = Vector2.Lerp(NPC.velocity, Helper.FromAToB(NPC.Center, player.Center) * 4, 0.05f);
                    if (player.Center.Distance(NPC.Center) < 300)
                        NPC.ai[1]++;
                    if (NPC.ai[1] >= 150)
                    {
                        NPC.ai[0] = 1;
                        NPC.ai[1] = 0;
                        NPC.frameCounter = 1;
                        NPC.frame.Y = 60 * 4;
                    }
                    break;
                case 1:
                    NPC.ai[1]++;
                    NPC.velocity *= 0.95f;
                    if (NPC.ai[1] == 5 * 5)
                        NPC.dontTakeDamage = true;
                    if (NPC.ai[1] == 7 * 5)
                    {
                        if (player.Center.Distance(NPC.Center) < 50)
                            NPC.Center = player.Center + -Helper.FromAToB(player.Center + Helper.FromAToB(player.Center, NPC.Center, reverse: true) * 100, NPC.Center, false).RotatedByRandom(0.2f);
                        NPC.Center = player.Center + -Helper.FromAToB(player.Center, NPC.Center, false).RotatedByRandom(0.2f);
                    }
                    if (NPC.ai[1] >= 16 * 5)
                    {
                        NPC.dontTakeDamage = false;
                        NPC.ai[1] = 0;
                        NPC.ai[0] = 2;
                        NPC.frameCounter = 1;
                        NPC.frame.Y = 60 * 21;
                        NPC.ai[2] = player.Center.X;
                        NPC.ai[3] = player.Center.Y;
                    }
                    break;
                case 2:
                    if (NPC.ai[1] < 5)
                    {
                        NPC.velocity += Helper.FromAToB(NPC.Center, new Vector2(NPC.ai[2], NPC.ai[3])) * 2.5f;
                    }
                    if (NPC.ai[1] > 17)
                        NPC.velocity *= 0.98f;
                    if (++NPC.ai[1] >= 5 * 5)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[0] = 0;
                    }
                    break;
            }
        }
    }
}
