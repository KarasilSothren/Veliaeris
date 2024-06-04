using RoR2;
using RoR2.Orbs;
using System;
using UnityEngine;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;

namespace VeliaerisMod.Modules
{
    public class GraspDamageOrb : GenericDamageOrb
    {
        public override void Begin()
        {
            this.speed = 500f;
            base.Begin();
        }
    }
}
