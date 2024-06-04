using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Survivors.Veliaeris;
using R2API;
using RoR2;
using VeliaerisMod.Modules.BaseStates;
using VeliaerisMod.Modules;
using RoR2.Skills;
using VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates;

//CharacterBody.BodyFlags.ImmuneToVoidDeath

namespace VeliaerisMod.Survivors.Veliaeris.SkillStates
{
    public class MergeandShift: BaseSkillState
    {
        
        private float stopwatch;
        public static SkillDef split;
        public static SkillDef reductionScythe;
        public static SkillDef selfBuffer;
        public static SkillDef circularSlash;
        public static VeliaerisRespawnSkillDef MergeandShiftSkill;
        public static SkillDef basicScythe;
        public static HuntressTrackingSkillDef CorruptAndHeal;
        public static HuntressTrackingSkillDef voidSkillDef;
        public static SkillDef allyBuff;
        public static SkillDef voidDetonation;
        public static SkillDef eldritchHealing;
        public override void OnEnter()
        {
            this.stopwatch = 0f;
            CharacterBody body;
            body = GetComponent<CharacterBody>();
            body.AddTimedBuff(VeliaerisBuffs.switchInvincibility, 1f);
        }

        public override void FixedUpdate()
        {

            base.FixedUpdate();
            if (base.inputBank.skill3.down)
            {
                //System.Console.WriteLine("skill3 is being held down");
                this.stopwatch += Time.fixedDeltaTime;
                //System.Console.WriteLine("stopwatch count: "+ stopwatch);
            }
            if(stopwatch>1)
            {
                if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris)
                {
                    VeliaerisPlugin.VeliaerisStates = VeliaerisState.Velia;
                    //System.Console.WriteLine("Entered Velia State");
                    VeliaerisPlugin.previousSplitSate = VeliaerisState.Velia;
                    HeldState.velState = VeliaerisState.Velia;
                }
                else if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
                {
                    VeliaerisPlugin.VeliaerisStates = VeliaerisState.Eris;
                    //System.Console.WriteLine("Entered Eris state");
                    VeliaerisPlugin.previousSplitSate = VeliaerisState.Eris;
                    HeldState.velState = VeliaerisState.Eris;
                }

                SkillSwitch(skillLocator,false);
                this.outer.SetNextStateToMain();
                return;
            }
            else if (base.inputBank.skill3.justReleased)
            {
                System.Console.WriteLine(VeliaerisPlugin.previousSplitSate);
                VeliaerisPlugin.VeliaerisStates = VeliaerisState.Veliaeris;
                HeldState.velState = VeliaerisState.Veliaeris;
                //System.Console.WriteLine("was pressed");
                SkillSwitch(skillLocator, false);
                this.outer.SetNextStateToMain();
                return;
            }



        }

        

        public override void OnExit() {
            base.OnExit();
        }
        
        
        

