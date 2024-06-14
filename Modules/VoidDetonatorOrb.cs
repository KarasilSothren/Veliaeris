using RoR2;
using RoR2.Orbs;
using System;
using UnityEngine;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;

namespace VeliaerisMod.Modules
{
    public class VoidDetonatorOrb : Orb
    {
        public override void Begin()
        {
            base.duration = base.distanceToTarget / this.travelSpeed;
            EffectData effectData = new EffectData
            {
                scale = 1f,
                origin = this.origin,
                genericFloat = base.duration
            };
            effectData.SetHurtBoxReference(this.target);
            if (this.orbEffectPrefab)
            {
                EffectManager.SpawnEffect(this.orbEffectPrefab, effectData, true);
            }
        }

        public override void OnArrival()
        {
            base.OnArrival();
            if (!this.target)
            {
                return;
            }
            HealthComponent healthComponent = this.target.healthComponent;
            if (!healthComponent)
            {
                return;
            }
            CharacterBody body = healthComponent.body;
            if (!body)
            {
                return;
            }
            int buffCount = body.GetBuffCount(VeliaerisBuffs.abyss);
            if (buffCount <= 0)
            {
                return;
            }
            float voidExecution = Util.ConvertAmplificationPercentageIntoReductionPercentage(5f) / 100f;
            body.ClearTimedBuffs(VeliaerisBuffs.abyss);
            Vector3 position = this.target.transform.position;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = this.baseDamage + this.damagePerStack * (float)buffCount;
            damageInfo.attacker = this.attacker;
            damageInfo.inflictor = null;
            damageInfo.force = Vector3.zero;
            damageInfo.crit = this.isCrit;
            damageInfo.procChainMask = this.procChainMask;
            damageInfo.procCoefficient = this.procCoefficient;
            damageInfo.position = position;
            damageInfo.damageColorIndex = this.damageColorIndex;
            healthComponent.TakeDamage(damageInfo);
            GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
            GlobalEventManager.instance.OnHitAll(damageInfo, healthComponent.gameObject);
            if (healthComponent.combinedHealthFraction < voidExecution)
            {
                if (healthComponent.health > 0f)
                {
                    healthComponent.Networkhealth = 0f;
                }
                if (healthComponent.shield > 0f)
                {
                    healthComponent.Networkshield = 0f;
                }
                if (healthComponent.barrier > 0f)
                {
                    healthComponent.Networkbarrier = 0f;
                }
            }
            //EffectManager.SpawnEffect(this.detonationEffectPrefab, new EffectData
            //{
            //    origin = position,
            //    rotation = Quaternion.identity,
            //    scale = Mathf.Log((float)buffCount, 5f)
            //}, true);
        }

        
        public float travelSpeed = 60f;

        public float baseDamage;

        public float damagePerStack;

        public GameObject attacker;

        public bool isCrit;

        public ProcChainMask procChainMask;

        public float procCoefficient;

        public DamageColorIndex damageColorIndex;

        public GameObject detonationEffectPrefab;


        public GameObject orbEffectPrefab;
    }
}
