using EntityStates;
using IL.RoR2.Orbs;
using On.RoR2.Orbs;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Survivors.Veliaeris;
using VeliaerisMod.Survivors.Veliaeris.SkillStates;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates
{
    public class GivenStrength :BaseSkillState
    {
        private HurtBox[] targetTargets;
        public float duration = 8f;
        public override void OnEnter()
        {
            System.Console.WriteLine("Entered given");
            CharacterBody body;
            body = this.GetComponent<CharacterBody>();
            this.characterBody.AddTimedBuff(VeliaerisBuffs.lesserSistersBlessing, duration);
            TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].teamIndex == this.teamComponent.teamIndex)
                {
                    System.Console.WriteLine("given strength team names: " + array[i].name);
                    array[i].GetComponent<CharacterBody>().AddTimedBuff(VeliaerisBuffs.lesserSistersBlessing, duration);
                }
            }
//            if (NetworkServer.active)
  //          {
                BullseyeSearch targetSearch = new BullseyeSearch();
                targetSearch.filterByDistinctEntity = true;
                targetSearch.filterByLoS = false;
                targetSearch.maxDistanceFilter = 30f;
                targetSearch.minDistanceFilter = 0f;
                targetSearch.minAngleFilter = 0f;
                targetSearch.maxAngleFilter = 360f;
                targetSearch.sortMode = BullseyeSearch.SortMode.Distance;
                targetSearch.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
                targetSearch.searchOrigin = base.characterBody.corePosition;
                targetSearch.RefreshCandidates();
                targetSearch.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> results = targetSearch.GetResults();
                this.targetTargets = results.ToArray<HurtBox>();
            //}
            System.Console.WriteLine("Before Zone");
    //        if(NetworkServer.active)
      //      {
                System.Console.WriteLine("Entered zone");
                for(int i = 0; i < this.targetTargets.Length; i++)
                {
                    targetTargets[i].healthComponent.body.AddTimedBuff(RoR2Content.Buffs.Cripple, 8f);
//                    targetTargets[i].healthComponent.body.AddBuff(RoR2Content.Buffs.PermanentCurse);
                }


        //    }
            base.OnEnter();
            VeliaerisStatuses.erisSecondaryStock--;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
