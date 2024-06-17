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
using System.Linq;
using UnityEngine.Networking;
using EntityStates.Missions.Moon;

//CharacterBody.BodyFlags.ImmuneToVoidDeath

namespace VeliaerisMod.Survivors.Veliaeris.SkillStates
{
    public class MergeandShift: BaseSkillState
    {
        private HurtBox[] targetTargets;
        private HurtBox[] targetTargetsCurse;
        private float stopwatch;
        private float VeliaDesolationRange = 40f;
        public override void OnEnter()
        {

            System.Console.WriteLine("Entered");
            this.stopwatch = 0f;
            CharacterBody body;
            body = GetComponent<CharacterBody>();
        }

        public override void FixedUpdate()
        {
            System.Console.WriteLine("fixed");
            CharacterBody body;
            body = GetComponent<CharacterBody>();
            base.FixedUpdate();

                body.AddTimedBuff(VeliaerisBuffs.switchInvincibility, 1f);
                //System.Console.WriteLine("skill3 is being held down");
                this.stopwatch += Time.fixedDeltaTime;
                //System.Console.WriteLine("stopwatch count: "+ stopwatch);
            System.Console.WriteLine("stopwatch:" + stopwatch);
            //            System.Console.WriteLine("t")
            //System.Console.WriteLine("stopwatch count: "+ stopwatch);
            if (stopwatch>1)
            {
                if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris)
                {

                    VeliaerisPlugin.VeliaerisStates = VeliaerisState.Velia;
                    //System.Console.WriteLine("Entered Velia State");
                    VeliaerisPlugin.previousSplitSate = VeliaerisState.Velia;
                    HeldState.velState = VeliaerisState.Velia;
                    HeldState.paststate = VeliaerisState.Velia;
                    SkillSwitch(skillLocator, false);
                }
                else if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
                {

                    HeldState.paststate = VeliaerisState.Eris;
//                    System.Console.WriteLine("refute stacks: " + VeliaerisSurvivor.DeathPreventionStacks);
                    VeliaerisPlugin.VeliaerisStates = VeliaerisState.Eris;
                    //System.Console.WriteLine("Entered Eris state");
                    VeliaerisPlugin.previousSplitSate = VeliaerisState.Eris;
                    HeldState.velState = VeliaerisState.Eris;
                    SkillSwitch(skillLocator, false);
                }
                this.outer.SetNextStateToMain();
                return;

            }
            if (!inputBank.skill3.down)
            {
                this.outer.SetNextStateToMain();
                return;
            }




        }



