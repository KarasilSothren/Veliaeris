//using EntityStates;
//using VeliaerisMod.Survivors.Veliaeris;
//using RoR2;
//using UnityEngine;
//using R2API;
//using UnityEngine.Networking;
//using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
//using VeliaerisMod.Modules;
//using RoR2.Orbs;
//using System;
//using System.Collections.Generic;

//namespace VeliaerisMod.Survivors.Veliaeris.SkillStates
//{
//    public class GraspOfOblivion : BaseSkillState
//    {
//        public static float procCoefficient = 1f;
//        public float duration = 0.6f;
//        //delay on firing is usually ass-feeling. only set this if you know what you're doing
//        private HuntressTracker hunterTracker;
//        private ChildLocator childLocator;
//        private HurtBox initialOrbTarget;
//        public float orbProcCoefficent;
//        private string muzzleString;
//        private Animator animator;
//        protected bool isCrit;
//        private int firedArrowCount;
//        private int maxArrowCount;
//        public static GameObject graspEffectPrefab;
//        public static GameObject orbEffectPrefab;
//        public override void OnEnter()
//        {
//            base.OnEnter();
//                Transform modelTransfrom = base.GetModelTransform();
//                this.hunterTracker = base.GetComponent<HuntressTracker>();
//                BullseyeSearch bullseyeSearch = new BullseyeSearch();
//                if (modelTransfrom)
//                {
//                    this.childLocator = modelTransfrom.GetComponent<ChildLocator>();
//                    this.animator = modelTransfrom.GetComponent<Animator>();

//                }
//                if (this.hunterTracker && base.isAuthority)
//                {
//                this.initialOrbTarget = this.hunterTracker.GetTrackingTarget();
//                }
//                if (base.characterBody)
//                {
//                    base.characterBody.SetAimTimer(this.duration + 1f);
//                }
//                this.isCrit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
//                Curse curseController = new Curse();
//                curseController.characterBody = base.characterBody;
//                curseController.curseTargets = initialOrbTarget;
//                curseController.active = true;
//                curseController.isCrit = this.isCrit;
            
//            //System.Console.WriteLine("looking:",hunterTracker.ToString());

//        }


        


         

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();

//            if (fixedAge >= duration && isAuthority)
//            {
//                outer.SetNextStateToMain();
//                return;
//            }
//        }




//        public override InterruptPriority GetMinimumInterruptPriority()
//        {
//            return InterruptPriority.Skill;
//        }

//        private class Curse
//        {
//            public bool active
//            {
//                get
//                {
//                    return this._active;
//                }
//                set
//                {
//                    if (this._active == value)
//                    {
//                        return;
//                    }
//                    this._active = value;
//                    if (this._active)
//                    {
//                        RoR2Application.onFixedUpdate += this.FixedUpdate;
//                        return;
//                    }
//                    RoR2Application.onFixedUpdate -= this.FixedUpdate;
//                }
//            }

//            // Token: 0x06000FEB RID: 4075 RVA: 0x00046A10 File Offset: 0x00044C10
//            private void FixedUpdate()
//            {
//                if (!this.characterBody || !this.characterBody.healthComponent || !this.characterBody.healthComponent.alive)
//                {
//                    this.active = false;
//                    return;
//                }
//                this.timer -= Time.deltaTime;
//                if (this.timer <= 0f)
//                {
//                    this.timer = this.interval;
                
//                        try
//                        {
//                         System.Console.WriteLine("curseTargets: " + curseTargets);
//                        if (this.DoVoid(curseTargets))
//                        {
//                            this.active = false;
//                        }
                            
//                        }
//                        catch (Exception message)
//                        {
//                            Debug.LogError(message);
//                        }
//                        this.i++;
                
//                }
//            }

//            private bool DoVoid(HurtBox targetHurtBox)
//            {
//                System.Console.WriteLine("step6");
//                System.Console.WriteLine("TargetHurtBox:" + targetHurtBox);
//                if (!targetHurtBox)
//                {
//                    return false;
//                }
//                System.Console.WriteLine("step7");
//                HealthComponent healthComponent = targetHurtBox.healthComponent;
//                if (!healthComponent)
//                {
//                    return false;
//                }
//                System.Console.WriteLine("step8");
//                CharacterBody body = healthComponent.body;
//                if (!body)
//                {
//                    return false;
//                }
//                System.Console.WriteLine("step9");
//                float hpDamage = this.characterBody.maxHealth * 0.5f;
//                float percentMaxHealthDamage = targetHurtBox.healthComponent.combinedHealth * 0.02f;
//                System.Console.WriteLine("grasp hurtbox: " + targetHurtBox);

//                GraspOfOblivionOrb graspDamageOrb = new GraspOfOblivionOrb();
//                graspDamageOrb.origin = this.characterBody.corePosition;
//                graspDamageOrb.target = targetHurtBox;
//                graspDamageOrb.attacker = this.characterBody.gameObject;
//                graspDamageOrb.baseDamage = (hpDamage + percentMaxHealthDamage);
//                graspDamageOrb.damageColorIndex = DamageColorIndex.Void;
//                graspDamageOrb.isCrit = this.isCrit;
//                graspDamageOrb.procChainMask = default(ProcChainMask);
//                graspDamageOrb.procCoefficient = 1f;
////                graspDamageOrb.
//                graspDamageOrb.orbEffectPrefab = orbEffectPrefab;
//                OrbManager.instance.AddOrb(graspDamageOrb);
//                System.Console.WriteLine("step10");
//                return true;
//            }

//            // Token: 0x0400147C RID: 5244
//            public HurtBox curseTargets;

//            // Token: 0x0400147D RID: 5245
//            public CharacterBody characterBody;

//            // Token: 0x0400147E RID: 5246
//            public float damageStat = 1f;

//            // Token: 0x0400147F RID: 5247
//            public bool isCrit;

//            // Token: 0x04001480 RID: 5248
//            public float interval;

//            // Token: 0x04001481 RID: 5249
//            private int i;

//            // Token: 0x04001482 RID: 5250
//            private float timer;

//            // Token: 0x04001483 RID: 5251
//            private bool _active;
//        }

//        //public override void OnSerialize(NetworkWriter writer)
//        //{
//        //    writer.Write(HurtBoxReference.FromHurtBox(this.initialOrbTarget));
//        //}

//        //// Token: 0x06000E60 RID: 3680 RVA: 0x0003E0A0 File Offset: 0x0003C2A0
//        //public override void OnDeserialize(NetworkReader reader)
//        //{
//        //    this.initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
//        //}
//    }
//}