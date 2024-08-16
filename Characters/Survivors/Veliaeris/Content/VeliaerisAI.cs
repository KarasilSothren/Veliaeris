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

            AISkillDriver voidScytheDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            voidScytheDriver.customName = "Use Primary Swing";
            voidScytheDriver.skillSlot = SkillSlot.Primary;
            voidScytheDriver.requiredSkill = VeliaerisSurvivor.basicScythe; //usually used when you have skills that override other skillslots like engi harpoons
            voidScytheDriver.requireSkillReady = false; //usually false for primaries
            voidScytheDriver.requireEquipmentReady = false;
            voidScytheDriver.minUserHealthFraction = float.NegativeInfinity;
            voidScytheDriver.maxUserHealthFraction = float.PositiveInfinity;
            voidScytheDriver.minTargetHealthFraction = float.NegativeInfinity;
            voidScytheDriver.maxTargetHealthFraction = float.PositiveInfinity;
            voidScytheDriver.minDistance = 0;
            voidScytheDriver.maxDistance = 8;
            voidScytheDriver.selectionRequiresTargetLoS = false;
            voidScytheDriver.selectionRequiresOnGround = false;
            voidScytheDriver.selectionRequiresAimTarget = false;
            voidScytheDriver.maxTimesSelected = -1;

            //Behavior
            voidScytheDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            voidScytheDriver.activationRequiresTargetLoS = false;
            voidScytheDriver.activationRequiresAimTargetLoS = false;
            voidScytheDriver.activationRequiresAimConfirmation = false;
            voidScytheDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            voidScytheDriver.moveInputScale = 1;
            voidScytheDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            voidScytheDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            voidScytheDriver.shouldSprint = false; 
            voidScytheDriver.shouldFireEquipment = false;
            voidScytheDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold; 

            //Transition Behavior
            voidScytheDriver.driverUpdateTimerOverride = -1;
            voidScytheDriver.resetCurrentEnemyOnNextDriverSelection = false;
            voidScytheDriver.noRepeat = false;
            voidScytheDriver.nextHighPriorityOverride = null;

            AISkillDriver graspDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            graspDriver.customName = "Use Primary Swing";
            graspDriver.skillSlot = SkillSlot.Primary;
            graspDriver.requiredSkill = VeliaerisSurvivor.basicScythe; //usually used when you have skills that override other skillslots like engi harpoons
            graspDriver.requireSkillReady = false; //usually false for primaries
            graspDriver.requireEquipmentReady = false;
            graspDriver.minUserHealthFraction = float.NegativeInfinity;
            graspDriver.maxUserHealthFraction = float.PositiveInfinity;
            graspDriver.minTargetHealthFraction = float.NegativeInfinity;
            graspDriver.maxTargetHealthFraction = float.PositiveInfinity;
            graspDriver.minDistance = 0;
            graspDriver.maxDistance = 8;
            graspDriver.selectionRequiresTargetLoS = false;
            graspDriver.selectionRequiresOnGround = false;
            graspDriver.selectionRequiresAimTarget = false;
            graspDriver.maxTimesSelected = -1;

            //Behavior
            graspDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            graspDriver.activationRequiresTargetLoS = false;
            graspDriver.activationRequiresAimTargetLoS = false;
            graspDriver.activationRequiresAimConfirmation = false;
            graspDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            graspDriver.moveInputScale = 1;
            graspDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            graspDriver.ignoreNodeGraph = false; //will chase relentlessly but be kind of stupid
            graspDriver.shouldSprint = false; 
            graspDriver.shouldFireEquipment = false;
            graspDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold; 

            //Transition Behavior
            graspDriver.driverUpdateTimerOverride = -1;
            graspDriver.resetCurrentEnemyOnNextDriverSelection = false;
            graspDriver.noRepeat = false;
            graspDriver.nextHighPriorityOverride = null;
            
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


            AISkillDriver slashDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            slashDriver.customName = "Use Secondary Shoot";
            slashDriver.skillSlot = SkillSlot.Secondary;
            slashDriver.requiredSkill = VeliaerisSurvivor.CorruptAndHeal;
            slashDriver.requireSkillReady = true;
            slashDriver.minDistance = 0;
            slashDriver.maxDistance = 25;
            slashDriver.selectionRequiresTargetLoS = true;
            slashDriver.selectionRequiresOnGround = false;
            slashDriver.selectionRequiresAimTarget = true;
            slashDriver.maxTimesSelected = -1;

            //Behavior
            slashDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            slashDriver.activationRequiresTargetLoS = true;
            slashDriver.activationRequiresAimTargetLoS = true;
            slashDriver.activationRequiresAimConfirmation = true;
            slashDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            slashDriver.moveInputScale = 1;
            slashDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            slashDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            AISkillDriver lesserSisterDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            lesserSisterDriver.customName = "Use Secondary Shoot";
            lesserSisterDriver.skillSlot = SkillSlot.Secondary;
            lesserSisterDriver.requiredSkill = VeliaerisSurvivor.CorruptAndHeal;
            lesserSisterDriver.requireSkillReady = true;
            lesserSisterDriver.minDistance = 0;
            lesserSisterDriver.maxDistance = 25;
            lesserSisterDriver.selectionRequiresTargetLoS = true;
            lesserSisterDriver.selectionRequiresOnGround = false;
            lesserSisterDriver.selectionRequiresAimTarget = true;
            lesserSisterDriver.maxTimesSelected = -1;

            //Behavior
            lesserSisterDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            lesserSisterDriver.activationRequiresTargetLoS = true;
            lesserSisterDriver.activationRequiresAimTargetLoS = true;
            lesserSisterDriver.activationRequiresAimConfirmation = true;
            lesserSisterDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            lesserSisterDriver.moveInputScale = 1;
            lesserSisterDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            lesserSisterDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            
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

            AISkillDriver mergeDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            mergeDriver.customName = "Use Utility Roll";
            mergeDriver.skillSlot = SkillSlot.Utility;
            mergeDriver.requiredSkill = VeliaerisSurvivor.split;
            mergeDriver.requireSkillReady = true;
            mergeDriver.minDistance = 8;
            mergeDriver.maxDistance = 20;
            mergeDriver.selectionRequiresTargetLoS = true;
            mergeDriver.selectionRequiresOnGround = false;
            mergeDriver.selectionRequiresAimTarget = false;
            mergeDriver.maxTimesSelected = -1;

            //Behavior
            mergeDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            mergeDriver.activationRequiresTargetLoS = false;
            mergeDriver.activationRequiresAimTargetLoS = false;
            mergeDriver.activationRequiresAimConfirmation = false;
            mergeDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            mergeDriver.moveInputScale = 1;
            mergeDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            mergeDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;


            AISkillDriver shiftDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            shiftDriver.customName = "Use Utility Roll";
            shiftDriver.skillSlot = SkillSlot.Utility;
            shiftDriver.requiredSkill = VeliaerisSurvivor.split;
            shiftDriver.requireSkillReady = true;
            shiftDriver.minDistance = 8;
            shiftDriver.maxDistance = 20;
            shiftDriver.selectionRequiresTargetLoS = true;
            shiftDriver.selectionRequiresOnGround = false;
            shiftDriver.selectionRequiresAimTarget = false;
            shiftDriver.maxTimesSelected = -1;

            //Behavior
            shiftDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shiftDriver.activationRequiresTargetLoS = false;
            shiftDriver.activationRequiresAimTargetLoS = false;
            shiftDriver.activationRequiresAimConfirmation = false;
            shiftDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            shiftDriver.moveInputScale = 1;
            shiftDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            shiftDriver.buttonPressType = AISkillDriver.ButtonPressType.Abstain;
            
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

            AISkillDriver healDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            healDriver.customName = "Use Special bomb";
            healDriver.skillSlot = SkillSlot.Special;
            healDriver.requiredSkill = VeliaerisSurvivor.voidDetonation;
            healDriver.requireSkillReady = true;
            healDriver.minDistance = 0;
            healDriver.maxDistance = float.PositiveInfinity;
            healDriver.selectionRequiresTargetLoS = false;
            healDriver.selectionRequiresOnGround = false;
            healDriver.selectionRequiresAimTarget = false;
            healDriver.maxTimesSelected = -1;
            
            //Behavior
            healDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            healDriver.activationRequiresTargetLoS = false;
            healhealDriver.activationRequiresAimTargetLoS = false;
            healDriver.activationRequiresAimConfirmation = false;
            healDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            healDriver.moveInputScale = 1;
            healDriver.aimType = AISkillDriver.AimType.None;
            healDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

            AISkillDriver buffDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            Driver.customName = "Use Special bomb";
            river.skillSlot = SkillSlot.Special;
            buffDriver.requiredSkill = VeliaerisSurvivor.voidDetonation;
            buffDriver.requireSkillReady = true;
            buffDriver.minDistance = 0;
            buffDriver.maxDistance = float.PositiveInfinity;
            buffDriver.selectionRequiresTargetLoS = false;
            buffDriver.selectionRequiresOnGround = false;
            buffDriver.selectionRequiresAimTarget = false;
            buffDriver.maxTimesSelected = -1;
            
            //Behavior
            buffDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            buffDriver.activationRequiresTargetLoS = false;
            buffDriver.activationRequiresAimTargetLoS = false;
            buffDriver.activationRequiresAimConfirmation = false;
            buffDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            buffDriver.moveInputScale = 1;
            buffDriver.aimType = AISkillDriver.AimType.None;
            buffDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;


            AISkillDriver callDriver = master.AddComponent<AISkillDriver>();
            //Selection Conditions
            callDriver.customName = "Use Special bomb";
            callDriver.skillSlot = SkillSlot.Special;
            callDriver.requiredSkill = VeliaerisSurvivor.voidDetonation;
            callDriver.requireSkillReady = true;
            callDriver.minDistance = 0;
            callDriver.maxDistance = float.PositiveInfinity;
            callDriver.selectionRequiresTargetLoS = false;
            callDriver.selectionRequiresOnGround = false;
            callDriver.selectionRequiresAimTarget = false;
            callDriver.maxTimesSelected = -1;
            
            //Behavior
            callDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            callDriver.activationRequiresTargetLoS = false;
            callDriver.activationRequiresAimTargetLoS = false;
            callDriver.activationRequiresAimConfirmation = false;
            callDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            callDriver.moveInputScale = 1;
            callDriver.aimType = AISkillDriver.AimType.None;
            callDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;

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
