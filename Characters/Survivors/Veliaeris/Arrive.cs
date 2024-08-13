using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;


namespace VeliaerisMod.Characters.Survivors.Veliaeris
{
    class Arrive: BaseState {
        public static float duration = 1f;
        protected string portalMuzzle = "Chest";

        private CameraRigController cameraController;
        private bool initCamera;

        public override void OnEnter()
        {
            base.OnEnter();
//            PlayAnimation("FullBody, Override", "Spawn");
            Util.PlaySound(EntityStates.NullifierMonster.SpawnState.spawnSoundString, gameObject);

            if (NetworkServer.active) characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            SpawnEffect();
        }

        public virtual void SpawnEffect()
        {
//            EffectManager.SimpleMuzzleFlash(EntityStates.VoidRaidCrab.SpawnState.spawnEffectPrefab, gameObject, portalMuzzle, false);
            //    if (EntityStates.NullifierMonster.SpawnState.spawnEffectPrefab)
            //   {
            //              if (VeliaerisPlugin.hasVoid)
            //            {
            //                    EffectManager.SimpleMuzzleFlash(EntityStates.NullifierMonster.SpawnState.spawnEffectPrefab, gameObject, portalMuzzle, false);
            //          }
            // }
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

        public override void OnExit()
        {
            base.OnExit();

            if (NetworkServer.active) characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}