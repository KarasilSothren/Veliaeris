using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates
{
    public class blessingsFromBeyond :BaseSkillState
    {


        public float duration = 20f;
        public override void OnEnter()
        {

            CharacterBody body;
            body = this.GetComponent<CharacterBody>();
            body.AddTimedBuff(VeliaerisBuffs.sistersBlessing,duration);
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
