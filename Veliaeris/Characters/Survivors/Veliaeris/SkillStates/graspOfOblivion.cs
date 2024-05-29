using EntityStates;
using VeliaerisMod.Survivors.Veliaeris;
using RoR2;
using UnityEngine;
using R2API;
using UnityEngine.Networking;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Modules;

namespace VeliaerisMod.Survivors.Veliaeris.SkillStates
{
    public class graspOfOblivion : BaseSkillState
    {
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
                this.animator = modelTransfrom.GetComponent<Animator>();

            }
            if (this.hunterTracker && base.isAuthority)
            {
                this.initialOrbTarget = this.hunterTracker.GetTrackingTarget();
            }
            this.duration = this.baseDuration / this.attackSpeedStat;
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.duration + 1f);
            }
            this.isCrit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            if (this.hunterTracker)
            {
                System.Console.WriteLine("Hunter true");
            }
            else
            {
                System.Console.WriteLine("Hunter false");
            }
            //System.Console.WriteLine("looking:",hunterTracker.ToString());

        }

        public override void OnExit()
        {
            base.OnExit();
            this.FireOrbArrow();
        }

        protected virtual RoR2.Orbs.GenericDamageOrb CreateVoidOrb()
        {
            return new RoR2.Orbs.GenericDamageOrb();
        }

        private Vector3? originalMuzzlePosition = null;
        private Transform _muzzleTransform;

        private Vector3? originalMuzzleDirection = null;

        private Vector3 muzzleDirection
        {
            get
            {
                if (originalMuzzleDirection == null)
                {
                    originalMuzzleDirection = _muzzleTransform.right + _muzzleTransform.up - _muzzleTransform.forward;
                }
                return originalMuzzleDirection.Value;
            }
        }

        private Vector3 muzzlePosition
        {
            get
            {
                if (originalMuzzlePosition == null)
                {
                    originalMuzzlePosition = _muzzleTransform.position;
                }
                return originalMuzzlePosition.Value;
            }
        }

        private Vector3 GetOrbOrigin
        {
            get
            {
                if (_muzzleTransform == null)
                {
                    return transform.position;
                }
                else
                {
                    return muzzlePosition + muzzleDirection;
                }
            }
        }

        private void FireOrbArrow()
        {
            //if (this.firedArrowCount >= this.maxArrowCount || !NetworkServer.active)
            //{

            //    return;
            //}
            //this.firedArrowCount++;
            HurtBox hurtBox = this.initialOrbTarget;
            RoR2.Orbs.GenericDamageOrb genericDamageOrb = this.CreateVoidOrb();
            float hpDamage = this.characterBody.maxHealth*0.5f;
            
            float percentMaxHealthDamage = hurtBox.healthComponent.combinedHealth * 0.02f;
            genericDamageOrb.damageValue = HenryStaticValues.gunDamageCoefficient*(hpDamage+percentMaxHealthDamage);
            genericDamageOrb.isCrit = this.isCrit;
            genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
            genericDamageOrb.attacker = base.gameObject;
            genericDamageOrb.procCoefficient = this.orbProcCoefficent;
            genericDamageOrb.damageColorIndex = DamageColorIndex.Void;

            if (hurtBox)
            {
                Transform transform = this.childLocator.FindChild(this.muzzleString);
                //                System.Console.WriteLine(transform.position);
                genericDamageOrb.origin = GetOrbOrigin;
                //              System.Console.WriteLine(genericDamageOrb.origin);
                genericDamageOrb.target = hurtBox;
                RoR2.Orbs.OrbManager.instance.AddOrb(genericDamageOrb);
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