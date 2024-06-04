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
            if (base.isAuthority && VeliaerisPlugin.firstChange)
            {
                VeliaerisPlugin.VeliaerisStates = VeliaerisState.Eris;
                VeliaerisPlugin.previousSplitSate = VeliaerisState.Eris;
                HeldState.velState = VeliaerisState.Eris;
                VeliaerisPlugin.firstChange = false;
            }
            else
            {
                VeliaerisPlugin.VeliaerisStates = VeliaerisPlugin.previousSplitSate;
                HeldState.velState = VeliaerisPlugin.previousSplitSate;
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
