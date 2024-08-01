using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MythosOfMoonlight.Common.Crossmod;
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
            // DisplayName.SetDefault("Chloroccyx");
            Main.projFrames[Projectile.type] = 1;
            Projectile.AddElement(CrossModHelper.Nature);
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Melee;
        }

        static List<NPC> GetTargets(Vector2 position, float radius) // get targets, organized by closest NPC
        {
            float closestDistance = -1;
            var npcs = new List<NPC>();
            for (int i = 0; i < Main.npc.Length; i++)
            {
                var NPC = Main.npc[i];
                var distance = Vector2.DistanceSquared(NPC.Center, position);
                if (distance < Math.Pow(radius, 2))
                {
                    if (closestDistance > distance || closestDistance == -1)
                    {
                        closestDistance = distance;
                        npcs.Insert(0, NPC);
                    }
                    else
                        npcs.Add(NPC);
                }
            }
            return npcs;
        }

        bool hit = false;
        const float BLOCK_LENGTH = 16;
        public override void AI()
        {
            var owner = Main.player[Projectile.owner];
            var distance = (Projectile.Center - owner.Center);
            Projectile.rotation = distance.ToRotation();
            Projectile.position = Projectile.position + owner.velocity;

            Projectile.frameCounter++;
            if (Projectile.frameCounter == 1)
            {
                var targets = GetTargets(Main.MouseScreen + Main.screenPosition, 42);
                if (targets.Count > 0)
                {
                    var target = targets[0].Center;
                    var diff = Projectile.Center - target;
                    Projectile.velocity = (-Vector2.UnitX * Projectile.velocity.Length()).RotatedBy(diff.ToRotation());
                }
                else
                    Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.Pi / 6f);
            }

            if (Projectile.frameCounter == 15 && !hit)
                Reflect();
            else if (distance.LengthSquared() < 256 && hit)
                Projectile.timeLeft = 0;
        }
        void Reflect()
        {
            if (!hit)
            {
                Projectile.velocity = -Projectile.oldVelocity;
                hit = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 1)
                Reflect();
            else
                Projectile.timeLeft = 0;
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 playerPosition = Main.player[Projectile.owner].MountedCenter;
            int amount = 12;
            var diff = playerPosition - Projectile.Center;
            var interval = diff / amount;
            float rotDir = diff.ToRotation();
            Vector2 off = new Vector2(0, 9).RotatedBy(Projectile.rotation);

            var chainTexture = ModContent.Request<Texture2D>("MythosOfMoonlight/Projectiles/MortKnife/MortKnifeChain").Value;
            var chainRect = chainTexture.Frame();

            for (int i = 1; i < amount; i++)
            {
                Main.spriteBatch.Draw(chainTexture, Projectile.Center + (interval * i) + off - Main.screenPosition, chainRect, lightColor, rotDir, default, 1f, SpriteEffects.None, 0f);
            }

            var handleTexture = ModContent.Request<Texture2D>("MythosOfMoonlight/Projectiles/MortKnife/MortKnifeBase").Value;
            var handleRect = handleTexture.Frame();
            var handleOff = new Vector2(14, 9).RotatedBy(rotDir);

            //var drawData = new DrawData(handleTexture, interval * (amount - 1) + handleOff - Main.screenPosition, handleRect, lightColor, Projectile.rotation, default, 1f, SpriteEffects.None, 0);
            //Main.playerDrawData.Add(drawData);
            //drawData.Draw(Main.spriteBatch);


            /*
            Vector2 playerPosition = Main.player[Projectile.owner].MountedCenter;
            int amount = 12;
            var diff = playerPosition - Projectile.Center;
            var interval = diff / amount;
            float rotDir = diff.ToRotation();
            Vector2 off = new Vector2(0, 9).RotatedBy(Projectile.rotation);

            var handleTexture = mod.GetTexture("Projectiles/MortKnife/MortKnifeBase");
            var handleRect = handleTexture.Frame();
            Main.spriteBatch.Draw(handleTexture, playerPosition + off - Main.screenPosition, handleRect, Color.White, rotDir, default, 1f, SpriteEffects.None, 3f);
            */
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 300);
            Reflect();
        }
    }
}