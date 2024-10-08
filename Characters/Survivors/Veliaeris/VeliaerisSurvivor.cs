﻿using BepInEx.Configuration;
using VeliaerisMod.Modules;
using VeliaerisMod.Modules.Characters;
//using VeliaerisMod.Survivors.Veliaeris.Components;
using VeliaerisMod.Survivors.Veliaeris.SkillStates;
using R2API;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using EntityStates;
using System.Runtime.CompilerServices;
using VeliaerisMod;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Characters.Survivors.Veliaeris;
using UnityEngine.Networking;
using VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates;
using UnityEngine.SceneManagement;
using RoR2.UI;
using RoR2.EntitlementManagement;
using RoR2.ExpansionManagement;

namespace VeliaerisMod.Survivors.Veliaeris
{
    public class VeliaerisSurvivor : SurvivorBase<VeliaerisSurvivor>
    {








        #region skillDefines
        #region utility
        public static VeliaerisRespawnSkillDef MergeandShiftSkill;
        public static SkillDef split;
        #endregion utility
        #region primary
        public static SkillDef basicScythe;
        public static SkillDef reductionScythe;
        public static HuntressTrackingSkillDef voidSkillDef;
        #endregion primary
        #region secondary
        public static SkillDef allyBuff;
        public static SkillDef circularSlash;
        public static HuntressTrackingSkillDef CorruptAndHeal;
        #endregion secondary
        #region special
        public static SkillDef voidDetonation;
        public static SkillDef eldritchHealing;
        public static SkillDef selfBuffer;
        public static SkillDef callUponSister;
        #endregion special
        #endregion skillDefines
        #region CorruptedskillDefines
        #region utility
        public static VeliaerisRespawnSkillDef MergeandShiftSkillCorrupted;
        public static SkillDef splitCorrupted;
        public static VeliaerisRespawnSkillDef MergeandShiftSkillIllusionCorrupted;
        public static SkillDef splitIllusionCorrupted;
        #endregion utility
        #region primary
        public static SkillDef basicScytheCorrupted;
        public static SkillDef reductionScytheCorrupted;
        public static HuntressTrackingSkillDef voidSkillDefCorrupted;
        #endregion primary
        #region secondary
        public static SkillDef allyBuffCorrupted;
        public static SkillDef circularSlashCorrupted;
        public static HuntressTrackingSkillDef CorruptAndHealCorrupted;
        #endregion secondary
        #region special
        public static SkillDef voidDetonationCorrupted;
        public static SkillDef eldritchHealingCorrupted;
        public static SkillDef selfBufferCorrupted;
        public static SkillDef callUponSisterCorrupted;
        #endregion special
        #endregion skillDefines
        #region skillcooldowns
        public static float utilitycooldown = 10f;
        public static float veliaerissecondarycooldown =5f;
        public static float veliaerisspecialcooldown = 30f;
        public static float erisprimarycooldown = 2f;
        public static float erissecondarycooldown = 15f;
        public static float erisspecialcooldown = 40f;
        public static float veliasecondarycooldown = 20f;
        public static float veliaspecialcooldown = 30f;
        #endregion
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "myassetbundlesr"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "VeliaerisBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "VeliaerisMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlHenry";
        public override string displayPrefabName => "HenryDisplay";
        public const string VELIAERIS_PREFIX = VeliaerisPlugin.DEVELOPER_PREFIX + "_VELIAERIS_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => VELIAERIS_PREFIX;
        Color VoidDisplay = new Color(82f,1f,75f);
        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = VELIAERIS_PREFIX + "NAME",
            subtitleNameToken = VELIAERIS_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texHenryIcon"),
            bodyColor = new Color(143f / 216f, 3f / 255f, 140f / 255f),
            sortPosition = 100,

            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = null,
//            podPrefab = LegacyResourcesAPI.Load<GameObject>(VeliaerisPlugin.pod),
//            moveSpeed = 100f,
            maxHealth = 120f,/*original max health 110f*/
            healthRegen = 1.5f,/*original healthRegen 1.5f*/
            armor = 10f,/*original armor 0f*/

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = assetBundle.LoadMaterial("matHenry"),
                },
                new CustomRendererInfo
                {
                    childName = "GunModel",
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                }
        };

        public override UnlockableDef characterUnlockableDef => VeliaerisUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays => null;

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }
        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "Henry");

            //if (!characterEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            VeliaerisUnlockables.Init();

            base.InitializeCharacter();
            VeliaerisConfig.Init();
            VeliaerisStatuses.Init();
            VeliaerisTokens.Init();
            VeliaerisBuffs.Init(assetBundle);
            VeliaerisDots.Init();
            DamageTypes.Init();
            VeliaerisAssets.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
