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

        

        public override void OnEnter()
        {
            System.Console.WriteLine("Entered MergeandShift");
            this.stopwatch = 0f;
            System.Console.WriteLine("entered state",VeliaerisPlugin.VeliaerisStates);
            
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

                SkillSwitch(skillLocator);
                this.outer.SetNextStateToMain();
                return;
            }
            else if (base.inputBank.skill3.justReleased)
            {
                VeliaerisPlugin.VeliaerisStates = VeliaerisState.Veliaeris;
                HeldState.velState = VeliaerisState.Veliaeris;
                //System.Console.WriteLine("was pressed");
                SkillSwitch(skillLocator);
                this.outer.SetNextStateToMain();
                return;
            }



        }

        public override void OnExit() {
            base.OnExit();
        }
        

        
        

        public static void SkillSwitch(SkillLocator skillLocator)
        {
            //System.Console.WriteLine("Skillocator info" + skillLocator);
            //System.Console.WriteLine("Exit state: " + VeliaerisPlugin.VeliaerisStates);
            //System.Console.WriteLine("exit held state: " + HeldState.velState);
            AssetBundle assets = VeliaerisSurvivor.instance.assetBundle;
                    string prefix = VeliaerisSurvivor.VELIAERIS_PREFIX;

            SkillDef MergeandShift = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Merge and Shift",
                skillNameToken = prefix + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(MergeandShift)),
                activationStateMachineName = "Slide",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
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

            SkillDef split = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Split",
                skillNameToken = prefix + "UTILITY_ROLL_NAME",
                skillDescriptionToken = prefix + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Split)),
                activationStateMachineName = "Slide",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
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



            SkillDef basicScythe = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            (
               "BasicScythe",
               prefix + "PRIMARY_SLASH_NAME",
               prefix + "PRIMARY_SLASH_DESCRIPTION",
               assets.LoadAsset<Sprite>("texPrimaryIcon"),
               new EntityStates.SerializableEntityStateType(typeof(SkillStates.BasicScytheSlash)),
               "Weapon",
               true
            ));

            SkillDef reductionScythe = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            (
               "BasicScythe",
               prefix + "PRIMARY_SLASH_NAME",
               prefix + "PRIMARY_SLASH_DESCRIPTION",
               assets.LoadAsset<Sprite>("texPrimaryIcon"),
               new EntityStates.SerializableEntityStateType(typeof(BasicScytheSlashWithReductions)),
               "Weapon",
               true
            ));


            HuntressTrackingSkillDef CorruptAndHeal = Skills.CreateSkillDef<HuntressTrackingSkillDef>(new SkillDefInfo
            {
                skillName = "HealandCorrupt",
                skillNameToken = prefix + "SECONDARY_GUN_NAME",
                skillDescriptionToken = prefix + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assets.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Corrupt)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval=15f,
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



            HuntressTrackingSkillDef voidSkillDef = Skills.CreateSkillDef<HuntressTrackingSkillDef>(new SkillDefInfo
            (
                "BasicScythe",
               prefix + "PRIMARY_SLASH_NAME",
               prefix + "PRIMARY_SLASH_DESCRIPTION",
               assets.LoadAsset<Sprite>("texPrimaryIcon"),
               new EntityStates.SerializableEntityStateType(typeof(graspOfOblivion)),
               "Weapon",
               true


            ));

            SkillDef allyBuff = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "TeamBuff",
                skillNameToken = prefix + "PRIMARY_SLASH_NAME",
                skillDescriptionToken = prefix + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assets.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(GivenStrength)),
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

            SkillDef circularSlash = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AoESlash",
                skillNameToken = prefix + "PRIMARY_SLASH_NAME",
                skillDescriptionToken = prefix + "PRIMARY_SLASH_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assets.LoadAsset<Sprite>("texPrimaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(CircularSlash)),
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

            SkillDef voidDetonation = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VoidDetonator",
                skillNameToken = prefix + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.VoidDetonator)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });


            SkillDef eldritchHealing = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HealingForEveryone",
                skillNameToken = prefix + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(EldritchHealing)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });


            SkillDef selfBuffer = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SelfBuff",
                skillNameToken = prefix + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assets.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(blessingsFromBeyond)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris)
            {
                skillLocator.primary.SetSkillOverride(skillLocator.primary, basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                skillLocator.utility.SetSkillOverride(skillLocator.utility, split, GenericSkill.SkillOverridePriority.Contextual);
                skillLocator.secondary.SetSkillOverride(skillLocator.secondary, CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                skillLocator.special.SetSkillOverride(skillLocator.special,voidDetonation,GenericSkill.SkillOverridePriority.Contextual);

            }
            if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris)
            {
                System.Console.WriteLine("Entered Eris Switch");
                skillLocator.special.SetSkillOverride(skillLocator.special, eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
                skillLocator.utility.SetSkillOverride(skillLocator.utility, MergeandShift, GenericSkill.SkillOverridePriority.Contextual);
                skillLocator.secondary.SetSkillOverride(skillLocator.secondary, allyBuff, GenericSkill.SkillOverridePriority.Contextual);
                skillLocator.primary.SetSkillOverride(skillLocator.primary,voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
            if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
            {
                System.Console.WriteLine("Entered Velia Switch");
                skillLocator.utility.SetSkillOverride(skillLocator.utility, MergeandShift, GenericSkill.SkillOverridePriority.Contextual);
                skillLocator.secondary.SetSkillOverride(skillLocator.secondary, circularSlash, GenericSkill.SkillOverridePriority.Contextual);
                skillLocator.primary.SetSkillOverride(skillLocator.primary, reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
                skillLocator.special.SetSkillOverride(skillLocator.special,selfBuffer, GenericSkill.SkillOverridePriority.Contextual);
            }
            System.Console.WriteLine("output");
            if (HeldState.hereticOverridesPrimary.Count > 0)
            {
                System.Console.WriteLine("heldcount: " + HeldState.hereticOverridesPrimary.Count);
                System.Console.WriteLine("Heldstate value: " + HeldState.hereticOverridesPrimary[0]);
            }
            if (HeldState.hereticOverridesPrimary.Count > 0 || HeldState.hereticOverridesSecondary.Count > 0 || HeldState.hereticOverridesUtility.Count > 0 || HeldState.hereticOverridesSpecial.Count > 0)
            {
                System.Console.WriteLine("Entered if");
                if ((HeldState.hereticOverridesPrimary.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates==VeliaerisState.Veliaeris) || (HeldState.hereticOverridesPrimary.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates ==VeliaerisState.Eris) || (HeldState.hereticOverridesPrimary.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                   
                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")),GenericSkill.SkillOverridePriority.Contextual);
                }
                if ((HeldState.hereticOverridesSecondary.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates ==VeliaerisState.Veliaeris) || (HeldState.hereticOverridesSecondary.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris) || (HeldState.hereticOverridesSecondary.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary,SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")),GenericSkill.SkillOverridePriority.Contextual);
                }
                if ((HeldState.hereticOverridesUtility.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris) || (HeldState.hereticOverridesUtility.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris) || (HeldState.hereticOverridesUtility.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    skillLocator.utility.UnsetSkillOverride(skillLocator.utility, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.utility.SetSkillOverride(skillLocator.utility,SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")),GenericSkill.SkillOverridePriority.Contextual);
                }
                if ((HeldState.hereticOverridesSpecial.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris) || (HeldState.hereticOverridesSpecial.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris) || (HeldState.hereticOverridesSpecial.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    skillLocator.special.UnsetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.special.SetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                }
            }
        }
    }
}
