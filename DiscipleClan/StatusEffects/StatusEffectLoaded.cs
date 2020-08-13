using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectLoaded : StatusEffectState
    {
        public const string statusId = "loaded";

        // Ideally lose gold every time they attack, ascend, or get hit and don't die. We may only be able to implement two of those

        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            GetAssociatedCharacter().RemoveStatusEffect(GetStatusId(), false, 1, true);
            yield break;
        }

        public override void OnStacksAdded(CharacterState character, int numStacksAdded)
        {
            GetAssociatedCharacter().AddDeathSignal(OnDeath, true);
        }

        private IEnumerator OnDeath(CharacterDeathParams deathParams)
        {
            PlayerManager playerManager;
            ProviderManager.TryGetProvider<PlayerManager>(out playerManager);
            CharacterState characterState = GetAssociatedCharacter();
            int GoldStacks = characterState.GetStatusEffectStacks(GetStatusId());

            if (GoldStacks == 0) { yield break; }

            if (characterState.PreviewMode) { yield break; }

            if (characterState != null)
            {
                characterState.ShowNotification("HudNotification_TreasureHeroTriggered".Localize(new LocalizedInteger(deathParams.saveManager.GetAdjustedGoldAmount(GoldStacks, isReward: true))), PopupNotificationUI.Source.General);
                //characterState.GetCharacterUI().ShowEffectVFX(characterState, cardEffectState.GetAppliedVFX());
            }
            playerManager.AdjustGold(GoldStacks, isReward: true); 
            yield break;
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectLoaded).AssemblyQualifiedName,
                StatusId = "loaded",
                DisplayCategory = StatusEffectData.DisplayCategory.Positive,
                TriggerStage = StatusEffectData.TriggerStage.OnPostAttacking,
                IsStackable = true,
                RemoveStackAtEndOfTurn = true,
                IconPath = "chrono/Status/two-coins.png",
            }.Build();
        }

    }
}
