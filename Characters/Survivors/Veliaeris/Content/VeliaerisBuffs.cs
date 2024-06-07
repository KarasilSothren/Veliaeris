﻿using RoR2;
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
        public static BuffDef missingSibling;
        public static BuffDef splitRevive;
        public static BuffDef switchInvincibility;
        public static BuffDef healthBlessing;
        public static BuffDef damageBlessing;
        public static BuffDef revokeDeath;
        public static BuffDef inflictDeath;

        public static void Init(AssetBundle assetBundle)
        {

            Color Abyss = new Color(71,0,69); 

            armorDesolation = Modules.Content.CreateAndAddBuff("destroyedArmor",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Pulverized").iconSprite,
                Color.magenta,
                true,
                true);

            abyss = Modules.Content.CreateAndAddBuff("abyss",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Blight").iconSprite,
                Abyss,
                true,
                true);

            sistersBlessing = Modules.Content.CreateAndAddBuff("blessingOfAzelythtsa",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.gray,
                false,
                false);

            lesserSistersBlessing = Modules.Content.CreateAndAddBuff("lesserBlessingOfAzelythtsa",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.cyan,
                false,
                false);
            missingSibling = Modules.Content.CreateAndAddBuff("missingsbibling",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Blight").iconSprite,
                Color.blue,
                false,
                false);

            splitRevive = Modules.Content.CreateAndAddBuff("SistersEnduranceCoolDown",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.red,
                false,
                false);
            switchInvincibility = Modules.Content.CreateAndAddBuff("switchInvincibility",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.blue,
                false,
                false);

            healthBlessing = Modules.Content.CreateAndAddBuff("BlessingofHealth",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.green,
                true,
                false);
            damageBlessing = Modules.Content.CreateAndAddBuff("BlessingofDamage",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Blight").iconSprite,
                Color.red,
                true,
                false);

            revokeDeath = Modules.Content.CreateAndAddBuff("PreventDeath",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.magenta,
                true,
                false);

            inflictDeath = Modules.Content.CreateAndAddBuff("VoidCorruption",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Blight").iconSprite,
                Color.black,
                true,
                false);
        }
    }
}
