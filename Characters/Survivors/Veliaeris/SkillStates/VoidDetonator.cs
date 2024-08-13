using System;
using System.Collections.Generic;
using System.Linq;
using EntityStates;
using RoR2;
using RoR2.Orbs;
using UnityEngine;
using UnityEngine.Networking;
using VeliaerisMod.Characters.Survivors.Veliaeris.Content;
using VeliaerisMod.Survivors.Veliaeris;
using VeliaerisMod.Modules;

namespace VeliaerisMod.Survivors.Veliaeris.SkillStates
{
    // Token: 0x02000375 RID: 885
    public class VoidDetonator : BaseState
    {
        // Token: 0x06000FE6 RID: 4070 RVA: 0x0004684C File Offset: 0x00044A4C
        public override void OnEnter()
        {
//            System.Console.WriteLine("Entered Detonate");
            base.OnEnter();
            this.duration = VoidDetonator.baseDuration / this.attackSpeedStat;
            //            EffectManager.SimpleImpactEffect(VoidDetonator.enterEffectPrefab, base.characterBody.corePosition, Vector3.up, false);
            //          Util.PlaySound(VoidDetonator.enterSoundString, base.gameObject);
            if (NetworkServer.active)
            {
                BullseyeSearch bullseyeSearch = new BullseyeSearch();
                bullseyeSearch.filterByDistinctEntity = true;
                bullseyeSearch.filterByLoS = false;
                bullseyeSearch.maxDistanceFilter = float.PositiveInfinity;
                bullseyeSearch.minDistanceFilter = 0f;
                bullseyeSearch.minAngleFilter = 0f;
                bullseyeSearch.maxAngleFilter = 360f;
                bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
                bullseyeSearch.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
                bullseyeSearch.searchOrigin = base.characterBody.corePosition;
                bullseyeSearch.viewer = null;
                bullseyeSearch.RefreshCandidates();
                bullseyeSearch.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> results = bullseyeSearch.GetResults();
                this.detonationTargets = results.ToArray<HurtBox>();
                VoidDetonator.VoidDetonatorController VoidDetonatorController = new VoidDetonator.VoidDetonatorController();
                VoidDetonatorController.characterBody = base.characterBody;
                VoidDetonatorController.interval = VoidDetonator.detonationInterval;
                VoidDetonatorController.detonationTargets = this.detonationTargets;
                VoidDetonatorController.damageStat = this.damageStat;
                VoidDetonatorController.isCrit = base.RollCrit();
                VoidDetonatorController.active = true;
            }
            VeliaerisStatuses.veliaerisSpecialStock--;
//            base.PlayAnimation(this.animationLayerName, this.animationStateName, this.playbackRateParam, this.duration);
        }

        // Token: 0x06000FE7 RID: 4071 RVA: 0x0004699E File Offset: 0x00044B9E
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        // Token: 0x0400146E RID: 5230
        public static float baseDuration;

        // Token: 0x0400146F RID: 5231
        public static float baseDamageCoefficient =4.5f;

        // Token: 0x04001470 RID: 5232
        public static float damageCoefficientPerStack = 2.5f;

        // Token: 0x04001471 RID: 5233
        public static float procCoefficient;

        // Token: 0x04001472 RID: 5234
        public static float detonationInterval;

        // Token: 0x04001473 RID: 5235
        public static GameObject detonationEffectPrefab;

        // Token: 0x04001474 RID: 5236
        public static GameObject orbEffectPrefab;

        // Token: 0x04001475 RID: 5237
        public static GameObject enterEffectPrefab;

        // Token: 0x04001476 RID: 5238
        public static string enterSoundString;

        // Token: 0x04001477 RID: 5239
        [SerializeField]
        public string animationLayerName;

        // Token: 0x04001478 RID: 5240
        [SerializeField]
        public string animationStateName;

        // Token: 0x04001479 RID: 5241
        [SerializeField]
        public string playbackRateParam;

        // Token: 0x0400147A RID: 5242
        private float duration;

        // Token: 0x0400147B RID: 5243
        private HurtBox[] detonationTargets;

