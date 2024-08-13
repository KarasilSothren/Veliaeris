using BepInEx;
using HG.Reflection;
using R2API.Networking;
using R2API.Utils;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "LoadoutAPI",
        "UnlockableAPI",
        "NetworkingAPI",
        "RecalculateStatsAPI",
        "DotAPI",
        "OrbAPI",
        "DamageTypeAPI",
        "R2API"
    })]
    public class VeliaerisPlugin : BaseUnityPlugin
    {
        // if you do not change this, you are giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.karasil.VeliaerisMod";
        public const string MODNAME = "VeliaerisMod";
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "KARASIL";

        public static VeliaerisPlugin instance;
        public VeliaerisPassive passiveSkillSlot;
        public static bool hasVoid =>hasVoidInstalled();
        public static bool CustomStagesEnabled => IsModEnabled("CoolerStages");
        public static bool DanCustomStagesEnabled => IsModEnabled("DanAesthetic");
                public static string pod = "Prefabs/NetworkedObjects/SurvivorPod";
//        public static string pod = "Prefabs/NetworkedObjects/VoidSurvivorPod";
        public static bool IsModInstalled(string GUID)
        {
            Debug.Log("ismodinstalled");
            return BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GUID);
        }
        public static bool IsModEnabled(string modFolderName)
        {   bool isEnabled = false;
            string path = BepInEx.Paths.BepInExRootPath;
            string modFolderPath = path + "\\plugins";
            string[] folderList = Directory.GetDirectories(modFolderPath);
            for(int i = 0; i < folderList.Length; i++)
            {
                string[] temp = Directory.GetFiles(folderList[i]);
                for (int j = 0; j < temp.Length; j++)
                {
                    if (temp[j].Contains(modFolderName))
                    {
                        if (temp[j].Contains(".old"))
                        {
                            isEnabled = false;
                        }
                        else
                        {
                            isEnabled = true;
                        }
                    }
                }
            }
            return isEnabled;
        }
        public static bool hasVoidInstalled()
        {
            bool isEnabled = false;
            Queue<SystemInitializerAttribute> queue = new Queue<SystemInitializerAttribute>();
            foreach (HG.Reflection.SearchableAttribute searchableAttribute in HG.Reflection.SearchableAttribute.GetInstances<SystemInitializerAttribute>())
            {
                SystemInitializerAttribute systemInitializerAttribute = (SystemInitializerAttribute)searchableAttribute;
                MethodInfo methodInfo = systemInitializerAttribute.target as MethodInfo;
                if (methodInfo != null && methodInfo.IsStatic)
                {
                    queue.Enqueue(systemInitializerAttribute);
                    systemInitializerAttribute.associatedType = methodInfo.DeclaringType;
                }
            }
//            Debug.Log("queue"+queue.Count);

            while (queue.Count > 0)
            {
                SystemInitializerAttribute systemInitializerAttribute2 = queue.Dequeue();
//                System.Console.WriteLine("inialized types custom:" + systemInitializerAttribute2.associatedType.ToString());
                if (systemInitializerAttribute2.associatedType.ToString() == "RoR2.Items.ExtraLifeVoidManager")
                {
                    isEnabled = true;
                }
            }
            return isEnabled;
        }

        void Awake()
        {
           
            
            NetworkingAPI.RegisterMessageType<SyncStoredStates>();
            NetworkingAPI.RegisterMessageType<SyncStates>();
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
