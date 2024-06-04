using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2.ConVar;
using UnityEngine;
using RoR2.CharacterSpeech;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    public class SpeechDriver : MonoBehaviour
    {
        
        public void enacteDialogue(String activeEvent)
        {
            String colorCodeEris = "<color=#026bf5>";
            String colorCodeVelia = "<color=#c40003>";
            String colorCodeVeliaeris = "<color=#a60fba>";
            String finalColorCode;

            switch (VeliaerisPlugin.VeliaerisStates)
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
            String voice = finalColorCode+VeliaerisPlugin.VeliaerisStates.ToString()+": voice check for colors and names</color>";

            Chat.AddMessage(voice);
           
        }

    }
}
