using System;
using System.Collections.Generic;
using System.Text;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.Cards.StatusEffects
{
    public class MTStatusEffect_Chronolock : IMTStatusEffect { public string ID => "chronolock"; }

    class StatusEffectChronolock : StatusEffectState
    {
        public const string StatusId = "chronolock";

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
                statusEffectStateName = typeof(StatusEffectChronolock).AssemblyQualifiedName,
                statusId = "chronolock",
                displayCategory = StatusEffectData.DisplayCategory.Negative,
                triggerStage = StatusEffectData.TriggerStage.OnPreMovement,
                removeStackAtEndOfTurn = true,
                icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
            }.Build();
        }
    }
}
