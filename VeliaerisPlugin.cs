using BepInEx;
using R2API.Utils;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Survivors.Veliaeris;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace VeliaerisMod
{
    //[BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class VeliaerisPlugin : BaseUnityPlugin
    {
        // if you do not change this, you are giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.karasil.VeliaerisMod";
        public const string MODNAME = "VeliaerisMod";
        public const string MODVERSION = "1.0.0";
        private const string ver = "1.1";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "KARASIL";

        public static VeliaerisPlugin instance;
        public static VeliaerisState previousSplitSate = VeliaerisState.Eris;
        public static VeliaerisState VeliaerisStates;
        public VeliaerisPassive passiveSkillSlot;
        public static bool hasVoid = true;
        public static bool CustomStagesInstalled => IsModInstalled("com.Nuxlar.CoolerStages");

        public static bool IsModInstalled(string GUID)
        {
            return BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID);
        }
        void Awake()
        {
            try
            {
                GameObject explosionPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/VoidRaidCrabSpawnEffect.prefab").WaitForCompletion();
            }
            catch
            {
                hasVoid = false;
            }
            System.Console.WriteLine("Check for void: " + hasVoid);
            Debug.Log("Current variant Version" + ver);
            instance = this;
//            On.RoR2.Networking.NetworkManagerSystemSteam.OnClientConnect += (s, u, t) => { };
            //easy to use logger
            Log.Init(Logger);

            // used when you want to properly set up language folders
            Modules.Language.Init();

            // character initialization
            new VeliaerisSurvivor().Initialize();

            // make a content pack and add it. this has to be last
            new Modules.ContentPacks().Initialize();
        }
    }
}
