using System;
using R2API;
using RoR2;
using UnityEngine;

namespace VeliaerisMod.Modules
{

    internal static class DamageTypes
    {
        public static DamageAPI.ModdedDamageType AbyssCorrosion;
        public static void Init()
        {
            AbyssCorrosion = DamageAPI.ReserveDamageType();
        }   
    }
}
