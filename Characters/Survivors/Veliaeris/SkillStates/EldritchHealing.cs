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
            body = GetComponent<CharacterBody>();
            float Erishealth = body.healthComponent.fullHealth;
            this.characterBody.healthComponent.Heal(Erishealth*0.5f,default(ProcChainMask));
            TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
            Util.CleanseBody(base.characterBody,true,false,false,true,true,false);
            System.Console.WriteLine("Entered Healing");
            for (int i = 0; i < array.Length; i++)
            {

                System.Console.WriteLine(array[i].name);
                if (array[i].teamIndex == this.teamComponent.teamIndex)
                {
                    System.Console.WriteLine("healing team names: " + array[i].name);
                    array[i].GetComponent<CharacterBody>().healthComponent.Heal(Erishealth * 0.5f, default(ProcChainMask));
                    Util.CleanseBody(array[i].GetComponent<CharacterBody>(),true,false,false,true,true,false);
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
