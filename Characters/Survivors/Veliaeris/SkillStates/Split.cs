﻿using EntityStates;
using RoR2;
using UnityEngine;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Modules;
using RoR2.Skills;

namespace VeliaerisMod.Survivors.Veliaeris.SkillStates
{
    public class Split : BaseSkillState
    {
        
        private Animator animator;
        private Vector3 previousPosition;
        private readonly AssetBundle assets = VeliaerisSurvivor.instance.assetBundle;
        private string prefix = VeliaerisSurvivor.VELIAERIS_PREFIX;
        private float duration =0.2f;
        private VeliaerisSurvivorController VeliaerisSurvivorController;

        public override void OnEnter()
        {
            CharacterBody body;
            body = GetComponent<CharacterBody>();
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
//            body.AddTimedBuff(VeliaerisBuffs.switchInvincibility, 1f);
            base.OnEnter();
            Debug.Log("initial state:"+VeliaerisSurvivorController.initalState);
            Debug.Log("current form:"+VeliaerisSurvivorController.VeliaerisState);
            Debug.Log("first change check:" + VeliaerisSurvivorController.firstChange);
            if(VeliaerisSurvivorController.firstChange && VeliaerisSurvivorController.initalState == VeliaerisState.Veliaeris)
            {
                Debug.Log("is true");
            }
            else
            {
                Debug.Log("is false");
            }
            if (VeliaerisSurvivorController.firstChange&&VeliaerisSurvivorController.initalState == VeliaerisState.Veliaeris)
            {
                Debug.Log("entered first switch");
                VeliaerisSurvivorController.network_veliaerisStates = VeliaerisState.Eris;
                VeliaerisSurvivorController.network_previousState = VeliaerisState.Eris;
                VeliaerisSurvivorController.network_velState = VeliaerisState.Eris;
            }
            else
            {
                VeliaerisSurvivorController.network_veliaerisStates = VeliaerisSurvivorController.previousSplitSate;
                VeliaerisSurvivorController.network_velState = VeliaerisSurvivorController.previousSplitSate;
            }
            body.SetBuffCount(VeliaerisBuffs.VeliaerisStatChanges.buffIndex, 0);
            VeliaerisSurvivorController.network_paststate = VeliaerisState.Veliaeris;
            if(VeliaerisSurvivorController.VeliaerisState==VeliaerisState.Eris)
            {
                body.SetBuffCount(VeliaerisBuffs.ErisStatChanges.buffIndex, 1);
                TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].teamIndex == this.teamComponent.teamIndex)
                    {
                        if (!array[i].GetComponent<CharacterBody>().master.GetBody())
                        {
                            array[i].GetComponent<CharacterBody>().AddBuff(VeliaerisBuffs.healthBlessing);
                        }
                    }
                }
                for (int i = 0; i < (VeliaerisSurvivor.voidInfluence / 5) + 1; i++)
                {
                    VeliaerisSurvivor.DeathPreventionStacks++;
                }
            }
            if(VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia)
            {
                body.SetBuffCount(VeliaerisBuffs.VeliaStatChanges.buffIndex, 1);
                TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i].teamIndex == this.teamComponent.teamIndex)
                        {
                        if (!array[i].GetComponent<CharacterBody>().master.GetBody())
                        {
                            array[i].GetComponent<CharacterBody>().AddBuff(VeliaerisBuffs.damageBlessing);
                        }
                        }
                    }
                for (int i = 0; i < (VeliaerisSurvivor.voidInfluence / VeliaerisStaticValues.firstTierCorruption) + 1;i++)
                {
                    VeliaerisSurvivor.VoidCorruptionStacks++;
                }
            }

//            System.Console.WriteLine("cooldown test:" + skillLocator.utility.cooldownRemaining);
            VeliaerisStatuses.utilityCooldown += skillLocator.utility.cooldownRemaining;
            MergeandShift.SkillSwitch(base.skillLocator,true,body,false,false);
            //if (HeldState.VeliaerisStates != VeliaerisStates.Veliaeris)
            //{
            //    if (HeldState.VeliaerisStates == VeliaerisStates.Velia)
            //    {
            //        base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, secondprimarySkillDef, GenericSkill.SkillOverridePriority.
            //
            //
            //
            //        );
            //    }
            //    else if (HeldState.VeliaerisStates == VeliaerisStates.Eris)
            //    {
            //        base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, primarySkillDef3, GenericSkill.SkillOverridePriority.Contextual);
            //    }
            //}
            //else if (HeldState.VeliaerisStates == VeliaerisStates.Veliaeris)
            //{
            //    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, primarySkillDef2, GenericSkill.SkillOverridePriority.Contextual);
            //}
            return;
        }
         
        


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            return;
//            animator = GetModelAnimator();
        }


        public override void OnExit()
        {


        }

    }
    }
