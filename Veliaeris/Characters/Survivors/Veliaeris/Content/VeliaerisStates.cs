using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2.Skills;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Content
{
    public class HeldState : MonoBehaviour
    {
        [SerializeField]

        public static VeliaerisState velState = VeliaerisState.Veliaeris;
        public static List<VeliaerisState> hereticOverridesPrimary = new List<VeliaerisState>();
        public static List<VeliaerisState> hereticOverridesSecondary = new List<VeliaerisState>();
        public static List<VeliaerisState> hereticOverridesUtility = new List<VeliaerisState>();
        public static List<VeliaerisState> hereticOverridesSpecial = new List<VeliaerisState>();
        public static int gatheredPrimary = 0;
        public static int gatheredSecondary = 0;
        public static int gatheredUtility = 0;
        public static int gatheredSpecial = 0;

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
                System.Console.WriteLine("Passive information: " + this.passiveSkillSlot);
                //                System.Console.WriteLine("Passive information skilldef: " + this.passiveSkillSlot.skillDef);
                if (this.passiveSkillSlot.skillDef == null)
                {
                    System.Console.WriteLine("is null");
                }
                else
                {
                    System.Console.WriteLine("is not null");
                }
                if(this.passiveSkillSlot.skillDef==this.VeliaerisStart)
                {
                    System.Console.Write("veliaerisstart");
                    return VeliaerisState.Veliaeris;
                }
                if (this.passiveSkillSlot.skillDef == this.ErisStart)
                {
                    System.Console.Write("erisstart");
                    return VeliaerisState.Eris;
                }
                if (this.passiveSkillSlot.skillDef == this.VeliaStart)
                {
                    System.Console.WriteLine("velia");
                    return VeliaerisState.Veliaeris;
                }
            }
            System.Console.WriteLine("default");
            return VeliaerisState.Veliaeris;
        }

    }
    public enum VeliaerisState
    {
        Veliaeris = 0,
        Velia =-1,
        Eris =1
    }
}
