﻿using System;
using System.Collections.Generic;
using System.Text;
using VeliaerisMod.Survivors.Veliaeris;
using R2API;
using RoR2;

namespace VeliaerisMod.Characters.Survivors.Veliaeris
{
    internal class VeliaerisDots
    {
        public static DotController.DotIndex AbyssCorrosion;
        public static DotController.DotIndex cripleInflict;
        public static void Init()
        {

            AbyssCorrosion = DotAPI.RegisterDotDef(new DotController.DotDef
            {
                interval = VeliaerisSurvivor.DotIntervalAbyssBleed,
                damageCoefficient = VeliaerisSurvivor.AbyssDamage,
                damageColorIndex = DamageColorIndex.Void,
                associatedBuff = Content.VeliaerisBuffs.abyss
            });



        }
    }
}