//            bodyPrefab.AddComponent<HenryWeaponComponent>();
            bodyPrefab.AddComponent<VeliaerisStatuses>();
            bodyPrefab.AddComponent<VeliaerisSurvivorController>();
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            Prefabs.SetupHitBoxGroup(characterModelObject, "SwordGroup", "SwordHitbox");
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            //Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
                //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Switch");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Buff");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Detonator");

        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();
        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            //bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            //{
            //    enabled = true,
            //    skillNameToken = VELIAERIS_PREFIX + "PASSIVE_NAME",
            //    skillDescriptionToken = VELIAERIS_PREFIX + "PASSIVE_DESCRIPTION",
            //    icon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            //};
            VeliaerisPassive passiveSkillSlot = bodyPrefab.AddComponent<VeliaerisPassive>();
            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            passiveSkillSlot.passiveSkillSlot = Modules.Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "Passive",true);
            SkillLocator skilllocat = bodyPrefab.GetComponent<SkillLocator>();
            skilllocat.passiveSkill.enabled = true;
            skilllocat.passiveSkill.skillNameToken = VELIAERIS_PREFIX + "PASSIVE_NAME";
            skilllocat.passiveSkill.skillDescriptionToken = VELIAERIS_PREFIX + "PASSIVE_DESCRIPTION";
            skilllocat.passiveSkill.keywordToken = VELIAERIS_PREFIX + "SKILL_INFORMATION";
            skilllocat.passiveSkill.icon = assetBundle.LoadAsset<Sprite>("texPassiveIcon");


            passiveSkillSlot.VeliaerisStart = Skills.CreateSkillDef(new SkillDefInfo
            {
                
                skillName = "VeliaerisPassive",
                skillNameToken = VELIAERIS_PREFIX + "VELIAERIS_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "SELECTION_PASSIVE_DESCRIPTION_VELIAERIS",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

                //unless you're somehow activating your passive like a skill, none of the following is needed.
                //but that's just me saying things. the tools are here at your disposal to do whatever you like with

                //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                //activationStateMachineName = "Weapon1",
                //interruptPriority = EntityStates.InterruptPriority.Skill,

                //baseRechargeInterval = 1f,
                //baseMaxStock = 1,

                //rechargeStock = 1,
                //requiredStock = 1,
                //stockToConsume = 1,

                //resetCooldownTimerOnUse = false,
                //fullRestockOnAssign = true,
                //dontAllowPastMaxStocks = false,
                //mustKeyPress = false,
                //beginSkillCooldownOnSkillEnd = false,

                //isCombatSkill = true,
                //canceledFromSprinting = false,
                //cancelSprintingOnActivation = false,
                //forceSprintDuringState = false,

            });
            passiveSkillSlot.ErisStart = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ErisPassive",
                skillNameToken = VELIAERIS_PREFIX + "ERIS_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "SELECTION_PASSIVE_DESCRIPTION_ERIS",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

            });

            passiveSkillSlot.VeliaStart = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VeliaPassive",
                skillNameToken = VELIAERIS_PREFIX + "VELIA_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "SELECTION_PASSIVE_DESCRIPTION_VELIA",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

            });
            AddPassiveSkills(passiveSkillSlot.passiveSkillSlot.skillFamily, new SkillDef[]
            {
                passiveSkillSlot.VeliaerisStart,
                passiveSkillSlot.ErisStart,
                passiveSkillSlot.VeliaStart
            });

            
        }
        public static void AddPassiveSkills(SkillFamily passiveSkillFamily, params SkillDef[] skillDefs)
        {
            Modules.Skills.AddSkillsToFamily(passiveSkillFamily, skillDefs);
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        public void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            //SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            //    (
            //        "HenrySlash",
            //        VELIAERIS_PREFIX + "PRIMARY_SLASH_NAME",
            //        VELIAERIS_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
            //        assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
            //        new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)),
            //        "Weapon",
            //        true
            //    ));
            ////custom Skilldefs can have additional fields that you can set manually
            //primarySkillDef1.stepCount = 2;
            //primarySkillDef1.stepGraceDuration = 0.5f;

            //Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
            basicScythe = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "BasicScythe",
                skillNameToken = VELIAERIS_PREFIX + "VELIAERIS_PRIMARY_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIAERIS_PRIMARY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "ABYSS_INFORMATION" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(BasicScytheSlash)),
                activationStateMachineName = "Weapon",
                isCombatSkill = true,
                canceledFromSprinting = false,
            }
                );
            reductionScythe = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName= "VoidScythe",
                skillNameToken=VELIAERIS_PREFIX + "VELIA_PRIMARY_NAME",
                skillDescriptionToken=VELIAERIS_PREFIX + "VELIA_PRIMARY_DESCRIPTION",
                skillIcon=assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                keywordTokens=new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "ABYSS_INFORMATION" },
                activationState=new EntityStates.SerializableEntityStateType(typeof(BasicScytheSlashWithReductions)),
                activationStateMachineName= "Weapon",
                isCombatSkill=true,
                canceledFromSprinting = false,
                });

            voidSkillDef = Skills.CreateSkillDef<HuntressTrackingSkillDef>(new SkillDefInfo
            {
                skillName = "Grasp",
                skillNameToken = VELIAERIS_PREFIX + "ERIS_PRIMARY_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "ERIS_PRIMARY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(GraspOfOblivion)),
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = erisprimarycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            basicScytheCorrupted = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "BasicScythe",
                skillNameToken = VELIAERIS_PREFIX + "VELIAERIS_PRIMARY_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIAERIS_PRIMARY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "ABYSS_INFORMATION" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(BasicScytheSlash)),
                activationStateMachineName = "Weapon",
                isCombatSkill = true,
                canceledFromSprinting = false,
            }
                );
            reductionScytheCorrupted = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VoidScythe",
                skillNameToken = VELIAERIS_PREFIX + "VELIA_PRIMARY_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIA_PRIMARY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "ABYSS_INFORMATION" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(BasicScytheSlashWithReductions)),
                activationStateMachineName = "Weapon",
                isCombatSkill = true,
                canceledFromSprinting = false,
            });

            voidSkillDefCorrupted = Skills.CreateSkillDef<HuntressTrackingSkillDef>(new SkillDefInfo
            {
                skillName = "Grasp",
                skillNameToken = VELIAERIS_PREFIX + "ERIS_PRIMARY_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "ERIS_PRIMARY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(GraspOfOblivion)),
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = erisprimarycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            Skills.AddPrimarySkills(bodyPrefab, basicScythe,voidSkillDef,reductionScythe);
        }

        public void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            




            CorruptAndHeal = Skills.CreateSkillDef<HuntressTrackingSkillDef>(new SkillDefInfo
            {
                skillName = "HealandCorrupt",
                skillNameToken = VELIAERIS_PREFIX + "VELIAERIS_SECONDARY_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIAERIS_SECONDARY_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "ABYSS_INFORMATION" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Corrupt)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = veliaerissecondarycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });





            allyBuff = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "TeamBuff",
                skillNameToken = VELIAERIS_PREFIX + "ERIS_SECONDARY_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "ERIS_SECONDARY_DESCRIPTION",
                keywordTokens = new string[] { VELIAERIS_PREFIX + "ABYSS_INFORMATION" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(GivenStrength)),
                activationStateMachineName = "Buff",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = erissecondarycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,


            });

            circularSlash = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AoESlash",
                skillNameToken = VELIAERIS_PREFIX + "VELIA_SECONDARY_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIA_SECONDARY_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "ABYSS_INFORMATION", VELIAERIS_PREFIX + "PERCENT_HEALTH_DAMAGE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(CircularSlash)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = veliasecondarycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });


            CorruptAndHealCorrupted = Skills.CreateSkillDef<HuntressTrackingSkillDef>(new SkillDefInfo
            {
                skillName = "HealandCorrupt",
                skillNameToken = VELIAERIS_PREFIX + "VELIAERIS_SECONDARY_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIAERIS_SECONDARY_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "ABYSS_INFORMATION" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Corrupt)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = veliaerissecondarycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });





            allyBuffCorrupted = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "TeamBuff",
                skillNameToken = VELIAERIS_PREFIX + "ERIS_SECONDARY_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "ERIS_SECONDARY_DESCRIPTION",
                keywordTokens = new string[] { VELIAERIS_PREFIX + "ABYSS_INFORMATION" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(GivenStrength)),
                activationStateMachineName = "Buff",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = erissecondarycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,


            });

            circularSlashCorrupted = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AoESlash",
                skillNameToken = VELIAERIS_PREFIX + "VELIA_SECONDARY_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIA_SECONDARY_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "ABYSS_INFORMATION", VELIAERIS_PREFIX + "PERCENT_HEALTH_DAMAGE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(CircularSlash)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = veliasecondarycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            Skills.AddSecondarySkills(bodyPrefab, CorruptAndHeal,allyBuff,circularSlash);
        }

        public void AddUtiitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            MergeandShiftSkill = Skills.CreateSkillDef<VeliaerisRespawnSkillDef>(new SkillDefInfo
            {
                skillName = "Merge and Shift",
                skillNameToken = VELIAERIS_PREFIX + "MERGE_UTILITY_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "MERGE_UTILITY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "MERGE_INFORMATION" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(MergeandShift)),
                activationStateMachineName = "Switch",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = utilitycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            split = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Split",
                skillNameToken = VELIAERIS_PREFIX + "SPLIT_UTILITY_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "SPLIT_UTILITY_DESCRIPTION_SPLIT",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "SWITCH_INFORMATION" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(Split)),
                activationStateMachineName = "Switch",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = utilitycooldown,
                
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            MergeandShiftSkillCorrupted = Skills.CreateSkillDef<VeliaerisRespawnSkillDef>(new SkillDefInfo
            {
                skillName = "Merge and Shift",
                skillNameToken = VELIAERIS_PREFIX + "MERGE_UTILITY_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "MERGE_UTILITY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "MERGE_INFORMATION" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(MergeandShift)),
                activationStateMachineName = "Switch",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = utilitycooldown,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            splitCorrupted = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Split",
                skillNameToken = VELIAERIS_PREFIX + "SPLIT_UTILITY_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "SPLIT_UTILITY_DESCRIPTION_SPLIT",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE", VELIAERIS_PREFIX + "SWITCH_INFORMATION" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(Split)),
                activationStateMachineName = "Switch",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = utilitycooldown,

                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });



            Skills.AddUtilitySkills(bodyPrefab, split, MergeandShiftSkill);
        }

        public void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            


            voidDetonation = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VoidDetonator",
                skillNameToken = VELIAERIS_PREFIX + "VELIAERIS_SPECIAL_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIAERIS_SPECIAL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.VoidDetonator)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = veliaerisspecialcooldown,
                fullRestockOnAssign = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
            });


            eldritchHealing = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HealingForEveryone",
                skillNameToken = VELIAERIS_PREFIX + "ERIS_SPECIAL_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "ERIS_SPECIAL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(EldritchHealing)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = erisspecialcooldown,
                fullRestockOnAssign = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
            });


            selfBuffer = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SelfBuff",
                skillNameToken = VELIAERIS_PREFIX + "VELIA_SPECIAL_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIA_SPECIAL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(BlessingsFromBeyond)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Buff",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = veliaspecialcooldown,
                fullRestockOnAssign = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
            });

            voidDetonationCorrupted = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VoidDetonator",
                skillNameToken = VELIAERIS_PREFIX + "VELIAERIS_SPECIAL_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIAERIS_SPECIAL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.VoidDetonator)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = veliaerisspecialcooldown,
                fullRestockOnAssign = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
            });


            eldritchHealingCorrupted = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HealingForEveryone",
                skillNameToken = VELIAERIS_PREFIX + "ERIS_SPECIAL_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "ERIS_SPECIAL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(EldritchHealing)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = erisspecialcooldown,
                fullRestockOnAssign = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
            });


            selfBufferCorrupted = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SelfBuff",
                skillNameToken = VELIAERIS_PREFIX + "VELIA_SPECIAL_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIA_SPECIAL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(BlessingsFromBeyond)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Buff",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = veliaspecialcooldown,
                fullRestockOnAssign = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                
            });

            callUponSister = Skills.CreateSkillDef<DrainCheckSkillDef>(new SkillDefInfo
            {
                skillName = "CallSister",
                skillNameToken = VELIAERIS_PREFIX + "VELIA_SECONDARY_SPECIAL_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIA_SECONDARY_SPECIAL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(CallSister)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 5,
                rechargeStock =0,
                baseRechargeInterval =0f,
                dontAllowPastMaxStocks = true,
                fullRestockOnAssign = true,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,

            });

            callUponSisterCorrupted = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CallSister",
                skillNameToken = VELIAERIS_PREFIX + "VELIA_SECONDARY_SPECIAL_NAME_CORRUPT",
                skillDescriptionToken = VELIAERIS_PREFIX + "VELIA_SECONDARY_SPECIAL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                activationState = new EntityStates.SerializableEntityStateType(typeof(CallSister)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 5,
                rechargeStock = 0,
                baseRechargeInterval = 0f,
                dontAllowPastMaxStocks = true,
                fullRestockOnAssign = true,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,

            });

            Skills.AddSpecialSkills(bodyPrefab, voidDetonation,eldritchHealing,selfBuffer);
            
        }
        #endregion skills
        
        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
                //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
                //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            
            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(VELIAERIS_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);
            
            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            VeliaerisAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            GlobalEventManager.onServerDamageDealt += this.GlobalEventManager_onServerDamageDealt;
            On.RoR2.HealthComponent.TakeDamage += this.HealthComponent_TakeDamage;
            On.RoR2.BuffCatalog.Init += BuffCatalog_Init;
            On.RoR2.CharacterBody.OnInventoryChanged += inventoryChange;
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
//            CharacterBody.onBodyAwakeGlobal += this.bodyReset;
            CharacterBody.onBodyStartGlobal += this.bodySetup;
            On.RoR2.CharacterMaster.OnBodyDeath += this.deathEffect;
            On.RoR2.GlobalEventManager.OnHitEnemy += this.allyAbyss;
            On.RoR2.UI.AssignStageToken.Start += this.gatherStageName;
            RoR2.Stage.onStageStartGlobal += this.stageSetup;
            On.RoR2.Stage.BeginServer += this.clearPreviousAttempts;
            On.RoR2.SkillLocator.ApplyAmmoPack += appliedPack;
            On.RoR2.SkillLocator.DeductCooldownFromAllSkillsServer += SkillLocator_DeductCooldownFromAllSkillsServer;
          
            //            On.RoR2.TeleporterInteraction.OnInteractionBegin += this.interacted;
            //On.RoR2.Stage.CompleteServer += Stage_CompleteServer;
            //On.RoR2.Stage.BeginServer += Stage_BeginServer;
            //On.RoR2.Stage.Start += Stage_Start;
            //On.RoR2.Stage.BeginAdvanceStage += this.preventHereticIssues;
        
