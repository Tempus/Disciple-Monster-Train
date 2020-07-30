using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectEmberboost : StatusEffectState
    {
        public const string statusId = "emberboost";

        // This makes them unable to be targetted
        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            yield return inputTriggerParams.roomManager.GetRoomUI().SetSelectedRoom(inputTriggerParams.associatedCharacter.GetCurrentRoomIndex());
            int statusEffectStacks = inputTriggerParams.associatedCharacter.GetStatusEffectStacks(GetStatusId());
            statusEffectStacks++;
            inputTriggerParams.playerManager.AddEnergy(statusEffectStacks);
            inputTriggerParams.associatedCharacter.ShowNotification(string.Format("StatusEffectEmberboostState_Activated".Localize(), statusEffectStacks), PopupNotificationUI.Source.General);

            GetAssociatedCharacter().RemoveStatusEffect(GetStatusId(), false, 1, true);
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectEmberboost).AssemblyQualifiedName,
                StatusId = "emberboost",
                DisplayCategory = StatusEffectData.DisplayCategory.Negative,
                TriggerStage = StatusEffectData.TriggerStage.OnMonsterTeamTurnBegin,
                IsStackable = true,
                Icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Status/fire-silhouette.png"),
            }.Build();
        }
    }
}
