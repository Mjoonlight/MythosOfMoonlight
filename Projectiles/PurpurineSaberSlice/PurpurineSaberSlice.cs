using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace MythosOfMoonLight.Projectiles.PurpurineSaberSlice
{
    public class PurpurineSaberSlice : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purpurine Saber"); 
            Main.projFrames[projectile.type] = 10;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 42;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 30;
            projectile.netUpdate = true;
            projectile.netUpdate2 = true;
            projectile.netImportant = true;
        }
        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                projectile.frameCounter++;
                Lighting.AddLight(projectile.Center, .5f, .5f, .5f);
                foreach (Player player in Main.player)
                {
                    if (player == Main.player[projectile.owner])
                    {
                        projectile.Center = player.Center + Utils.SafeNormalize(Main.MouseWorld - player.Center, Microsoft.Xna.Framework.Vector2.UnitX) * 36f;
                        projectile.rotation = (Main.MouseWorld - player.Center).ToRotation();
                    }
                }
                if (projectile.frameCounter % 4 == 0)
                {
                    Main.PlaySound(SoundID.Item15, projectile.Center);
                }
                if (projectile.frameCounter % 4 == 0)
                {
                    Vector2 dPos = projectile.Center - new Vector2(20, 0).RotatedBy(projectile.rotation);
                    Vector2 dVel = MathHelper.ToRadians(Main.rand.NextFloat(-30, 30)).ToRotationVector2().RotatedBy(projectile.rotation) * 4f;
                    Dust dust = Dust.NewDustDirect(dPos, 1, 1, ModContent.DustType<PurpurineDust>());
                    dust.noGravity = true;
                    dust.velocity = dVel;
                    Vector2 dPos2 = projectile.Center + new Vector2(Main.rand.NextFloat(-20, 20), Main.rand.NextFloat(-21, 21)).RotatedBy(projectile.rotation);
                    Vector2 dVel2 = new Vector2(2, 0).RotatedBy(projectile.rotation);
                    Dust dust1 = Dust.NewDustDirect(dPos2, 1, 1, ModContent.DustType<PurpurineDust>());
                    dust1.noGravity = true;
                    dust1.velocity = dVel2;
                }
                if (projectile.frameCounter >= 40)
                {
                    projectile.frameCounter = 1;
                }
                projectile.frame = projectile.frameCounter / 4;
                if (Main.player[projectile.owner].channel)
                {
                    projectile.timeLeft = 3;
                }
                if (projectile.frameCounter == 5 || projectile.frameCounter == 25)
                {
                    Projectile.NewProjectile(projectile.Center + new Vector2(21, 0).RotatedBy(projectile.rotation), new Vector2(8, 0).RotatedBy(projectile.rotation), ModContent.ProjectileType<PurpurineSaberSlice>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.frame);
                }
            }
            else
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 20;
                projectile.frame = (int)projectile.ai[0];
                projectile.velocity *= .96f;
                projectile.alpha += 8;
                projectile.rotation = projectile.velocity.ToRotation();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0]>0)
            {
                Texture2D tex = ModContent.GetTexture("MythosOfMoonlight/Projectiles/PurpurineSaber/PurpurineSaber");
                Vector2 ori = new Vector2(20, 21);
                Rectangle rec = new Rectangle(0, (int)projectile.ai[0] * 42, 40, 42);
                float rot = projectile.rotation;
                for (int i = 0; i <=6; i +=1)
                {
                    Vector2 pos = projectile.oldPos[i] + ori - Main.screenPosition;
                    Color color = Color.White * (float)((float)((float)projectile.oldPos.Length - i) / (float)projectile.oldPos.Length) * ((float)(255 - (float)projectile.alpha) / 255f);
                    spriteBatch.Draw(tex, pos, rec, color, rot, ori, 1f, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
    }
}
