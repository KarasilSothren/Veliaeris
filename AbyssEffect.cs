using HG;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using static RoR2.DotController;
using static R2API.DamageAPI;
using HenryMod.Survivors.Henry;
using VeliaerisMod.Buffs;

namespace VeliaerisMod.DamageTypes
{

    public class AbyssEffect
    {

        internal static ModdedDamageType  ModdedDamageType;
        public static DotIndex DotIndex;

        public static float Duration = 50;

        public static ModdedDamageType abyssDamageType;

        public void Init()
        {
            abyssDamageType = ModdedDamageType;
            DotIndex = Abyss.index;
        }

        public void Delegates()
        {
            GlobalEventManager.onServerDamageDealt += ApplyAbyss;
        }

        private void ApplyAbyss(DamageReport report)
        {
            System.Console.WriteLine("Reporting");
            var victimBody = report.victimBody;
            var attackerBody = report.attackerBody;
            var damageInfo = report.damageInfo;
            if (DamageAPI.HasModdedDamageType(damageInfo, ModdedDamageType))
            {
                var dotInfo = new InflictDotInfo()
                {
                    attackerObject = attackerBody.gameObject,
                    victimObject = victimBody.gameObject,
                    dotIndex = Abyss.index,
                    duration = 2,
                    damageMultiplier = 1,
                };
                DotController.InflictDot(ref dotInfo);
            }
        }
    }
}
