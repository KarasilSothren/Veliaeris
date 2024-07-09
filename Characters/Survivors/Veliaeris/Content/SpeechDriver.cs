using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2.ConVar;
using UnityEngine;
using RoR2.CharacterSpeech;
using RoR2.UI;
using VeliaerisMod.Survivors.Veliaeris;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    public class SpeechDriver : MonoBehaviour
    {
        public static AssignStageToken tokenName;
        private static String voiceText;
        private static String displayedName;
        private VeliaerisSurvivorController VeliaerisSurvivorController;

        public void enacteDialogue(String activeEvent,CharacterBody body)
        {
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
            displayedName = tokenName.titleText.text;
            String colorCodeEris = "<color=#026bf5>";
            String colorCodeVelia = "<color=#c40003>";
            String colorCodeVeliaeris = "<color=#a60fba>";
            String finalColorCode;
            switch (VeliaerisSurvivorController.VeliaerisStates)
            {
                case VeliaerisState.Velia:
                    finalColorCode = colorCodeVelia;
                    break;
                case VeliaerisState.Eris:
                    finalColorCode = colorCodeEris;
                    break;
                case VeliaerisState.Veliaeris:
                    finalColorCode = colorCodeVeliaeris;
                    break;
                default:
                    finalColorCode = colorCodeVeliaeris;
                    break;
            }
            switch (activeEvent)
            {
                case "stage":
                    stageDialogue(body);
                    break;
                default:
                    break;
            }
            String voice = "<size=120%>" + finalColorCode +VeliaerisSurvivorController.VeliaerisStates.ToString()+": "+voiceText+"</size></color>";

            Chat.AddMessage(voice);
           
        }
        /*
         * Verdent Fields: [Info   :   Console] Stage Name: lakes
           void fields: [Info   :   Console] Stage Name: arena
            aph sanct: [Info   :   Console] Stage Name: ancientloft
            balwark: [Info   :   Console] Stage Name: artifactworld
            bazaar[Info   :   Console] Stage Name: bazaar
            siphoned: [Info   :   Console] Stage Name: snowyforest
            gilded: [Info   :   Console] Stage Name: goldshores
            a moment whole: [Info   :   Console] Stage Name: limbo
            a moment fractured: [Info   :   Console] Stage Name: mysteryspace
            titan plains: [Info   :   Console] Stage Name: golemplains
            titan plains2: [Info   :   Console] Stage Name: golemplains2
            roost: [Info   :   Console] Stage Name: blackbeach
            roost2: [Info   :   Console] Stage Name: blackbeach2
            abyssal: [Info   :   Console] Stage Name: dampcavesimple
            wetland: [Info   :   Console] Stage Name: foggyswamp
            rally: [Info   :   Console] Stage Name: frozenwall
            abandoned[Info   :   Console] Stage Name: goolake
            comencement: moon2(moon is first version)
            sundred: [Info   :   Console] Stage Name: rootjungle
            sirens: [Info   :   Console] Stage Name: shipgraveyard
            sky meadow:[Info   :   Console] Stage Name: skymeadow
            sulfur: [Info   :   Console] Stage Name: sulfurpools
            scorched: [Info   :   Console] Stage Name: wispgraveyard
            planetarium: [Info   :   Console] Stage Name: voidraid
            void locus: [Info   :   Console] Stage Name: voidstage
            procedual stages: random
            fogbound: [Info   :   Console] Internal Stage Name: FBLScene
            catacombs: [Info   :   Console] Internal Stage Name: catacombs_DS1_Catacombs
            drybasin: [Info   :   Console] Internal Stage Name: drybasin
            slumbering: [Info   :   Console] Internal Stage Name: slumberingsatellite
            mario bomb fields: [Info   :   Console] Internal Stage Name: sm64_bbf_SM64_BBF
            desolation reef: voidstage
            alt momentwhole: [Info   :   Console] Internal Stage Name: BulwarksHaunt_GhostWave
        */
        private void stageDialogue(CharacterBody body)
        {
            VeliaerisSurvivorController = body.GetComponent<VeliaerisSurvivorController>();
            String variant;
            switch (VeliaerisSurvivor.StageIdentity)
            {
                case "lakes":
                    if (VeliaerisSurvivor.TrueStageName != displayedName)
                    {
                        variant = displayedName.Replace(VeliaerisSurvivor.TrueStageName,"");
                        variant.Replace("(", "");
                        variant.Replace(")", "");
                    }
                    else
                    {
                        voiceText = "lakes";
                    }
                    break;
                case "arena":
                    voiceText = "arena";
                    break;
                case "ancientloft":
                    voiceText = "loft";
                    break;
                case "artifactworld":
                    break;
                case "bazaar":
                    break;
                case "snowyforest":
                    voiceText = "forest";
                    break;
                case "goldshores":
                    break;
                case "limbo":
                    break;
                case "mysteryspace":
                    break;
                case "golemplains":
                case "golemplains2":
                    voiceText = "golems";
                    break;
                case "blackbeach":
                case "blackbeach2":
                    switch (VeliaerisSurvivorController.VeliaerisStates) 
                    {
                        case VeliaerisState.Veliaeris:
                            voiceText = "This beach feels rather distant from the world";
                            break;
                        case VeliaerisState.Eris:
                            voiceText = "This places color is rather calming";
                            break;
                        case VeliaerisState.Velia:
                            voiceText = "I wonder if this Saria would like this place";
                            break;
                    }
                    break;
                case "dampcavesimple":
                    break;
                case "foggyswamp":
                    break;
                case "frozenwall":
                    break;
                case "moon":
                case "moon2":
                    break;
                case "rootjungle":
                    break;
                case "shipgraveyard":
                    break;
                case "skymeadow":
                    break;
                case "sulfurpools":
                    break;
                case "wispgraveyard":
                    break;
                case "voidraid":
                    break;
                case "voidstage":
                    break;
                case "FBLScene":
                    break;
                case "catacombs_DS1_Catacombs":
                    break;
                case "drybasin":
                    break;
                case "slumberingsatellite":
                    break;
                case "sm64_bbf_SM64_BBF":
                    break;
                case "BulwarksHaunt_GhostWave":
                    break;
            }
        }

    }
}
