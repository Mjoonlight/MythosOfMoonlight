using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MythosOfMoonlight.Dusts;
using Terraria.DataStructures;
using MythosOfMoonlight.Projectiles.IridicProjectiles;
using MythosOfMoonlight.NPCs.Minibosses.RupturedPilgrim;

namespace MythosOfMoonlight.Common.Globals
{
    public class MoMGlobalProj : GlobalProjectile
    {
        public bool HasTarget = false;
        public int TargetIndex = -1;
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
            if (projectile.type == ProjectileID.FallingStar)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.type == ModContent.NPCType<Starine_Symbol>())
                    {
                        if (npc.ai[1] > 0 && Vector2.Distance(npc.Center, projectile.Center) < 420)
                            projectile.velocity = -projectile.velocity;
                    }
                }
            }
            return base.PreAI(projectile);
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
                if (target == null || !target.active || target.type == NPCID.TargetDummy || !Collision.CanHit(proj, target)) HasTarget = false;
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
