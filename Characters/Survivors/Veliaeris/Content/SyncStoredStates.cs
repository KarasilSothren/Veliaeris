using System;
using System.Collections.Generic;
using System.Text;
using R2API.Networking.Interfaces;
using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using RoR2.Networking;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    internal class SyncStoredStates: INetMessage
    {
        private NetworkInstanceId netId;
        private bool firstChange;
        private VeliaerisState State;
        private List<VeliaerisState> hereticOverridesPrimary = new List<VeliaerisState>();
        private List<VeliaerisState> hereticOverridesSecondary = new List<VeliaerisState>();
        private List<VeliaerisState> hereticOverridesUtility = new List<VeliaerisState>();
        private List<VeliaerisState> hereticOverridesSpecial = new List<VeliaerisState>();
        private bool hereticOverridesPrimaryVeliaeris;
        private bool hereticOverridesPrimaryEris;
        private bool hereticOverridesPrimaryVelia;
        private bool hereticOverridesSecondaryVeliaeris;
        private bool hereticOverridesSecondaryEris;
        private bool hereticOverridesSecondaryVelia;
        private bool hereticOverridesUtilityVeliaeris;
        private bool hereticOverridesUtilityEris;
        private bool hereticOverridesUtilityVelia;
        private bool hereticOverridesSpecialVeliaeris;
        private bool hereticOverridesSpecialEris;
        private bool hereticOverridesSpecialVelia;
        private int gatheredPrimary;
        private int gatheredSecondary;
        private int gatheredUtility;
        private int gatheredSpecial;
        private VeliaerisState previousState;
        private float networkedReviveTimer;
        private bool isCorrupted;
        public SyncStoredStates()
        {

        }
        public SyncStoredStates(NetworkInstanceId netId, bool firstChange, VeliaerisState State, int gatheredPrimary, int gatheredSecondary, int gatheredUtility, int gatheredSpecial, VeliaerisState previousState, float networkedReviveTimer, List<VeliaerisState> hereticOverridesPrimary, List<VeliaerisState> hereticOverridesSecondary, List<VeliaerisState> hereticOverridesUtility, List<VeliaerisState> hereticOverridesSpecial, bool isCorrupted)
        {
            this.netId = netId;
            this.firstChange = firstChange;
            this.State = State;
            this.gatheredPrimary = gatheredPrimary;
            this.gatheredSecondary = gatheredSecondary;
            this.gatheredUtility = gatheredUtility;
            this.gatheredSpecial = gatheredSpecial;
            this.previousState = previousState;
            this.networkedReviveTimer = networkedReviveTimer;
            //can not use lists for network writing
            this.hereticOverridesPrimaryVeliaeris = hereticOverridesPrimary.Contains(VeliaerisState.Veliaeris);
            this.hereticOverridesSecondaryVeliaeris = hereticOverridesSecondary.Contains(VeliaerisState.Veliaeris);
            this.hereticOverridesUtilityVeliaeris = hereticOverridesUtility.Contains(VeliaerisState.Veliaeris);
            this.hereticOverridesSpecialVeliaeris = hereticOverridesSpecial.Contains(VeliaerisState.Veliaeris);
            this.hereticOverridesPrimaryEris = hereticOverridesPrimary.Contains(VeliaerisState.Eris);
            this.hereticOverridesSecondaryEris = hereticOverridesSecondary.Contains(VeliaerisState.Eris);
            this.hereticOverridesUtilityEris = hereticOverridesUtility.Contains(VeliaerisState.Eris);
            this.hereticOverridesSpecialEris = hereticOverridesSpecial.Contains(VeliaerisState.Eris);
            this.hereticOverridesPrimaryVelia = hereticOverridesPrimary.Contains(VeliaerisState.Velia);
            this.hereticOverridesSecondaryVelia = hereticOverridesSecondary.Contains(VeliaerisState.Velia);
            this.hereticOverridesUtilityVelia = hereticOverridesUtility.Contains(VeliaerisState.Velia);
            this.hereticOverridesSpecialVelia = hereticOverridesSpecial.Contains(VeliaerisState.Velia);
            this.isCorrupted = isCorrupted;
        }

        public void Deserialize(NetworkReader reader)
        {
            Debug.Log("SyncStoredStateDeseralize");
            this.netId = reader.ReadNetworkId();
            this.State = (VeliaerisState)reader.ReadByte();
            this.firstChange = reader.ReadBoolean();
            this.gatheredPrimary = reader.ReadInt32();
            this.gatheredSecondary = reader.ReadInt32();
            this.gatheredUtility = reader.ReadInt32();
            this.gatheredSpecial = reader.ReadInt32();
            this.previousState = (VeliaerisState)reader.ReadByte();
            this.networkedReviveTimer = reader.ReadSingle();
            this.hereticOverridesPrimaryVeliaeris = reader.ReadBoolean();
            this.hereticOverridesSecondaryVeliaeris = reader.ReadBoolean();
            this.hereticOverridesUtilityVeliaeris = reader.ReadBoolean();
            this.hereticOverridesSpecialVeliaeris = reader.ReadBoolean();
            this.hereticOverridesPrimaryEris = reader.ReadBoolean();
            this.hereticOverridesSecondaryEris = reader.ReadBoolean();
            this.hereticOverridesUtilityEris = reader.ReadBoolean();
            this.hereticOverridesSpecialEris = reader.ReadBoolean();
            this.hereticOverridesPrimaryVelia = reader.ReadBoolean();
            this.hereticOverridesSecondaryVelia = reader.ReadBoolean();
            this.hereticOverridesUtilityVelia = reader.ReadBoolean();
            this.hereticOverridesSpecialVelia = reader.ReadBoolean();
            this.isCorrupted = reader.ReadBoolean();
        }

        public void OnReceived()
        {
            #region primaryif
            if (hereticOverridesPrimaryVeliaeris)
            {
                hereticOverridesPrimary.Add(VeliaerisState.Veliaeris);
            }
            if (hereticOverridesPrimaryEris)
            {
                hereticOverridesPrimary.Add(VeliaerisState.Eris);
            }
            if (hereticOverridesPrimaryVelia)
            {
                hereticOverridesPrimary.Add(VeliaerisState.Velia);
            }
            #endregion
            #region secondaryif
            if (hereticOverridesSecondaryVeliaeris)
            {
                hereticOverridesSecondary.Add(VeliaerisState.Veliaeris);
            }
            if (hereticOverridesSecondaryEris)
            {
                hereticOverridesSecondary.Add(VeliaerisState.Eris);
            }
            if (hereticOverridesSecondaryVelia)
            {
                hereticOverridesSecondary.Add(VeliaerisState.Velia);
            }
            #endregion
            #region utilityif
            if (hereticOverridesUtilityVeliaeris)
            {
                hereticOverridesUtility.Add(VeliaerisState.Veliaeris);
            }
            if (hereticOverridesUtilityEris)
            {
                hereticOverridesUtility.Add(VeliaerisState.Eris);
            }
            if (hereticOverridesUtilityVelia)
            {
                hereticOverridesUtility.Add(VeliaerisState.Velia);
            }
            #endregion
            #region specialif
            if (hereticOverridesSpecialVeliaeris)
            {
                hereticOverridesSpecial.Add(VeliaerisState.Veliaeris);
            }
            if (hereticOverridesSpecialEris)
            {
                hereticOverridesSpecial.Add(VeliaerisState.Eris);
            }
            if (hereticOverridesSpecialVelia)
            {
                hereticOverridesSpecial.Add(VeliaerisState.Velia);
            }
            #endregion
            Debug.Log("secon count:" + this.hereticOverridesSecondary.Count);
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            VeliaerisSurvivorController velController = bodyObject.GetComponent<VeliaerisSurvivorController>();
            if (velController)
            {

                velController.setCharacterStates(this.firstChange,this.State,this.gatheredPrimary,this.gatheredSecondary,this.gatheredUtility,this.gatheredSpecial,this.previousState,this.networkedReviveTimer,this.hereticOverridesPrimary,this.hereticOverridesSecondary,this.hereticOverridesUtility,this.hereticOverridesSpecial,this.isCorrupted);
            }
        }

        public void Serialize(NetworkWriter writer)
        {

            writer.Write(this.netId);
            writer.Write(this.firstChange);
            writer.Write((byte)State);
            writer.Write(this.hereticOverridesPrimaryVeliaeris);
            writer.Write(this.hereticOverridesPrimaryEris);
            writer.Write(this.hereticOverridesPrimaryVelia);
            writer.Write(this.hereticOverridesSecondaryVeliaeris);
            writer.Write(this.hereticOverridesSecondaryEris);
            writer.Write(this.hereticOverridesSecondaryVelia);
            writer.Write(this.hereticOverridesUtilityVeliaeris);
            writer.Write(this.hereticOverridesUtilityEris);
            writer.Write(this.hereticOverridesUtilityVelia);
            writer.Write(this.hereticOverridesSpecialVeliaeris);
            writer.Write(this.hereticOverridesSpecialEris);
            writer.Write(this.hereticOverridesSpecialVelia);
            writer.Write(gatheredPrimary);
            writer.Write(gatheredSecondary);
            writer.Write(gatheredUtility);
            writer.Write(gatheredSpecial);
            writer.Write((byte)previousState);
            writer.Write(networkedReviveTimer);
            writer.Write(isCorrupted);


        }

        
    }
}

