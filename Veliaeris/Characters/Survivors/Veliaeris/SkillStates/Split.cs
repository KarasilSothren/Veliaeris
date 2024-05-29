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
            base.OnEnter();
            System.Console.WriteLine("Current state: "+VeliaerisPlugin.VeliaerisStates);
            System.Console.WriteLine("first change?: "+VeliaerisPlugin.firstChange);
            System.Console.WriteLine("splitstate on active: "+VeliaerisPlugin.previousSplitSate);
            if (base.isAuthority && VeliaerisPlugin.firstChange)
            {
                VeliaerisPlugin.VeliaerisStates = VeliaerisState.Eris;
                VeliaerisPlugin.previousSplitSate = VeliaerisState.Eris;
                HeldState.velState = VeliaerisState.Eris;
                VeliaerisPlugin.firstChange = false;
                System.Console.WriteLine("first change check: " + VeliaerisPlugin.firstChange);
                System.Console.WriteLine("veliaeris state check after first change: " + VeliaerisPlugin.VeliaerisStates);
            }
            else
            {
                VeliaerisPlugin.VeliaerisStates = VeliaerisPlugin.previousSplitSate;
                HeldState.velState = VeliaerisPlugin.previousSplitSate;
            }
            System.Console.WriteLine("Exit state: " + VeliaerisPlugin.VeliaerisStates);
            System.Console.WriteLine("exit held state: " + HeldState.velState);
            if (VeliaerisSurvivor.instance.assetBundle != null)
            {
                System.Console.WriteLine("assetbundle is not null");
            }
   


            VeliaerisMod.Survivors.Veliaeris.SkillStates.MergeandShift.SkillSwitch(base.skillLocator);
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
