using R2API.Networking.Interfaces;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    internal class SyncStates: INetMessage
    {
        private NetworkInstanceId netId;
        private bool firstChange;
        private VeliaerisState State;
        private float VeliaerisStates;
        private List<VeliaerisState> hereticOverridesPrimary;
        private List<VeliaerisState> hereticOverridesSecondary;
        private List<VeliaerisState> hereticOverridesUtility;
        private List<VeliaerisState> hereticOverridesSpecial;
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
        private bool isCorrupted;
        private VeliaerisState previousState;
        public SyncStates()
        {

        }
        public SyncStates(NetworkInstanceId netId, bool firstChange, VeliaerisState State, float VeliaerisStates, int gatheredPrimary, int gatheredSecondary, int gatheredUtility, int gatheredSpecial, VeliaerisState previousState, List<VeliaerisState> hereticOverridesPrimary, List<VeliaerisState> hereticOverridesSecondary, List<VeliaerisState> hereticOverridesUtility, List<VeliaerisState> hereticOverridesSpecial, bool isCorrupted)
        {
            this.netId = netId;
            this.firstChange = firstChange;
            this.State = State;
            this.VeliaerisStates = VeliaerisStates;
            this.gatheredPrimary = gatheredPrimary;
            this.gatheredSecondary = gatheredSecondary;
            this.gatheredUtility = gatheredUtility;
            this.gatheredSpecial = gatheredSpecial;
            this.previousState = previousState;
            //can not use lists for network writing
            this.hereticOverridesPrimaryVeliaeris = hereticOverridesPrimary.Contains(VeliaerisState.Eris);
            this.hereticOverridesSecondaryVeliaeris = hereticOverridesSecondary.Contains(VeliaerisState.Eris);
            this.hereticOverridesUtilityVeliaeris = hereticOverridesUtility.Contains(VeliaerisState.Eris);
            this.hereticOverridesSpecialVeliaeris = hereticOverridesSpecial.Contains(VeliaerisState.Eris);
            this.hereticOverridesPrimaryEris = hereticOverridesPrimary.Contains(VeliaerisState.Eris);
            this.hereticOverridesSecondaryEris = hereticOverridesSecondary.Contains(VeliaerisState.Eris);
            this.hereticOverridesUtilityEris = hereticOverridesUtility.Contains(VeliaerisState.Eris);
            this.hereticOverridesSpecialEris = hereticOverridesSpecial.Contains(VeliaerisState.Eris);
            this.hereticOverridesPrimaryVelia = hereticOverridesPrimary.Contains(VeliaerisState.Eris);
            this.hereticOverridesSecondaryVelia = hereticOverridesSecondary.Contains(VeliaerisState.Eris);
            this.hereticOverridesUtilityVelia = hereticOverridesUtility.Contains(VeliaerisState.Eris);
            this.hereticOverridesSpecialVelia = hereticOverridesSpecial.Contains(VeliaerisState.Eris);
            this.isCorrupted = isCorrupted;
            Debug.Log("SyncStateDeseralize");
        }

        public void Deserialize(NetworkReader reader)
        {
            this.netId = reader.ReadNetworkId();
            this.State = (VeliaerisState)reader.ReadByte();
            this.firstChange = reader.ReadBoolean();
            this.VeliaerisStates = reader.ReadSingle();
            this.gatheredPrimary = reader.ReadInt32();
            this.gatheredSecondary = reader.ReadInt32();
            this.gatheredUtility = reader.ReadInt32();
            this.gatheredSpecial = reader.ReadInt32();
            this.previousState = (VeliaerisState)reader.ReadByte();
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
            //convert bool "lists" back into proper lists
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
        }

        public void OnReceived()
        {
            Debug.Log("SyncStateOnRecived");
            GameObject bodyObject = Util.FindNetworkObject(this.netId);
            if (!bodyObject) return;

            VeliaerisSurvivorController velController = bodyObject.GetComponent<VeliaerisSurvivorController>();
            if (velController)
            {

                velController.setCharacterStates(this.firstChange, this.State, this.gatheredPrimary, this.gatheredSecondary, this.gatheredUtility, this.gatheredSpecial, this.previousState, this.VeliaerisStates, this.hereticOverridesPrimary, this.hereticOverridesSecondary, this.hereticOverridesUtility, this.hereticOverridesSpecial,this.isCorrupted);
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            Debug.Log("SyncStateSeralize");
            writer.Write(this.netId);
            writer.Write(this.firstChange);
            writer.Write((byte)State);
            writer.Write(this.VeliaerisStates);
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
            writer.Write(isCorrupted);


        }
    }
}
