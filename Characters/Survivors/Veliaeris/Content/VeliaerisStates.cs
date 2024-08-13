using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2.Skills;
using VeliaerisMod.Survivors.Veliaeris;
using VeliaerisMod.Survivors.Veliaeris.SkillStates;
using System.Collections.ObjectModel;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Modules;
using RoR2.UI;
using VeliaerisMod.Characters.Survivors.Veliaeris.SkillStates;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    public class HeldState : MonoBehaviour
    {
        [SerializeField]
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
                   // System.Console.WriteLine("VeliaerisReturn");
                    return VeliaerisState.Veliaeris;
                }
                if (this.passiveSkillSlot.skillDef == this.ErisStart)
                {
                   // System.Console.WriteLine("ErisReturn");
                    return VeliaerisState.Eris;
                }
                if (this.passiveSkillSlot.skillDef == this.VeliaStart)
                {
                   // System.Console.WriteLine("VeliaReturn");
                    return VeliaerisState.Velia;
                }
            }
//            System.Console.WriteLine("Returned default");
            return VeliaerisState.Veliaeris;
        }

    }
    public class VeliaerisStatuses : MonoBehaviour
    {
        #region variables
        private static BodyIndex MithrixBodyIndex = BodyIndex.None;
        private static BodyIndex GoldTitanBodyIndex = BodyIndex.None;
        //private static BodyIndex HereticBodyIndex = BodyIndex.None;
        private static BodyIndex VoidlingBodyIndex = BodyIndex.None;
        public static CharacterMaster masterCharacter;
        public static float timeUntilSistersRevival = -1f;//useless
        public static float startUpTimer;
        public static bool stageStarted = false;
        private bool mithrixFlag = false;
        private bool voidlingFlag = false;
        public static bool switchToEris = false;
        private VeliaerisSurvivorController VeliaerisSurvivorController;
        private readonly string VeliaerisBodyName = "mdlHenry";
        public static bool notSeenBoss = true;
        public static float utilityCooldown = -1f;
        public static float erisSpecialCooldownStock = -1f;
        public static float erisSecondaryCooldownStock = -1f;
        public static float veliaerisSpecialCooldownStock = -1f;
        public static float veliaSpecialCooldownStock = -1f;
        public static float veliaSecondaryCooldownStock = -1f;
        public static float veliaerisSecondaryCooldownStock = -1f;
        public static int erisSpecialStock = 0;
        public static int erisSecondaryStock = 0;
        public static int veliaerisSpecialStock = 0;
        public static int veliaSpecialStock = 0;
        public static int veliaSecondaryStock = 0;
        public static int veliaerisSecondaryStock = 0;
        public static bool pickedUpPack;
        public static int erisSpecialStockMax = 0;
        public static int erisSecondaryStockMax = 0;
        public static int veliaSpecialStockMax = 0;
        public static int veliaSecondaryStockMax = 0;
        public static int veliaerisSpecialStockMax = 0;
        public static int veliaerisSecondaryStockMax = 0;
        public static bool doSpecialStockEris = false;
        public static bool doSecondaryStockEris = false;
        public static bool doSpecialStockVelia = false;
        public static bool doSecondaryStockVelia = false;
        public static bool doSpecialStockVeliaeris = false;
        public static bool doSecondaryStockVeliaeris = false;
        #endregion
        public static void Init()
        {
            //            Modules.Content.AddEntityState(typeof(VeliaerisRespawnSkillDef));

            Modules.Content.AddEntityState(typeof(BasicScytheSlash));

            Modules.Content.AddEntityState(typeof(Corrupt));

            Modules.Content.AddEntityState(typeof(Split));

            Modules.Content.AddEntityState(typeof(VoidDetonator));

            Modules.Content.AddEntityState(typeof(MergeandShift));

            Modules.Content.AddEntityState(typeof(GraspOfOblivion));

            Modules.Content.AddEntityState(typeof(GivenStrength));

            Modules.Content.AddEntityState(typeof(EldritchHealing));

            Modules.Content.AddEntityState(typeof(BasicScytheSlashWithReductions));

            Modules.Content.AddEntityState(typeof(CircularSlash));

            Modules.Content.AddEntityState(typeof(BlessingsFromBeyond));

            Modules.Content.AddEntityState(typeof(GraspOfOblivionOrb));

            Modules.Content.AddEntityState(typeof(CallSister));


        }
        //
        public void Update()
        {
            CharacterBody body = this.GetComponent<CharacterBody>();
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
            float Erishealth = body.healthComponent.fullHealth;
            float erisShield = body.healthComponent.fullShield * 0.5f;
            TeamComponent teamComponent = GetComponent<TeamComponent>();
            TeamComponent[] array = UnityEngine.Object.FindObjectsOfType<TeamComponent>();
            SkillLocator skillLocator = body.GetComponent<SkillLocator>();
            ////5 minutes is 300f
            if (!body.HasBuff(VeliaerisBuffs.sistersBlessing)||VeliaerisSurvivorController.VeliaerisState!=VeliaerisState.Velia)
            {
                skillLocator.special.UnsetSkillOverride(skillLocator.special, VeliaerisSurvivor.callUponSister, GenericSkill.SkillOverridePriority.Contextual);
            }
            #region switcheffects
            if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Eris)
            {
                body.SetBuffCount(VeliaerisBuffs.inflictDeath.buffIndex, 0);
                body.SetBuffCount(VeliaerisBuffs.revokeDeath.buffIndex, VeliaerisSurvivor.DeathPreventionStacks);
            }
            if (VeliaerisSurvivorController.VeliaerisState == VeliaerisState.Velia)
            {
                body.SetBuffCount(VeliaerisBuffs.revokeDeath.buffIndex, 0);
                body.SetBuffCount(VeliaerisBuffs.inflictDeath.buffIndex, VeliaerisSurvivor.VoidCorruptionStacks);

            }
            float heldHealValue = 0.7f;
            if (VeliaerisSurvivor.voidInfluence >= VeliaerisStaticValues.secondTierCorruption)
            {
                heldHealValue = 2.7f;
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].teamIndex == teamComponent.teamIndex)
                {
                    if ((array[i].GetComponent<CharacterBody>().healthComponent.health + array[i].GetComponent<CharacterBody>().healthComponent.shield) / array[i].GetComponent<CharacterBody>().healthComponent.fullCombinedHealth <= 0.15f && body.GetBuffCount(VeliaerisBuffs.revokeDeath) > 0)
                    {
                        VeliaerisSurvivor.DeathPreventionStacks--;
                        body.SetBuffCount(VeliaerisBuffs.revokeDeath.buffIndex, VeliaerisSurvivor.DeathPreventionStacks);
                        array[i].GetComponent<CharacterBody>().healthComponent.Heal(((Erishealth + erisShield) * heldHealValue), default(ProcChainMask));
                    }
                }
            }
            #endregion
