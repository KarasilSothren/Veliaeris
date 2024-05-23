using EntityStates;
using VeliaerisMod.Survivors.Henry;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Modules;
using RoR2.Skills;

namespace VeliaerisMod.Survivors.Henry.SkillStates
{
    public class Roll : BaseSkillState
    {
        public AssetBundle assetBundle { get; protected set; }
        public static SkillDef secondprimarySkillDef;
        public static SteppedSkillDef primarySkillDef2;
        private Animator animator;
        private Vector3 previousPosition;
        private VeliaerisStates splitSate;

        public override void OnEnter()
        {
            System.Console.WriteLine("rolled");
            base.OnEnter();
            System.Console.WriteLine(VeliaerisPlugin.VeliaerisStates);
            System.Console.WriteLine(VeliaerisPlugin.firstChange);
            System.Console.WriteLine(splitSate.ToString());
            if (base.isAuthority && !VeliaerisPlugin.firstChange)
            {
                if (VeliaerisPlugin.VeliaerisStates != VeliaerisStates.Veliaeris)
                {
                    splitSate = VeliaerisPlugin.VeliaerisStates;
                }
                System.Console.WriteLine(VeliaerisPlugin.VeliaerisStates);
                if (VeliaerisPlugin.VeliaerisStates == VeliaerisStates.Veliaeris)
                {
                    VeliaerisPlugin.VeliaerisStates = splitSate;
                }
                else if (VeliaerisPlugin.VeliaerisStates == VeliaerisStates.Eris || VeliaerisPlugin.VeliaerisStates == VeliaerisStates.Velia)
                {
                    VeliaerisPlugin.VeliaerisStates = VeliaerisStates.Veliaeris;
                }
                System.Console.WriteLine(VeliaerisPlugin.VeliaerisStates);
            }
            if (base.isAuthority && VeliaerisPlugin.firstChange)
            {
                VeliaerisPlugin.VeliaerisStates = VeliaerisStates.Eris;
                VeliaerisPlugin.firstChange = false;
                System.Console.WriteLine(VeliaerisPlugin.firstChange);
            }
            animator = GetModelAnimator();
            string prefix = VeliaerisPlugin.DEVELOPER_PREFIX;
            secondprimarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryGun",
                skillNameToken = prefix + "SECONDARY_GUN_NAME",
                skillDescriptionToken = prefix + "SECONDARY_GUN_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SlashCombo)),
                activationStateMachineName = "Weapon",
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            }
            );
            primarySkillDef2 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
    (
        "HenrySlash",
        prefix + "PRIMARY_SLASH_NAME",
        prefix + "PRIMARY_SLASH_DESCRIPTION",
        assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
        new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)),
        "Weapon",
        true
    ));

            if (VeliaerisPlugin.VeliaerisStates != VeliaerisStates.Veliaeris)
            {
                base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, secondprimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
            else if(VeliaerisPlugin.VeliaerisStates==VeliaerisStates.Veliaeris)
            {
                base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, primarySkillDef2, GenericSkill.SkillOverridePriority.Contextual);
            }


            

        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();


            //if (isAuthority && fixedAge >= duration)
            //{
            //    outer.SetNextStateToMain();
            //    return;
            //}
        }


        public override void OnExit()
        {

        }



    }
    }
