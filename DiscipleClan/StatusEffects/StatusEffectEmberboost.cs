using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.StatusEffects
{
    public class MTStatusEffect_Emberboost : IMTStatusEffect { public string ID => "emberboost"; }

    class StatusEffectEmberboost : StatusEffectState
    {
        public const string StatusId = "emberboost";

        // This makes them unable to be targetted
        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            yield return inputTriggerParams.roomManager.GetRoomUI().SetSelectedRoom(inputTriggerParams.associatedCharacter.GetCurrentRoomIndex());
            int statusEffectStacks = inputTriggerParams.associatedCharacter.GetStatusEffectStacks(GetStatusId());
            statusEffectStacks++;
            inputTriggerParams.playerManager.AddEnergy(statusEffectStacks);
            inputTriggerParams.associatedCharacter.ShowNotification(string.Format("StatusEffectEmberboostState_Activated".Localize(), statusEffectStacks), PopupNotificationUI.Source.General);
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                statusEffectStateName = typeof(StatusEffectEmberboost).AssemblyQualifiedName,
                statusId = "emberboost",
                displayCategory = StatusEffectData.DisplayCategory.Positive,
                triggerStage = StatusEffectData.TriggerStage.OnMonsterTeamTurnBegin,
                isStackable = true,
                removeStackAtEndOfTurn = true,
                icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
            }.Build();
        }
    }
}
