using BepInEx.Configuration;
using VeliaerisMod.Modules;
using VeliaerisMod.Modules.Characters;
using VeliaerisMod.Survivors.Veliaeris.Components;
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

namespace VeliaerisMod.Survivors.Veliaeris
{
    public class VeliaerisSurvivor : SurvivorBase<VeliaerisSurvivor>
    {
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
        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = VELIAERIS_PREFIX + "NAME",
            subtitleNameToken = VELIAERIS_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texHenryIcon"),
            bodyColor = Color.white,
            sortPosition = 100,

            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = null,
            //podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),
//            moveSpeed = 100f,
            maxHealth = 9999f,/*original max health 110f*/
            healthRegen = 101.5f,/*original healthRegen 1.5f*/
            armor = 9999f,/*original armor 0f*/

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

        public override UnlockableDef characterUnlockableDef => HenryUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => new HenryItemDisplays();

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
            HenryUnlockables.Init();

            base.InitializeCharacter();
            HenryConfig.Init();
            HenryStates.Init();
            HenryTokens.Init();
            VeliaerisBuffs.Init(assetBundle);
            VeliaerisDots.Init();
            DamageTypes.Init();
            HenryAssets.Init(assetBundle);


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
            bodyPrefab.AddComponent<HenryWeaponComponent>();
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
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
                //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Slide");
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
            VeliaerisPassive startStatePassive = bodyPrefab.AddComponent<VeliaerisPassive>();
            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            startStatePassive.passiveSkillSlot = Modules.Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "Passive",true);
            SkillLocator skilllocat = bodyPrefab.GetComponent<SkillLocator>();
            skilllocat.passiveSkill.enabled = true;
            skilllocat.passiveSkill.skillNameToken = VELIAERIS_PREFIX + "PASSIVE_NAME";
            skilllocat.passiveSkill.skillDescriptionToken = VELIAERIS_PREFIX + "PASSIVE_DESCRIPTION";
            skilllocat.passiveSkill.icon = assetBundle.LoadAsset<Sprite>("texPassiveIcon");


            startStatePassive.VeliaerisStart = Skills.CreateSkillDef(new SkillDefInfo
            {
                
                skillName = "VeliaerisPassive",
                skillNameToken = VELIAERIS_PREFIX + "VELIAERIS_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "PASSIVE_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
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
            System.Console.WriteLine("check9");
            startStatePassive.ErisStart = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ErisPassive",
                skillNameToken = VELIAERIS_PREFIX + "ERIS_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "PASSIVE_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

            });

