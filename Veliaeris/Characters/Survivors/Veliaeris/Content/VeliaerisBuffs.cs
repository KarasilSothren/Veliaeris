using RoR2;
using UnityEngine;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    public static class VeliaerisBuffs
    {
        // armor buff gained during roll
        public static BuffDef abyss;
        public static BuffDef sistersBlessing;
        public static BuffDef lesserSistersBlessing;
        public static BuffDef armorDesolation;

        public static void Init(AssetBundle assetBundle)
        {


            armorDesolation = Modules.Content.CreateAndAddBuff("destroyedArmor",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Pulverized").iconSprite,
                Color.magenta,
                true,
                true);

            abyss = Modules.Content.CreateAndAddBuff("abyss",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Blight").iconSprite,
                Color.magenta,
                true,
                true);

            sistersBlessing = Modules.Content.CreateAndAddBuff("blessingOfAzelythtsa",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.magenta,
                false,
                false);
            lesserSistersBlessing = Modules.Content.CreateAndAddBuff("lesserBlessingOfAzelythtsa",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.magenta,
                false,
                false);

        }
    }
}
