using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim;
using MythosOfMoonlight.Projectiles;
using MythosOfMoonlight.Projectiles.IridicProjectiles;
using MythosOfMoonlight.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace MythosOfMoonlight
{
    public class MoMPlayer : ModPlayer
    {
        static NPC Sym => Starine_Symbol.symbol;
        //public override void UpdateBiomeVisuals()
        //{
        //    var purpleComet = PurpleCometEvent.PurpleComet && Main.LocalPlayer.ZoneOverworldHeight;
        //    player.ManageSpecialBiomeVisuals("PurpleComet", purpleComet);
        //}
        public bool CommunicatorEquip = false;
        public float CommunicatorCD;
        public Vector2 targetCameraPosition = new(-1, -1);
        public readonly Vector2 setToPlayer = new(-1, -1);
        public int source = -1;
        public float lerpSpeed;
        public float LerpTimer;
        public bool StarBitShot = false;
        public override void OnEnterWorld()
        {
            for (int i = 1; i < Main.maxTilesX; i++)
            {
                for (int j = 1; j < Main.maxTilesY; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile.TileType == ModContent.TileType<SymbolPointTile>())
                    {
                        if (!NPC.AnyNPCs(ModContent.NPCType<Starine_Symbol>()))
                        {
                            NPC npc = NPC.NewNPCDirect(null, (i - 1) * 16, (j - 2) * 16, ModContent.NPCType<Starine_Symbol>());
                            npc.homeTileX = i - 2;
                            npc.homeTileY = j - 1;
                            if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.SyncNPC);
                        }
                    }
                }
            }
        }
        public override void ResetEffects()
        {
            StarBitShot = false;
            CommunicatorEquip = false;
            foreach (NPC NPC in Main.npc)
            {
                if (NPC.type == ModContent.NPCType<Starine_Symbol>())
                {
                    if (NPC.active)
                    {
                        if (NPC.ai[0] == 1 || NPC.ai[0] == 2)
                        {
                            LerpTimer++;
                        }
                        else
                        {
                            LerpTimer = LerpTimer > 1 ? LerpTimer *= .9f : 0;
                        }
                    }
                }
            }
        }
        Vector2 location2 = new Vector2(0, 0);

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (StarBitShot && !target.friendly && hit.Crit && target.lifeMax > 10 && target.type != NPCID.TargetDummy)
            {
                // TO DO : Projectile spawn from sky
                Player p = Main.LocalPlayer;

                int random = Main.rand.Next(1, 3);
                Vector2 spawnpos = new Vector2(0, p.position.Y + 900);
                if (random == 1)
                    location2 = new Vector2(p.position.X + 1000, spawnpos.Y - Main.rand.Next(1, 1800));
                if (random == 2)
                {
                    location2 = new Vector2(p.position.X - 1000, spawnpos.Y - Main.rand.Next(1, 1800));
                }
                Vector2 direction = Helper.FromAToB(location2, target.Center + target.velocity);

                Projectile.NewProjectile(Main.LocalPlayer.GetSource_FromThis(), location2, direction * 25, ModContent.ProjectileType<StarBitBlue>(), Main.LocalPlayer.HeldItem.damage * 2, 4f, Main.myPlayer);

            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (StarBitShot && !target.friendly && hit.Crit && target.lifeMax > 10 && target.type != NPCID.TargetDummy)
            {
                Player p = Main.LocalPlayer;

                int random = Main.rand.Next(1, 3);
                Vector2 spawnpos = new Vector2(0, p.position.Y + 900);
                if (random == 1)
                    location2 = new Vector2(p.position.X + 1000, spawnpos.Y - Main.rand.Next(1, 1800));
                if (random == 2)
                {
                    location2 = new Vector2(p.position.X - 1000, spawnpos.Y - Main.rand.Next(1, 1800));
                }
                Vector2 direction = Helper.FromAToB(location2, target.Center + target.velocity);

                Projectile.NewProjectile(Main.LocalPlayer.GetSource_FromThis(), location2, direction * 25, ModContent.ProjectileType<StarBitBlue>(), Main.LocalPlayer.HeldItem.damage * 2, 4f, Main.myPlayer);

            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (CommunicatorEquip)
            {
                if (CommunicatorCD > 0) CommunicatorCD--;
                if (CommunicatorCD == 1)
                {
                    SoundEngine.PlaySound(SoundID.MaxMana, Main.LocalPlayer.Center);
                    for (int i = 6; i <= 360; i += 6)
                    {
                        Vector2 vel = MathHelper.ToRadians(i).ToRotationVector2() * 60f;
                        Dust dust = Dust.NewDustDirect(Main.LocalPlayer.Center + vel, 1, 1, ModContent.DustType<PurpurineDust>(), 0, 0, 0, default, 1.33f);
                        dust.noGravity = true;
                        dust.velocity = Vector2.Zero;
                    }
                }
            }
            else CommunicatorCD = 300;
        }
        public void NewCameraPosition(Vector2 point, float lerpSpeed, int source)
        {
            targetCameraPosition = point - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            this.lerpSpeed = lerpSpeed;
            this.source = source;
        }
        public override void PostUpdate()
        {
            if (Main.rand.NextBool((int)(3000 / (Star.starfallBoost + 0.01f))) && !Main.dayTime)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(1920 * Main.rand.NextFloat() - 960, -2500), new Vector2(Main.rand.NextFloat(-1, 1), 20f), ModContent.ProjectileType<FallingStarBig>(), 2000, 0);
            }
            if (PurpleCometEvent.PurpleComet)
                Player.noFallDmg = true;
        }
        public override void ModifyScreenPosition()
        {
            foreach (NPC NPC in Main.npc)
            {
                if (NPC.type == ModContent.NPCType<Starine_Symbol>())
                {
                    if (Sym != null)
                    {
                        if (Sym.active)
                        {
                            if (Vector2.Distance(Player.Center, ((Starine_Symbol)Sym.ModNPC).CircleCenter) < 1000f)
                            {
                                //Main.screenPosition = Player.Center - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) + (((Starine_Symbol)Sym.ModNPC).CircleCenter - Player.Center) * (1 - (float)Math.Pow(0.95f, LerpTimer));
                            }
                        }
                    }
                }
            }
        }
    }
}
