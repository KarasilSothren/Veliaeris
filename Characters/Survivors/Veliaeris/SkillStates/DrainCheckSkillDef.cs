using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/DrainCheckSkillDef")]
    class DrainCheckSkillDef :SkillDef
    {
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            SkillLocator skillLocator = skillSlot.characterBody.GetComponent<SkillLocator>();
            int stocksLeft = (5 - skillLocator.special.stock) - 1;
            float drainPercentage = 0.35f + (0.15f * stocksLeft);
            float drain = skillSlot.characterBody.healthComponent.fullCombinedHealth * drainPercentage;
            return base.CanExecute(skillSlot) && skillSlot.characterBody.healthComponent.health>drain;
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            SkillLocator skillLocator = skillSlot.characterBody.GetComponent<SkillLocator>();
            int stocksLeft = (5 - skillLocator.special.stock) - 1;
            float drainPercentage = 0.35f + (0.15f * stocksLeft);
            float drain = skillSlot.characterBody.healthComponent.fullCombinedHealth * drainPercentage;
            return base.IsReady(skillSlot) && skillSlot.characterBody.healthComponent.health > drain;
        }
    }
}