        // Token: 0x02000376 RID: 886
        private class VoidDetonatorController
        {
            // Token: 0x170000EF RID: 239
            // (get) Token: 0x06000FE9 RID: 4073 RVA: 0x000469C8 File Offset: 0x00044BC8
            // (set) Token: 0x06000FEA RID: 4074 RVA: 0x000469D0 File Offset: 0x00044BD0
            public bool active
            {
                get
                {
                    return this._active;
                }
                set
                {
                    if (this._active == value)
                    {
                        return;
                    }
                    this._active = value;
                    if (this._active)
                    {
                        RoR2Application.onFixedUpdate += this.FixedUpdate;
                        return;
                    }
                    RoR2Application.onFixedUpdate -= this.FixedUpdate;
                }
            }

            // Token: 0x06000FEB RID: 4075 RVA: 0x00046A10 File Offset: 0x00044C10
            private void FixedUpdate()
            {
                if (!this.characterBody || !this.characterBody.healthComponent || !this.characterBody.healthComponent.alive)
                {
                    this.active = false;
                    return;
                }
                this.timer -= Time.deltaTime;
                if (this.timer <= 0f)
                {
                    this.timer = this.interval;
                    while (this.i < this.detonationTargets.Length)
                    {
                        try
                        {
                            HurtBox targetHurtBox = null;
                            Util.Swap<HurtBox>(ref targetHurtBox, ref this.detonationTargets[this.i]);
                            if (this.DoDetonation(targetHurtBox))
                            {
                                break;
                            }
                        }
                        catch (Exception message)
                        {
                            Debug.LogError(message);
                        }
                        this.i++;
                    }
                    if (this.i >= this.detonationTargets.Length)
                    {
                        this.active = false;
                    }
                }
            }

            // Token: 0x06000FEC RID: 4076 RVA: 0x00046AF8 File Offset: 0x00044CF8
            private bool DoDetonation(HurtBox targetHurtBox)
            {
                System.Console.WriteLine("Detonate");
                if (!targetHurtBox)
                {
                    return false;
                }
                HealthComponent healthComponent = targetHurtBox.healthComponent;
                if (!healthComponent)
                {
                    return false;
                }
                CharacterBody body = healthComponent.body;
                if (!body)
                {
                    return false;
                }
                if (body.GetBuffCount(VeliaerisBuffs.abyss) <= 0)
                {
                    return false;
                }
                System.Console.WriteLine("detenator hurtbox: " + targetHurtBox);
                VoidDetonatorOrb VoidDetonatorOrb = new VoidDetonatorOrb();
                VoidDetonatorOrb.origin = this.characterBody.corePosition;
                VoidDetonatorOrb.target = targetHurtBox;
                VoidDetonatorOrb.attacker = this.characterBody.gameObject;
                VoidDetonatorOrb.baseDamage = this.damageStat * VoidDetonator.baseDamageCoefficient;
                VoidDetonatorOrb.damagePerStack = this.damageStat * VoidDetonator.damageCoefficientPerStack;
                VoidDetonatorOrb.damageColorIndex = DamageColorIndex.Default;
                VoidDetonatorOrb.isCrit = this.isCrit;
                VoidDetonatorOrb.procChainMask = default(ProcChainMask);
                VoidDetonatorOrb.procCoefficient = 0.8f;
                VoidDetonatorOrb.detonationEffectPrefab = VoidDetonator.detonationEffectPrefab;
                VoidDetonatorOrb.travelSpeed = 120f;
                VoidDetonatorOrb.orbEffectPrefab = VoidDetonator.orbEffectPrefab;
                OrbManager.instance.AddOrb(VoidDetonatorOrb);
                return true;
            }

            // Token: 0x0400147C RID: 5244
            public HurtBox[] detonationTargets;

            // Token: 0x0400147D RID: 5245
            public CharacterBody characterBody;

            // Token: 0x0400147E RID: 5246
            public float damageStat =1.2f;

            // Token: 0x0400147F RID: 5247
            public bool isCrit;

            // Token: 0x04001480 RID: 5248
            public float interval;

            // Token: 0x04001481 RID: 5249
            private int i;

            // Token: 0x04001482 RID: 5250
            private float timer;

            // Token: 0x04001483 RID: 5251
            private bool _active;
        }
    }
}
