using RoR2.Orbs;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;

namespace VeliaerisMod.Modules
{
    public class GraspOfOblivionOrb : Orb
    {
        public override void Begin()
        {
            base.duration = 0f;
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
            System.Console.WriteLine("arrived in grasporb");
            System.Console.WriteLine("base target data:"+target);
            base.OnArrival();
            if (!this.target)
            {
                return;
            }
            HealthComponent healthComponent = this.target.healthComponent;
            System.Console.WriteLine("healthcomponent target data:" + target.healthComponent);
            if (!healthComponent){
                return;
            }
            CharacterBody body = healthComponent.body;
            if(!body){
                return;
            }
           
            Vector3 position = this.target.transform.position;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = this.baseDamage;
            damageInfo.attacker = this.attacker;
            damageInfo.inflictor = null;
            damageInfo.force = Vector3.zero;
            damageInfo.crit = this.isCrit;
            damageInfo.procChainMask = this.procChainMask;
            damageInfo.procCoefficient= this.procCoefficient;
            damageInfo.position = position;
            damageInfo.damageColorIndex = this.damageColorIndex;
            healthComponent.TakeDamage(damageInfo);
            GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
            GlobalEventManager.instance.OnHitAll(damageInfo, healthComponent.gameObject);
        }

        public float travelSpeed = 600f;

        public float baseDamage;


        public GameObject attacker;

        public bool isCrit;

        public ProcChainMask procChainMask;

        public float procCoefficient;

        public DamageColorIndex damageColorIndex;

        public GameObject orbEffectPrefab;
    }
}
