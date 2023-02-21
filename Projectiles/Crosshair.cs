using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ConstantSniping.Projectiles
{
    public class Crosshair : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 800;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;

            base.SetDefaults();
        }

        

        public override void AI()
        {

            if (Projectile.alpha > 0)
            {
                Projectile.alpha-=3;
            }

            Player target = null;
            foreach (Player player in Main.player)
            {
                if (target == null)
                {
                    target = player;
                }
                else
                {
                    float tarDis = Vector2.Distance(Projectile.position, target.position);
                    float playerDis = Vector2.Distance(Projectile.position, player.position);

                    if (playerDis < tarDis)
                    {
                        target = player;
                    }
                }
            }


            Vector2 dir = new Vector2(target.position.X - Projectile.position.X, target.position.Y - Projectile.position.Y) + new Vector2(-35, -18);
            dir = Vector2.Normalize(dir);

            Projectile.position += dir * 3;

            base.AI();
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.NewText("insert a gunshot here");
            base.Kill(timeLeft);
        }
    }
}
