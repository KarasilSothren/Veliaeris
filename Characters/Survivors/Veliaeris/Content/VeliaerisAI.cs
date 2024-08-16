using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace VeliaerisMod.Survivors.Veliaeris
{
    public static class VeliaerisAI
    {
        public static void Init(GameObject bodyPrefab, string masterName)
        {
            GameObject master = Modules.Prefabs.CreateBlankMasterPrefab(bodyPrefab, masterName);

            BaseAI baseAI = master.GetComponent<BaseAI>();
            baseAI.aimVectorDampTime = 0.1f;
            baseAI.aimVectorMaxSpeed = 360;

            //mouse over these fields for tooltips
            AISkillDriver basicScytheDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            basicScytheDriver.customName = "Use Primary Swing";
            basicScytheDriver.skillSlot = SkillSlot.Primary;
            basicScytheDriver.requiredSkill = VeliaerisSurvivor.basicScythe; //usually used when you have skills that override other skillslots like engi harpoons
            basicScytheDriver.requireSkillReady = false; //usually false for primaries
            basicScytheDriver.requireEquipmentReady = false;
            basicScytheDriver.minUserHealthFraction = float.NegativeInfinity;
            basicScytheDriver.maxUserHealthFraction = float.PositiveInfinity;
            basicScytheDriver.minTargetHealthFraction = float.NegativeInfinity;
            basicScytheDriver.maxTargetHealthFraction = float.PositiveInfinity;
            basicScytheDriver.minDistance = 0;
            basicScytheDriver.maxDistance = 8;
            basicScytheDriver.selectionRequiresTargetLoS = false;
            basicScytheDriver.selectionRequiresOnGround = false;
            basicScytheDriver.selectionRequiresAimTarget = false;
            basicScytheDriver.maxTimesSelected = -1;

            //Behavior
            basicScytheDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            basicScytheDriver.activationRequiresTargetLoS = false;
            basicScytheDriver.activationRequiresAimTargetLoS = false;
            basicScytheDriver.activationRequiresAimConfirmation = false;
            basicScytheDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            basicScytheDriver.moveInputScale = 1;
            basicScytheDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            basicScytheDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            basicScytheDriver.shouldSprint = false; 
            basicScytheDriver.shouldFireEquipment = false;
            basicScytheDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold; 

            //Transition Behavior
            basicScytheDriver.driverUpdateTimerOverride = -1;
            basicScytheDriver.resetCurrentEnemyOnNextDriverSelection = false;
            basicScytheDriver.noRepeat = false;
            basicScytheDriver.nextHighPriorityOverride = null;

            //some fields omitted that aren't commonly changed. will be set to default values
            AISkillDriver corruptDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            corruptDriver.customName = "Use Secondary Shoot";
            corruptDriver.skillSlot = SkillSlot.Secondary;
            corruptDriver.requiredSkill = VeliaerisSurvivor.CorruptAndHeal;
            corruptDriver.requireSkillReady = true;
            corruptDriver.minDistance = 0;
            corruptDriver.maxDistance = 25;
            corruptDriver.selectionRequiresTargetLoS = true;
            corruptDriver.selectionRequiresOnGround = false;
            corruptDriver.selectionRequiresAimTarget = true;
            corruptDriver.maxTimesSelected = -1;

            //Behavior
            corruptDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            corruptDriver.activationRequiresTargetLoS = true;
            corruptDriver.activationRequiresAimTargetLoS = true;
            corruptDriver.activationRequiresAimConfirmation = true;
            corruptDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            corruptDriver.moveInputScale = 1;
            corruptDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            corruptDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold; 
            
            AISkillDriver splitDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            splitDriver.customName = "Use Utility Roll";
            splitDriver.skillSlot = SkillSlot.Utility;
            splitDriver.requiredSkill = VeliaerisSurvivor.split;
            splitDriver.requireSkillReady = true;
            splitDriver.minDistance = 8;
            splitDriver.maxDistance = 20;
            splitDriver.selectionRequiresTargetLoS = true;
            splitDriver.selectionRequiresOnGround = false;
            splitDriver.selectionRequiresAimTarget = false;
            splitDriver.maxTimesSelected = -1;

            //Behavior
            splitDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            splitDriver.activationRequiresTargetLoS = false;
            splitDriver.activationRequiresAimTargetLoS = false;
            splitDriver.activationRequiresAimConfirmation = false;
            splitDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            splitDriver.moveInputScale = 1;
            splitDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            splitDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;

            AISkillDriver voidDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            voidDriver.customName = "Use Special bomb";
            voidDriver.skillSlot = SkillSlot.Special;
            voidDriver.requiredSkill = VeliaerisSurvivor.voidDetonation;
            voidDriver.requireSkillReady = true;
            voidDriver.minDistance = 0;
            voidDriver.maxDistance = float.PositiveInfinity;
            voidDriver.selectionRequiresTargetLoS = false;
            voidDriver.selectionRequiresOnGround = false;
            voidDriver.selectionRequiresAimTarget = false;
            voidDriver.maxTimesSelected = -1;
            
            //Behavior
            voidDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            voidDriver.activationRequiresTargetLoS = false;
            voidDriver.activationRequiresAimTargetLoS = false;
            voidDriver.activationRequiresAimConfirmation = false;
            voidDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            voidDriver.moveInputScale = 1;
            voidDriver.aimType = AISkillDriver.AimType.None;
            voidDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            AISkillDriver chaseDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            chaseDriver.customName = "Chase";
            chaseDriver.skillSlot = SkillSlot.None;
            chaseDriver.requireSkillReady = false;
            chaseDriver.minDistance = 0;
            chaseDriver.maxDistance = float.PositiveInfinity;

            //Behavior
            chaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseDriver.activationRequiresTargetLoS = false;
            chaseDriver.activationRequiresAimTargetLoS = false;
            chaseDriver.activationRequiresAimConfirmation = false;
            chaseDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseDriver.moveInputScale = 1;
            chaseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            //recommend taking these for a spin in game, messing with them in runtimeinspector to get a feel for what they should do at certain ranges and such
        }
    }
}
