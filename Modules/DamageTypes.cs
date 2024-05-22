using System;
using R2API;
using RoR2;
using UnityEngine;

namespace VeliaerisMod.Modules
{

    internal static class DamageTypes
    {

        public static void RegisterDamageTypes()
        {
            AbyssCorrosion = DamageAPI.ReserveDamageType();
        }


        public static DamageAPI.ModdedDamageType AbyssCorrosion;
    }
}
