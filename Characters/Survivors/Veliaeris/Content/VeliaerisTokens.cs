using System;
using VeliaerisMod.Modules;
using VeliaerisMod.Survivors.Veliaeris.Achievements;
using VeliaerisMod.Survivors.Veliaeris.SkillStates;

namespace VeliaerisMod.Survivors.Veliaeris
{
    public static class VeliaerisTokens
    {
        public static void Init()
        {
            AddVeliaerisTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddVeliaerisTokens()
        {
            string prefix = VeliaerisSurvivor.VELIAERIS_PREFIX;

            string desc = "Veliaeris is a mutli-survivor character that can change based on the situation with 3 different personas to either take down foes or help allies.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Veliaeris has a combonation of Velia's and Eris' capability of both damage and healing" + Environment.NewLine + Environment.NewLine
             + "< ! > Eris is a party buffer that buffs allies and can heal them at a moments notice" + Environment.NewLine + Environment.NewLine
             + "< ! > Velia is a high damage dealer with increased damage and ability to deal high enemy percent health damage" + Environment.NewLine + Environment.NewLine;

            string outro = "..and so she left, a new understanding of themselves.";
            string outroFailure = "..and so she vanished, two halves no longer as one.";

            Language.Add(prefix + "NAME", "Veliaeris");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Azelythtsa's Sister");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion
            String voidPassive="";
            if (VeliaerisPlugin.hasVoid)
            {
                voidPassive = "\nCollecting Void infused items will increase the Persona's capabilities";
            }
            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Sister of The One Who Usurped The Void");
            Language.Add(prefix + "VELIAERIS_NAME", "Start as Veliaeris");
            Language.Add(prefix + "ERIS_NAME", "Start as Eris");
            Language.Add(prefix + "VELIA_NAME", "Start as Velia");
            Language.Add(prefix + "SELECTION_PASSIVE_DESCRIPTION", "You will start this run as the currently selected persona.");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "While split you are able to revive once every 5 minute.\nHeretic Items will only apply to the Persona they are picked up on."+voidPassive);
            #endregion

            #region StatePrimaries
            Language.Add(prefix + "VELIAERIS_PRIMARY_NAME", "Scythe");
            Language.Add(prefix + "VELIAERIS_PRIMARY_DESCRIPTION", Tokens.agilePrefix + $"Slash your enemies for <style=cIsDamage>{100f * VeliaerisStaticValues.scytheDamageCoefficient}</style>% damage along with inflicting a stack of Abyss.");
            Language.Add(prefix + "ERIS_PRIMARY_NAME", "Grasp of Oblivion");
            Language.Add(prefix + "ERIS_PRIMARY_DESCRIPTION", Tokens.agilePrefix + $"Grasp the enemies soul dealing <style=cIsDamage>{100f*0.5f}%</style> of your max health plus <style=cIsDamage>{100f*0.02f}% of the enemies max health damage</style>.");
            Language.Add(prefix + "VELIA_PRIMARY_NAME", "Void Infused Scythe");
            Language.Add(prefix + "VELIA_PRIMARY_DESCRIPTION", Tokens.agilePrefix + $"Slash your enemies for <style=cIsDamage>{100f * VeliaerisStaticValues.scytheDamageCoefficient}% damage </style> along with inflicting a stack of Abyss. There is also a {100f * 0.15f}% chance to inflict Armor Desolation");
            #endregion

            #region StateSecondaries
            Language.Add(prefix + "VELIAERIS_SECONDARY_NAME", "Abyssal Crush");
            Language.Add(prefix + "VELIAERIS_SECONDARY_DESCRIPTION", Tokens.agilePrefix + $"Target an enemy inflicting a stack of abyss and healing for <style=cIsHealing>{100f*0.3f}%</style> of your max health per stack on the targeted enemy.");
            Language.Add(prefix + "ERIS_SECONDARY_NAME", "Gaze of Azelythtsa");
            Language.Add(prefix + "ERIS_SECONDARY_DESCRIPTION", $"Buff yourself and your allies and inflicting cripple on nearby enemies for 8 seconds.\n<color=#8f038c>During this time period the void can not affect you.</color>");
            Language.Add(prefix + "VELIA_SECONDARY_NAME", "Slash of The Abyss");
            Language.Add(prefix + "VELIA_SECONDARY_DESCRIPTION", Tokens.agilePrefix + $"Slash around you dealing <style=cIsDamage>{100f *VeliaerisStaticValues.scytheDamageCoefficient}% damage</style> plus extra percent health damage based on the enemy. Inflict two stacks of Abyss to each Enemy hit.");
            #endregion

            #region DualUtility
            Language.Add(prefix + "SPLIT_UTILITY_NAME", "Split");
            Language.Add(prefix + "SPLIT_UTILITY_DESCRIPTION_Eris", Tokens.agilePrefix + $"Split into two personas changing into either Eris or Velia.\nOn this switch you will change into <color=#0787f0>Eris</color>");
            Language.Add(prefix + "SPLIT_UTILITY_DESCRIPTION_Velia", Tokens.agilePrefix + $"Split into two personas changing into either Eris or Velia.\nOn this switch you will change into <color=#fc0000>Velia</color>");
            Language.Add(prefix + "SPLIT_UTILITY_DESCRIPTION_MENU", Tokens.agilePrefix + $"Split into two personas changing into either Eris or Velia. On switching you will Switch to the previous Persona you were, Eris by default.");
            Language.Add(prefix + "MERGE_UTILITY_NAME", "Merge Or Switch");
            Language.Add(prefix + "MERGE_UTILITY_DESCRIPTION", Tokens.agilePrefix + "Press to merge back into Veliaeris or hold to switch to the other Persona.");
            #endregion

            #region StateSpecials
            Language.Add(prefix + "VELIAERIS_SPECIAL_NAME", "Void Annihilation");
            Language.Add(prefix + "VELIAERIS_SPECIAL_DESCRIPTION", Tokens.agilePrefix + $"Detonate all stacks of Abyss on enemies at infinite range dealing <style=cIsDamage>{100f*VoidDetonator.baseDamageCoefficient}% damage</style> plus <style=cIsDamage>{100f*VoidDetonator.damageCoefficientPerStack}% damage per stack of abyss</style>. Enemies below 5% hp are executed.");
            Language.Add(prefix + "ERIS_SPECIAL_NAME", "Refresh Fortitude");
            Language.Add(prefix + "ERIS_SPECIAL_DESCRIPTION", Tokens.agilePrefix + $"<style=cIsHealing>Heal yourself and your Allies for {100f*0.5f}% of you max hp</style>. Also cleanse all debuffs and current damage of time effects for yourself and Allies.");
            Language.Add(prefix + "VELIA_SPECIAL_NAME", "Awaken the Endless Abyss");
            Language.Add(prefix + "VELIA_SPECIAL_DESCRIPTION", Tokens.agilePrefix + $"Buff yourself with the power of the Endless Abyss. Granting you great strength.\n<color=#8f038c>During this time period the void can not affect you.</color>");
            #endregion

            #region extraInformation
            Language.Add(prefix + "ABYSS_INFORMATION", "<style=cKeywordName>Abyss</style>Deals 1 damage per stack for 10 seconds. Each added stack resets the duration.");
            Language.Add(prefix + "PERCENT_HEALTH_DAMAGE", $"<style=cKeywordName>Percent Health Damage</style>Deals damage based on the enemies health rather than a damage multiplier. For Regular enemies it is {100f*0.25f}% of their max health. For bosses it is {100f*0.05f}% of their max health.");
            Language.Add(prefix + "SKILL_INFORMATION", "<style=cKeywordName>Stagnat Skills</style>All Skills are Persona specfic the selection of skills is display only and will not affect what skills the survivor will use in what form. Their purpose is to give a view into what each form can do.");
            #endregion

            //#region Achievements
            //Language.Add(Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier), "Henry: Mastery");
            //Language.Add(Tokens.GetAchievementDescriptionToken(HenryMasteryAchievement.identifier), "As Henry, beat the game or obliterate on Monsoon.");
            //#endregion
        }
    }
}
