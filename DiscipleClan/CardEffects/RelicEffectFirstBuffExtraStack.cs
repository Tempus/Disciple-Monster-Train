using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RelicEffectFirstBuffExtraStack : RelicEffectBase, IOnStatusEffectAddedRelicEffect, IRelicEffect, IStatusEffectRelicEffect, IStartOfPlayerTurnAfterDrawRelicEffect
	{
		private StringBuilder stringBuilder;
		private Team.Type team;
		private List<string> statusIds;
		private List<int> additionalStacks;
		private int timesPerTurn;
		private int currentCount;
		private string lastStatus;
		StatusEffectStackData[] statusStacks;

		public override bool CanApplyInPreviewMode => true;
		public override bool CanShowNotifications => false;

		public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
		{
			base.Initialize(relicState, relicData, relicEffectData);
			stringBuilder = new StringBuilder(20);
			team = relicEffectData.GetParamSourceTeam();
			statusIds = (from status in relicEffectData.GetParamStatusEffects() select status.statusId).ToList();
			additionalStacks = (from status in relicEffectData.GetParamStatusEffects() select status.count).ToList();
			timesPerTurn = currentCount = relicEffectData.GetParamInt();
			statusStacks = relicEffectData.GetParamStatusEffects();
			API.Log(BepInEx.Logging.LogLevel.All, "Effects: " + statusIds.ToList().Join());
		}

		public override bool AreConditionsTrue(CardStatistics cardStatistics)
		{
			return true;
		}

		public void OnPreStatusAdded(OnStatusEffectAddedRelicEffectParams relicEffectParams)
		{
			if (statusIds.Contains(relicEffectParams.statusId) && !(relicEffectParams.fromEffectType == typeof(CardEffectTransferAllStatusEffects)) && !(relicEffectParams.fromEffectType == typeof(CardEffectMultiplyStatusEffect)))
			{
				API.Log(BepInEx.Logging.LogLevel.All, "Pre-Status effect: " + relicEffectParams.statusId + " - Count: " + currentCount + "/" + timesPerTurn);
				relicEffectParams.stacksAdded += additionalStacks[statusIds.IndexOf(relicEffectParams.statusId)];
				lastStatus = relicEffectParams.statusId;
				NotifyRelicTriggered(relicEffectParams.relicManager, relicEffectParams.character);
			}
		}

		public void OnPreStatusRemoved(OnStatusEffectAddedRelicEffectParams relicEffectParams)
		{
		}

		public void OnStatusAdded(OnStatusEffectAddedRelicEffectParams relicEffectParams)
		{
		}

		public int GetModifiedStatusEffectStacks(StatusEffectStackData statusEffectStackData, CharacterState onCharacter)
		{
			if (!statusIds.Contains(statusEffectStackData.statusId) || (onCharacter == null))
			{
				return statusEffectStackData.count;
			}

			ProviderManager.TryGetProvider<StatusEffectManager>(out StatusEffectManager statusEffectManager);
			if (statusEffectManager.GetStatusEffectDataById(statusEffectStackData.statusId).GetDisplayCategory() == StatusEffectData.DisplayCategory.Positive && onCharacter.GetTeamType() == Team.Type.Heroes)
            {
				return statusEffectStackData.count;
			}

			API.Log(BepInEx.Logging.LogLevel.All, "Status effect: " + statusEffectStackData.statusId + " - Count: " + currentCount + "/" + timesPerTurn);
			if (currentCount >= timesPerTurn) { return statusEffectStackData.count; }
			currentCount++;

			API.Log(BepInEx.Logging.LogLevel.All, "It is happening");
			return Mathf.Max(statusEffectStackData.count + additionalStacks[statusIds.IndexOf(statusEffectStackData.statusId)], 0);
		}

		public override string GetActivatedDescription()
		{
			stringBuilder.Clear();
			stringBuilder.Append($"+{additionalStacks} ");
			stringBuilder.Append(StatusEffectManager.GetLocalizedName(lastStatus));
			return stringBuilder.ToString();
		}

        public StatusEffectStackData[] GetStatusEffects()
        {

			return statusStacks;

		}

        public bool TestEffect(RelicEffectParams relicEffectParams)
        {
			return false;
		}

        public IEnumerator ApplyEffect(RelicEffectParams relicEffectParams)
        {
			currentCount = 0;
			yield break; ;
		}
	}
}
