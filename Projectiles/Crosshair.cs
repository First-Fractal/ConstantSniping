﻿using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConstantSniping.Projectiles
{
    public class Crosshair : ModProjectile
    {
        //variable for tracking the target
        Player target = null;
        public override void SetDefaults()
        {
            //make the  projectile invincible with custom ai and infinate peneration
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.tileCollide = false;
            Projectile.timeLeft = ffFunc.TimeToTick(ConstantSnipingConfig.Instance.CrosshairDuration);
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;

            base.SetDefaults();

            //change the time left if a boss is alive
            if (ffFunc.IsBossAlive())
            {
                Projectile.timeLeft = ffFunc.TimeToTick(ConstantSnipingConfig.Instance.CrosshairDurationBoss);
            }
        }

        //play the cocking sfx when spawing in
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(new SoundStyle("ConstantSniping/SFX/cock"));
            base.OnSpawn(source);
        }

        //changes the crosshiar speed based on progression
        public float CrosshairSpeedProgression()
        {
            //check if the player defeated any of the early bosses
            if (NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedSlimeKing) return 4f;
            //check if the player defeated the evil boss
            else if (NPC.downedBoss3) return 5f;
            else if (Main.hardMode) return 7f;
            else if (NPC.downedPlantBoss) return 8.5f;
            else if (NPC.downedAncientCultist) return 10f;
            else return 2.5f;
        }

        public override void AI()
        {
            //make it fade in
            if (Projectile.alpha > 0)
            {
                Projectile.alpha-=3;
            }

            //reset the target
            target = null;

            //loop through all of the players in the world
            foreach (Player player in Main.player)
            {
                //if their is not target, then set it to the first player that is active and not dead
                if (target == null)
                {
                    if (player.active && player.dead == false)
                    {
                        target = player;
                    }
                }
                else
                {
                    //get the distance between the current target and the current player
                    float tarDis = Vector2.Distance(Projectile.position, target.position);
                    float playerDis = Vector2.Distance(Projectile.position, player.position);


                    //check if the current player is closer then the target
                    if (playerDis < tarDis)
                    {
                        //if the closer player is not dead and active, then switch to that target
                        if (player.active && player.dead == false)
                        { 
                            target = player;
                        }
                    }
                }
            }

            //if their is a target, then move torwards it
            if (target != null)
            {
                //get the direction of the target
                Vector2 dir = new Vector2(target.position.X - Projectile.position.X, target.position.Y - Projectile.position.Y) + new Vector2(-35, -18);
                dir.Normalize();

                float spd = CrosshairSpeedProgression() * ConstantSnipingConfig.Instance.CrosshairSpeedMulti;

                //move torwards the target
                Projectile.position += dir * spd;

                //set the values for clamping the projectile
                float X = target.position.X - (target.width / 2);
                float Y = target.position.Y - (target.height / 2);
                float rad = 200f;

                //prevent the projectile from going too far from it's target
                Projectile.position.X = MathHelper.Clamp(Projectile.position.X, X - rad, X + rad);
                Projectile.position.Y = MathHelper.Clamp(Projectile.position.Y, Y - rad, Y + rad);
            }


            base.AI();
        }

        //make it unable to hurt any npc
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        //make it unable to hurt any player
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        //make it unable to hurt any player in pvp
        public override bool CanHitPvp(Player target)
        {
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            //make it player the shooting sfx when the time left is up
            SoundEngine.PlaySound(new SoundStyle("ConstantSniping/SFX/shoot"));

            //if the target is inside the crosshair, then shoot them
            if (target != null && Vector2.Distance(Projectile.position, target.position) < 45)
            {
                //deal the damage torwards the player that they cant dodge
                target.Hurt(PlayerDeathReason.ByProjectile(target.whoAmI, Projectile.whoAmI), ConstantSnipingConfig.Instance.CrosshairDamage, 0, dodgeable:false);
            }

            //run the vanilla code
            base.OnKill(timeLeft);
        }
    }
}