        public override void OnExit() {
            CharacterBody body;
            body = GetComponent<CharacterBody>();
            if (stopwatch < 1)
            {
                HeldState.paststate = VeliaerisPlugin.previousSplitSate;
                System.Console.WriteLine("split state:" + VeliaerisPlugin.previousSplitSate);
                VeliaerisPlugin.VeliaerisStates = VeliaerisState.Veliaeris;
                HeldState.velState = VeliaerisState.Veliaeris;
                SkillSwitch(skillLocator, false);
             
                if (body.HasBuff(VeliaerisBuffs.revokeDeath))
                {
                    body.SetBuffCount(VeliaerisBuffs.revokeDeath.buffIndex, 0);
                }
                if (body.HasBuff(VeliaerisBuffs.inflictDeath))
                {
                    body.SetBuffCount(VeliaerisBuffs.inflictDeath.buffIndex, 0);
                }
            }

            System.Console.WriteLine("Exit plugin: " + VeliaerisPlugin.VeliaerisStates);
            if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris)
            {
                System.Console.WriteLine("entered curses");
                BullseyeSearch targetSearch = new BullseyeSearch();
                targetSearch.filterByDistinctEntity = true;
                targetSearch.filterByLoS = false;
                targetSearch.maxDistanceFilter = 30f;
                targetSearch.minDistanceFilter = 0f;
                targetSearch.minAngleFilter = 0f;
                targetSearch.maxAngleFilter = 360f;
                targetSearch.sortMode = BullseyeSearch.SortMode.Distance;
                targetSearch.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
                targetSearch.searchOrigin = base.characterBody.corePosition;
                targetSearch.RefreshCandidates();
                targetSearch.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> resultsCurse = targetSearch.GetResults();
                this.targetTargetsCurse = resultsCurse.ToArray<HurtBox>();
                //            }

                //          if (NetworkServer.active)
                //        {
                for (int i = 0; i < this.targetTargetsCurse.Length; i++)
                {
                    for (int j = 0; j < (VeliaerisSurvivor.voidInfluence / 10) + 1; j++)
                    {
                        targetTargetsCurse[i].healthComponent.body.AddTimedBuffAuthority(RoR2Content.Buffs.PermanentCurse.buffIndex,float.PositiveInfinity);
                    }
                    //                    targetTargets[i].healthComponent.body.AddBuff(RoR2Content.Buffs.PermanentCurse);
                }
            }
            if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
            {
                if (VeliaerisSurvivor.voidInfluence >= 20)
            {
                //  if (NetworkServer.active)
                //{
                BullseyeSearch targetSearch = new BullseyeSearch();
                targetSearch.filterByDistinctEntity = true;
                targetSearch.filterByLoS = false;
                targetSearch.maxDistanceFilter = 15f;
                targetSearch.minDistanceFilter = 0f;
                targetSearch.minAngleFilter = 0f;
                targetSearch.maxAngleFilter = 360f;
                targetSearch.sortMode = BullseyeSearch.SortMode.Distance;
                targetSearch.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
                targetSearch.searchOrigin = base.characterBody.corePosition;
                targetSearch.RefreshCandidates();
                targetSearch.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> results = targetSearch.GetResults();
                this.targetTargets = results.ToArray<HurtBox>();
                DamageInfo desolation = new DamageInfo();
                HurtBox targetHurtBox = null;
                for (int l = 0; l < targetTargets.Length; l++)
                {
                    Util.Swap<HurtBox>(ref targetHurtBox, ref this.targetTargets[l]);
                        if (!targetHurtBox.healthComponent.body.isBoss)
                        {
                            desolation.damage = targetHurtBox.healthComponent.combinedHealth;
                            desolation.attacker = this.characterBody.gameObject;
                            desolation.inflictor = null;
                            desolation.force = Vector3.zeroVector;
                            desolation.procCoefficient = 0f;
                            desolation.position = targetHurtBox.transform.position;
                            desolation.damageColorIndex = DamageColorIndex.Void;
                            desolation.damageType = DamageType.VoidDeath;
                            targetHurtBox.healthComponent.TakeDamage(desolation);
                        }
                }
                // }
            }

                //                    if (NetworkServer.active)
                //                  {
                BullseyeSearch targetSearchVoid = new BullseyeSearch();
                targetSearchVoid.filterByDistinctEntity = true;
                targetSearchVoid.filterByLoS = false;
                targetSearchVoid.maxDistanceFilter = VeliaDesolationRange + VeliaerisSurvivor.voidInfluence;
                targetSearchVoid.minDistanceFilter = 0f;
                targetSearchVoid.minAngleFilter = 0f;
                targetSearchVoid.maxAngleFilter = 360f;
                targetSearchVoid.sortMode = BullseyeSearch.SortMode.Distance;
                targetSearchVoid.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
                targetSearchVoid.searchOrigin = base.characterBody.corePosition;
                targetSearchVoid.RefreshCandidates();
                targetSearchVoid.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> resultsvoid = targetSearchVoid.GetResults();
                this.targetTargets = resultsvoid.ToArray<HurtBox>();
                DamageInfo desolationvoid = new DamageInfo();
                HurtBox targetHurtBoxvoid = null;
                for (int l = 0; l < targetTargets.Length; l++)
                {
                    Util.Swap<HurtBox>(ref targetHurtBoxvoid, ref this.targetTargets[l]);
                    //float hitSeverity;
                    //float finalRadius = 30f;
                    //float num = Vector3.Distance(body.corePosition, targetHurtBoxvoid.healthComponent.body.corePosition);
                    //hitSeverity = Mathf.Clamp01(1f - num / finalRadius);
                    desolationvoid.damage = targetHurtBoxvoid.healthComponent.combinedHealth * 0.25f;
                    desolationvoid.attacker = this.characterBody.gameObject;
                    desolationvoid.inflictor = null;
                    desolationvoid.force = Vector3.zero;
                    desolationvoid.procCoefficient = 0f;
                    desolationvoid.position = targetHurtBoxvoid.transform.position;
                    desolationvoid.damageColorIndex = DamageColorIndex.Void;
                    desolationvoid.damageType = DamageType.BonusToLowHealth;
                    targetHurtBoxvoid.healthComponent.TakeDamage(desolationvoid);
 //                   Vector3 normalized = (targetHurtBoxvoid.healthComponent.body.corePosition - body.corePosition).normalized;
  //                  body.characterMotor.ApplyForce(normalized * (20f));
                }
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = body.gameObject;
                blastAttack.inflictor = body.gameObject;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.position = body.footPosition;
                blastAttack.procCoefficient = 1f;
                blastAttack.radius = VeliaDesolationRange + VeliaerisSurvivor.voidInfluence;
                blastAttack.baseForce = 200f * 50f+(VeliaerisSurvivor.voidInfluence*5f);
//                blastAttack.bonusForce = Vector3.up * 2000f;
                blastAttack.baseDamage = 0;
                blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                blastAttack.crit = Util.CheckRoll(body.crit, body.master);
                blastAttack.damageColorIndex = DamageColorIndex.Void;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.Fire();
            }
            if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris)
            {
                float Erishealth = body.healthComponent.fullHealth+(body.healthComponent.fullHealth*0.75f);
                TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].teamIndex == teamComponent.teamIndex)
                    {
                        array[i].GetComponent<CharacterBody>().healthComponent.AddBarrierAuthority(Erishealth * 0.5f);
                    }
                }
            }
            //                }

            base.OnExit();
        }


        public static void skillSet(SkillLocator skillLocator)
        {
            System.Console.WriteLine("Skillset heldstate: " + HeldState.velState);
            switch (HeldState.initalState)
            {
                case VeliaerisState.Veliaeris:
                    System.Console.WriteLine("SkillSetVeliaeris");
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);
                    break;
                case VeliaerisState.Eris:
                    System.Console.WriteLine("SkillSetEris");
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
                    break;
                case VeliaerisState.Velia:
                    System.Console.WriteLine("SkillSetVelia");
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Contextual);
                    break;
                default:
                    System.Console.WriteLine("SkillSetDefault");
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
                    skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);
                    break;
            }
        }

        public static void SkillSwitch(SkillLocator skillLocator, bool isHereticPickup)
        {
           // if (isHereticPickup==false)
          //  {
                System.Console.WriteLine("Skillswitch heldstate: " + HeldState.velState);
                System.Console.WriteLine("Pluginstate: " + VeliaerisPlugin.VeliaerisStates);
                System.Console.WriteLine("inital" + HeldState.initalState);
                switch (HeldState.initalState)
                {
                    case VeliaerisState.Veliaeris:
                        System.Console.WriteLine("Switch Case: Veliaeris");
                        
                                skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Network);
                                skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Network);
                                skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Network);
                                skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Network);
                                skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Network);
                                skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Network);
                                skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Network);
                                skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Network);
                        
                        if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris)
                        {
                            skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Network);
                        }
                        if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
                        {
                            skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Network);
                        }
                        break;
                    case VeliaerisState.Eris:
                        System.Console.WriteLine("Switch Case: Eris");

                        System.Console.WriteLine("entered unset");
                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Network);
                        
                        if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris)
                        {
                        System.Console.WriteLine("Entered veliaeris set");
                            skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Network);
                        }
                        if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
                        {
                        System.Console.WriteLine("Entered velia set");
                        skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Network);
                        }
                            
                        break;
                    case VeliaerisState.Velia:
                        System.Console.WriteLine("Switch Case: Velia");


                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Network);
                        
                        if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris)
                        {
                            skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Network);
                        }
                        if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris)
                        {
                            skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Network);
                            skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Network);
                        }
                        break;
                    default:
                        break;
                }
            //}
            System.Console.WriteLine("ranswitch");
            int hereticLimit = 0;
            if (HeldState.gatheredPrimary > 0 || HeldState.gatheredSecondary> 0 || HeldState.gatheredUtility> 0 || HeldState.gatheredSpecial> 0)
            {
                System.Console.WriteLine("Entered heretic");
                if ((HeldState.hereticOverridesPrimary.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates==VeliaerisState.Veliaeris) || (HeldState.hereticOverridesPrimary.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates ==VeliaerisState.Eris) || (HeldState.hereticOverridesPrimary.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    hereticLimit++;
                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")), GenericSkill.SkillOverridePriority.Network);
                    skillLocator.primary.SetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")),GenericSkill.SkillOverridePriority.Network);
                }
                if ((HeldState.hereticOverridesSecondary.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates ==VeliaerisState.Veliaeris) || (HeldState.hereticOverridesSecondary.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris) || (HeldState.hereticOverridesSecondary.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    hereticLimit++;
                    skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")), GenericSkill.SkillOverridePriority.Network);
                    skillLocator.secondary.SetSkillOverride(skillLocator.secondary,SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")),GenericSkill.SkillOverridePriority.Network);
                }
                if ((HeldState.hereticOverridesUtility.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris) || (HeldState.hereticOverridesUtility.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris) || (HeldState.hereticOverridesUtility.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    hereticLimit++;
                    skillLocator.utility.UnsetSkillOverride(skillLocator.utility, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")), GenericSkill.SkillOverridePriority.Network);
                    skillLocator.utility.SetSkillOverride(skillLocator.utility,SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")),GenericSkill.SkillOverridePriority.Network);
                }
                if ((HeldState.hereticOverridesSpecial.Contains(VeliaerisState.Veliaeris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris) || (HeldState.hereticOverridesSpecial.Contains(VeliaerisState.Eris) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris) || (HeldState.hereticOverridesSpecial.Contains(VeliaerisState.Velia) && VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia))
                {
                    System.Console.WriteLine("Entered heretic Special");
                    hereticLimit++;
                    skillLocator.special.UnsetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Network);
                    skillLocator.special.SetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Network);
                }
            }
            System.Console.WriteLine("remove");
            if (HeldState.hereticOverridesPrimary.Contains(VeliaerisPlugin.VeliaerisStates) == false)
            {
                skillLocator.primary.UnsetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")), GenericSkill.SkillOverridePriority.Network);
            }
            if (HeldState.hereticOverridesSecondary.Contains(VeliaerisPlugin.VeliaerisStates) == false)
            {
                skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")), GenericSkill.SkillOverridePriority.Network);
            }
            if (HeldState.hereticOverridesUtility.Contains(VeliaerisPlugin.VeliaerisStates) == false)
            {
                skillLocator.utility.UnsetSkillOverride(skillLocator.utility, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")), GenericSkill.SkillOverridePriority.Network);
            }
            if (HeldState.hereticOverridesSpecial.Contains(VeliaerisPlugin.VeliaerisStates) == false)
            {
                System.Console.WriteLine("remove special");
                skillLocator.special.UnsetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Network);
            }                 
            
//            System.Console.WriteLine("Output");

/*                if (paststate == VeliaerisState.Veliaeris)
    {

        System.Console.WriteLine("entered veliaeris unbind");
        skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);
    }
    if (paststate == VeliaerisState.Eris)
    {
        System.Console.WriteLine("entered eris unbind");
        skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
    }
    if (paststate == VeliaerisState.Velia)
    {
        System.Console.WriteLine("entered velia unbind");
        skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Contextual);
    }


//            VeliaerisPlugin.VeliaerisStates = HeldState.velState;
System.Console.WriteLine("Velastate merge:" + VeliaerisPlugin.VeliaerisStates);
if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Veliaeris)
{
    System.Console.WriteLine("Entered Veliaeris");
    skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
    if (!onHereticPickup)
    {
        skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
    }
    skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
    skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);
}

    if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Eris)
    {
                        System.Console.WriteLine("Entered Eris");
        skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
    if (!onHereticPickup)
    {
        skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
    }
        skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
    }
    if (VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
    {
    System.Console.WriteLine("Entered Velia");
    if (!onHereticPickup)
    {
        skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
    }
        skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
        skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Contextual);
    }*/
}
}
}
