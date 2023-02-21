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
        public void Talk(string message, Color color)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(message, color);
            }
            else
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
            }
        }
    }
    public class ConstantSnipingSystem : ModSystem
    {
        int counter = 0;
        public int spawnerCounter = 0;
        ConstantSniping CS = new ConstantSniping();
        public void bomberPlaneSpawner()
        {
            counter++;
            if (counter >= 60)
            {
                spawnerCounter++;
                counter = 0;
                CS.Talk(spawnerCounter.ToString(), Color.LightSeaGreen);
            }

            if (spawnerCounter >= 5)
            {
                CS.Talk("Target Sighted", Color.OrangeRed);

                Player target = null;
                while (true) {
                    target = Main.player[Main.rand.Next(0, Main.player.Length)];
                    if (target.active)
                    {
                        break;
                    }
                }

                CS.Talk(target.position.ToString(), Color.OliveDrab);
                Projectile.NewProjectile(target.GetSource_FromThis(), target.position, Vector2.Zero, ModContent.ProjectileType<Crosshair>(), 0, 0);

                spawnerCounter = 0;
            }
        }

        public override void PostUpdateEverything()
        {
            bomberPlaneSpawner();
            base.PostUpdateEverything();
        }
    }
}