//            Debug.Log("current cooldown:" + (VeliaerisStates.erissecondarycooldown / VeliaerisSurvivor.erissecondarycooldown));
            #region reviveevents
            if (timeUntilSistersRevival >= 0)
            {
                timeUntilSistersRevival -= Time.deltaTime;
                //                System.Console.WriteLine("time until:" + timeUntilSistersRevival);
            }
            //            Debug.Log("Revive Timer:" + VeliaerisSurvivorController.ReviveDisabledTimer);
            if (VeliaerisSurvivorController.ReviveDisabledTimer >= 0)
            {
                VeliaerisSurvivorController.setNetworkReviveTimer -= Time.deltaTime;
                if (!body.HasBuff(VeliaerisBuffs.splitRevive))
                {
                    body.AddTimedBuff(VeliaerisBuffs.splitRevive, VeliaerisSurvivorController.ReviveDisabledTimer);
                }
            }

            Corpse corpseIdentity = null;
            for (int i = Corpse.instancesList.Count - 1; i >= 0; i--)
            {
                if (Corpse.instancesList[i].name.ToString() == (VeliaerisBodyName))
                {
                    corpseIdentity = Corpse.instancesList[i];
                    break;
                }
            }
            if (corpseIdentity != null && HeldState.destroyVeliaerisCorpse)
            {
                Corpse.DestroyCorpse(corpseIdentity);
                HeldState.destroyVeliaerisCorpse = false;
                corpseIdentity = null;
            }
            #endregion
            #region speechevents
            MithrixBodyIndex = BodyCatalog.FindBodyIndex("BrotherBody");
            VoidlingBodyIndex = BodyCatalog.FindBodyIndex("MiniVoidRaidCrabBodyPhase1");
            SpeechDriver speech = new SpeechDriver();
            if (startUpTimer < 1)
            {
                startUpTimer += Time.fixedDeltaTime;
            }
            else if (stageStarted == false)
            {
                Debug.Log("StageIdentity:" + VeliaerisSurvivor.StageIdentity);
                stageStarted = true;

                if (body.ToString().Contains("VeliaerisBody"))
                {
                    speech.enacteDialogue("stage", body, "");
                }
            }
            ReadOnlyCollection<CharacterBody> readOnlyInstancesList = CharacterBody.readOnlyInstancesList;

            for (int i = 0; i < readOnlyInstancesList.Count; i++)
            {
                BodyIndex bodyIndex = readOnlyInstancesList[i].bodyIndex;
                mithrixFlag |= bodyIndex == MithrixBodyIndex;
                voidlingFlag |= bodyIndex == VoidlingBodyIndex;
            }
            if (notSeenBoss)
            {
                if (mithrixFlag)
                {
                    notSeenBoss = false;
                    speech.enacteDialogue("enemy", body, "mithrix");
                }
                if (voidlingFlag)
                {
                    notSeenBoss = false;
                    speech.enacteDialogue("enemy", body, "voidling");
                }
            }
            #endregion
            #region skillcooldowntracking
            erisSpecialStockMax = skillLocator.special.maxStock;
            erisSecondaryStockMax = skillLocator.secondary.maxStock;
            veliaSpecialStockMax = skillLocator.special.maxStock;
            veliaSecondaryStockMax = skillLocator.secondary.maxStock;
            veliaerisSpecialStockMax = skillLocator.special.maxStock;
            veliaerisSecondaryStockMax = skillLocator.secondary.maxStock;
            #region nocooldownsset
            if (body.HasBuff(RoR2Content.Buffs.NoCooldowns))
            {
                    erisSpecialCooldownStock = 0.5f;
                    erisSecondaryCooldownStock = 0.5f;
                veliaSpecialCooldownStock = 0.5f;
                veliaSecondaryCooldownStock = 0.5f;
                veliaerisSpecialCooldownStock = 0.5f;
                veliaerisSecondaryCooldownStock = 0.5f;
            }
            else
            {
                    erisSpecialCooldownStock = VeliaerisSurvivor.erisspecialcooldown*skillLocator.special.cooldownScale;
                    erisSecondaryCooldownStock = VeliaerisSurvivor.erissecondarycooldown * skillLocator.secondary.cooldownScale;
                veliaSpecialCooldownStock = VeliaerisSurvivor.veliaspecialcooldown * skillLocator.special.cooldownScale;
                veliaSecondaryCooldownStock = VeliaerisSurvivor.veliasecondarycooldown * skillLocator.secondary.cooldownScale;
                veliaerisSpecialCooldownStock = VeliaerisSurvivor.veliaerisspecialcooldown * skillLocator.special.cooldownScale;
                veliaerisSecondaryCooldownStock = VeliaerisSurvivor.veliaerissecondarycooldown * skillLocator.secondary.cooldownScale;
            }
            #endregion
            #region packreset
            if (pickedUpPack)
            {
                if (erisSpecialStock < erisSpecialStockMax)
                {
                    erisSpecialStock++;
                }
                if (erisSecondaryStock < erisSecondaryStockMax)
                {
                    erisSecondaryStock++;
                }
                if (veliaSpecialStock < veliaSpecialStockMax)
                {
                    veliaSpecialStock++;
                }
                if (veliaSecondaryStock < veliaSecondaryStockMax)
                {
                    veliaSecondaryStock++;
                }
                if (veliaerisSpecialStock < veliaerisSpecialStockMax)
                {
                    veliaerisSpecialStock++;
                }
                if (veliaerisSecondaryStock < veliaerisSecondaryStockMax)
                {
                    veliaerisSecondaryStock++;
                }
                pickedUpPack = false;
            }
            #endregion
            #region stockcooldown
            if (erisSpecialCooldownStock <= 0 && erisSpecialStock<erisSpecialStockMax)
            {
                if (body.HasBuff(RoR2Content.Buffs.NoCooldowns))
                {
                    erisSpecialCooldownStock = 0.5f;
                }
                else
                {
                    erisSpecialCooldownStock = VeliaerisSurvivor.erisspecialcooldown*skillLocator.special.cooldownScale;
                }
            }
            if (erisSecondaryCooldownStock <= 0 && erisSecondaryStock < erisSecondaryStockMax)
            {
                if (body.HasBuff(RoR2Content.Buffs.NoCooldowns))
                {
                    erisSecondaryCooldownStock = 0.5f;
                }
                else
                {
                    erisSecondaryCooldownStock = VeliaerisSurvivor.erissecondarycooldown * skillLocator.secondary.cooldownScale;
                }
            }
            if (veliaSpecialCooldownStock <= 0 && veliaSpecialStock < veliaSpecialStockMax)
            {
                if (body.HasBuff(RoR2Content.Buffs.NoCooldowns))
                {
                    veliaSpecialCooldownStock = 0.5f;
                }
                else
                {
                    veliaSpecialCooldownStock = VeliaerisSurvivor.veliaspecialcooldown * skillLocator.special.cooldownScale;
                }
            }
            if (veliaSecondaryCooldownStock <= 0 && veliaSecondaryStock < veliaSecondaryStockMax)
            {
                if (body.HasBuff(RoR2Content.Buffs.NoCooldowns))
                {
                    veliaSecondaryCooldownStock = 0.5f;
                }
                else
                {
                    veliaSecondaryCooldownStock = VeliaerisSurvivor.veliasecondarycooldown * skillLocator.secondary.cooldownScale;
                }
            }
            if (veliaerisSpecialCooldownStock <= 0 && veliaerisSpecialStock < veliaerisSpecialStockMax)
            {
                if (body.HasBuff(RoR2Content.Buffs.NoCooldowns))
                {
                    veliaerisSpecialCooldownStock = 0.5f;
                }
                else
                {
                    veliaerisSpecialCooldownStock = VeliaerisSurvivor.veliaerisspecialcooldown * skillLocator.special.cooldownScale;
                }
            }
            if (veliaerisSecondaryCooldownStock <= 0 && veliaerisSecondaryStock < veliaerisSecondaryStockMax)
            {
                if (body.HasBuff(RoR2Content.Buffs.NoCooldowns))
                {
                    veliaerisSecondaryCooldownStock = 0.5f;
                }
                else
                {
                    veliaerisSecondaryCooldownStock = VeliaerisSurvivor.veliaerissecondarycooldown * skillLocator.secondary.cooldownScale;
                }
            }
            #endregion
            #region stock charger
            if (erisSpecialStock < erisSpecialStockMax)
            {
                erisSpecialCooldownStock -= Time.deltaTime;
                doSpecialStockEris = true;
            }
            if (erisSecondaryStock < erisSecondaryStockMax)
            {
                erisSecondaryCooldownStock -= Time.deltaTime;
                doSecondaryStockEris = true;
            }
            if (veliaSpecialStock < veliaSpecialStockMax)
            {
                veliaSpecialCooldownStock -= Time.deltaTime;
                doSpecialStockVelia = true;
            }
            if (veliaSecondaryStock < veliaSecondaryStockMax)
            {
                veliaSecondaryCooldownStock -= Time.deltaTime;
                doSecondaryStockVelia = true;
            }
            if (veliaerisSpecialStock < veliaerisSpecialStockMax)
            {
                veliaerisSpecialCooldownStock -= Time.deltaTime;
                doSpecialStockVeliaeris = true;
            }
            if (veliaerisSecondaryStock < veliaerisSecondaryStockMax)
            {
                veliaerisSecondaryCooldownStock -= Time.deltaTime;
                doSecondaryStockVeliaeris = true;
            }
            #endregion
            #region rechargestocks
            if (erisSpecialCooldownStock <= 0 && doSpecialStockEris)
            {
                 erisSpecialStock += 1;
                 doSpecialStockEris = false;
            }
            if (erisSecondaryCooldownStock <= 0 && doSecondaryStockEris)
            {
                erisSecondaryStock += 1;
                doSecondaryStockEris = false;
            }
            if (veliaSecondaryCooldownStock <= 0 && doSecondaryStockVelia)
            {
                veliaSecondaryStock += 1;
                doSecondaryStockVelia = false;
            }
            if (veliaSpecialCooldownStock <= 0 && doSpecialStockVelia)
            {
                veliaSpecialStock += 1;
                doSpecialStockVelia = false;

            }
            if (veliaerisSecondaryCooldownStock <= 0 && doSecondaryStockVeliaeris)
            {
                veliaerisSecondaryStock += 1;
                doSecondaryStockVeliaeris = false;

            }
            if (veliaerisSpecialCooldownStock <= 0 && doSpecialStockVeliaeris)
            {
                veliaerisSpecialStock += 1;
                doSpecialStockVeliaeris = false;

            }
            #endregion


            #endregion
        }
    }



    


    public enum VeliaerisState
    {
        Veliaeris = 0,
        Velia =-1,
        Eris =1
    }

      
}
