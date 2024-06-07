using EntityStates;
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
        public override void OnEnter()
        {
            CharacterBody body;
            body = GetComponent<CharacterBody>();
            body.AddTimedBuff(VeliaerisBuffs.switchInvincibility, 1f);
            base.OnEnter();
            if (base.isAuthority && HeldState.firstChange)
            {
                VeliaerisPlugin.VeliaerisStates = VeliaerisState.Eris;
                VeliaerisPlugin.previousSplitSate = VeliaerisState.Eris;
                HeldState.velState = VeliaerisState.Eris;
                HeldState.firstChange = false;
            }
            else
            {
                VeliaerisPlugin.VeliaerisStates = VeliaerisPlugin.previousSplitSate;
                HeldState.velState = VeliaerisPlugin.previousSplitSate;
            }
            if(VeliaerisPlugin.VeliaerisStates==VeliaerisState.Eris)
            {
                TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].teamIndex == this.teamComponent.teamIndex)
                    {
                        if (!array[i].GetComponent<CharacterBody>().ToString().Contains("VeliaerisBody"))
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
            if(VeliaerisPlugin.VeliaerisStates == VeliaerisState.Velia)
            {
                    TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i].teamIndex == this.teamComponent.teamIndex)
                        {
                        if (!array[i].GetComponent<CharacterBody>().ToString().Contains("VeliaerisBody"))
                        {
                            array[i].GetComponent<CharacterBody>().AddBuff(VeliaerisBuffs.damageBlessing);
                        }
                        }
                    }
                for (int i = 0; i < (VeliaerisSurvivor.voidInfluence / 10) + 1;i++)
                {
                    VeliaerisSurvivor.VoidCorruptionStacks++;
                }
            }
   


            VeliaerisMod.Survivors.Veliaeris.SkillStates.MergeandShift.SkillSwitch(base.skillLocator,false);
            //if (VeliaerisPlugin.VeliaerisStates != VeliaerisStates.Veliaeris)
            //{
            //    if (VeliaerisPlugin.VeliaerisStates == VeliaerisStates.Velia)
            //    {
            //        base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, secondprimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
            //    }
            //    else if (VeliaerisPlugin.VeliaerisStates == VeliaerisStates.Eris)
            //    {
            //        base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, primarySkillDef3, GenericSkill.SkillOverridePriority.Contextual);
            //    }
            //}
            //else if (VeliaerisPlugin.VeliaerisStates == VeliaerisStates.Veliaeris)
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
