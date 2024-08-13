using System;
using R2API;
using RoR2;
using UnityEngine;

namespace VeliaerisMod.Modules
{

    internal static class DamageTypes
    {
        public static DamageAPI.ModdedDamageType AbyssCorrosion;
        public static DamageAPI.ModdedDamageType percentHealth;
        public static DamageAPI.ModdedDamageType armorVoid;
        public static DamageAPI.ModdedDamageType voidCorrosion;
        public static void Init()
        {
            AbyssCorrosion = DamageAPI.ReserveDamageType();
            percentHealth = DamageAPI.ReserveDamageType();
            armorVoid = DamageAPI.ReserveDamageType();
            voidCorrosion = DamageAPI.ReserveDamageType();
        }
    }
}
