using RoR2;
using System;
using System.Linq;
using UnityEngine;

namespace VeliaerisMod.Characters.Survivors.Veliaeris.Components
{
    // Token: 0x0200073D RID: 1853
    [RequireComponent(typeof(InputBankTest))]
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(TeamComponent))]
    public class EntityTracker : MonoBehaviour
    {
        // Token: 0x0600267F RID: 9855 RVA: 0x000A7ED5 File Offset: 0x000A60D5
        private void Awake()
        {
            //this.indicator = new Indicator(base.gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
        }

        // Token: 0x06002680 RID: 9856 RVA: 0x000A7EF2 File Offset: 0x000A60F2
        private void Start()
        {
            this.characterBody = base.GetComponent<CharacterBody>();
            System.Console.WriteLine(this.characterBody.ToString());
            this.inputBank = base.GetComponent<InputBankTest>();
            this.teamComponent = base.GetComponent<TeamComponent>();
        }

        // Token: 0x06002681 RID: 9857 RVA: 0x000A7F18 File Offset: 0x000A6118
        public HurtBox GetTrackingTarget()
        {
            return this.trackingTarget;
        }

        // Token: 0x06002682 RID: 9858 RVA: 0x000A7F20 File Offset: 0x000A6120
        private void OnEnable()
        {
            this.indicator.active = true;
        }

        // Token: 0x06002683 RID: 9859 RVA: 0x000A7F2E File Offset: 0x000A612E
        private void OnDisable()
        {
            this.indicator.active = false;
        }

        // Token: 0x06002684 RID: 9860 RVA: 0x000A7F3C File Offset: 0x000A613C
        private void FixedUpdate()
        {
            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            System.Console.WriteLine("update");
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
            {
                this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
                HurtBox hurtBox = this.trackingTarget;
                Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
                this.SearchForTarget(aimRay);
                this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);
            }
        }

        // Token: 0x06002685 RID: 9861 RVA: 0x000A7FDC File Offset: 0x000A61DC
        private void SearchForTarget(Ray aimRay)
        {
            System.Console.WriteLine("searching");
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(this.teamComponent.teamIndex);
            this.search.filterByLoS = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = this.maxTrackingDistance;
            this.search.maxAngleFilter = this.maxTrackingAngle;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            this.trackingTarget = this.search.GetResults().FirstOrDefault<HurtBox>();
        }

        // Token: 0x04002A70 RID: 10864
        public float maxTrackingDistance = 20f;

        // Token: 0x04002A71 RID: 10865
        public float maxTrackingAngle = 20f;

        // Token: 0x04002A72 RID: 10866
        public float trackerUpdateFrequency = 10f;

        // Token: 0x04002A73 RID: 10867
        private HurtBox trackingTarget;

        // Token: 0x04002A74 RID: 10868
        private CharacterBody characterBody;

        // Token: 0x04002A75 RID: 10869
        private TeamComponent teamComponent;

        // Token: 0x04002A76 RID: 10870
        private InputBankTest inputBank;

        // Token: 0x04002A77 RID: 10871
        private float trackerUpdateStopwatch;

        // Token: 0x04002A78 RID: 10872
        private Indicator indicator;

        // Token: 0x04002A79 RID: 10873
        private readonly BullseyeSearch search = new BullseyeSearch();
    }
}
