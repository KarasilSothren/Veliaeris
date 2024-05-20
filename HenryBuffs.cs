using RoR2;
using UnityEngine;

namespace HenryMod.Survivors.Henry
{
    public static class VeliaerisBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;
        public static BuffDef abyss;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            abyss = Modules.Content.CreateAndAddBuff("abyss",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.magenta,
                true,
                true);

        }
    }
}
