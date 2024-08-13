using EntityStates;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Survivors.Veliaeris;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/VeliaerisRespawnSkillDef")]
    public class VeliaerisRespawnSkillDef : SkillDef
    {
        
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return base.CanExecute(skillSlot) && !skillSlot.characterBody.HasBuff(VeliaerisBuffs.missingSibling);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && !skillSlot.characterBody.HasBuff(VeliaerisBuffs.missingSibling);
        }
    }

 
}
