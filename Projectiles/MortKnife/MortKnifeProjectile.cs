using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythosOfMoonlight.Projectiles.MortKnife
{
    public class MortKnifeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MortKnife");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 18;
            projectile.friendly = true;
            projectile.timeLeft = 30;
            projectile.penetrate = 2;
            projectile.melee = true;
        }

        List<NPC> GetTargets(Vector2 position, float radius) // get targets, organized by closest npc
        {
            float closestDistance = -1;
            var npcs = new List<NPC>();
            for (int i = 0; i < Main.npc.Length; i++)
            {
                var npc = Main.npc[i];
                var distance = Vector2.DistanceSquared(npc.Center, position);
                if (distance < Math.Pow(radius, 2))
                {
                    if (closestDistance > distance || closestDistance == -1)
                    {
                        closestDistance = distance;
                        npcs.Insert(0, npc);
                    }

                    else
                    {
                        npcs.Add(npc);
                    }
                }
            }
            return npcs;
        }

        bool hit = false;
        const float BLOCK_LENGTH = 16;
        public override void AI()
        {
            var owner = Main.player[projectile.owner];
            var distance = (projectile.Center - owner.Center);
            projectile.rotation = distance.ToRotation();
            projectile.position = projectile.position + owner.velocity;

            projectile.frameCounter++;
            if (projectile.frameCounter == 1)
            {
                var targets = GetTargets(Main.MouseScreen + Main.screenPosition, 42);
                Main.NewText(targets.Count);
                if (targets.Count > 0)
                {
                    var target = targets[0].Center;
                    var diff = projectile.Center - target;
                    projectile.velocity = (-Vector2.UnitX * projectile.velocity.Length()).RotatedBy(diff.ToRotation());
                }

                else
                {
                    projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.Pi / 6f);
                }
            }

            if (projectile.frameCounter == 15 && !hit)
            {
                Reflect();
            }

            else if (distance.LengthSquared() < 256 && hit)
            {
                projectile.timeLeft = 0;
            }
        }
        void Reflect()
        {
            if (!hit)
            {
                projectile.velocity = -projectile.oldVelocity;
                hit = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.ai[0]++;
            if (projectile.ai[0] == 1)
                Reflect();
            else
                projectile.timeLeft = 0;
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 playerPosition = Main.player[projectile.owner].MountedCenter;
            int amount = 12;
            var diff = playerPosition - projectile.Center;
            var interval = diff / amount;
            float rotDir = diff.ToRotation();
            Vector2 off = new Vector2(0, 9).RotatedBy(projectile.rotation);

            var chainTexture = mod.GetTexture("Projectiles/MortKnife/MortKnifeChain");
            var chainRect = chainTexture.Frame();

            for (int i = 1; i < amount; i++)
            {
                Main.spriteBatch.Draw(chainTexture, projectile.Center + (interval * i) + off - Main.screenPosition, chainRect, lightColor, rotDir, default, 1f, SpriteEffects.None, 0f);
            }

            var handleTexture = mod.GetTexture("Projectiles/MortKnife/MortKnifeBase");
            var handleRect = handleTexture.Frame();
            var handleOff = new Vector2(14, 9).RotatedBy(rotDir);

            var drawData = new Terraria.DataStructures.DrawData(handleTexture, interval * (amount - 1) + handleOff - Main.screenPosition, handleRect, lightColor, projectile.rotation, default, 1f, SpriteEffects.None, 0);
            Main.playerDrawData.Add(drawData);
            drawData.Draw(Main.spriteBatch);


            /*
            Vector2 playerPosition = Main.player[projectile.owner].MountedCenter;
            int amount = 12;
            var diff = playerPosition - projectile.Center;
            var interval = diff / amount;
            float rotDir = diff.ToRotation();
            Vector2 off = new Vector2(0, 9).RotatedBy(projectile.rotation);

            var handleTexture = mod.GetTexture("Projectiles/MortKnife/MortKnifeBase");
            var handleRect = handleTexture.Frame();
            Main.spriteBatch.Draw(handleTexture, playerPosition + off - Main.screenPosition, handleRect, Color.White, rotDir, default, 1f, SpriteEffects.None, 3f);
            */
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 300);
            Reflect();
        }
    }
}