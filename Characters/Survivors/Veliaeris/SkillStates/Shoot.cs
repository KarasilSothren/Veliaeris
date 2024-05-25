using EntityStates;
using VeliaerisMod.Survivors.Veliaeris;
using RoR2;
using RoR2.Skills;
using System.Linq;
using UnityEngine;
using VeliaerisMod;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using IL.RoR2.Orbs;
using On.RoR2.Orbs;
using R2API;
using VeliaerisMod.Modules;
using UnityEngine.Networking;

namespace VeliaerisMod.Survivors.Veliaeris.SkillStates
{
    public class Shoot : BaseState
    {
        public static float damageCoefficient = HenryStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public float baseDuration = 0.6f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        private HuntressTracker hunterTracker;
        private float duration;
        private ChildLocator childLocator;
        private HurtBox initialOrbTarget;
        public float orbProcCoefficent;
        private string muzzleString;
        private Animator animator;
        protected bool isCrit;
        private int firedArrowCount;
        private int maxArrowCount;

        public override void OnEnter()
        {
            base.OnEnter();
            Transform modelTransfrom = base.GetModelTransform();
            this.hunterTracker = base.GetComponent<HuntressTracker>();
            if (modelTransfrom)
            {
                this.childLocator = modelTransfrom.GetComponent<ChildLocator>();
                this.animator = modelTransfrom.GetComponent <Animator>();
                
            }
            if(this.hunterTracker&&base.isAuthority)
            {
                this.initialOrbTarget = this.hunterTracker.GetTrackingTarget();
            }
            this.duration = this.baseDuration / this.attackSpeedStat;
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.duration+1f);
            }
            this.isCrit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            //System.Console.WriteLine("looking:",hunterTracker.ToString());
            
        }

        public override void OnExit()
        {
            base.OnExit();
            this.FireOrbArrow();
        }

        protected virtual RoR2.Orbs.GenericDamageOrb CreateArrowOrb()
        {
            return new RoR2.Orbs.HuntressArrowOrb();
        }

        private void FireOrbArrow()
        {
            //if (this.firedArrowCount >= this.maxArrowCount || !NetworkServer.active)
            //{

            //    return;
            //}
            //this.firedArrowCount++;
            RoR2.Orbs.GenericDamageOrb genericDamageOrb = this.CreateArrowOrb();

           // genericDamageOrb.AddModdedDamageType(DamageTypes.AbyssCorrosion);
            genericDamageOrb.damageValue = damageCoefficient;
            genericDamageOrb.isCrit = this.isCrit;
            genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
            genericDamageOrb.attacker = base.gameObject;
            genericDamageOrb.procCoefficient = this.orbProcCoefficent;
            HurtBox hurtBox = this.initialOrbTarget;
            System.Console.WriteLine("Outside of Hurtbox");
            if (hurtBox)
            {
                Transform transform = this.childLocator.FindChild(this.muzzleString);
                System.Console.WriteLine(transform.position);
                genericDamageOrb.origin = transform.position;
                System.Console.WriteLine(genericDamageOrb.origin);
                genericDamageOrb.target = hurtBox;
                RoR2.Orbs.OrbManager.instance.AddOrb(genericDamageOrb);
                System.Console.WriteLine("Shot has been fired");
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();


            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }


        

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.initialOrbTarget));
        }

        // Token: 0x06000E60 RID: 3680 RVA: 0x0003E0A0 File Offset: 0x0003C2A0
        public override void OnDeserialize(NetworkReader reader)
        {
            this.initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}
