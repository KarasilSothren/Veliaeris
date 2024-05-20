using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using RoR2;
using UnityEngine.Networking;
using System.Linq;
using R2API.ScriptableObjects;
using HenryMod.Survivors.Henry;

namespace VeliaerisMod.Buffs
{
    public class Abyss
    {
        public static DotController.DotIndex index;
        public void Initialize()
        {
            index = DotAPI.RegisterDotDef(0.25f, 0.25f, DamageColorIndex.Void, VeliaerisBuffs.abyss);
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            bool triggerAbyss = false;
            if (NetworkServer.active)
            {
                if (damageInfo.dotIndex == index && damageInfo.procCoefficient == 0f && self.alive)
                {
                    if (damageInfo.attacker)
                    {
                        CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                        if (attackerBody)
                        {
                            damageInfo.crit = Util.CheckRoll(attackerBody.crit, attackerBody.master);
                        }
                    }
                    damageInfo.procCoefficient = 0.5f;
                    triggerAbyss = true;
                }
            }
            orig(self, damageInfo);

            if (NetworkServer.active && !damageInfo.rejected && self.alive)
            {
                if (triggerAbyss)
                {
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, self.gameObject);
                }
            }
        }

    }
}
