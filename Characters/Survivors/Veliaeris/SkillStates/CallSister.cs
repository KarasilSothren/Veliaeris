using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using VeliaerisMod.Survivors.Veliaeris;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates
{
    class CallSister : BaseState
    {
        private HurtBox[] targetTargets;
        private float ExplosionRange = 20f;
        public override void OnEnter()
        {
            base.OnEnter();
            SkillLocator skillLocator = base.characterBody.GetComponent<SkillLocator>();
            int stocksLeft = (skillLocator.special.maxStock - skillLocator.special.stock) - 1;
            System.Console.WriteLine("remaining stocks: "+stocksLeft*10);
            BullseyeSearch targetSearchWarp = new BullseyeSearch();
            targetSearchWarp.filterByDistinctEntity = true;
            targetSearchWarp.filterByLoS = false;
            targetSearchWarp.maxDistanceFilter = ExplosionRange+stocksLeft*10;
            targetSearchWarp.minDistanceFilter = 0f;
            targetSearchWarp.minAngleFilter = 0f;
            targetSearchWarp.maxAngleFilter = 360f;
            targetSearchWarp.sortMode = BullseyeSearch.SortMode.Distance;
            targetSearchWarp.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
            targetSearchWarp.searchOrigin = base.characterBody.corePosition;
            targetSearchWarp.RefreshCandidates();
            targetSearchWarp.FilterOutGameObject(base.gameObject);
            IEnumerable<HurtBox> resultsvoid = targetSearchWarp.GetResults();
            this.targetTargets = resultsvoid.ToArray<HurtBox>();
            DamageInfo desolationvoid = new DamageInfo();
            HurtBox targetHurtBoxvoid = null;
            //            base.characterBody.healthComponent.TakeDamage();
            float drainPercentage = 0.35f + (0.15f * stocksLeft);
            float drain = base.characterBody.healthComponent.fullCombinedHealth * drainPercentage;
            System.Console.WriteLine(drain);
            if (NetworkServer.active && base.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = drain;
                damageInfo.position = base.characterBody.corePosition;
                damageInfo.force = Vector3.zero;
                damageInfo.damageColorIndex = DamageColorIndex.Void;
                damageInfo.crit = false;
                damageInfo.attacker = null;
                damageInfo.inflictor = null;
                damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor|DamageType.BypassBlock);
                damageInfo.procCoefficient = 0f;
                damageInfo.procChainMask = default(ProcChainMask);
                base.healthComponent.TakeDamage(damageInfo);
            }
            for (int l = 0; l < targetTargets.Length; l++)
            {
                Util.Swap<HurtBox>(ref targetHurtBoxvoid, ref this.targetTargets[l]);
                //float hitSeverity;
                //float finalRadius = 30f;
                //float num = Vector3.Distance(body.corePosition, targetHurtBoxvoid.healthComponent.body.corePosition);
                //hitSeverity = Mathf.Clamp01(1f - num / finalRadius);
                desolationvoid.damage = drain*VeliaerisStaticValues.callDamageCoefficent;
                desolationvoid.attacker = this.characterBody.gameObject;
                desolationvoid.inflictor = null;
                desolationvoid.force = Vector3.zero;
                desolationvoid.procCoefficient = 0f;
                desolationvoid.position = targetHurtBoxvoid.transform.position;
                desolationvoid.damageColorIndex = DamageColorIndex.Void;
                desolationvoid.damageType = DamageType.AOE;
                targetHurtBoxvoid.healthComponent.TakeDamage(desolationvoid);
                //                   Vector3 normalized = (targetHurtBoxvoid.healthComponent.body.corePosition - body.corePosition).normalized;
                //                  body.characterMotor.ApplyForce(normalized * (20f));
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
