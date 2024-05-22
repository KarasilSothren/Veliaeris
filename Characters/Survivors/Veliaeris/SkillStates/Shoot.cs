using EntityStates;
using VeliaerisMod.Survivors.Henry;
using RoR2;
using RoR2.Skills;
using System.Linq;
using UnityEngine;
using VeliaerisMod;

namespace VeliaerisMod.Survivors.Henry.SkillStates
{
    public class Shoot : BaseSkillState
    {
        public static float damageCoefficient = HenryStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        private HuntressTracker hunterTracker;
        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private CameraTargetParams.AimRequest aimRequest;
        private readonly BullseyeSearch search = new BullseyeSearch();
        public override void OnEnter()
        {
            base.OnEnter();
            this.hunterTracker = base.GetComponent<HuntressTracker>();
            skillLocator.enabled = false;
            //if (hunterTracker.GetTrackingTarget() != null)
            //{
            //    System.Console.WriteLine("not null");
            //}

            
            //if(this.search.GetResults().FirstOrDefault<HurtBox>() != null )
            //{
            //    System.Console.WriteLine("Null is not");
            //}
            //else
            //{
            //    System.Console.WriteLine("Null");
            //}
            //if (hunterTracker.GetTrackingTarget() != null)
            //{
            //    System.Console.WriteLine("Is not null");
            //}
            //else
            //{
            //    System.Console.WriteLine("is null");
            //}

            if (this.hunterTracker)
            {
                System.Console.WriteLine("hunter true");
            }
            if (hunterTracker==null)
            {
                System.Console.WriteLine("hunter false");
            }
            if (this.hunterTracker)
            {
                this.hunterTracker.enabled = false;
            }
            if (base.cameraTargetParams)
            {
                this.aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
            }


            //System.Console.WriteLine("looking:",hunterTracker.ToString());
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            CameraTargetParams.AimRequest aimRequest = this.aimRequest;
            if (aimRequest != null)
            {
                aimRequest.Dispose();
            }
            if (this.hunterTracker)
            {
                this.hunterTracker.enabled = true;
            }
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryShootPistol", gameObject);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damageCoefficient * damageStat,
                        damageColorIndex = DamageColorIndex.Void,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = range,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = RollCrit(),
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        procChainMask = default,
                        procCoefficient = procCoefficient,
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = tracerEffectPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    }.Fire();
                }
            }
        }
        

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}