//            On.RoR2.CharacterBody.OnDestroy += CharacterBody_OnDestroy;
            
        }

        private void SkillLocator_DeductCooldownFromAllSkillsServer(On.RoR2.SkillLocator.orig_DeductCooldownFromAllSkillsServer orig, SkillLocator self, float deduction)
        {
            orig(self, deduction);
            Debug.Log("deduction:" + deduction);
        }

        private void appliedPack(On.RoR2.SkillLocator.orig_ApplyAmmoPack orig, SkillLocator self)
        {
            orig(self);
            if (VeliaerisStatuses.erisSecondaryStock <VeliaerisStatuses.erisSpecialStockMax)
            {
                VeliaerisStatuses.pickedUpPack = true;
            }
            Debug.Log("pickedup pack");
        }



        //private void Stage_Start(On.RoR2.Stage.orig_Start orig, Stage self)
        //{
        //    orig(self);
        //    Debug.Log("stageServerstart");
        //}

        //private void Stage_BeginServer(On.RoR2.Stage.orig_BeginServer orig, Stage self)
        //{
        //    orig(self);
        //    Debug.Log("stageServerbegin");
        //}

        //private void Stage_CompleteServer(On.RoR2.Stage.orig_CompleteServer orig, Stage self)
        //{
        //    orig(self);
        //    Debug.Log("stageServercomplete");
        //}

        private void CharacterMaster_OnInventoryChanged(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);
            VeliaerisSurvivorController veliaerisSurvivorController = bodyPrefab.GetComponent<VeliaerisSurvivorController>();
            if (veliaerisSurvivorController.gatheredPrimary > 0 && veliaerisSurvivorController.gatheredSecondary > 0 && veliaerisSurvivorController.gatheredUtility > 0 && veliaerisSurvivorController.gatheredSpecial > 0)
            {
                if (self.GetBody())
                {
                    hereticPrevention = true;
                }
            }
        }

        //private void CharacterBody_OnDestroy(On.RoR2.CharacterBody.orig_OnDestroy orig, CharacterBody self)
        //{
        //    Debug.Log("destroyhook");
        //    orig(self);
        //}

        //private void interacted(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig, TeleporterInteraction self, Interactor activator)
        //{

        //    Debug.Log("Launched intereacted hook");
        //    if (isCharged)
        //    {
        //        hereticPrevention = true;
        //        isCharged = false;
        //    }
        //    else
        //    {
        //        isCharged = true;
        //    }
        //    orig(self, activator);
        //}


        //private void preventHereticIssues(On.RoR2.Stage.orig_BeginAdvanceStage orig, Stage self, SceneDef destinationStage)
        //{
        //    Debug.Log("launched prevention hook");
        //    orig(self,destinationStage);
        //}

        private void gatherStageName(On.RoR2.UI.AssignStageToken.orig_Start orig, RoR2.UI.AssignStageToken self)
        {
            orig(self);
            TrueStageName = self.titleText.text;
          //  System.Console.WriteLine("displayed name:"+TrueStageName);
            SpeechDriver.tokenName = self;
        }

        private void clearPreviousAttempts(On.RoR2.Stage.orig_BeginServer orig, Stage self)
        {
            VeliaerisSurvivorController = bodyPrefab.GetComponent<VeliaerisSurvivorController>();
            Run instance = Run.instance;
            if (instance.stageClearCount < 1)
            {
                VeliaerisSurvivorController.setNetworkReviveTimer = 0f;
                VeliaerisSurvivorController.network_gatheredPrimary = 0;
                VeliaerisSurvivorController.hereticOverridesPrimary.Clear();
                VeliaerisSurvivorController.network_gatheredSecondary = 0;
                VeliaerisSurvivorController.hereticOverridesSecondary.Clear();
                VeliaerisSurvivorController.network_gatheredUtility = 0;
                VeliaerisSurvivorController.hereticOverridesUtility.Clear();
                VeliaerisSurvivorController.network_gatheredSpecial = 0;
                VeliaerisSurvivorController.hereticOverridesSpecial.Clear();
            }
            orig(self);
        }

        private void allyAbyss(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            
            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            if (attackerBody.HasBuff(VeliaerisBuffs.lesserSistersBlessing))
            {

                damageInfo.AddModdedDamageType(DamageTypes.AbyssCorrosion);
                victimBody.healthComponent.TakeDamage(damageInfo);
//                System.Console.WriteLine("Inflicting abyss");
//                DamageAPI.AddModdedDamageType(damageInfo, DamageTypes.AbyssCorrosion);
                
            }

        }

        private void deathEffect(On.RoR2.CharacterMaster.orig_OnBodyDeath orig, CharacterMaster self, CharacterBody body)
        {
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
            if (VeliaerisSurvivorController!=null)
            {
//                    Debug.Log("entered dead");
                    Inventory inventory = body.inventory;
                    bool doesNotHaveExtraLife = true;
                    bool isPlayer = true;
                    if (inventory.GetItemCount(DLC1Content.Items.GummyCloneIdentifier) > 0 || inventory.GetItemCount(RoR2.RoR2Content.Items.InvadingDoppelganger) > 0|| !body.isPlayerControlled)
                    {
                        isPlayer = false;
                    }
                    if(inventory.GetItemCount(RoR2.RoR2Content.Items.ExtraLife) < 0)
                    {
                        doesNotHaveExtraLife = false;
                    }
  //              Debug.Log("has void extra life");
                if (VeliaerisPlugin.hasVoid)
                    {
                    if (inventory.GetItemCount(RoR2.DLC1Content.Items.ExtraLifeVoid) > 0)
                    {
                        doesNotHaveExtraLife = false;
                    }
                    }
                    if (VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Veliaeris&&VeliaerisSurvivorController.ReviveDisabledTimer<=0&&doesNotHaveExtraLife && isPlayer)
                    {
                        justDied = true;
                        self.lostBodyToDeath = false;
                        Vector3 vector = body.footPosition;
                        var temporaryBody = self.destroyOnBodyDeath;
                        self.destroyOnBodyDeath = false;
                        HeldState.destroyVeliaerisCorpse = true;
                        orig(self, body);
                        self.destroyOnBodyDeath = temporaryBody;
                        self.preventGameOver = true;
                        self.preventRespawnUntilNextStageServer = true;
                        preventInventoryUpdate = true;
                        self.Respawn(vector, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
//                        self.ResetLifeStopwatch();
                        CharacterBody revivedbody = self.GetBody();
                        VeliaerisStatuses.timeUntilSistersRevival = 20f;
                        VeliaerisSurvivorController.setNetworkReviveTimer = 300f;
                    
                    SkillLocator switchSkills = revivedbody.GetComponent<SkillLocator>();
              //      System.Console.WriteLine("Vel state check: " + VeliaerisSurvivorController.VeliaerisState);
                        VeliaerisSurvivorController.network_paststate = VeliaerisSurvivorController.VeliaerisState;
                        if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris)
                        {
                            VeliaerisSurvivorController.network_veliaerisStates = VeliaerisState.Velia;
                        }
                        else if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia)
                        {
                            VeliaerisSurvivorController.network_veliaerisStates = VeliaerisState.Eris;
                        }
                    VeliaerisSurvivorController.network_previousState = VeliaerisSurvivorController.VeliaerisState;
                    revivedbody.AddTimedBuffAuthority(VeliaerisBuffs.missingSibling.buffIndex, 20f);
                    revivedbody = body;
                    VeliaerisState velStartstate;
                    velStartstate = body.GetComponent<VeliaerisPassive>().getStartState();
                    VeliaerisSurvivorControllerrevived = revivedbody.GetComponent<VeliaerisSurvivorController>();
//                    System.Console.WriteLine("VeliaerisStates on death revive:" + VeliaerisSurvivorController.VeliaerisState);
 //                   System.Console.WriteLine("VeliaerisStates on death revive revivedbody:" + VeliaerisSurvivorControllerrevived.VeliaerisStates);
                    System.Console.WriteLine("VeliaerisStates body:" + VeliaerisSurvivorController.ReviveDisabledTimer);
                    if (voidInfluence >= VeliaerisStaticValues.firstTierCorruption)
                    {
                    //    Debug.Log("entered influence revive");
                        MergeandShift.skillSetCorrupted(switchSkills, revivedbody);
                    }
                    else
                    {
                        MergeandShift.skillSet(switchSkills, revivedbody);
                    }
                        MergeandShift.SkillSwitch(switchSkills,false,revivedbody,false,false);
                    SpeechDriver speech = new SpeechDriver();
                    speech.enacteDialogue("revive",body,"");
                    }
                    //else
                    //{
                    //    VeliaerisStates.timeUntilSistersRevival = 0f;
                    //    VeliaerisSurvivorController.setNetworkReviveTimer = 0f;
                    //    orig(self, body);
                    //}
            }


            //            }        
            orig(self, body);
        }

        private void inventoryChange(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody body)
        {
            try
            {
                orig(body);
            }
            catch
            {
                return;
            }
//            System.Console.WriteLine("exist");
            try
            {
                SkillLocator skillLocator = body.GetComponent<SkillLocator>();
                //Debug.Log("skillnametoken:" + skillLocator.utility.skillNameToken);
                ////                    Debug.Log("skillOverrides:" + skillLocator.utility.skillOverrides[0]);
                //Debug.Log("skilldef:" + skillLocator.utility.skillDef);
                if (!preventInventoryUpdate)
                {
                    VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
                if (body.ToString().Contains("VeliaerisBody"))
                {
                    Inventory inventory = body.inventory;
                    //headsCollected = inventory.GetItemCount(RoR2.RoR2Content.Items.AlienHead);
                    //hardLightsCollected = inventory.GetItemCount(RoR2.RoR2Content.Items.UtilitySkillMagazine);
                    //backupsCollected = inventory.GetItemCount(RoR2.RoR2Content.Items.SecondarySkillMagazine);
               //         Debug.Log("has void inventory");
                    if (VeliaerisPlugin.hasVoid)
                    {
                        if (inventory)
                        {
                            voidInfluence = 0;
//                            voidCellsCollected = inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
                            voidInfluence = inventory.GetTotalItemCountOfTier(ItemTier.VoidTier1) + inventory.GetTotalItemCountOfTier(ItemTier.VoidTier2) + inventory.GetTotalItemCountOfTier(ItemTier.VoidTier3) + inventory.GetTotalItemCountOfTier(ItemTier.VoidBoss);
                        }
                        if (voidInfluence >= VeliaerisStaticValues.secondTierCorruption)
                        {
                            body.bodyFlags |= CharacterBody.BodyFlags.Void;
                        }
                    }
                        #region heretic Mechanic Inventory Management
                        if (body.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) < VeliaerisSurvivorController.gatheredPrimary && VeliaerisSurvivorController.gatheredPrimary != 0)
                    {
                        VeliaerisSurvivorController.network_gatheredPrimary--;
                        if (VeliaerisSurvivorController.hereticOverridesPrimary.Contains(VeliaerisSurvivorController.VeliaerisState))
                        {
                            VeliaerisSurvivorController.hereticOverridesPrimary.Remove(VeliaerisSurvivorController.VeliaerisState);
                        }
                        else
                        {
                            VeliaerisSurvivorController.hereticOverridesPrimary.Remove(VeliaerisSurvivorController.hereticOverridesPrimary[1]);
                        }
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement) < VeliaerisSurvivorController.gatheredSecondary && VeliaerisSurvivorController.gatheredSecondary != 0)
                    {
                        VeliaerisSurvivorController.network_gatheredSecondary--;
                        if (VeliaerisSurvivorController.hereticOverridesSecondary.Contains(VeliaerisSurvivorController.VeliaerisState))
                        {
                            VeliaerisSurvivorController .hereticOverridesSecondary.Remove(VeliaerisSurvivorController.VeliaerisState);
                        }
                        else
                        {
                            VeliaerisSurvivorController.hereticOverridesSecondary.Remove(VeliaerisSurvivorController.hereticOverridesSecondary[1]);
                        }
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement) < VeliaerisSurvivorController.gatheredUtility && VeliaerisSurvivorController.gatheredUtility != 0)
                    {
                        VeliaerisSurvivorController.network_gatheredUtility--;
                        if (VeliaerisSurvivorController.hereticOverridesUtility.Contains(VeliaerisSurvivorController.VeliaerisState))
                        {
                            VeliaerisSurvivorController.hereticOverridesUtility.Remove(VeliaerisSurvivorController.VeliaerisState);
                        }
                        else
                        {
                            VeliaerisSurvivorController.hereticOverridesUtility.Remove(VeliaerisSurvivorController.hereticOverridesUtility[1]);
                        }
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement) < VeliaerisSurvivorController.gatheredSpecial && VeliaerisSurvivorController.gatheredSpecial != 0)
                    {
                       VeliaerisSurvivorController.network_gatheredSpecial--;
                        if (VeliaerisSurvivorController.hereticOverridesSpecial.Contains(VeliaerisSurvivorController.VeliaerisState))
                        {
                            VeliaerisSurvivorController.hereticOverridesSpecial.Remove(VeliaerisSurvivorController.VeliaerisState);
                        }
                        else
                        {
                            VeliaerisSurvivorController.hereticOverridesSpecial.Remove(VeliaerisSurvivorController.hereticOverridesSpecial[1]);
                        }
                    }


                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) < 1)
                    {
                        VeliaerisSurvivorController.network_gatheredPrimary = 0;
                        VeliaerisSurvivorController.hereticOverridesPrimary.Clear();
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement) < 1)
                    {
                        VeliaerisSurvivorController.network_gatheredSecondary = 0;
                        VeliaerisSurvivorController.hereticOverridesSecondary.Clear();
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement) < 1)
                    {
                        VeliaerisSurvivorController.network_gatheredUtility = 0;
                        VeliaerisSurvivorController.hereticOverridesUtility.Clear();
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement) < 1)
                    {
                       // System.Console.WriteLine("entered");
                        VeliaerisSurvivorController.network_gatheredSpecial = 0;
                        VeliaerisSurvivorController.hereticOverridesSpecial.Clear();
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) > VeliaerisSurvivorController.gatheredPrimary)
                    {
                        if (!VeliaerisSurvivorController.hereticOverridesPrimary.Contains(VeliaerisSurvivorController.VeliaerisState))
                        {
                            VeliaerisSurvivorController.hereticOverridesPrimary.Add(VeliaerisSurvivorController.VeliaerisState);
                        }
                        VeliaerisSurvivorController.network_gatheredPrimary++;
                            hereticPickuped = true;

                        }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement) > VeliaerisSurvivorController.gatheredSecondary)
                    {
                        if (!VeliaerisSurvivorController.hereticOverridesSecondary.Contains(VeliaerisSurvivorController.VeliaerisState))
                        {
                            VeliaerisSurvivorController.hereticOverridesSecondary.Add(VeliaerisSurvivorController.VeliaerisState);
                        }
                        VeliaerisSurvivorController.network_gatheredSecondary++;
                            hereticPickuped = true;
                        }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement) > VeliaerisSurvivorController.gatheredUtility)
                    {
                        if (!VeliaerisSurvivorController.hereticOverridesUtility.Contains(VeliaerisSurvivorController.VeliaerisState))
                        {
                            VeliaerisSurvivorController.hereticOverridesUtility.Add(VeliaerisSurvivorController.VeliaerisState);
                        }
                        VeliaerisSurvivorController.network_gatheredUtility++;
                            hereticPickuped = true;
                        }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement) > VeliaerisSurvivorController.gatheredSpecial)
                    {
                           // Debug.Log("Entered check");
                            if (!VeliaerisSurvivorController.hereticOverridesSpecial.Contains(VeliaerisSurvivorController.VeliaerisState))
                        {
                            VeliaerisSurvivorController.hereticOverridesSpecial.Add(VeliaerisSurvivorController.VeliaerisState);
                        }
                        VeliaerisSurvivorController.network_gatheredSpecial++;
                            hereticPickuped = true;
                        }
                        #endregion
                       
                  //  System.Console.WriteLine("check form:" + VeliaerisSurvivorController.velState);
                    if(!VeliaerisSurvivorController.isCorrupted && voidInfluence >= VeliaerisStaticValues.firstTierCorruption)
                        {
                    //        Debug.Log("entered influence inventory");
                            MergeandShift.skillSetCorrupted(skillLocator,body);
                            VeliaerisSurvivorController.isCorrupted = true;
                        }
                        if (VeliaerisSurvivorController.isCorrupted && voidInfluence < VeliaerisStaticValues.firstTierCorruption)
                        {
                            MergeandShift.skillSet(skillLocator, body);
                            VeliaerisSurvivorController.isCorrupted = false;
                        }
                    MergeandShift.SkillSwitch(skillLocator, false, body,hereticPickuped,true);
                    }
                    hereticPickuped = false;
                    preventInventoryUpdate = false;
                }

            }
            catch { return; }
            
        }

        private void bodySetup(CharacterBody body)
        {
            if (body != null)
            {
                VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
                //System.Console.WriteLine("Body grabbed: " + body);

                if (body.ToString().Contains("VeliaerisBody"))
                {
                    SkillLocator skillLocator = body.GetComponent<SkillLocator>();
                    VeliaerisStatuses.erisSpecialStock=skillLocator.special.maxStock;
                    VeliaerisStatuses.utilityCooldown = 0f;
                    VeliaerisStatuses.erisSpecialCooldownStock = 0f;
                    VeliaerisStatuses.erisSecondaryCooldownStock = 0f;
                    VeliaerisStatuses.veliaerisSpecialCooldownStock = 0f;
                    VeliaerisStatuses.veliaSpecialCooldownStock = 0f;
                    VeliaerisStatuses.veliaSecondaryCooldownStock = 0f;
                    VeliaerisStatuses.veliaerisSecondaryCooldownStock = 0f;
                 
                    Run instance = Run.instance;
                    //                    PlayerCharacterMasterControllerEntitlementTracker component = body.master.playerCharacterMasterController.GetComponent<PlayerCharacterMasterControllerEntitlementTracker>();
                    //                  EntitlementDef requiredEntitlement = this.requiredExpansion.requiredEntitlement;
                    //if (!component.HasEntitlement(requiredEntitlement)){
                    //    VeliaerisPlugin.hasVoid = false;
                    //}
                    
                        VeliaerisState velStartstate;
                        velStartstate = body.GetComponent<VeliaerisPassive>().getStartState();
                    VeliaerisSurvivorController.network_initial = velStartstate;
                    if (instance.stageClearCount < 1 && !justDied)
                    {
                        VeliaerisSurvivorController.network_veliaerisStates = velStartstate;
                        VeliaerisSurvivorController.setNetworkReviveTimer = 0f;

                        if (velStartstate == VeliaerisState.Velia || velStartstate == VeliaerisState.Eris)
                        {
                            VeliaerisSurvivorController.network_previousState = velStartstate;

                        }
                    }
                    if (justDied)
                    {
                        VeliaerisSurvivorController.setNetworkReviveTimer = 300f;
                        VeliaerisSurvivorController.network_veliaerisStates = VeliaerisSurvivorController.previousSplitSate;
                        justDied = false;
                    }
                    if (voidInfluence >= VeliaerisStaticValues.firstTierCorruption)
                    {
                        Debug.Log("Entered bodysetup influence");
                        MergeandShift.skillSetCorrupted(skillLocator, body);
                    }
                    else
                    {
                        MergeandShift.skillSet(skillLocator, body);
                    }
                    MergeandShift.SkillSwitch(skillLocator, true,body,false,false);
                    body.SetBuffCount(VeliaerisBuffs.VeliaerisStatChanges.buffIndex, 0);
                    body.SetBuffCount(VeliaerisBuffs.ErisStatChanges.buffIndex, 0);
                    body.SetBuffCount(VeliaerisBuffs.VeliaStatChanges.buffIndex, 0);
                    if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Veliaeris)
                    {
                        body.SetBuffCount(VeliaerisBuffs.VeliaerisStatChanges.buffIndex, 1);
                    }
                    if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris)
                    {
                        body.SetBuffCount(VeliaerisBuffs.ErisStatChanges.buffIndex, 1);
                    }
                    if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia)
                    {
                        body.SetBuffCount(VeliaerisBuffs.VeliaStatChanges.buffIndex, 1);
                    }

                }

            }
        }
        private void stageSetup(Stage stage)
        {
            StageIdentity = SceneManager.GetActiveScene().name;
       //     System.Console.WriteLine("Internal Stage Name: " + StageIdentity);
        //    System.Console.WriteLine("HasMod?: " + VeliaerisPlugin.CustomStagesEnabled);
            //            System.Console.WriteLine("Check outlog");
            VeliaerisStatuses.stageStarted = false;
            firstUse = true;
            stagePhasing = true;
            VeliaerisStatuses.startUpTimer = 0f;
            DeathPreventionStacks = 0;
            VoidCorruptionStacks = 0;
            VeliaerisStatuses.notSeenBoss = true;
        }
        private void BuffCatalog_Init(On.RoR2.BuffCatalog.orig_Init orig)
        {
            orig();
            for (int i = 0; i < BuffCatalog.buffDefs.Length; i++)
            {
                string buffName = BuffCatalog.buffDefs[i].name.ToLowerInvariant();
                if (buffName.Contains("abyss"))
                {
                    InflictionBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
              //      System.Console.WriteLine("an abyss was found");
                    if (!BuffCatalog.buffDefs[i].canStack)
                    {
//                        System.Console.WriteLine("A void was found");
                        InflictionBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
                        InflictionBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
                    }
                }
            }
        }
        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            /*if (combinedHealth >= this.fullCombinedHealth * 0.9f)
						{
							int itemCount2 = master.inventory.GetItemCount(RoR2Content.Items.Crowbar);
							if (itemCount2 > 0)
							{
								num *= 1f + 0.75f * (float)itemCount2;
							}
						}*/

            //minus from damage info for damage reduction
            //attackerBody.healthComponent.AddBarrier
            float enhancedRange = 0f;
            for(int i=0;i< voidInfluence / VeliaerisStaticValues.firstTierCorruption; i++)
            {
                enhancedRange+=0.01f;
            }
            if (DamageAPI.HasModdedDamageType(damageInfo, DamageTypes.voidCorrosion))
            {
                if (self.combinedHealth >= self.fullCombinedHealth * (0.95f-enhancedRange))
                {
                    damageInfo.damage += 1f * 0.80f+ (float)((((VoidCorruptionStacks / 2) - 0.5) * 2) / VeliaerisStaticValues.firstTierCorruption);
                }
            }
            if (DamageAPI.HasModdedDamageType(damageInfo, DamageTypes.AbyssCorrosion))
            {
                
                int corrosion;
                if (self.body==null)
                {
                    corrosion = 0;
                }
                else
                {
                    corrosion = self.body.GetBuffCount(VeliaerisBuffs.abyss) /*+ this.GetInflictionBuffs(self.body)*/;
                }
                damageInfo.damage += DamageMultipliterPerAbyssStack * corrosion * damageInfo.damage;
                
            }

            if (DamageAPI.HasModdedDamageType(damageInfo, DamageTypes.armorVoid))
            {
                self.body.AddBuff(VeliaerisBuffs.armorDesolation);
            }

            if (DamageAPI.HasModdedDamageType(damageInfo, DamageTypes.percentHealth))
            {
                float health;
                if(self.body==null)
                {
                    health = 0;
                }
                else
                {
                    health = self.body.maxHealth;
                }
                if (!self.body.isBoss)
                {
                    damageInfo.damage += health * 0.25f;
                }
                else
                {
                    damageInfo.damage += health * 0.02f;
                }
                damageInfo.damageType = DamageType.BypassArmor;
            }
            orig.Invoke(self, damageInfo);
        }
        private int GetInflictionBuffs(CharacterBody body)
        {
            int num = 0;
            for(int i = 0; i < InflictionBuffs.Count; i++)
            {
                num += body.GetBuffCount(InflictionBuffs[i]);
            }
            return num;
        }
        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, DamageTypes.AbyssCorrosion))
            {

                inflictOblivion(damageReport.victim.gameObject,damageReport.attacker,damageReport.damageInfo.procCoefficient,false);
            }
        }
        private void inflictOblivion(GameObject victim, GameObject attacker, float procCoefficient, bool crit)
        {
            
            DotController.InflictDot(victim, attacker, VeliaerisDots.AbyssCorrosion, DotDuration, (crit ? 2 : 1) * procCoefficient);
        }
        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(VeliaerisBuffs.switchInvincibility))
            {
                sender.bodyFlags |= CharacterBody.BodyFlags.ImmuneToExecutes|CharacterBody.BodyFlags.ImmuneToVoidDeath;
                args.armorAdd += 9999f;
            }
            if (sender.HasBuff(VeliaerisBuffs.armorDesolation))
            {
                args.armorAdd -= sender.GetBuffCount(VeliaerisBuffs.armorDesolation) * desolationReduction;
            }
            
            if (sender.HasBuff(VeliaerisBuffs.sistersBlessing))
            {
                args.attackSpeedMultAdd += 1.2f;
                args.moveSpeedMultAdd += 0.7f;
                args.armorAdd += 20f;
                args.damageMultAdd += 1.1f;
                args.regenMultAdd += 2f;
                
                
                sender.bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath;
            }
            if (sender.HasBuff(VeliaerisBuffs.lesserSistersBlessing))
            {
                args.armorAdd+= 10f;
                args.attackSpeedMultAdd += 0.8f;
                args.moveSpeedMultAdd += 0.25f;
                args.damageMultAdd += 0.5f;
                sender.bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath;
            }

            if (sender.HasBuff(VeliaerisBuffs.VeliaerisStatChanges) && VeliaerisPlugin.hasVoid)
            {
    //            Debug.Log("has void veliaeris stat changes");
                args.baseHealthAdd += (sender.baseMaxHealth * 0.025f) * voidInfluence;
                args.damageMultAdd += 0.0125f * voidInfluence;
            }
            if (sender.HasBuff(VeliaerisBuffs.ErisStatChanges))
            {
//                System.Console.WriteLine("is eris");
                args.baseHealthAdd += sender.baseMaxHealth * 0.75f;
  //              Debug.Log("has void eris stat changes");
                if (VeliaerisPlugin.hasVoid)
                {
                    args.baseHealthAdd += (sender.baseMaxHealth * 0.1f) *voidInfluence;
                }
                args.baseDamageAdd -= sender.baseDamage;
                args.armorAdd -= 10f;
            }
            if (sender.HasBuff(VeliaerisBuffs.VeliaStatChanges))
            {
                     args.baseHealthAdd -= sender.baseMaxHealth * 0.25f;
                //args.baseHealthAdd -= sender.baseMaxHealth * 0.9f;
                args.damageMultAdd += 0.5f;
//                Debug.Log("has void velia stat changes");
                if (VeliaerisPlugin.hasVoid)
                {
                    args.damageMultAdd += 0.25f*voidInfluence;
                }
                args.armorAdd += 10f;
            }

            if (sender.HasBuff(VeliaerisBuffs.healthBlessing))
            {
                args.baseHealthAdd += (sender.baseMaxHealth * 0.05f) * sender.GetBuffCount(VeliaerisBuffs.healthBlessing);
            }
            if (sender.HasBuff(VeliaerisBuffs.damageBlessing))
            {
                args.baseDamageAdd += 0.1f * sender.GetBuffCount(VeliaerisBuffs.damageBlessing);
            }
            //if (sender.HasBuff(VeliaerisBuffs.switchInvincibility))
            //{
            //    if (sender.healthComponent.health > sender.healthComponent.fullHealth)
            //    {
            //        sender.healthComponent.health = sender.healthComponent.health - sender.healthComponent.fullHealth;
            //    }
            //}
        }



        public static float DamageMultipliterPerAbyssStack = 0.1f;
        public static List<BuffIndex> InflictionBuffs = new List<BuffIndex>();
        public static float AbyssDamage = 0.1f;
        public static float AbyssCollapseDamage = 2f;
        public static float DotIntervalAbyssBleed = 0.15f;
        public static float DotIntervalAbyssCollapse = 3f;
        public static float DotDuration = 10f;
        public static float CollapseExtraDuration = 3f;
        public static float desolationReduction = 10f;
        public static bool unstable = false;
        public static bool justDied = false;
        private DamageColorIndex damageColor = DamageColorIndex.Void;
        public static int voidInfluence;
        public static int DeathPreventionStacks = 0;
        public static int VoidCorruptionStacks = 0;
        public static String StageIdentity;
        public static String TrueStageName;
        public ExpansionDef requiredExpansion;
        private VeliaerisSurvivorController VeliaerisSurvivorController;
        private bool preventInventoryUpdate;
        private VeliaerisSurvivorController VeliaerisSurvivorControllerrevived;
        public static bool firstUse = true;
        public static bool stagePhasing;
        public static bool hereticPrevention;
        private bool isCharged;
        private bool hereticPickuped = false;
    }
}