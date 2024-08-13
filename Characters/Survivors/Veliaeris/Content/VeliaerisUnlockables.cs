using VeliaerisMod.Survivors.Veliaeris.Achievements;
using RoR2;
using UnityEngine;

namespace VeliaerisMod.Survivors.Veliaeris
{
    public static class VeliaerisUnlockables
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
