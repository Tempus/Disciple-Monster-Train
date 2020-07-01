using System;
using System.Collections.Generic;
using System.Text;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.Cards.StatusEffects
{
    public class MTStatusEffect_Chronolock : IMTStatusEffect { public string ID => "icarian"; }

    class StatusEffectIcarian : StatusEffectState
    {
        public const string StatusId = "icarian";

        // This makes them unable to be targetted
        public override bool GetUnitIsTargetable(bool inCombat)
        {
            return !inCombat;
        }

        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            // At end of turn, ascend and if we try to ascend into the Pyre then we kaboom and do something.
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                statusEffectStateName = typeof(StatusEffectIcarian).AssemblyQualifiedName,
                statusId = "icarian",
                displayCategory = StatusEffectData.DisplayCategory.Neutral,
                triggerStage = StatusEffectData.TriggerStage.OnPreMovement,
                icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
            }.Build();
        }
    }
}
