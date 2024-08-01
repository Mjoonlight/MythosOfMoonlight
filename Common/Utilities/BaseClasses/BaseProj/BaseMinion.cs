using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MythosOfMoonlight.Common.Utilities.BaseClasses.BaseProj
{
    public abstract class BaseMinion : FriendlyProj
    {
        public int MinionOrderNum = 0;
        public void MinionFeatures(int type)
        {
            ProjectileID.Sets.MinionTargettingFeature[type] = true;
            ProjectileID.Sets.MinionSacrificable[type] = true;
            ProjectileID.Sets.CultistIsResistantTo[type] = true;
        }
        public void MinionDefaults(float slot)
        {
            Projectile.minion = true;
            Projectile.minionSlots = slot;
        }
        /// <summary>
        /// To sort all the minions of this type in a row.
        /// The minionOrderNum parameter returns the order of this minion in the row.
        /// </summary>
        public void MinionSort()
        {
            MinionOrderNum = 0;
            int type = Projectile.type;
            int index = Projectile.whoAmI;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile target = Main.projectile[i];
                if (target.active)
                {
                    if (target.type == type && target.whoAmI < index) MinionOrderNum++;
                }
            }
        }
        /// <summary>
        /// This function is for checking if the owner of this minion is still alive & active.
        /// </summary>
        /// <param name="buffType">the corrosponding buff type of this minion.</param>
        /// <param name="time">the timeleft this minion will be set after checking owner's active state.</param>
        public void OwnerCheckMinions(int buffType, int time)
        {
            Player owner = Main.player[Projectile.owner];
            if (!CheckActive(owner, buffType, time))
            {
                return;
            }
        }
        private bool CheckActive(Player owner, int buffType, int time)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(buffType);

                return false;
            }
            if (owner.HasBuff(buffType))
            {
                Projectile.timeLeft = time;
            }
            return true;
        }
        public void OwnerCheckMinions(int time, float itemType)
        {
            Player owner = Main.player[Projectile.owner];
            if (!CheckActive(owner, time, itemType))
            {
                return;
            }
        }
        private bool CheckActive(Player owner, int time, float itemType)
        {
            if (owner.dead || !owner.active || owner.HeldItem.type != (int)itemType || owner.HeldItem.IsAir)
            {
                return false;
            }
            else
            {
                Projectile.timeLeft = time;
            }
            return true;
        }
        /// <summary>
        /// This function is for minions to quickly search for possible targets.
        /// </summary>
        /// <param name="owner">the owner index of this minion.</param>
        /// <param name="targetIndex">the return value of its target's index.</param>
        /// <param name="checkRadius">the radius when searching for target.</param>
        /// <param name="deactiveRate">the rate of the radius where this minion gets passive comparing to the check radius.</param>
        /// <param name="ignoreBlocks">whether or not this minion will ignore blocks when searching for targets.</param>
        public void SearchTargets(Player owner, out int targetIndex, float checkRadius = 700f, float deactiveRate = 1.25f, bool ignoreBlocks = false)
        {
            bool foundTarget = false;
            targetIndex = -1;
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                if (Vector2.Distance(npc.Center, owner.Center) < checkRadius * deactiveRate)
                {
                    foundTarget = true;
                    targetIndex = npc.whoAmI;
                }
            }
            if (!foundTarget)
            {
                targetIndex = -1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        if (Vector2.Distance(npc.Center, owner.Center) <= checkRadius * deactiveRate)
                        {
                            float between = Vector2.Distance(npc.Center, Projectile.Center);
                            bool inRange = between < checkRadius;
                            bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                            bool closeThroughWall = between < 100f;
                            if (!ignoreBlocks)
                            {
                                if (inRange && (lineOfSight || closeThroughWall))
                                {
                                    targetIndex = i;
                                }
                            }
                            else
                            {
                                if (inRange)
                                {
                                    targetIndex = i;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void SearchTargets(Player owner, out int targetIndex, Vector2 seekOrigin, float maxSeekRange = 600f, float checkRadius = 700f, float deactiveRate = 1.25f, bool ignoreBlocks = false)
        {
            bool foundTarget = false;
            targetIndex = -1;
            if (Vector2.Distance(owner.Center, seekOrigin) > maxSeekRange)
            {
                seekOrigin = owner.Center + (seekOrigin - owner.Center) / (seekOrigin - owner.Center).Length() * maxSeekRange;
            }
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                if (Vector2.Distance(npc.Center, seekOrigin) < checkRadius * deactiveRate)
                {
                    foundTarget = true;
                    targetIndex = npc.whoAmI;
                }
            }
            if (!foundTarget)
            {
                targetIndex = -1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        if (Vector2.Distance(npc.Center, seekOrigin) <= checkRadius * deactiveRate)
                        {
                            float between = Vector2.Distance(npc.Center, Projectile.Center);
                            bool inRange = between < checkRadius;
                            bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                            bool closeThroughWall = between < 100f;
                            if (!ignoreBlocks)
                            {
                                if (inRange && (lineOfSight || closeThroughWall))
                                {
                                    targetIndex = i;
                                    break;
                                }
                            }
                            else
                            {
                                if (inRange)
                                {
                                    targetIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
