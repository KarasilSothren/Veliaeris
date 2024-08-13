using VeliaerisMod.Modules.BaseStates;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using VeliaerisMod.Modules;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;

namespace VeliaerisMod.Survivors.Veliaeris.SkillStates
{
    public class CircularSlash : BaseMeleeAttack
    {
     //   private OverlapAttack attack;
//        protected List<DamageAPI.ModdedDamageType> moddedDamageTypeHolder = new List<DamageAPI.ModdedDamageType>();
        protected float attackStartTime = 0.2f;
        protected float attackEndTime = 0.4f;
        protected new DamageType damageType = DamageType.Generic;
        public override void OnEnter()
        {

            hitboxGroupName = "SwordGroup";
            

            damageCoefficient = VeliaerisStaticValues.scytheDamageCoefficient;
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
            swingEffectPrefab = VeliaerisAssets.swordSwingEffect;
            hitEffectPrefab = VeliaerisAssets.swordHitImpactEffect;

            impactSound = VeliaerisAssets.swordHitSoundEvent.index;
            base.OnEnter();
            attack.damageType = damageType;
            attack.AddModdedDamageType(DamageTypes.AbyssCorrosion);
            attack.AddModdedDamageType(DamageTypes.AbyssCorrosion);
            attack.AddModdedDamageType(DamageTypes.percentHealth);
            if (HasBuff(VeliaerisBuffs.inflictDeath))
            {
                attack.AddModdedDamageType(DamageTypes.voidCorrosion);
            }
            VeliaerisStatuses.veliaSecondaryStock--;
        }



        protected override void PlayAttackAnimation()
        {
            PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
        }



        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
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