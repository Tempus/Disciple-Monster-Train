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
            GetAssociatedCharacter().AddDeathSignal(OnDeath, true);
            GetAssociatedCharacter().RemoveStatusEffect(GetStatusId(), false, 1, true);
            yield break;
        }

        private IEnumerator OnDeath(CharacterDeathParams deathParams)
        {
            PlayerManager playerManager;
            ProviderManager.TryGetProvider<PlayerManager>(out playerManager);
            CharacterState characterState = GetAssociatedCharacter();
            int GoldStacks = characterState.GetStatusEffectStacks(GetStatusId());

            if (characterState != null)
            {
                characterState.ShowNotification("HudNotification_TreasureHeroTriggered".Localize(new LocalizedInteger(deathParams.saveManager.GetAdjustedGoldAmount(GoldStacks, isReward: true))), PopupNotificationUI.Source.General);
                //characterState.GetCharacterUI().ShowEffectVFX(characterState, cardEffectState.GetAppliedVFX());
            }
            playerManager.AdjustGold(GoldStacks, isReward: true); yield break;
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
                Icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Status/two-coins.png"),
            }.Build();
        }

    }
}
