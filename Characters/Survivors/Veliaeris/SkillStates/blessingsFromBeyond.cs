using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Survivors.Veliaeris;
using VeliaerisMod.Survivors.Veliaeris.SkillStates;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates
{
    public class BlessingsFromBeyond :BaseSkillState
    {


        public float duration = 10f;
        public override void OnEnter()
        {
  //          System.Console.WriteLine("Entered buff");
            CharacterBody body;
            body = this.GetComponent<CharacterBody>();
//            System.Console.WriteLine("The body:" + body);
            body.AddTimedBuffAuthority(VeliaerisBuffs.sistersBlessing.buffIndex,duration);
            base.OnEnter();
            VeliaerisStatuses.veliaSpecialStock--;
            base.skillLocator.special.SetSkillOverride(skillLocator.special, VeliaerisSurvivor.callUponSister, GenericSkill.SkillOverridePriority.Contextual);
            skillLocator.special.RemoveAllStocks();
            for(int i = 0; i < 5; i++)
            {
                skillLocator.special.AddOneStock();
            }
        }   

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
