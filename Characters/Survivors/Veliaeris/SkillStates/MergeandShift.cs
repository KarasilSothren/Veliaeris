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

//CharacterBody.BodyFlags.ImmuneToVoidDeath

namespace VeliaerisMod.Survivors.Veliaeris.SkillStates
{
    public class MergeandShift: BaseSkillState
    {
        private HurtBox[] targetTargets;
        private HurtBox[] targetTargetsCurse;
        private float stopwatch;
        private float VeliaDesolationRange = 40f;
        private static VeliaerisSurvivorController VeliaerisSurvivorController;
        private static VeliaerisState velStartstate;

        public override void OnEnter()
        {
            CharacterBody body;
            body = GetComponent<CharacterBody>();
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
            System.Console.WriteLine("controller state: " + VeliaerisSurvivorController.VeliaerisState);
            //            System.Console.WriteLine("Entered");
            this.stopwatch = 0f;
            Debug.Log("current body:" + body.name);
        }

        public override void FixedUpdate()
        {
//            System.Console.WriteLine(VeliaerisSurvivorController.testState);
            System.Console.WriteLine("fixed");
            CharacterBody body;
            body = GetComponent<CharacterBody>();
            //            base.FixedUpdate();
            base.FixedUpdate();
            //System.Console.WriteLine("skill3 is being held down");
            this.stopwatch += Time.fixedDeltaTime;
                System.Console.WriteLine("stopwatch count: "+ stopwatch);
            //            System.Console.WriteLine("t")
            //System.Console.WriteLine("stopwatch count: "+ stopwatch);
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
            if (NetworkServer.active)
            {
                System.Console.WriteLine("networkactive");
                Debug.Log("networksuvivorteststate:"+VeliaerisSurvivorController.VeliaerisState);
            }
            else
            {
                System.Console.WriteLine("networknotactive");
                Debug.Log("networksuvivorteststate:"+VeliaerisSurvivorController.VeliaerisState);
            }
            float baseMaxeHealth = this.characterBody.baseMaxHealth;
            float erisHealth = baseMaxeHealth * 0.75f + baseMaxeHealth + baseMaxeHealth * 0.1f * VeliaerisSurvivor.voidInfluence;
            Debug.Log("base max health:" + baseMaxeHealth);
            Debug.Log("fake eris health:" + erisHealth );
            if ((stopwatch>1.2 &&base.isAuthority)||(stopwatch>1&&NetworkServer.active))
            {
                if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris)
                {
                    body.SetBuffCount(VeliaerisBuffs.ErisStatChanges.buffIndex, 0);
                    body.SetBuffCount(VeliaerisBuffs.VeliaStatChanges.buffIndex, 1);
                    VeliaerisSurvivorController.network_veliaerisStates = VeliaerisState.Velia;
                    //System.Console.WriteLine("Entered Velia State");
                    VeliaerisSurvivorController.network_previousState = VeliaerisState.Velia;
                    VeliaerisSurvivorController.network_velState = VeliaerisState.Velia;
                    VeliaerisSurvivorController.network_paststate = VeliaerisState.Velia;
                    SkillSwitch(skillLocator, true,body,false,false);
                }
                else if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia)
                {
                    body.SetBuffCount(VeliaerisBuffs.VeliaStatChanges.buffIndex, 0);
                    body.SetBuffCount(VeliaerisBuffs.ErisStatChanges.buffIndex, 1);
                    VeliaerisSurvivorController.network_paststate = VeliaerisState.Eris;
//                    System.Console.WriteLine("refute stacks: " + VeliaerisSurvivor.DeathPreventionStacks);
                    VeliaerisSurvivorController.network_veliaerisStates = VeliaerisState.Eris;
                    System.Console.WriteLine("Entered Eris state");
                    VeliaerisSurvivorController.network_previousState = VeliaerisState.Eris;
                    VeliaerisSurvivorController.network_velState = VeliaerisState.Eris;
                    TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
                    //(sender.baseMaxHealth * 0.1f) *voidInfluence
                    //args.baseHealthAdd += sender.baseMaxHealth * 0.75f;
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i].teamIndex == teamComponent.teamIndex)
                        {

                            array[i].GetComponent<CharacterBody>().healthComponent.AddBarrierAuthority(erisHealth);
                        }
                    }
                    SkillSwitch(skillLocator, true,body,false,false);
                }
                body.AddTimedBuff(VeliaerisBuffs.switchInvincibility, 1f);
                this.outer.SetNextStateToMain();
                return;

            }
            if (!inputBank.skill3.down && base.isAuthority)
            {
                body.SetBuffCount(VeliaerisBuffs.ErisStatChanges.buffIndex, 0);
                body.SetBuffCount(VeliaerisBuffs.VeliaStatChanges.buffIndex, 0);
                body.SetBuffCount(VeliaerisBuffs.VeliaerisStatChanges.buffIndex, 1);
                System.Console.WriteLine("Let go");
                body.AddTimedBuff(VeliaerisBuffs.switchInvincibility, 1f);
                this.outer.SetNextStateToMain();
                return;
            }
            




        }



        public override void OnExit() {
            CharacterBody body;
            body = GetComponent<CharacterBody>();
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
            if (stopwatch < 1)
            {
                VeliaerisSurvivorController.network_paststate = VeliaerisSurvivorController.previousSplitSate;
                System.Console.WriteLine("split state:" + VeliaerisSurvivorController.previousSplitSate);
                VeliaerisSurvivorController.network_veliaerisStates = VeliaerisState.Veliaeris;
                VeliaerisSurvivorController.network_velState = VeliaerisState.Veliaeris;
                SkillSwitch(skillLocator, true, body,false,false);
             
                if (body.HasBuff(VeliaerisBuffs.revokeDeath))
                {
                    body.SetBuffCount(VeliaerisBuffs.revokeDeath.buffIndex, 0);
                }
                if (body.HasBuff(VeliaerisBuffs.inflictDeath))
                {
                    body.SetBuffCount(VeliaerisBuffs.inflictDeath.buffIndex, 0);
                }
            }

            System.Console.WriteLine("Exit plugin: " + VeliaerisSurvivorController.VeliaerisState);
            switch(VeliaerisSurvivorController.VeliaerisState)
            {
                case VeliaerisState.Veliaeris:
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
                    for (int j = 0; j < (VeliaerisSurvivor.voidInfluence / VeliaerisStaticValues.firstTierCorruption) + 1; j++)
                    {
                        targetTargetsCurse[i].healthComponent.body.AddTimedBuff(RoR2Content.Buffs.PermanentCurse.buffIndex,float.PositiveInfinity);
                    }
                    //                    targetTargets[i].healthComponent.body.AddBuff(RoR2Content.Buffs.PermanentCurse);
                }
                    break;


                case VeliaerisState.Velia:
                System.Console.WriteLine("Entered burst");
                if (VeliaerisSurvivor.voidInfluence >= VeliaerisStaticValues.secondTierCorruption)
                {
                //  if (NetworkServer.active)
                //{
                BullseyeSearch targetSearchChasm = new BullseyeSearch();
                targetSearchChasm.filterByDistinctEntity = true;
                targetSearchChasm.filterByLoS = false;
                targetSearchChasm.maxDistanceFilter = 15f/*+((VeliaerisSurvivor.voidInfluence-20)/2)/10f*/;
                targetSearchChasm.minDistanceFilter = 0f;
                targetSearchChasm.minAngleFilter = 0f;
                targetSearchChasm.maxAngleFilter = 360f;
                targetSearchChasm.sortMode = BullseyeSearch.SortMode.Distance;
                targetSearchChasm.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
                targetSearchChasm.searchOrigin = base.characterBody.corePosition;
                targetSearchChasm.RefreshCandidates();
                targetSearchChasm.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> results = targetSearchChasm.GetResults();
                this.targetTargets = results.ToArray<HurtBox>();
                DamageInfo desolation = new DamageInfo();
                HurtBox targetHurtBox = null;
                for (int l = 0; l < targetTargets.Length; l++)
                {
                    Util.Swap<HurtBox>(ref targetHurtBox, ref this.targetTargets[l]);
                        if (!targetHurtBox.healthComponent.body.isBoss||!targetHurtBox.healthComponent.body.isPlayerControlled)
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
                BullseyeSearch targetSearchWarp = new BullseyeSearch();
                targetSearchWarp.filterByDistinctEntity = true;
                targetSearchWarp.filterByLoS = false;
                targetSearchWarp.maxDistanceFilter = VeliaDesolationRange + VeliaerisSurvivor.voidInfluence;
                targetSearchWarp.minDistanceFilter = 0f;
                targetSearchWarp.minAngleFilter = 0f;
                targetSearchWarp.maxAngleFilter = 360f;
                targetSearchWarp.sortMode = BullseyeSearch.SortMode.Distance;
                targetSearchWarp.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
                targetSearchWarp.searchOrigin = base.characterBody.corePosition;
                targetSearchWarp.RefreshCandidates();
                targetSearchWarp.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> resultsvoid = targetSearchWarp.GetResults();
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
                blastAttack.baseForce = 200f * 50f+(VeliaerisSurvivor.voidInfluence*0.5f);
//                blastAttack.bonusForce = Vector3.up * 2000f;
                blastAttack.baseDamage = 0;
                blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                blastAttack.crit = Util.CheckRoll(body.crit, body.master);
                blastAttack.damageColorIndex = DamageColorIndex.Void;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.Fire();
                break;

                            }
            //                }

            base.OnExit();
            VeliaerisStatuses.utilityCooldown = VeliaerisSurvivor.utilitycooldown;
        }

        public static void skillSet(SkillLocator skillLocator,CharacterBody body)
        {
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
           // Debug.Log("entered uncorrupted");
            velStartstate = body.GetComponent<VeliaerisPassive>().getStartState();
           // System.Console.WriteLine("Skillset heldstate: " + VeliaerisSurvivorController.velState);
            switch (velStartstate)
            {
                case VeliaerisState.Veliaeris:
             //       System.Console.WriteLine("SkillSetVeliaeris");
                    skillLocator.primary.SetBaseSkill(VeliaerisSurvivor.basicScythe);
                    skillLocator.secondary.SetBaseSkill(VeliaerisSurvivor.CorruptAndHeal);
                    skillLocator.utility.SetBaseSkill(VeliaerisSurvivor.split);
                    skillLocator.special.SetBaseSkill(VeliaerisSurvivor.voidDetonation);
//                    skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);
                    break;
                case VeliaerisState.Eris:
               //     System.Console.WriteLine("SkillSetEris");
                    skillLocator.primary.SetBaseSkill(VeliaerisSurvivor.voidSkillDef);
                    skillLocator.secondary.SetBaseSkill(VeliaerisSurvivor.allyBuff);
                    skillLocator.utility.SetBaseSkill(VeliaerisSurvivor.MergeandShiftSkill);
                    skillLocator.special.SetBaseSkill(VeliaerisSurvivor.eldritchHealing);
                    //skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
                    break;
                case VeliaerisState.Velia:
                 //   System.Console.WriteLine("SkillSetVelia");
                    skillLocator.primary.SetBaseSkill(VeliaerisSurvivor.reductionScythe);
                    skillLocator.secondary.SetBaseSkill(VeliaerisSurvivor.circularSlash);
                    skillLocator.utility.SetBaseSkill(VeliaerisSurvivor.MergeandShiftSkill);
                    skillLocator.special.SetBaseSkill(VeliaerisSurvivor.selfBuffer);
                    //skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Contextual);
                    break;
                default:
                   // System.Console.WriteLine("SkillSetDefault");
                    skillLocator.primary.SetBaseSkill(VeliaerisSurvivor.basicScythe);
                    skillLocator.secondary.SetBaseSkill(VeliaerisSurvivor.CorruptAndHeal);
                    skillLocator.utility.SetBaseSkill(VeliaerisSurvivor.split);
                    skillLocator.special.SetBaseSkill(VeliaerisSurvivor.voidDetonation);
                    //skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
                    //skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);
                    break;
            }
        }

        public static void skillSetCorrupted(SkillLocator skillLocator, CharacterBody body)
        {
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
            //Debug.Log("entered corrupted");
            velStartstate = body.GetComponent<VeliaerisPassive>().getStartState();
          //  System.Console.WriteLine("Skillset heldstate: " + VeliaerisSurvivorController.velState);
            switch (velStartstate)
            {
                case VeliaerisState.Veliaeris:
                  //  System.Console.WriteLine("SkillSetVeliaerisCorrupted");
                    skillLocator.primary.SetBaseSkill(VeliaerisSurvivor.basicScytheCorrupted);
                    skillLocator.secondary.SetBaseSkill(VeliaerisSurvivor.CorruptAndHealCorrupted);
                    skillLocator.utility.SetBaseSkill(VeliaerisSurvivor.splitCorrupted);
                    skillLocator.special.SetBaseSkill(VeliaerisSurvivor.voidDetonationCorrupted);
                    break;
                case VeliaerisState.Eris:
                  //  System.Console.WriteLine("SkillSetErisCorrupted");
                    skillLocator.primary.SetBaseSkill(VeliaerisSurvivor.voidSkillDefCorrupted);
                    skillLocator.secondary.SetBaseSkill(VeliaerisSurvivor.allyBuffCorrupted);
                    skillLocator.utility.SetBaseSkill(VeliaerisSurvivor.MergeandShiftSkillCorrupted);
                    skillLocator.special.SetBaseSkill(VeliaerisSurvivor.eldritchHealingCorrupted);
                    break;
                case VeliaerisState.Velia:
                   // System.Console.WriteLine("SkillSetVeliaCorrupted");
                    skillLocator.primary.SetBaseSkill(VeliaerisSurvivor.reductionScytheCorrupted);
                    skillLocator.secondary.SetBaseSkill(VeliaerisSurvivor.circularSlashCorrupted);
                    skillLocator.utility.SetBaseSkill(VeliaerisSurvivor.MergeandShiftSkillCorrupted);
                    skillLocator.special.SetBaseSkill(VeliaerisSurvivor.selfBufferCorrupted);
                    break;
                default:
                   // System.Console.WriteLine("SkillSetDefaultCorrupted");
                    skillLocator.primary.SetBaseSkill(VeliaerisSurvivor.basicScytheCorrupted);
                    skillLocator.secondary.SetBaseSkill(VeliaerisSurvivor.CorruptAndHealCorrupted);
                    skillLocator.utility.SetBaseSkill(VeliaerisSurvivor.splitCorrupted);
                    skillLocator.special.SetBaseSkill(VeliaerisSurvivor.voidDetonationCorrupted);
                    break;
            }
        }

        public static void SkillSwitch(SkillLocator skillLocator, bool changeUtility, CharacterBody body, bool hereticPickup, bool InventoryUpdate)
        {
            // if (isHereticPickup==false)
            //  {
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
                velStartstate = body.GetComponent<VeliaerisPassive>().getStartState();
            int utilitystocks = skillLocator.utility.stock;
            float utilityremainingcooldown = skillLocator.utility.cooldownRemaining;
          //  Debug.Log("firstchange check:" + VeliaerisSurvivorController.firstChange);
          //  Debug.Log("stagestarted:" + VeliaerisStatuses.stageStarted);
            #region cooldown variables setup
           // Debug.Log("change utility:"+changeUtility);
            #endregion
            if (!hereticPickup)
            {
               
                if (VeliaerisSurvivor.voidInfluence >= VeliaerisStaticValues.firstTierCorruption)
                {
                   // Debug.Log("entered influence switch");
                    SkillSwitchVoid(skillLocator, changeUtility, body, InventoryUpdate);
                }
                else
                {
                    #region skill changer
                    switch (velStartstate)
                    {
                        case VeliaerisState.Veliaeris:
                            //System.Console.WriteLine("Switch Case: Veliaeris");
                            //                            cooldown = skillLocator.utility.cooldownRemaining;
                            //                          skillLocator.utility.flatCooldownReduction = cooldown;
                            //                            Debug.Log("cooldown:" + skillLocator.utility.cooldownRemaining);
                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Contextual);
                            if (changeUtility)
                            {
                                if (VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Velia && VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Eris)//prevents skill from being unset when inventory cycles through this function
                                {
                                    skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                                }
                            }
                            if (VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Eris)
                            {
                                skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
                            }
                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Contextual);

                            if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris)
                            {
                                skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                                skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Contextual);
                                if (changeUtility)
                                {
                                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                                }
                                skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
                            }
                            if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia)
                            {
                                skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
                                skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Contextual);
                                if (changeUtility)
                                {
                                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                                }
                                skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Contextual);
                            }
                            break;
                        case VeliaerisState.Eris:
                           // System.Console.WriteLine("Switch case: Eris");
                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Contextual);
                            if (VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Veliaeris)
                            {
                                if (changeUtility)
                                {
                                    skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
                                }
                            }
                            //                            Debug.Log("state:" + VeliaerisSurvivorController.VeliaerisState);
                            if (changeUtility)
                            {
                                if (VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Eris && VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Velia)
                                {
                                    //Debug.Log("unset");
                                    skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                                }
                                else
                                {
                                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                                }
                            }
                            skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                            //                            cooldown = skillLocator.utility.cooldownRemaining;
                            skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);
                            //                          Debug.Log("state:" + VeliaerisSurvivorController.VeliaerisState);
                            if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Veliaeris)
                            {
                               // Debug.Log("switch");
                                skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                                skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                                if (changeUtility)
                                {
                                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
                                }
                                skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);
                            }
                            if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia)
                            {
                                skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.reductionScythe, GenericSkill.SkillOverridePriority.Contextual);
                                skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.circularSlash, GenericSkill.SkillOverridePriority.Contextual);
                                if (changeUtility)
                                {
                                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                                }

                                skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.selfBuffer, GenericSkill.SkillOverridePriority.Contextual);
                            }
                            break;
                        case VeliaerisState.Velia:
                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.primary.UnsetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                            skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                            if (changeUtility)
                            {
                                if (VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Veliaeris)
                                {
                                    skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
                                }
                            }
                            //                            Debug.Log("state:" + VeliaerisSurvivorController.VeliaerisState);
                            if (changeUtility)
                            {
                                if (VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Eris && VeliaerisSurvivorController.VeliaerisState != VeliaerisState.Velia)
                                {
                                   // Debug.Log("unset");
                                    skillLocator.utility.UnsetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                                }
                                else
                                {
                                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.MergeandShiftSkill, GenericSkill.SkillOverridePriority.Contextual);
                                }
                            }
                            skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);

                            if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Veliaeris)
                            {
                                skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.basicScythe, GenericSkill.SkillOverridePriority.Contextual);
                                skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.CorruptAndHeal, GenericSkill.SkillOverridePriority.Contextual);
                                if (changeUtility)
                                {
                                    skillLocator.utility.SetSkillOverride(skillLocator.utility, VeliaerisSurvivor.split, GenericSkill.SkillOverridePriority.Contextual);
                                }

                                skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.voidDetonation, GenericSkill.SkillOverridePriority.Contextual);
                            }
                            if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris)
                            {
                                skillLocator.primary.SetSkillOverride(skillLocator.primary, VeliaerisSurvivor.voidSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                                skillLocator.secondary.SetSkillOverride(skillLocator.secondary, VeliaerisSurvivor.allyBuff, GenericSkill.SkillOverridePriority.Contextual);
                                skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.eldritchHealing, GenericSkill.SkillOverridePriority.Contextual);
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
                //                Debug.Log("maxstock:"+skillLocator.secondary.maxStock);
                //                Debug.Log("stock count:"+utilitystocks);
                //                Debug.Log("stocks remaining to be charged:" + (skillLocator.secondary.maxStock - skillLocator.secondary.stock));
                //                if (skillLocator.secondary.maxStock > skillLocator.secondary.stock)
                //                  {
                //               Debug.Log("total cooldown:" + VeliaerisStates.erissecondarycooldown);
                //            Debug.Log("remaining:" + skillLocator.secondary.finalRechargeInterval);
                //          Debug.Log("flat reduction amount:" + skillLocator.secondary.flatCooldownReduction);
                //        Debug.Log("base recharge interval"+skillLocator.secondary.baseRechargeInterval);
                //       Debug.Log("recharge stopwatch:" + skillLocator.secondary.baseRechargeStopwatch);
                //                }
                //     Debug.Log("recharge:" + skillLocator.secondary.cooldownRemaining);
                //                float stocksUsed = VeliaerisStates.erissecondarycooldown / VeliaerisSurvivor.erissecondarycooldown;
                //               float currentcooldown = VeliaerisStates.erissecondarycooldown / stocksUsed;
                //              Debug.Log("stocks used:" + stocksUsed);
                //            Debug.Log("current:" + currentcooldown);
                // Debug.Log("utility stocks:" + skillLocator.utility.stock);
                #region hereticmechanic
                int hereticLimit = 0;
                if (VeliaerisSurvivorController.gatheredPrimary > 0 || VeliaerisSurvivorController.gatheredSecondary > 0 || VeliaerisSurvivorController.gatheredUtility > 0 || VeliaerisSurvivorController.gatheredSpecial > 0)
                {
                    // System.Console.WriteLine("Entered heretic");
                    if ((VeliaerisSurvivorController.hereticOverridesPrimary.Contains(VeliaerisState.Veliaeris) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Veliaeris) || (VeliaerisSurvivorController.hereticOverridesPrimary.Contains(VeliaerisState.Eris) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris) || (VeliaerisSurvivorController.hereticOverridesPrimary.Contains(VeliaerisState.Velia) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia))
                    {
                        hereticLimit++;
                        skillLocator.primary.UnsetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                        skillLocator.primary.SetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    }
                    if ((VeliaerisSurvivorController.hereticOverridesSecondary.Contains(VeliaerisState.Veliaeris) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Veliaeris) || (VeliaerisSurvivorController.hereticOverridesSecondary.Contains(VeliaerisState.Eris) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris) || (VeliaerisSurvivorController.hereticOverridesSecondary.Contains(VeliaerisState.Velia) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia))
                    {
                        //  System.Console.WriteLine("Entered Secondary");
                        hereticLimit++;
                        skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                        skillLocator.secondary.SetSkillOverride(skillLocator.secondary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    }
                    if ((VeliaerisSurvivorController.hereticOverridesUtility.Contains(VeliaerisState.Veliaeris) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Veliaeris) || (VeliaerisSurvivorController.hereticOverridesUtility.Contains(VeliaerisState.Eris) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris) || (VeliaerisSurvivorController.hereticOverridesUtility.Contains(VeliaerisState.Velia) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia))
                    {
                        hereticLimit++;
                        skillLocator.utility.UnsetSkillOverride(skillLocator.utility, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                        skillLocator.utility.SetSkillOverride(skillLocator.utility, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    }
                    if ((VeliaerisSurvivorController.hereticOverridesSpecial.Contains(VeliaerisState.Veliaeris) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Veliaeris) || (VeliaerisSurvivorController.hereticOverridesSpecial.Contains(VeliaerisState.Eris) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris) || (VeliaerisSurvivorController.hereticOverridesSpecial.Contains(VeliaerisState.Velia) && VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia))
                    {
                        // System.Console.WriteLine("Entered heretic Special");
                        hereticLimit++;
                        skillLocator.special.UnsetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                        skillLocator.special.SetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                    }
                }
                if (VeliaerisSurvivorController.hereticOverridesPrimary.Contains(VeliaerisSurvivorController.VeliaerisState) == false)
                {
                    skillLocator.primary.UnsetSkillOverride(skillLocator.primary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarPrimaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                }
                if (VeliaerisSurvivorController.hereticOverridesSecondary.Contains(VeliaerisSurvivorController.VeliaerisState) == false)
                {
                    skillLocator.secondary.UnsetSkillOverride(skillLocator.secondary, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSecondaryReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                }
                if (VeliaerisSurvivorController.hereticOverridesUtility.Contains(VeliaerisSurvivorController.VeliaerisState) == false)
                {
                    skillLocator.utility.UnsetSkillOverride(skillLocator.utility, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarUtilityReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                }
                if (VeliaerisSurvivorController.hereticOverridesSpecial.Contains(VeliaerisSurvivorController.VeliaerisState) == false)
                {
                    skillLocator.special.UnsetSkillOverride(skillLocator.special, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("LunarSpecialReplacement")), GenericSkill.SkillOverridePriority.Contextual);
                }
                #endregion
                if (VeliaerisSurvivorController.firstChange && !InventoryUpdate && VeliaerisStatuses.stageStarted)
                {
                    VeliaerisSurvivorController.network_firstchange = false;
                }
                #region erisskillcooldownmanipulation
                if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris)
                {
                    //special
                    if (skillLocator.special.stock < VeliaerisStatuses.erisSpecialStock)
                    {
                        for (int i = skillLocator.special.stock; i < VeliaerisStatuses.erisSpecialStock; i++)
                        {
                            if (skillLocator.special.stock < VeliaerisStatuses.erisSpecialStockMax)
                            {
                                skillLocator.special.AddOneStock();
                            }

                        }
                        if (skillLocator.special.stock < VeliaerisStatuses.erisSpecialStockMax)
                        {
                            skillLocator.special.RunRecharge(skillLocator.special.cooldownRemaining - VeliaerisStatuses.erisSpecialCooldownStock);
                        }
                            
                    }
                    //secondary
                    if (skillLocator.secondary.stock < VeliaerisStatuses.erisSecondaryStock)
                    {
                        for (int i = skillLocator.secondary.stock; i < VeliaerisStatuses.erisSecondaryStock; i++)
                        {
                            if (skillLocator.secondary.stock < VeliaerisStatuses.erisSecondaryStockMax)
                            {
                                skillLocator.special.AddOneStock();
                            }

                        }
                        if (skillLocator.secondary.stock < VeliaerisStatuses.erisSecondaryStockMax)
                        {
                            skillLocator.secondary.RunRecharge(skillLocator.secondary.cooldownRemaining - VeliaerisStatuses.erisSecondaryCooldownStock);
                        }

                    }



                }
                #endregion
                #region veliaerisskillcooldownmanipulation
                if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Veliaeris)
                {
                    //special
                    if (skillLocator.special.stock < VeliaerisStatuses.veliaerisSpecialStock)
                    {
                        for (int i = skillLocator.special.stock; i < VeliaerisStatuses.veliaerisSpecialStock; i++)
                        {
                            if (skillLocator.special.stock < VeliaerisStatuses.veliaerisSpecialStockMax)
                            {
                                skillLocator.special.AddOneStock();
                            }

                        }
                        if (skillLocator.special.stock < VeliaerisStatuses.veliaerisSpecialStockMax)
                        {
                            skillLocator.special.RunRecharge(skillLocator.special.cooldownRemaining - VeliaerisStatuses.veliaerisSpecialCooldownStock);
                        }

                    }
                    //secondary
                    if (skillLocator.secondary.stock < VeliaerisStatuses.veliaerisSecondaryStock)
                    {
                        for (int i = skillLocator.secondary.stock; i < VeliaerisStatuses.veliaerisSecondaryStock; i++)
                        {
                            if (skillLocator.secondary.stock < VeliaerisStatuses.veliaerisSecondaryStockMax)
                            {
                                skillLocator.special.AddOneStock();
                            }

                        }
                        if (skillLocator.secondary.stock < VeliaerisStatuses.veliaerisSecondaryStockMax)
                        {
                            skillLocator.secondary.RunRecharge(skillLocator.secondary.cooldownRemaining - VeliaerisStatuses.veliaerisSpecialCooldownStock);
                        }

                    }

                }
                #endregion
                #region veliaskillcooldownmanipulation
                if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia)
                {
                    //special
                    if (skillLocator.special.stock < VeliaerisStatuses.veliaSpecialStock)
                    {
                        for (int i = skillLocator.special.stock; i < VeliaerisStatuses.veliaSpecialStock; i++)
                        {
                            if (skillLocator.special.stock < VeliaerisStatuses.veliaSpecialStockMax)
                            {
                                skillLocator.special.AddOneStock();
                            }

                        }
                        if (skillLocator.special.stock < VeliaerisStatuses.veliaSpecialStockMax)
                        {
                            skillLocator.special.RunRecharge(skillLocator.special.cooldownRemaining - VeliaerisStatuses.veliaSpecialCooldownStock);
                        }

                    }
                    //secondary
                    if (skillLocator.secondary.stock < VeliaerisStatuses.veliaSecondaryStock)
                    {
                        for (int i = skillLocator.secondary.stock; i < VeliaerisStatuses.veliaSecondaryStock; i++)
                        {
                            if (skillLocator.secondary.stock < VeliaerisStatuses.veliaSecondaryStockMax)
                            {
                                skillLocator.special.AddOneStock();
                            }

                        }
                        if (skillLocator.secondary.stock < VeliaerisStatuses.veliaSecondaryStockMax)
                        {
                            skillLocator.secondary.RunRecharge(skillLocator.secondary.cooldownRemaining - VeliaerisStatuses.veliaSpecialCooldownStock);
                        }

                    }
                }
                #endregion
                if (skillLocator.utility.maxStock>1 && utilitystocks<skillLocator.utility.maxStock && !InventoryUpdate)
                {
                    for(int i = 0; i < utilitystocks; i++)
                    {
                        if (utilitystocks > 0)
                        {
                            skillLocator.utility.AddOneStock();
                        }
                    }
                    skillLocator.utility.RunRecharge(skillLocator.utility.cooldownRemaining - utilityremainingcooldown);
                }
            }

        }

        private static void SkillSwitchVoid(SkillLocator skillLocator, bool changeUtility, CharacterBody body, bool InventoryUpdate)
        {
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
            velStartstate = body.GetComponent<VeliaerisPassive>().getStartState();

        }
    }
}
