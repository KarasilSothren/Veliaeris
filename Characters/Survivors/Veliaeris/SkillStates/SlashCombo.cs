using VeliaerisMod.Modules.BaseStates;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using VeliaerisMod.Modules;

namespace VeliaerisMod.Survivors.Henry.SkillStates
{
    public class SlashCombo : BaseMeleeAttack
    {
        private OverlapAttack attack;
        protected List<DamageAPI.ModdedDamageType> moddedDamageTypeHolder = new List<DamageAPI.ModdedDamageType>();
        protected float attackStartTime = 0.2f;
        protected float attackEndTime = 0.4f;
        protected new DamageType damageType = DamageType.Generic;
        public override void OnEnter()
        {
            base.OnEnter();
            hitboxGroupName = "SwordGroup";
            attack = new OverlapAttack();
            attack.damageType = damageType;
            damageCoefficient = HenryStaticValues.swordDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.4f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.6f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = "HenrySwordSwing";
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = HenryAssets.swordSwingEffect;
            hitEffectPrefab = HenryAssets.swordHitImpactEffect;

            impactSound = HenryAssets.swordHitSoundEvent.index;


        }

        protected void infliction(OverlapAttack attack)
        {
            System.Console.WriteLine("Infliction go!");
            attack.AddModdedDamageType(DamageTypes.AbyssCorrosion);
        }

        protected override void PlayAttackAnimation()
        {
            PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
        }

        private void FireAttack()
        {
            System.Console.WriteLine("Fire attack go!");
            if(base.isAuthority)
            {
                infliction(this.attack);
            }
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        public override void FixedUpdate()
        {
            System.Console.WriteLine("Fixed update go!");
            base.FixedUpdate();
            if (this.stopwatch >= (this.duration * this.attackStartTime) && this.stopwatch <= (this.duration * this.attackEndTime))
            {
                this.FireAttack();
            }
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