        public static void SkillSwitch(SkillLocator skillLocator, bool onHereticPickup)
        {
            //System.Console.WriteLine("Skillocator info" + skillLocator);
            //System.Console.WriteLine("Exit state: " + VeliaerisPlugin.VeliaerisStates);
            //System.Console.WriteLine("exit held state: " + HeldState.velState);

            

            AssetBundle assets = VeliaerisSurvivor.instance.assetBundle;
            string prefix = VeliaerisSurvivor.VELIAERIS_PREFIX;

            MergeandShiftSkill = Skills.CreateSkillDef<VeliaerisRespawnSkillDef>(new SkillDefInfo
            {
                skillName = "Merge and Shift",
                skillNameToken = prefix + "MERGE_UTILITY_NAME",
                skillDescriptionToken = prefix + "MERGE_UTILITY_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(MergeandShift)),
                activationStateMachineName = "Switch",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 15f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            split = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Split",
                skillNameToken = prefix + "SPLIT_UTILITY_NAME",
                skillDescriptionToken = prefix + "SPLIT_UTILITY_DESCRIPTION_"+VeliaerisPlugin.previousSplitSate,
                skillIcon = assets.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Split)),
                activationStateMachineName = "Switch",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 15f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });



            basicScythe = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            (
               "BasicScythe",
               prefix + "VELIAERIS_PRIMARY_NAME",
               prefix + "PRIMARY_SLASH_DESCRIPTION",
               assets.LoadAsset<Sprite>("texPrimaryIcon"),
               new EntityStates.SerializableEntityStateType(typeof(SkillStates.BasicScytheSlash)),
               "Weapon",
               true
            ));

            reductionScythe = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            (
               "VoidScythe",
               prefix + "VELIA_PRIMAR_NAME",
               prefix + "VELIA_PRIMARY_DESCRIPTION",
               assets.LoadAsset<Sprite>("texPrimaryIcon"),
               new EntityStates.SerializableEntityStateType(typeof(BasicScytheSlashWithReductions)),
               "Weapon",
               true
            ));


            CorruptAndHeal = Skills.CreateSkillDef<HuntressTrackingSkillDef>(new SkillDefInfo
            {
                skillName = "HealandCorrupt",
                skillNameToken = prefix + "VELIAERIS_SECONDARY_NAME",
                skillDescriptionToken = prefix + "VELIAERIS_SECONDARY_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assets.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Corrupt)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval=5f,
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



            voidSkillDef = Skills.CreateSkillDef<HuntressTrackingSkillDef>(new SkillDefInfo
            {
                skillName = "Grasp",
                skillNameToken = prefix + "ERIS_PRIMARY_NAME",
                skillDescriptionToken = prefix + "ERIS_PRIMARY_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(GraspOfOblivion)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 2f,
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

            allyBuff = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "TeamBuff",
                skillNameToken = prefix + "ERIS_SECONDARY_NAME",
                skillDescriptionToken = prefix + "ERIS_SECONDARY_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(GivenStrength)),
                activationStateMachineName = "Buff",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 15f,
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
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,


            });

            circularSlash = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AoESlash",
                skillNameToken = prefix + "VELIA_SECONDARY_NAME",
                skillDescriptionToken = prefix + "VELIA_SECONDARY_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assets.LoadAsset<Sprite>("texPrimaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(CircularSlash)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 20f,
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

            voidDetonation = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VoidDetonator",
                skillNameToken = prefix + "VELIAERIS_SPECIAL_NAME",
                skillDescriptionToken = prefix + "VELIAERIS_SPECIAL_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.VoidDetonator)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 30f,

                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
            });


            eldritchHealing = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HealingForEveryone",
                skillNameToken = prefix + "ERIS_SPECIAL_NAME",
                skillDescriptionToken = prefix + "ERIS_SPECIAL_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(EldritchHealing)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Detonator",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 20f,

                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
            });


            selfBuffer = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SelfBuff",
                skillNameToken = prefix + "VELIA_SPECIAL_NAME",
                skillDescriptionToken = prefix + "VELIA_SPECIAL_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(BlessingsFromBeyond)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Buff",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 30f,

                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
            });


            //            if (onDeath)
            //            {
            //                //System.Console.WriteLine("Check form");
            //                //System.Console.WriteLine("Form: " + VeliaerisPlugin.VeliaerisStates);
            //                skillLocator.primary.UnsetSkillOverride(skillLocator.primary, voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            //                skillLocator.primary.UnsetSkillOverride(skillLocator.primary, reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
            //                skillLocator.primary.UnsetSkillOverride(skillLocator.primary, voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            //                if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
            //                {
            //                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary, voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            //                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary, reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
            //                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary, basicScythe, GenericSkill.SkillOverridePriority.Contextual);

            ////                    System.Console.WriteLine("unset velia?");
            //                }
            //  //              System.Console.WriteLine("checking");
            //                if(VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris)
            //                {
            //                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary,voidSkillDef,GenericSkill.SkillOverridePriority.Contextual);
            //                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary,reductionScythe , GenericSkill.SkillOverridePriority.Contextual);
            //                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary, basicScythe, GenericSkill.SkillOverridePriority.Contextual);
            //    //                System.Console.WriteLine("unset eris?");
            //                }
            //            }
            //      System.Console.WriteLine("end of ondeathevents");
                
                if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris)
                {
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                if (!onHereticPickup)
                {
                    skillLocator.utility.SetSkillOverride(skillLocator.utility, split, GenericSkill.SkillOverridePriority.Contextual);
                }
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary, CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.special.SetSkillOverride(skillLocator.special, voidDetonation, GenericSkill.SkillOverridePriority.Contextual);

                }
                if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris)
                {
                    //                System.Console.WriteLine("Entered Eris");
                    skillLocator.special.SetSkillOverride(skillLocator.special, eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
                if (!onHereticPickup)
                {
                    skillLocator.utility.SetSkillOverride(skillLocator.utility, MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                }
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary, allyBuff, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                }
                if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
                {
                if (!onHereticPickup)
                {
                    skillLocator.utility.SetSkillOverride(skillLocator.utility, MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                }
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary, circularSlash, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.special.SetSkillOverride(skillLocator.special, selfBuffer, GenericSkill.SkillOverridePriority.Contextual);
                }
            int hereticLimit = 0;
            if (HeldState.hereticOverridesPrimary.Count > 0 || HeldState.hereticOverridesSecondary.Count > 0 || HeldState.hereticOverridesUtility.Count > 0 || HeldState.hereticOverridesSpecial.Count > 0)
            {
                if ((HeldState.hereticOverridesPrimary.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates==VeliaerisState.Veliaeris) || (HeldState.hereticOverridesPrimary.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates ==VeliaerisState.Eris) || (HeldState.hereticOverridesPrimary.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    hereticLimit++;
                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")),GenericSkill.SkillOverridePriority.Contextual);
                }
                if ((HeldState.hereticOverridesSecondary.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates ==VeliaerisState.Veliaeris) || (HeldState.hereticOverridesSecondary.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris) || (HeldState.hereticOverridesSecondary.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    hereticLimit++;
                    skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary,SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")),GenericSkill.SkillOverridePriority.Contextual);
                }
                if ((HeldState.hereticOverridesUtility.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris) || (HeldState.hereticOverridesUtility.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris) || (HeldState.hereticOverridesUtility.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    hereticLimit++;
                    skillLocator.utility.UnsetSkillOverride(skillLocator.utility, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.utility.SetSkillOverride(skillLocator.utility,SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")),GenericSkill.SkillOverridePriority.Contextual);
                }
                if ((HeldState.hereticOverridesSpecial.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris) || (HeldState.hereticOverridesSpecial.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris) || (HeldState.hereticOverridesSpecial.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    hereticLimit++;
                    skillLocator.special.UnsetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.special.SetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                }
            }
//            System.Console.WriteLine("Output");
        }
    }
}