            startStatePassive.VeliaStart = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VeliaPassive",
                skillNameToken = VELIAERIS_PREFIX + "VELIA_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "PASSIVE_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

            });
            AddPassiveSkills(startStatePassive.passiveSkillSlot.skillFamily, new SkillDef[]
            {
                startStatePassive.VeliaerisStart,
                startStatePassive.ErisStart,
                startStatePassive.VeliaStart
            });

            System.Console.WriteLine("check10");
            
        }
        public static void AddPassiveSkills(SkillFamily passiveSkillFamily, params SkillDef[] skillDefs)
        {
            System.Console.WriteLine("Skilldefs length: " + skillDefs.Length);
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
            SkillDef primaryTracking = Skills.CreateSkillDef(new SkillDefInfo(
                                    "HenrySlash",
                    VELIAERIS_PREFIX + "PRIMARY_SLASH_NAME",
                    VELIAERIS_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.BasicScytheSlash)),
                    "Weapon",
                    true
                )
                );
            Skills.AddPrimarySkills(bodyPrefab, primaryTracking);
        }

        public void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            HuntressTrackingSkillDef secondarySkillDef1 = Skills.CreateSkillDef<HuntressTrackingSkillDef>(new SkillDefInfo
            {
                skillName = "HenryGun",
                skillNameToken = VELIAERIS_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Corrupt)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                /*baseRechargeInterval=15f,*/
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
                

            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
        }

        public void AddUtiitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryRoll",
                skillNameToken = VELIAERIS_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Split)),
                activationStateMachineName = "Slide",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
        }

        public void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryBomb",
                skillNameToken = VELIAERIS_PREFIX + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = VELIAERIS_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.VoidDetonator)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2", interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
            
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
            HenryAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            GlobalEventManager.onServerDamageDealt += this.GlobalEventManager_onServerDamageDealt;
            On.RoR2.HealthComponent.TakeDamage += this.HealthComponent_TakeDamage;
            On.RoR2.BuffCatalog.Init += BuffCatalog_Init;
            RoR2.Stage.onStageStartGlobal += this.stageSetup;
            RoR2.Stage.onServerStageComplete += this.stageSetup;
            On.RoR2.CharacterBody.OnInventoryChanged += bodyChange;
            CharacterBody.onBodyStartGlobal += this.bodySetup;
 //           On.RoR2.CharacterBody.OnDeathStart
        }

        private void bodyChange(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody body)
        {
            if (body == null)
            {
                System.Console.WriteLine("The body is null you fool");
            }
            else
            {
                System.Console.WriteLine("Not null?");
            }
            try
            {
                orig(body);
            }
            catch
            {
                return;
            }
            System.Console.WriteLine("exist");
            try
            {
                if (body.ToString().Contains("VeliaerisBody"))
                {

                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) < HeldState.gatheredPrimary && HeldState.gatheredPrimary != 0)
                    {
                        HeldState.gatheredPrimary--;
                        if (HeldState.hereticOverridesPrimary.Contains(VeliaerisPlugin.VeliaerisStates))
                        {
                            HeldState.hereticOverridesPrimary.Remove(VeliaerisPlugin.VeliaerisStates);
                        }
                        else
                        {
                            HeldState.hereticOverridesPrimary.Remove(HeldState.hereticOverridesPrimary[1]);
                        }
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement) < HeldState.gatheredSecondary && HeldState.gatheredSecondary != 0)
                    {
                        HeldState.gatheredSecondary--;
                        if (HeldState.hereticOverridesSecondary.Contains(VeliaerisPlugin.VeliaerisStates))
                        {
                            HeldState.hereticOverridesSecondary.Remove(VeliaerisPlugin.VeliaerisStates);
                        }
                        else
                        {
                            HeldState.hereticOverridesSecondary.Remove(HeldState.hereticOverridesSecondary[1]);
                        }
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement) < HeldState.gatheredUtility && HeldState.gatheredUtility != 0)
                    {
                        HeldState.gatheredUtility--;
                        if (HeldState.hereticOverridesUtility.Contains(VeliaerisPlugin.VeliaerisStates))
                        {
                            HeldState.hereticOverridesUtility.Remove(VeliaerisPlugin.VeliaerisStates);
                        }
                        else
                        {
                            HeldState.hereticOverridesUtility.Remove(HeldState.hereticOverridesUtility[1]);
                        }
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement) < HeldState.gatheredSpecial && HeldState.gatheredSpecial != 0)
                    {
                        HeldState.gatheredSpecial--;
                        if (HeldState.hereticOverridesSpecial.Contains(VeliaerisPlugin.VeliaerisStates))
                        {
                            HeldState.hereticOverridesSpecial.Remove(VeliaerisPlugin.VeliaerisStates);
                        }
                        else
                        {
                            HeldState.hereticOverridesSpecial.Remove(HeldState.hereticOverridesSpecial[1]);
                        }
                    }


                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) < 1)
                    {
                        HeldState.gatheredPrimary = 0;
                        HeldState.hereticOverridesPrimary.Clear();
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement) < 1)
                    {
                        HeldState.gatheredSecondary = 0;
                        HeldState.hereticOverridesSecondary.Clear();
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement) < 1)
                    {
                        HeldState.gatheredUtility = 0;
                        HeldState.hereticOverridesUtility.Clear();
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement) < 1)
                    {
                        HeldState.gatheredSpecial = 0;
                        HeldState.hereticOverridesSpecial.Clear();
                    }

                    System.Console.WriteLine("Before inventory checks");
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement) > HeldState.gatheredPrimary)
                    {
                        System.Console.WriteLine("inventory check");
                        if (!HeldState.hereticOverridesPrimary.Contains(VeliaerisPlugin.VeliaerisStates))
                        {
                            HeldState.hereticOverridesPrimary.Add(VeliaerisPlugin.VeliaerisStates);
                        }
                        System.Console.WriteLine("post add");
                        HeldState.gatheredPrimary++;
                        System.Console.WriteLine("post increase");
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement) > HeldState.gatheredSecondary)
                    {
                        System.Console.WriteLine("inventory check");
                        if (!HeldState.hereticOverridesSecondary.Contains(VeliaerisPlugin.VeliaerisStates))
                        {
                            HeldState.hereticOverridesSecondary.Add(VeliaerisPlugin.VeliaerisStates);
                        }
                        System.Console.WriteLine("post add");
                        HeldState.gatheredSecondary++;
                        System.Console.WriteLine("post increase");
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement) > HeldState.gatheredUtility)
                    {
                        System.Console.WriteLine("inventory check");
                        if (!HeldState.hereticOverridesPrimary.Contains(VeliaerisPlugin.VeliaerisStates))
                        {
                            HeldState.hereticOverridesUtility.Add(VeliaerisPlugin.VeliaerisStates);
                        }
                        System.Console.WriteLine("post add");
                        HeldState.gatheredUtility++;
                        System.Console.WriteLine("post increase");
                    }
                    if (body.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement) > HeldState.gatheredSpecial)
                    {
                        System.Console.WriteLine("inventory check");
                        if (!HeldState.hereticOverridesPrimary.Contains(VeliaerisPlugin.VeliaerisStates))
                        {
                            HeldState.hereticOverridesSpecial.Add(VeliaerisPlugin.VeliaerisStates);
                        }
                        System.Console.WriteLine("post add");
                        HeldState.gatheredSpecial++;
                        System.Console.WriteLine("post increase");
                    }
                }
                if (HeldState.hereticOverridesPrimary.Count > 0)
                {
                    System.Console.WriteLine("HereticOverride: " + HeldState.hereticOverridesPrimary[0]);
                }

                SkillLocator skillLocator = body.GetComponent<SkillLocator>();
                MergeandShift.SkillSwitch(skillLocator);
            }
            catch { return; }
        }

        private void bodySetup(CharacterBody body)
        {
            
            if (body != null)
            {
            //    System.Console.WriteLine("Body grabbed: " + body);
                if (body.ToString().Contains("VeliaerisBody"))
                {
//                    System.Console.WriteLine("Character body");
                    SkillLocator skillLocator = body.GetComponent<SkillLocator>();
                    //System.Console.WriteLine("passivename1: " + this,passiveSkillSlot.skillDef);
  //                  System.Console.WriteLine("Current grabbed skillocator: " + skillLocator);
                    MergeandShift.SkillSwitch(skillLocator);
                }
            }

        }

        private void stageSetup(Stage stage)
        {   VeliaerisPassive skillPassive = bodyPrefab.GetComponent<VeliaerisPassive>();
            CharacterBody body = bodyPrefab.GetComponent<CharacterBody>();
            System.Console.WriteLine("stages cleared: " + Run.instance.stageClearCount);
//            if(Run.instance.stageClearCount< 1)
  //          {
                System.Console.WriteLine("Passive set: " + skillPassive.getStartState());
            //    System.Console.WriteLine(HeldState.velState);
            //    HeldState.velState = VeliaerisState.Veliaeris;
    //        }
            //System.Console.WriteLine("heldstate: " + HeldState.velState);
            //System.Console.WriteLine("VeliaerisState pre global" + VeliaerisPlugin.VeliaerisStates);
            VeliaerisPlugin.VeliaerisStates = HeldState.velState;
            //System.Console.WriteLine("Veliaerisstate post global" + VeliaerisPlugin.VeliaerisStates);
            //System.Console.WriteLine("Current global velstate: " + HeldState.velState);

            SkillLocator skillLocator = body.GetComponent<SkillLocator>();
            //System.Console.WriteLine("Current grabbed skillocator: " + skillLocator);
            MergeandShift.SkillSwitch(skillLocator);


            //if (SkillLocators == null)
            //{
            //    System.Console.WriteLine("body.skillocator is null");
            //}
            //else
            //{
            //    System.Console.WriteLine("body.skilllocator is not null");
            //}
            //            MergeandShift.SkillSwitch(body.skillLocator);
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
                    System.Console.WriteLine("an abyss was found");
                    if (!BuffCatalog.buffDefs[i].canStack)
                    {
                        System.Console.WriteLine("A void was found");
                        InflictionBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
                        InflictionBuffs.Add(BuffCatalog.buffDefs[i].buffIndex);
                    }
                }
            }
        }

        

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            //if(damageInfo.attacker.GetComponent<CharacterBody>().GetBuffCount(VeliaerisBuffs.lesserSistersBlessing) > 0)
            //{
            //    DamageAPI.AddModdedDamageType(damageInfo, DamageTypes.AbyssCorrosion);
            //}
            if (DamageAPI.HasModdedDamageType(damageInfo, DamageTypes.AbyssCorrosion))
            {
                
                int corrosion;
                if (self.body==null)
                {
                    corrosion = 0;
                }
                else
                {
                    corrosion = self.body.GetBuffCount(VeliaerisBuffs.abyss) + this.GetInflictionBuffs(self.body);
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
                    damageInfo.damage += health * 0.05f;
                }
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


            if (sender.HasBuff(VeliaerisBuffs.armorDesolation))
            {
                args.armorAdd -= sender.GetBuffCount(VeliaerisBuffs.armorDesolation) * desolationReduction;
            }

            if (sender.HasBuff(VeliaerisBuffs.sistersBlessing))
            {
                sender.maxHealth *=1.5f;
//                args.healthMultAdd *= 1.5f;
                args.shieldMultAdd *= 1.5f;
                args.attackSpeedMultAdd *= 1.7f;
                args.moveSpeedMultAdd *= 1.3f;
                args.armorAdd += 20f;
                args.damageMultAdd *= 3f;
                args.regenMultAdd *= 4f;
                
                sender.bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath;
            }
            if (sender.HasBuff(VeliaerisBuffs.lesserSistersBlessing))
            {
                args.armorAdd+= 10f;
                args.attackSpeedMultAdd *= 1.2f;
                args.moveSpeedMultAdd *= 1.3f;
                args.damageMultAdd *= 1.5f;
                args.regenMultAdd *= 2f;
                sender.bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath;
            }

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
    }
}