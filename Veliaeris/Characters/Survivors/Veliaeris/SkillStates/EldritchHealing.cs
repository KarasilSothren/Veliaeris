using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates
{
    public class EldritchHealing: BaseSkillState
    {
        private HurtBox[] targetTargets;
        public float duration = 10f;
        public override void OnEnter()
        {

            CharacterBody body;
            body = this.GetComponent<CharacterBody>();
            float Erishealth = body.healthComponent.health;
            this.characterBody.healthComponent.Heal(Erishealth*0.5f,default(ProcChainMask));
            TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].teamIndex == this.teamComponent.teamIndex)
                {
                    array[i].GetComponent<CharacterBody>().healthComponent.Heal(Erishealth * 0.75f, default(ProcChainMask));
                }
            }
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
