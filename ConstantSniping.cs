using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using ConstantSniping.Projectiles;
using System.ComponentModel;
using Terraria.ModLoader.Config;

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

            if (spawnerCounter >= ConstantSnipingConfig.Instance.CrosshairCooldown)
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

    [Label("$Mods.ConstantSniping.Config.Label")]
    public class ConstantSnipingConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static ConstantSnipingConfig Instance;

        [Header("$Mods.ConstantSniping.Config.Header.GeneralOptions")]

        [Label("$Mods.ConstantSniping.Config.CrosshairCooldown.Label")]
        [Tooltip("$Mods.ConstantSniping.Config.CrosshairCooldown.Tooltip")]
        [DefaultValue(60)]
        public int CrosshairCooldown;

        [Label("$Mods.ConstantSniping.Config.CrosshairDuration.Label")]
        [Tooltip("$Mods.ConstantSniping.Config.CrosshairDuration.Tooltip")]
        [DefaultValue(5)]
        public int CrosshairDuration;

        [Label("$Mods.ConstantSniping.Config.CrosshairDamage.Label")]
        [Tooltip("$Mods.ConstantSniping.Config.CrosshairDamage.Tooltip")]
        [DefaultValue(99999)]
        [Range(1, 99999)]
        public int CrosshairDamage;

        [Label("$Mods.ConstantSniping.Config.CrosshairSpeed.Label")]
        [Tooltip("$Mods.ConstantSniping.Config.CrosshairSpeed.Tooltip")]
        [DefaultValue(1)]
        [Range(1, 10)]
        [DrawTicks()]
        [Slider()]
        public int CrosshairSpeed;
    }
}