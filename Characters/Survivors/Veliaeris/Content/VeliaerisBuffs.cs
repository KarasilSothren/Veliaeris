using RoR2;
using UnityEngine;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    public static class VeliaerisBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;
        public static BuffDef abyss;
        public static BuffDef buffOfAzelythtsa;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            abyss = Modules.Content.CreateAndAddBuff("abyss",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Blight").iconSprite,
                Color.magenta,
                true,
                true);

            buffOfAzelythtsa = Modules.Content.CreateAndAddBuff("blessingOfAzelythtsa",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.magenta,
                false,
                false);

        }
    }
}
