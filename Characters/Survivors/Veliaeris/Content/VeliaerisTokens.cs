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
            string outroFailure = "..and so she vanished, the void having consumed her.";
            string lore = "Further, Further into the endless ocean it calls." + Environment.NewLine
            + "Deeper, Deeper into the Endless sea it still calls." + Environment.NewLine
            + "Into a hole, a hole surrounded by florescent coral. I still hear it calling." + Environment.NewLine + Environment.NewLine
            + "Deeper, Deeper into the Abyss. It's call still comes to me, 『con?sumes」me." + Environment.NewLine + Environment.NewLine
            + "Deeper, Deeper into the dark vo?id it beckons me clo?ser " + Environment.NewLine + Environment.NewLine
            + "Deeper, even Deeper, Darker, Yet even Darker. I go, growing ev?er cl?os?er to the ca??ll " + Environment.NewLine + Environment.NewLine
            + "Beyond my dimmed sight, within this〖「END??LES?S V?O??ID】I see them. Creatures of the void within reach, their 【PO?WE?R】close enough to touch" + Environment.NewLine + Environment.NewLine
            + "【「FUR?THER,DEE?PER I S??W?IM〗,sinking beyond the ones apart of 「TH?E V?OI??D』" + Environment.NewLine + Environment.NewLine
            + "【『DEE?PE?R CLO?SE?R』」」 it calls my name 『B?UT I STI??LL ?CA?N N?OT H?E??A?R IT??」" + Environment.NewLine + Environment.NewLine
            + "So I must go even 【『??DEE?PE??R」" + Environment.NewLine + Environment.NewLine
            + "Until even this darkness feels like an 〖?EN??DL??ESS ET??ERN?IT?Y』" + Environment.NewLine + Environment.NewLine
            + "As I feel my 【EN?DLE?SS BR??EAT?H」 start to 【?SUF??FOC?ATE】』me"
            + "I【NO??W SW?IM, NO ST?AND BE??FO??RE IT』before,『「TH?E V??OI?D〗】" + Environment.NewLine + Environment.NewLine
            + "I finaly hear it, the voice of 『??TH?E VO??I?D】" + Environment.NewLine + Environment.NewLine
            + "And with it, I hear my new name.\nAs my descent.\nno,my fall into 『?MAD??NE?SS.〗】\nlet me 【?USU?RPE」and become the new ruler of this 〖「??EN??DLE??SS VO??ID?】\nAnd the name THE VOID gave me was " + Environment.NewLine + Environment.NewLine
            + "<style=cMono>『Azelythtsa〗</style>";
            Language.Add(prefix + "NAME", "Veliaeris");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Entropic Duality");
            Language.Add(prefix + "LORE", lore);
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion
            String voidPassive="";
            if (VeliaerisPlugin.hasVoid)
            {
                voidPassive = "\n<color=#8f038c>Influences from the void will give strength but also corrupt.</color>";
            }
            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Sister of The One Who Usurped The Void");
            Language.Add(prefix + "VELIAERIS_NAME", "Start as Veliaeris");
            Language.Add(prefix + "ERIS_NAME", "Start as Eris");
            Language.Add(prefix + "VELIA_NAME", "Start as Velia");
            Language.Add(prefix + "SELECTION_PASSIVE_DESCRIPTION_VELIAERIS", "You will start this run as the currently selected persona.\nNo Stats are Changed at start.");
            Language.Add(prefix + "SELECTION_PASSIVE_DESCRIPTION_ERIS", $"You will start this run as the currently selected persona.\nGain max health equal to <style=cIsHealing>{100*0.75f}% of your base max health, but have <style=cIsUtility>your armor reduced by 10 and <style=cIsDamage>your attack reduced to 0.");
            Language.Add(prefix + "SELECTION_PASSIVE_DESCRIPTION_VELIA", $"You will start this run as the currently selected persona.\nGain <style=cIsDamage>{100*0.5f}% increase to damage along with <style=cIsUtility>10 bonus armor but, have <style=cIsHealing>your max health reduced by {100*0.25f}% of your base max health");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "While split you are able to revive once every 5 minute.\nHeretic Items will only apply to the Persona they are picked up on and are the only items able to replace skills."+voidPassive);
            #endregion

            #region StatePrimaries
            Language.Add(prefix + "VELIAERIS_PRIMARY_NAME", "Oblivions Finality");
            Language.Add(prefix + "VELIAERIS_PRIMARY_NAME_CORRUPT", "?O??blivi?ons Fi??nality");
            Language.Add(prefix + "VELIAERIS_PRIMARY_DESCRIPTION", $"Slash your enemies for <style=cIsDamage>{100f * VeliaerisStaticValues.scytheDamageCoefficient}% damage</style> along with inflicting a stack of Abyss.");
            Language.Add(prefix + "ERIS_PRIMARY_NAME", "Grasp of Oblivion");
            Language.Add(prefix + "ERIS_PRIMARY_NAME_CORRUPT", "Gra??sp o?f O??blivi??on");
            Language.Add(prefix + "ERIS_PRIMARY_DESCRIPTION", $"Grasp the enemies soul dealing <style=cIsDamage>{100f*VeliaerisStaticValues.grasppersonalHealthCoefficent}% of your max health plus <style=cIsDamage>{100f*VeliaerisStaticValues.graspenemyhealthCoefficent}% of the enemies max health damage</style>.");
            Language.Add(prefix + "VELIA_PRIMARY_NAME", "Void Infused Finality");
            Language.Add(prefix + "VELIA_PRIMARY_NAME_CORRUPT", "??V??oid Inf??used Fi??nali??ty");
            Language.Add(prefix + "VELIA_PRIMARY_DESCRIPTION", $"Slash your enemies for <style=cIsDamage>{100f * VeliaerisStaticValues.scytheDamageCoefficient}% damage</style> along with inflicting a stack of Abyss. Strikes have a chance to inflict Armor Desolation");
            #endregion

            #region StateSecondaries
            Language.Add(prefix + "VELIAERIS_SECONDARY_NAME", "Abyssal Crush");
            Language.Add(prefix + "VELIAERIS_SECONDARY_NAME_CORRUPT", "A??bys??sal Cr??ush");
            Language.Add(prefix + "VELIAERIS_SECONDARY_DESCRIPTION", Tokens.agilePrefix + $"Target an enemy dealing damage equal to <style=cIsHealing>{100f * VeliaerisStaticValues.crushDamageCoefficent}% of your max health and inflicting a stack of abyss healing for <style=cIsHealing>{100f*VeliaerisStaticValues.crushHealingCoefficent}% of your max health</style> per stack on the target enemy</style>.");
            Language.Add(prefix + "ERIS_SECONDARY_NAME", "Gaze of Azelythtsa");
            Language.Add(prefix + "ERIS_SECONDARY_NAME_CORRUPT", "Ga??ze o??f Az??ely??tht??sa");
            Language.Add(prefix + "ERIS_SECONDARY_DESCRIPTION", $"Buff yourself and your allies and inflict cripple on nearby enemies for 8 seconds.\n<color=#8f038c>During this time period the void can not affect you.</color>");
            Language.Add(prefix + "VELIA_SECONDARY_NAME", "Slash of The Abyss");
            Language.Add(prefix + "VELIA_SECONDARY_NAME_CORRUPT", "Sl??ash o??f The Aby??ss");
            Language.Add(prefix + "VELIA_SECONDARY_DESCRIPTION", $"Slash around you dealing <style=cIsDamage>{100f *VeliaerisStaticValues.scytheDamageCoefficient}% damage</style> plus extra percent health damage. Inflict two stacks of Abyss to each Enemy hit.");
            #endregion

            #region DualUtility
            Language.Add(prefix + "SPLIT_UTILITY_NAME", "Split");
            Language.Add(prefix + "SPLIT_UTILITY_NAME_CORRUPT", "S??und??er");
            Language.Add(prefix + "SPLIT_UTILITY_DESCRIPTION_SPLIT", Tokens.agilePrefix + $"Split into two personas changing into either <color=#0787f0>Eris</color> or <color=#fc0000>Velia</color> based on who you were previously.");
            Language.Add(prefix + "MERGE_UTILITY_NAME", "Merge Or Switch");
            Language.Add(prefix + "MERGE_UTILITY_NAME_CORRUPT", "Co??nver??ge O??r Mo??r??ph");
            Language.Add(prefix + "MERGE_UTILITY_DESCRIPTION", Tokens.agilePrefix + "Press to merge back into Veliaeris or hold to switch to the other Persona.");
            #endregion

            #region StateSpecials
            Language.Add(prefix + "VELIAERIS_SPECIAL_NAME", "Void Annihilation");
            Language.Add(prefix + "VELIAERIS_SPECIAL_NAME_CORRUPT", "Vo??id Anni??hilati??on");
            Language.Add(prefix + "VELIAERIS_SPECIAL_DESCRIPTION", Tokens.agilePrefix + $"Detonate all stacks of Abyss on enemies at infinite range dealing <style=cIsDamage>{100f*VoidDetonator.baseDamageCoefficient}% damage</style> plus <style=cIsDamage>{100f*VoidDetonator.damageCoefficientPerStack}% damage</style> per stack of abyss. Enemies below 5% hp are executed.");
            Language.Add(prefix + "ERIS_SPECIAL_NAME", "Refresh Fortitude");
            Language.Add(prefix + "ERIS_SPECIAL_NAME_CORRUPT", "Re??set Exsi??ste??nce");
            Language.Add(prefix + "ERIS_SPECIAL_DESCRIPTION", $"<style=cIsHealing>Heal yourself and your Allies for {100f*VeliaerisStaticValues.healCoefficent}% of you max hp</style>. Also cleanse all debuffs and current damage over time effects for yourself and Allies.");
            Language.Add(prefix + "VELIA_SPECIAL_NAME", "Awaken the Endless Abyss");
            Language.Add(prefix + "VELIA_SPECIAL_NAME_CORRUPT", "Awa??ke??n th??e Infi??ni??te Aby??ss");
            Language.Add(prefix + "VELIA_SPECIAL_DESCRIPTION", Tokens.agilePrefix + $"Buff yourself with the power of the Endless Abyss. Granting you great strength.\n<color=#8f038c>During this time period the void can not affect you.</color>");
            Language.Add(prefix + "VELIA_SECONDARY_SPECIAL_NAME", "Call Upon Azelythtsa");
            Language.Add(prefix + "VELIA_SECONDARY_SPECIAL_NAME_CORRUPT", "Th?e ??V?oi?ds R??espo??nse");
            Language.Add(prefix + "VELIA_SECONDARY_SPECIAL_DESCRIPTION", $"Consumes 35%-95% hp depending on the amount of charges remaining and deals {100f*VeliaerisStaticValues.callDamageCoefficent} of the hp lost upon skill usage to enemies");
            #endregion

            #region extraInformation
            Language.Add(prefix + "ABYSS_INFORMATION", "<style=cKeywordName>Abyss</style>Deals 1 damage per stack for 10 seconds. Each added stack resets the duration.");
            Language.Add(prefix + "PERCENT_HEALTH_DAMAGE", $"<style=cKeywordName>Percent Health Damage</style>Deals damage based on the enemies health rather than a damage multiplier. For Regular enemies it is {100f*0.25f}% of their max health. For bosses it is {100f*0.05f}% of their max health.");
            Language.Add(prefix + "SKILL_INFORMATION", "<style=cKeywordName>Stagnat Skills</style>All Skills are Persona specfic the selection of skills is display only and will not affect what skills the survivor will use in what form. Their purpose is to give a view into what each form can do.");
            Language.Add(prefix + "SWITCH_INFORMATION", "<style=cKeywordName>Veliaeris Split Effects</style>Spliting to Eris will cause allies to gain a Blessing of Health while Eris gains a stack of Death Prevention.\nSplitting to Velia will cause allies to gain a Blessing of Damage while Velia gains a stack of Void Corruption.\n<style=cKeywordName>Split Buffs</style>Blessing of Health:\n<style=cIsHealing>Gives allies bonus health equal to 5% of their own max health</style> per stack of the blessing.\nBlessing of Damage:\n<style=cIsDamage>Gives allies 1% bonus damage</style> per stack of the blessing.\nDeath Protection:\nEach stack acts a healing potion for all allies which heals when an ally reaches 15% of their max health, they <style=cIsHealing>heal for 70% of your max health.</style>\nVoid Corruption:\nThe first stack gives <style=cIsDamage>80% damage</style> each stack after increasing the damage by 1 per every 2 stacks you have.");
            Language.Add(prefix + "MERGE_INFORMATION", "<style=cKeywordName>Merge and Switch Effects</style>Switching to Eris from Velia will give all allies a shield equal to <style=cIsHealing>50% of your current health.</style>\nSwitching to Velia from Eris will cause enemies to be thrown away and dealt damage equal to <style=cIsDamage>25% of their max hp.</style></style>\nMerging back together will cause nearby enemies to be inflicted with a permanent curse.");
            #endregion

            //#region Achievements
            //Language.Add(Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier), "Henry: Mastery");
            //Language.Add(Tokens.GetAchievementDescriptionToken(HenryMasteryAchievement.identifier), "As Henry, beat the game or obliterate on Monsoon.");
            //#endregion
        }
    }
}
