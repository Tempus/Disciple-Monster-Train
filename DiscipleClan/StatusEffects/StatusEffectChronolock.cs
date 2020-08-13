using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectChronolock : StatusEffectState
    {
        public const string statusId = "chronolock";

        // This makes them unable to be targetted
        public override bool GetUnitIsTargetable(bool inCombat)
        {
            return !inCombat;
        }

        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            // This makes them unable to move
            outputTriggerParams.movementSpeed = 0;

            // This should make them unable to attack. Need to check about the TriggerStage to confirm it's firing at the right time.
            if (inputTriggerParams.canAttackOrHeal || inputTriggerParams.canFireTriggers)
            {
                outputTriggerParams.canAttackOrHeal = false;
                outputTriggerParams.canFireTriggers = false;
                return true;
            }
            return true;
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectChronolock).AssemblyQualifiedName,
                StatusId = "chronolock",
                DisplayCategory = StatusEffectData.DisplayCategory.Negative,
                TriggerStage = StatusEffectData.TriggerStage.OnPreMovement,
                IsStackable = true,
                RemoveStackAtEndOfTurn = true,
                IconPath = "chrono/Clan Assets/clan_32.png",
            }.Build();
        }
    }
}
