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
using Terraria.Audio;
using Terraria.DataStructures;
using MythosOfMoonlight.Projectiles.IridicProjectiles;

namespace MythosOfMoonlight.Common.Globals
{
    public class MoMGlobalProj : GlobalProjectile
    {
        public bool HasTarget = false;
        public int TargetIndex = -1;
        public bool CommunicatorEffect = false;
        public float CommunicatorCD;
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.friendly && ProjectileID.Sets.CultistIsResistantTo[projectile.type] == true)
            {
                HasTarget = false;
                TargetIndex = -1;
            }
        }
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.minion || projectile.sentry)
            {
                if (Main.player[projectile.owner].GetModPlayer<MoMPlayer>().CommunicatorEquip)
                {
                    if (CommunicatorCD <= 0)
                    {
                        CommunicatorEffect = true;
                    }
                    else
                    {
                        CommunicatorCD--;
                        CommunicatorEffect = false;
                    }
                    if (CommunicatorEffect)
                    {
                        if (Main.rand.NextBool(4))
                        {
                            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<PurpurineDust>(), 0, 0, 0, default, Main.rand.NextFloat(1f, 1.8f));
                            // dust.noGravity = true;
                        }
                    }
                }
            }
            return base.PreAI(projectile);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.DamageType == DamageClass.Summon && projectile.type != ModContent.ProjectileType<IrisStar>())
            {
                Player player = Main.player[projectile.owner];
                if (player.GetModPlayer<MoMPlayer>().CommunicatorEquip)
                {
                    if (CommunicatorCD <= 0)
                    {
                        Vector2 vel = Utils.SafeNormalize(projectile.Center - target.Center, Vector2.Zero) * 8f;
                        float randRot = Main.rand.Next<int>(new int[4] { -90, 90, -45, 45 });
                        Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center + vel * 5, vel.RotatedBy(MathHelper.ToRadians(randRot)), ModContent.ProjectileType<IrisStar>(), 10, 1f, projectile.owner);
                        proj.DamageType = DamageClass.Summon;
                        proj.tileCollide = false;
                        CommunicatorCD = 300;
                        MoMGlobalProj pro = proj.GetGlobalProjectile<MoMGlobalProj>();
                        pro.HasTarget = true;
                        pro.TargetIndex = target.whoAmI;
                    }
                }
            }
        }
        public bool TryFindTarget(Projectile proj, float radius)
        {
            bool find = false;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    if (!npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage)
                    {
                        if (Vector2.Distance(npc.Center, proj.Center) <= radius)
                        {
                            find = true;
                            TargetIndex = npc.whoAmI;
                        }
                    }
                }
            }
            return find;
        }
        public void HomingActions(Projectile proj, float homingSpeed, float maxSpeed, float radius)
        {
            if (!HasTarget)
            {
                HasTarget = TryFindTarget(proj, radius);
            }
            if (TargetIndex != -1)
            {
                NPC target = Main.npc[TargetIndex];
                if (target == null || !target.active) HasTarget = false;
                else
                {
                    if (Vector2.Distance(target.Center, proj.Center) > radius * 2f) HasTarget = false;
                }
                if (!HasTarget)
                {
                    TryFindTarget(proj, radius);
                }
                else
                {
                    if (proj.velocity.Length() >= maxSpeed && Vector2.Distance(target.Center, proj.Center) > radius / 5f)
                    {
                        proj.velocity = (proj.velocity + Utils.SafeNormalize(target.Center - proj.Center, Vector2.Zero) * proj.velocity.Length() * homingSpeed) / (1 + homingSpeed);
                    }
                    else
                    {
                        proj.velocity = proj.velocity * (1 - homingSpeed) + Utils.SafeNormalize(target.Center - proj.Center, Vector2.Zero) * proj.velocity.Length() * homingSpeed * 1.33f;
                    }
                }
            }
        }
    }
}
