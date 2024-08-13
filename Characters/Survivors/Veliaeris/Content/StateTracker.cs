using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    class StateTracker: MonoBehaviour
    {
        public struct storedStates
        {
            public bool firstChangesave;
            public VeliaerisState pausedState;
            public float ReviveTimerRemaining;
            public List<VeliaerisState> hereticOverridesPrimarysave;
            public List<VeliaerisState> hereticOverridesSecondarysave;
            public List<VeliaerisState> hereticOverridesUtilitysave;
            public List<VeliaerisState> hereticOverridesSpecialsave;
            public int gatheredPrimarysave;
            public int gatheredSecondarysave;
            public int gatheredUtilitysave;
            public int gatheredSpecialsave;
            public VeliaerisState previousStatesave;
            public bool isCorruptedSave;
        };
        public bool firstChangestored;
        public VeliaerisState storedState;
        public float storedReviveTimer;
        public List<VeliaerisState> hereticOverridesPrimarystored;
        public List<VeliaerisState> hereticOverridesSecondarystored;
        public List<VeliaerisState> hereticOverridesUtilitystored;
        public List<VeliaerisState> hereticOverridesSpecialstored;
        public int gatheredPrimarystored;
        public int gatheredSecondarystored;
        public int gatheredUtilitystored;
        public int gatheredSpecialstored;
        public VeliaerisState previousStatestored;
        public bool isCorruptedStore;

        public bool isstoringData;
        public void storeState(bool isFirstChangeNetwork, VeliaerisState state, float currentTimer, List<VeliaerisState> HOP, List<VeliaerisState> HOS, List<VeliaerisState> HOU, List<VeliaerisState> HOSp,int GPS, int GSS, int GUS, int GSpS,VeliaerisState prevState,bool isCorrupted)
        {
            this.firstChangestored = isFirstChangeNetwork;
            this.isstoringData = true;
            this.storedState = state;
            this.storedReviveTimer = currentTimer;
            this.hereticOverridesPrimarystored = HOP;
            this.hereticOverridesSecondarystored=HOS;
            this.hereticOverridesUtilitystored =HOU;
            this.hereticOverridesSpecialstored = HOSp;
            this.gatheredPrimarystored = GPS;
            this.gatheredSecondarystored = GSS;
            this.gatheredUtilitystored = GUS;
            this.gatheredSpecialstored = GSpS;
            this.previousStatestored = prevState;
            this.isCorruptedStore = isCorrupted;
        }

        public storedStates retrieveStateData()
        {
            this.isstoringData = false;
            return new storedStates
            {
                firstChangesave = firstChangestored,
                pausedState = storedState,
                ReviveTimerRemaining = storedReviveTimer,
                hereticOverridesPrimarysave = hereticOverridesPrimarystored,
                hereticOverridesSecondarysave = hereticOverridesSecondarystored,
                hereticOverridesUtilitysave = hereticOverridesUtilitystored,
                hereticOverridesSpecialsave = hereticOverridesSpecialstored,
                gatheredPrimarysave = gatheredPrimarystored,
                gatheredSecondarysave = gatheredSecondarystored,
                gatheredUtilitysave = gatheredUtilitystored,
                gatheredSpecialsave = gatheredSpecialstored,
                previousStatesave = previousStatestored,
                isCorruptedSave = isCorruptedStore
            };
        }
     }
}
