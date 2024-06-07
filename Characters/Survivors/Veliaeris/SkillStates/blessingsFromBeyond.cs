using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates
{
    public class BlessingsFromBeyond :BaseSkillState
    {


        public float duration = 10f;
        public override void OnEnter()
        {
            System.Console.WriteLine("Entered buff");
            CharacterBody body;
            body = this.GetComponent<CharacterBody>();
            System.Console.WriteLine("The body:" + body);
            body.AddTimedBuffAuthority(VeliaerisBuffs.sistersBlessing.buffIndex,duration);
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
