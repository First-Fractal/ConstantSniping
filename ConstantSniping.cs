using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using ConstantSniping.Projectiles;

namespace ConstantSniping
{
	public class ConstantSniping : Mod
	{
    }
    public class ConstantSnipingSystem : ModSystem
    {
        int counter = 0;
        public int spawnerCounter = 0;
        public void CrosshairTargeting()
        {
            counter++;
            if (counter >= 60)
            {
                spawnerCounter++;
                counter = 0;
            }

            if (spawnerCounter >= 10)
            {
                Player target;
                while (true) {
                    target = Main.player[Main.rand.Next(0, Main.player.Length)];
                    if (target.active)
                    {
                        break;
                    }
                }

                Projectile.NewProjectile(target.GetSource_FromThis(), target.position, Vector2.Zero, ModContent.ProjectileType<Crosshair>(), 0, 0);
                spawnerCounter = 0;
            }
        }

        public override void PostUpdateEverything()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                CrosshairTargeting();
            }
            base.PostUpdateEverything();
        }

        //spawn in the npcs for singleplayer
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