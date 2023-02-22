using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace ConstantSniping.Projectiles
{
    public class Crosshair : ModProjectile
    {
        Player target;
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(new SoundStyle("ConstantSniping/SFX/cock"));
            base.OnSpawn(source);
        }

        public override void AI()
        {

            if (Projectile.alpha > 0)
            {
                Projectile.alpha-=3;
            }

            target = null;
            foreach (Player player in Main.player)
            {
                if (target == null)
                {
                    if (player.active && player.dead == false)
                    {
                        target = player;
                    }
                }
                else
                {
                    float tarDis = Vector2.Distance(Projectile.position, target.position);
                    float playerDis = Vector2.Distance(Projectile.position, player.position);

                    if (playerDis < tarDis)
                    {
                        if (player.active && player.dead == false)
                        { 
                            target = player;
                        }
                    }
                }
            }

            if (target != null)
            {
                Vector2 dir = new Vector2(target.position.X - Projectile.position.X, target.position.Y - Projectile.position.Y) + new Vector2(-35, -18);
                Projectile.position += dir/10;
            }

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
            SoundEngine.PlaySound(new SoundStyle("ConstantSniping/SFX/shoot"));
            int dis = 45;
            //if (Main.netMode == NetmodeID.MultiplayerClient) { dis = 43; }
            if (Vector2.Distance(Projectile.position, target.position) < dis)
            {
                target.Hurt(PlayerDeathReason.ByProjectile(target.whoAmI, Projectile.whoAmI), 99999, 0);
            }
            base.Kill(timeLeft);
        }
    }
}
