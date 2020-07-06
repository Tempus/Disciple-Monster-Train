using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using System.Collections;

namespace DiscipleClan.StatusEffects
{
	public class MTStatusEffect_Loaded : IMTStatusEffect { public string ID => "loaded"; }

	class StatusEffectLoaded : StatusEffectState
    {
        public const string StatusId = "loaded";

        // Every turn, descend
        // Every turn, heal to max
        // Every turn, gain max hp
        // When dead, give big gold (based on MaxHP? Based on turns?

        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            // This makes them unable to move
            outputTriggerParams.movementSpeed = -1;

            // Increase Max HP and full heal
            yield return inputTriggerParams.associatedCharacter.BuffMaxHP(10);
            yield return inputTriggerParams.associatedCharacter.ApplyHeal(999);

            // Give gold based on HP... 10% a turn.
            int goldGain = inputTriggerParams.associatedCharacter.GetMaxHP() / 10;

            var characterState = inputTriggerParams.associatedCharacter;
            characterState.ShowNotification("HudNotification_TreasureHeroTriggered".Localize(new LocalizedInteger(inputTriggerParams.combatManager.GetSaveManager().GetAdjustedGoldAmount(goldGain, isReward: true))), PopupNotificationUI.Source.General);
            //characterState.GetCharacterUI().ShowEffectVFX(characterState, cardEffectState.GetAppliedVFX());
            inputTriggerParams.combatManager.GetSaveManager().AdjustGold(goldGain, isReward: false);
        }

		public static void Make()
        {
            new StatusEffectDataBuilder
            {
                statusEffectStateName = typeof(StatusEffectLoaded).AssemblyQualifiedName,
                statusId = "loaded",
                displayCategory = StatusEffectData.DisplayCategory.Negative,
                triggerStage = StatusEffectData.TriggerStage.OnPreMovement,
                isStackable = false,
				icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
			}.Build();
        }

    }
}
