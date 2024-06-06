using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ConstantSniping
{
    public class ConstantSnipingConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static ConstantSnipingConfig Instance;

        [Header("GeneralOptions")]

        [DefaultValue(30)]
        public int CrosshairCooldown;

        [DefaultValue(8)]
        public int CrosshairDuration;

        [DefaultValue(5)]
        public int CrosshairCooldownBoss;

        [DefaultValue(3)]
        public int CrosshairDurationBoss;


        [DefaultValue(99999)]
        [Range(1, 99999)]
        public int CrosshairDamage;

        [DefaultValue(1)]
        [Range(0.1f, 2f)]
        [DrawTicks()]
        [Increment(0.25f)]
        [Slider()]
        public float CrosshairSpeedMulti;
    }
}
