﻿using VeliaerisMod.Survivors.Henry.Achievements;
using RoR2;
using UnityEngine;

namespace VeliaerisMod.Survivors.Henry
{
    public static class HenryUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            //masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
            //    HenryMasteryAchievement.unlockableIdentifier,
            //    Modules.Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier),
            //    VeliaerisSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
