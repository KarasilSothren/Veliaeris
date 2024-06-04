using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2.Skills;
using VeliaerisMod.Survivors.Veliaeris;
using VeliaerisMod.Survivors.Veliaeris.SkillStates;
using System.Collections.ObjectModel;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    public class HeldState : MonoBehaviour
    {
        [SerializeField]

        public static VeliaerisState velState = VeliaerisState.Veliaeris;
        public static List<VeliaerisState> hereticOverridesPrimary = new List<VeliaerisState>();
        public static List<VeliaerisState> hereticOverridesSecondary = new List<VeliaerisState>();
        public static List<VeliaerisState> hereticOverridesUtility = new List<VeliaerisState>();
        public static List<VeliaerisState> hereticOverridesSpecial = new List<VeliaerisState>();
        public static int gatheredPrimary = 0;
        public static int gatheredSecondary = 0;
        public static int gatheredUtility = 0;
        public static int gatheredSpecial = 0;
        public static bool destroyVeliaerisCorpse = false;
    }
    public class VeliaerisPassive: MonoBehaviour {
        public GenericSkill passiveSkillSlot;
        public SkillDef VeliaerisStart;
        public SkillDef ErisStart;
        public SkillDef VeliaStart;


        public VeliaerisState getStartState()
        {
            if (this.passiveSkillSlot)
            {
                if(this.passiveSkillSlot.skillDef==this.VeliaerisStart)
                {
                    return VeliaerisState.Veliaeris;
                }
                if (this.passiveSkillSlot.skillDef == this.ErisStart)
                {
                    return VeliaerisState.Eris;
                }
                if (this.passiveSkillSlot.skillDef == this.VeliaStart)
                {
                    return VeliaerisState.Velia;
                }
            }
            return VeliaerisState.Veliaeris;
        }

    }
    public class ReviveTimer: MonoBehaviour
    {
        private static BodyIndex MithrixBodyIndex = BodyIndex.None;
        private static BodyIndex GoldTitanBodyIndex = BodyIndex.None;
        //private static BodyIndex HereticBodyIndex = BodyIndex.None;
        private static BodyIndex VoidlingBodyIndex = BodyIndex.None;
        public static CharacterMaster masterCharacter;
        public static float timeUntilSistersRevival = 0f;
        public static float ReviveDisabledTimer = 0f;
        public static float startUpTimer;
        public static bool stageStarted = false;
        public static String stage;
        private bool mithrixFlag = false;
        private bool voidlingFlag = false;

        public void Update()
        {

            TeamComponent teamComponent = GetComponent<TeamComponent>();
//            System.Console.WriteLine("Update teamComponent:" + teamComponent);
            TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].teamIndex == teamComponent.teamIndex)
                {

                    //add eris buff check
                    if (array[i].GetComponent<CharacterBody>().healthComponent.combinedHealth< array[i].GetComponent<CharacterBody>().healthComponent.fullCombinedHealth*0.05f)
                    {
                        array[i].GetComponent<CharacterBody>().healthComponent.Heal(array[i].GetComponent<CharacterBody>().healthComponent.combinedHealth * 0.5f, default(ProcChainMask));
                    }
                    //                    array[i].GetComponent<CharacterBody>().AddTimedBuff(VeliaerisBuffs.lesserSistersBlessing, duration);
                }
            }
            //5 minutes is 300f
            //System.Console.WriteLine("Time until sisters revival:" + timeUntilSistersRevival);
            //System.Console.WriteLine("Time on Revive timer" + ReviveDisabledTimer);
            CharacterBody body = this.GetComponent<CharacterBody>();
            if (timeUntilSistersRevival >= 0)
            {
                timeUntilSistersRevival -= Time.deltaTime;
            }
            if(ReviveDisabledTimer >= 0)
            {
                ReviveDisabledTimer -= Time.deltaTime;
                if (!body.HasBuff(VeliaerisBuffs.splitRevive))
                {
                    body.AddTimedBuff(VeliaerisBuffs.splitRevive, ReviveDisabledTimer);
                }
            }
            else
            {
               VeliaerisSurvivor.justDied = false;
            }

            Corpse corpseIdentity = null;
            //if (Corpse.instancesList.Count > 0)
            //{
            //    corpseIdentity = Corpse.instancesList[0];
            //}
//            System.Console.WriteLine("corpse count:"+Corpse.instancesList.Count);
            for (int i = 0; i < Corpse.instancesList.Count; i++)
            {
                if (Corpse.instancesList[i].name.ToString()==("mdlHenry"))
                {
                    corpseIdentity = Corpse.instancesList[i];
                }
            }
            if (corpseIdentity != null)
            {
            }
            if (corpseIdentity != null&& HeldState.destroyVeliaerisCorpse)
            {
                Corpse.DestroyCorpse(corpseIdentity);
                HeldState.destroyVeliaerisCorpse = false;
                corpseIdentity=null;
            }

            MithrixBodyIndex = BodyCatalog.FindBodyIndex("BrotherBody");
            VoidlingBodyIndex = BodyCatalog.FindBodyIndex("MiniVoidRaidCrabBodyPhase1");
            if (startUpTimer <1)
            {
                startUpTimer += Time.fixedDeltaTime;
            }
            else if (stageStarted == false)
            {
                stageStarted = true;
                SpeechDriver speech = new SpeechDriver();
                speech.enacteDialogue(stage);
            }
            ReadOnlyCollection<CharacterBody> readOnlyInstancesList = CharacterBody.readOnlyInstancesList;

            for (int i = 0; i < readOnlyInstancesList.Count; i++)
            {
                BodyIndex bodyIndex = readOnlyInstancesList[i].bodyIndex;
                mithrixFlag |= bodyIndex == MithrixBodyIndex;
                voidlingFlag |= bodyIndex == VoidlingBodyIndex;
            }

        }
    }

    


    public enum VeliaerisState
    {
        Veliaeris = 0,
        Velia =-1,
        Eris =1
    }

    public static class VeliaerisStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(BasicScytheSlash));

            Modules.Content.AddEntityState(typeof(Corrupt));

            Modules.Content.AddEntityState(typeof(Split));

            Modules.Content.AddEntityState(typeof(VoidDetonator));
        }
    }
}
