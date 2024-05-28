using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ConstantSniping
{
    public class ConstantSnipingConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static ConstantSnipingConfig Instance;

        [Header("GeneralOptions")]

        [DefaultValue(60)]
        public int CrosshairCooldown;


        [DefaultValue(5)]
        public int CrosshairDuration;


        [DefaultValue(99999)]
        [Range(1, 99999)]
        public int CrosshairDamage;

        [DefaultValue(1)]
        [Range(1, 10)]
        [DrawTicks()]
        [Slider()]
        public int CrosshairSpeed;
    }
}
