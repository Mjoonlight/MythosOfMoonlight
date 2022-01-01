using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MythosOfMoonlight.NPCs.Enemies.EntropicTotem.EntropicTotemProjectile;
using MythosOfMoonlight.Dusts;

namespace MythosOfMoonlight.NPCs.Enemies.EntropicTotem
{
    public class EntropicTotem : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropic Totem");
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 62;
            npc.height = 70;
            npc.aiStyle = -1;
            npc.damage = 15;
            npc.defense = 10;
            npc.lifeMax = 260;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath43;
        }

        const float SPEED = 4.2f, MINIMUM_DISTANCE = 60f;
        int State
        {
            get => (int)npc.ai[0];
            set => npc.ai[0] = value;
        }
        Vector2 direction;
        void IncreaseFrameCounter() => npc.frameCounter++;
        void GetTarget() => npc.TargetClosest();
        void MovementLogic() // self explanatory
        {
            var player = Main.player[npc.target];
            var distance = npc.DistanceSQ(player.position); // check distance between player and npc
            if (distance >= MINIMUM_DISTANCE * MINIMUM_DISTANCE) // if horizontal distance exceeds the minimum distance, change npc direction to aim at player
            {
                direction = -(npc.position - player.position).SafeNormalize(Vector2.UnitX);
            }
            switch (State)
            {
                case 0: // when not firing the projectiles
                    npc.velocity = Vector2.Lerp(npc.velocity, direction * SPEED, 0.03f);
                    break;
                case 1: // while firing the projectiles
                    npc.velocity = Vector2.Lerp(npc.velocity, direction * SPEED/2f, 0.03f);
                    break;
            }
            
        }
        void TiltSprite() => npc.rotation = MathHelper.Clamp(npc.velocity.X * .25f, MathHelper.ToRadians(-30), MathHelper.ToRadians(30));
        void StateTransitionManagement()
        {
            var maxTime = EntropicTotemProjectile.EntropicTotemProjectile.MAX_TIMELEFT;
            if (State == 0 && npc.frameCounter > maxTime * 2) // fires every 2 projectile lifespans
            {
                int etpType = ModContent.ProjectileType<EntropicTotemProjectile.EntropicTotemProjectile>();
                for (int i = 0; i < 4; i++)
                {
                    var projectile = Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(i * MathHelper.PiOver2) * 3, etpType, npc.damage, 2, 255);
                    Main.projectile[projectile].ai[0] = 3f;
                    Main.projectile[projectile].ai[1] = npc.whoAmI;
                }
                State = 1;
            }
            else if (npc.frameCounter > maxTime * 3)
            {
                State = 0;
                npc.frameCounter = 0;
            }
        }
        int AnimationFrame
        {
            get => (int)npc.ai[1];
            set => npc.ai[1] = value;
        }
        int TrueFrame => AnimationFrame / FRAME_RATE;
        const int FRAME_RATE = 5;
        public override void FindFrame(int frameHeight)
        {
            if (AnimationFrame++ % FRAME_RATE == 0) // every 5 frames, go to next step of animation
            {
                var trueFrame = AnimationFrame / FRAME_RATE;
                npc.frame.Y = frameHeight * trueFrame;
                if (trueFrame > Main.npcFrameCount[npc.type] - 2)
                {
                    AnimationFrame = 0;
                }
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            int dustAmount = 5;
            if (npc.life <= 0)
            {
                Helper.SpawnGore(npc, "Gores/Enemies/EntroTotem", 1, 1);
                Helper.SpawnGore(npc, "Gores/Enemies/EntroTotem", 8, 2);
                Helper.SpawnGore(npc, "Gores/Enemies/EntroTotem", 3, 3);
                Helper.SpawnGore(npc, "Gores/Enemies/EntroTotem", 6, 4);
                Helper.SpawnDust(npc.position, npc.Size, ModContent.DustType<EntropicTotemProjectileDust>(), new Vector2(-hitDirection * Math.Abs(npc.oldVelocity.X), -1.5f), dustAmount);
                dustAmount = 10;

            }
            Helper.SpawnDust(npc.position, npc.Size, DustID.Stone, new Vector2(-hitDirection * Math.Abs(npc.oldVelocity.X), -1.5f), dustAmount);

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //3hi31mg
            var clr = new Color(255, 255, 255, 255); // full white
            var drawPos = npc.Center - Main.screenPosition;
            var texture = mod.GetTexture("NPCs/Enemies/EntropicTotem/EntropicTotem_Glow");
            var origTexture = Main.npcTexture[npc.type];
            var frame = new Rectangle(0, npc.frame.Y, npc.width, npc.height);
            var orig = frame.Size() / 2f;
            Main.spriteBatch.Draw(origTexture, drawPos, frame, drawColor, npc.rotation, orig, npc.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawPos, frame, clr, npc.rotation, orig, npc.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void AI()
        {
            IncreaseFrameCounter();
            GetTarget();
            MovementLogic();
            TiltSprite();
            StateTransitionManagement();
        }
    }
}
