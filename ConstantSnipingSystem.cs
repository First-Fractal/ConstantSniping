using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ConstantSniping.Projectiles;

namespace ConstantSniping
{
    public class ConstantSnipingSystem : ModSystem
    {
        //timer variables for countdown
        int counter = 0;
        public int spawnerCounter = 0;


        //function for spawning in the crosshiar
        public void CrosshairTargeting()
        {
            //count up the cooldown every second
            counter++;
            if (counter >= ffFunc.TimeToTick(1))
            {
                spawnerCounter++;
                counter = 0;
            }

            //if it's time to spawn in, then spawn it in
            if (spawnerCounter >= ConstantSnipingConfig.Instance.CrosshairCooldown)
            {
                //look for a target that is active and not dead
                Player target;
                while (true)
                {
                    target = Main.player[Main.rand.Next(0, Main.player.Length)];
                    if (target.active && !target.dead)
                    {
                        break;
                    }
                }

                //spawn in the projectile
                Projectile.NewProjectile(target.GetSource_FromThis(), target.position, Vector2.Zero, ModContent.ProjectileType<Crosshair>(), 0, 0);
                spawnerCounter = 0;
            }
        }

        //spawn in the crosshiar in multiplayer
        public override void PostUpdateEverything()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                CrosshairTargeting();
            }
            base.PostUpdateEverything();
        }

        //spawn in the crosshiar in singleplayer
        public override void PostUpdateWorld()
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                CrosshairTargeting();
            }
            base.PostUpdateWorld();
        }
    }
}
