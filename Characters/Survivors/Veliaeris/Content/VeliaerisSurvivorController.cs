using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(InputBankTest))]
    [RequireComponent(typeof(TeamComponent))]
    class VeliaerisSurvivorController : NetworkBehaviour
    {
        public CharacterBody characterBody;
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
        #region VeliaerisStates
        public VeliaerisState VeliaerisStates
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


        //private HealthComponent bodyHealthComponent
        //{
        //    get
        //    {
        //        return this.characterBody.healthComponent;
        //    }
        //}


    }
}
