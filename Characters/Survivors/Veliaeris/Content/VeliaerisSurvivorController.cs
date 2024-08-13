using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using VeliaerisMod.Survivors.Veliaeris;
using VeliaerisMod.Survivors.Veliaeris.SkillStates;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    //[RequireComponent(typeof(CharacterBody))]
    //[RequireComponent(typeof(InputBankTest))]
    //[RequireComponent(typeof(TeamComponent))]
    class VeliaerisSurvivorController : MonoBehaviour
    {
        public CharacterBody characterBody;
        private VeliaerisSurvivorController veliaerisSurvivorController;
        private VeliaerisState _state;
        private VeliaerisState _inital;
        private VeliaerisState _networkedVelState;
        public List<VeliaerisState> hereticOverridesPrimary = new List<VeliaerisState>();
        public List<VeliaerisState> hereticOverridesSecondary = new List<VeliaerisState>();
        public List<VeliaerisState> hereticOverridesUtility = new List<VeliaerisState>();
        public List<VeliaerisState> hereticOverridesSpecial = new List<VeliaerisState>();
        private VeliaerisState _paststate;
        private int _gatheredPrimary;
        private bool _firstChange = true;
        private int _gatheredSecondary;
        private int _gatheredUtility;
        private int _gatheredSpecial;
        private static VeliaerisState _previousState;
        private float _networkedReviveTimer = 0f;
        private bool corrupted = false;
        #region corrupted
        public bool isCorrupted
        {
            get
            {
                return this.corrupted;
            }
            [param:In]
            set
            {
                this.corrupted = value;
            }
        }
        #endregion
        #region dataSetup
        #region initalState
        public VeliaerisState initalState
        {
            get
            {
                return this._inital;
            }
        }

        public VeliaerisState network_initial
        {
            get
            {
                return this._inital;
            }
            [param: In]
            set
            {
                this._inital = value;
            }
        }
        #endregion
        #region VeliaerisState
        public VeliaerisState VeliaerisState
        {
            get
            {
                return this._state;
            }
        }

        public VeliaerisState network_veliaerisStates
        {
            get
            {
                return this._state;
            }
            [param: In]
            set
            {
                this._state = value;
            }
        }
        #endregion
        #region velState
        public VeliaerisState velState
        {
            get
            {
                return this._networkedVelState;
            }
        }

        public VeliaerisState network_velState
        {
            get
            {
                return this._networkedVelState;
            }
            [param: In]
            set
            {
                this._networkedVelState = value;
            }
        }

        #endregion
        #region crossStageReviveTimer
        public float ReviveDisabledTimer
        {
            get
            {
                return this._networkedReviveTimer;
            }
        }
        public float setNetworkReviveTimer
        {
            get
            {
                return this._networkedReviveTimer;
            }
            [param: In]
            set
            {
                this._networkedReviveTimer = value;
            }
        }
        #endregion
        #region paststate
        public VeliaerisState paststate
        {
            get
            {
                return this._paststate;
            }
        }

        public VeliaerisState network_paststate
        {
            get
            {
                return this._paststate;
            }
            [param: In]
            set
            {
                this._paststate = value;
            }
        }
        #endregion
        #region gatheredPrimary
        public int gatheredPrimary
        {
            get
            {
                return this._gatheredPrimary;
            }
        }

        public int network_gatheredPrimary
        {
            get
            {
                return this._gatheredPrimary;
            }
            [param: In]
            set
            {
                this._gatheredPrimary = value;
            }
        }
        #endregion
        #region gatheredSecondary
        public int gatheredSecondary
        {
            get
            {
                return this._gatheredSecondary;
            }
        }

        public int network_gatheredSecondary
        {
            get
            {
                return this._gatheredSecondary;
            }
            [param: In]
            set
            {
                this._gatheredSecondary = value;
            }
        }
        #endregion
        #region gatheredUtility
        public int gatheredUtility
        {
            get
            {
                return this._gatheredUtility;
            }
        }

        public int network_gatheredUtility
        {
            get
            {
                return this._gatheredUtility;
            }
            [param: In]
            set
            {
                this._gatheredUtility = value;
            }
        }
        #endregion
        #region gatheredSpecial
        public int gatheredSpecial
        {
            get
            {
                return this._gatheredSpecial;
            }
        }

        public int network_gatheredSpecial
        {
            get
            {
                return this._gatheredSpecial;
            }
            [param: In]
            set
            {
                this._gatheredSpecial = value;
            }
        }
        #endregion
        #region firstChange
        public bool firstChange
        {
            get
            {
                return this._firstChange;
            }
        }

        public bool network_firstchange
        {
            get
            {
                return this._firstChange;
            }
            [param: In]
            set
            {
                this._firstChange = value;
            }
        }
        #endregion
        #region previoussplitstate
        public VeliaerisState previousSplitSate
        {
            get
            {
                return _previousState;
            }
        }
        public VeliaerisState network_previousState
        {
            get
            {
                return _previousState;
            }
            [param: In]
            set
            {
                _previousState = value;
            }
        }
        #endregion
        #endregion
        private StateTracker dataTracker
        {
            get
            {
                if (this.characterBody && this.characterBody.master)
                {
                    StateTracker i = this.characterBody.master.GetComponent<StateTracker>();
                    if (!i) i = this.characterBody.master.gameObject.AddComponent<StateTracker>();
                    return i;
                }
                return null;
            }
        }

        private void Awake()
        {
            this.characterBody = this.GetComponent<CharacterBody>();
            ModelLocator modelLocator = this.GetComponent<ModelLocator>();
            this.Invoke(nameof(setStateHook),0f);
        }
        
        private void setStateHook()
        {
            this.CheckForStoredData();
            this.characterBody = this.GetComponent<CharacterBody>();
            SkillLocator skillLocator = characterBody.GetComponent<SkillLocator>();
            MergeandShift.SkillSwitch(skillLocator, false, characterBody,false,false);
            characterBody.SetBuffCount(VeliaerisBuffs.VeliaerisStatChanges.buffIndex, 0);
            characterBody.SetBuffCount(VeliaerisBuffs.ErisStatChanges.buffIndex, 0);
            characterBody.SetBuffCount(VeliaerisBuffs.VeliaStatChanges.buffIndex, 0);
            if (this.VeliaerisState == VeliaerisState.Veliaeris)
            {
                characterBody.SetBuffCount(VeliaerisBuffs.VeliaerisStatChanges.buffIndex, 1);
            }
            if (this.VeliaerisState == VeliaerisState.Eris)
            {
                characterBody.SetBuffCount(VeliaerisBuffs.ErisStatChanges.buffIndex, 1);
            }
            if (this.VeliaerisState == VeliaerisState.Velia)
            {
                characterBody.SetBuffCount(VeliaerisBuffs.VeliaStatChanges.buffIndex, 1);
            }
            VeliaerisSurvivor.stagePhasing = false;
        }

        private void OnDestroy()
        {
            Debug.Log("was destroyed");
            int hereticLimit = 0;
            bool doTransform = false;
            this.characterBody = this.GetComponent<CharacterBody>();
            veliaerisSurvivorController = characterBody.GetComponent<VeliaerisSurvivorController>();
            if (veliaerisSurvivorController.hereticOverridesPrimary.Contains(VeliaerisState.Veliaeris)) hereticLimit++;
            if (veliaerisSurvivorController.hereticOverridesSecondary.Contains(VeliaerisState.Veliaeris)) hereticLimit++;
            if (veliaerisSurvivorController.hereticOverridesUtility.Contains(VeliaerisState.Veliaeris)) hereticLimit++;
            if (veliaerisSurvivorController.hereticOverridesSpecial.Contains(VeliaerisState.Veliaeris)) hereticLimit++;
            Debug.Log("HereticCount:" + hereticLimit);
            if (hereticLimit > 3)
            {
                doTransform = true;
            }
            hereticLimit = 0;
            if (veliaerisSurvivorController.hereticOverridesPrimary.Contains(VeliaerisState.Eris)) hereticLimit++;
            if (veliaerisSurvivorController.hereticOverridesSecondary.Contains(VeliaerisState.Eris)) hereticLimit++;
            if (veliaerisSurvivorController.hereticOverridesUtility.Contains(VeliaerisState.Eris)) hereticLimit++;
            if (veliaerisSurvivorController.hereticOverridesSpecial.Contains(VeliaerisState.Eris)) hereticLimit++;
            if (hereticLimit > 3)
            {
                doTransform = true;
            }
            hereticLimit = 0;
            if (veliaerisSurvivorController.hereticOverridesPrimary.Contains(VeliaerisState.Velia)) hereticLimit++;
            if (veliaerisSurvivorController.hereticOverridesSecondary.Contains(VeliaerisState.Velia)) hereticLimit++;
            if (veliaerisSurvivorController.hereticOverridesUtility.Contains(VeliaerisState.Velia)) hereticLimit++;
            if (veliaerisSurvivorController.hereticOverridesSpecial.Contains(VeliaerisState.Velia)) hereticLimit++;
            if (hereticLimit > 3)
            {
                doTransform = true;
            }
            hereticLimit = 0;
            Debug.Log("Verify");
            if (!doTransform&&VeliaerisSurvivor.hereticPrevention)
            {
                Debug.Log("would transform");
                characterBody.master.TransformBody("VeliaerisBody");
            }
            VeliaerisSurvivor.hereticPrevention = false;
            if (NetworkServer.active)
            {
                
                this.storeCurrentState();
            }
        }

        private void storeCurrentState()
        {
            Debug.Log("Entered store");
            this.dataTracker?.storeState(firstChange,this._state, this.ReviveDisabledTimer, this.hereticOverridesPrimary, this.hereticOverridesSecondary, this.hereticOverridesUtility, hereticOverridesSpecial, this.gatheredPrimary, this.gatheredSecondary, this.gatheredUtility, this.gatheredSpecial, this.previousSplitSate,this.isCorrupted);
        }

        private void CheckForStoredData()
        {
            if (NetworkServer.active)
            {
                StateTracker dataTracker = this.dataTracker;
                if (dataTracker?.isstoringData == true)
                {
                    StateTracker.storedStates storedstate = dataTracker.retrieveStateData();
                    this.serverGetStoredData(storedstate.firstChangesave,storedstate.pausedState,storedstate.gatheredPrimarysave,storedstate.gatheredSecondarysave,storedstate.gatheredUtilitysave,storedstate.gatheredSpecialsave,storedstate.previousStatesave,storedstate.ReviveTimerRemaining,storedstate.hereticOverridesPrimarysave,storedstate.hereticOverridesSecondarysave,storedstate.hereticOverridesUtilitysave,storedstate.hereticOverridesSpecialsave,storedstate.isCorruptedSave);
                }
            }
        }
        public void serverSyncData(VeliaerisSurvivorController veliaerisSurvivorController, bool firstChange, VeliaerisState state, int gatheredPrimary, int gatheredSecondary, int gatheredUtility, int gatheredSpecial, VeliaerisState previousSplitState, float networkedReviveTimer, List<VeliaerisState> HOP, List<VeliaerisState> HOS, List<VeliaerisState> HOU, List<VeliaerisState> HOSp,bool isCorrupted)
        {
            Debug.Log("Entered serversync");
            NetworkIdentity identity = veliaerisSurvivorController.GetComponent<NetworkIdentity>();
            if (!identity) return;

            new SyncStates(identity.netId, firstChange, state, networkedReviveTimer, gatheredPrimary, gatheredSecondary, gatheredUtility, gatheredSpecial, previousSplitState, HOP, HOS, HOU, HOSp,isCorrupted).Send(NetworkDestination.Clients);
        }
        private void serverGetStoredData(bool firstChange, VeliaerisState state, int GP, int GS,int GU, int GSp,VeliaerisState previousSplit, float VeliaerisStates,List<VeliaerisState>HOP,List<VeliaerisState>HOS,List<VeliaerisState>HOU,List<VeliaerisState>HOSp,bool isCorrupted)
        {
            NetworkIdentity identity = this.gameObject.GetComponent<NetworkIdentity>();
            if (!identity) return;
            new SyncStoredStates(identity.netId,firstChange,state,GP,GS,GU,GSp,previousSplit,VeliaerisStates,HOP,HOS,HOU,HOSp,isCorrupted).Send(NetworkDestination.Clients);
        }

        public void setCharacterStates(bool firstChange, VeliaerisState state, int gatheredPrimary,int gatheredSecondary, int gatheredUtility,int gatheredSpecial, VeliaerisState previousSplitState, float networkedReviveTimer, List<VeliaerisState> HOP, List<VeliaerisState> HOS, List<VeliaerisState> HOU, List<VeliaerisState> HOSp , bool isCorrupted)
        {
            this.network_firstchange = firstChange;
            this.network_veliaerisStates = state;
            this.network_gatheredPrimary = gatheredPrimary;
            this.network_gatheredSecondary = gatheredSecondary;
            this.network_gatheredUtility = gatheredUtility;
            this.network_gatheredSpecial = gatheredSpecial;
            this.network_previousState = previousSplitState;
            this.setNetworkReviveTimer = networkedReviveTimer;
            this.hereticOverridesPrimary = HOP;
            this.hereticOverridesSecondary = HOS;
            this.hereticOverridesUtility = HOU;
            this.hereticOverridesSpecial = HOSp;
            this.corrupted = isCorrupted;
        }
        //private HealthComponent bodyHealthComponent
        //{
        //    get
        //    {
        //        return this.characterBody.healthComponent;
        //    }
        //}


    }
}
