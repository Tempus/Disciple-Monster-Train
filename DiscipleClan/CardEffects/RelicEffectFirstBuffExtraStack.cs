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
    class RelicEffectFirstBuffExtraStack : RelicEffectBase, IOnStatusEffectAddedRelicEffect, IRelicEffect, IStatusEffectRelicEffect, IEndOfTurnRelicEffect, IStartOfPlayerTurnAfterDrawRelicEffect
	{
		private StringBuilder stringBuilder;
		private Team.Type team;
		private List<string> statusIds;
		private List<int> additionalStacks;
		private int timesPerTurn;
		private int currentCount;
		private string lastStatus;
		StatusEffectStackData[] statusStacks;
		private bool canApply = false;

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
			if (!canApply) { return; }

			if (statusIds.Contains(relicEffectParams.statusId))
			{
				relicEffectParams.stacksAdded += additionalStacks[statusIds.IndexOf(relicEffectParams.statusId)];
                lastStatus = relicEffectParams.statusId;
				NotifyRelicTriggered(relicEffectParams.relicManager, relicEffectParams.character);
			}
			return; 
		}

		public void OnPreStatusRemoved(OnStatusEffectAddedRelicEffectParams relicEffectParams)
		{
		}

		public void OnStatusAdded(OnStatusEffectAddedRelicEffectParams relicEffectParams)
		{
			API.Log(BepInEx.Logging.LogLevel.All, "Hi: " + relicEffectParams.statusId);
			if (lastStatus == relicEffectParams.statusId && !relicEffectParams.saveManager.PreviewMode && relicEffectParams.character != null)
			{

				canApply = false;
				API.Log(BepInEx.Logging.LogLevel.All, "Bye: " + relicEffectParams.statusId);

			}
		}
		public int GetModifiedStatusEffectStacks(StatusEffectStackData statusEffectStackData, CharacterState onCharacter)
		{
			//API.Log(BepInEx.Logging.LogLevel.All, "Hi: " + statusEffectStackData.statusId + " - " + canApply);
			//if (!statusIds.Contains(statusEffectStackData.statusId))
			//{
			//	return statusEffectStackData.count;
			//}

			//ProviderManager.TryGetProvider<StatusEffectManager>(out StatusEffectManager statusEffectManager);
			////if (statusEffectManager.GetStatusEffectDataById(statusEffectStackData.statusId).GetDisplayCategory() == StatusEffectData.DisplayCategory.Positive && onCharacter.GetTeamType() == Team.Type.Heroes)
			////         {
			////	return statusEffectStackData.count;
			////}

			//API.Log(BepInEx.Logging.LogLevel.All, "Status effect: " + statusEffectStackData.statusId + " - Count: " + currentCount + "/" + timesPerTurn);
			//if (currentCount >= timesPerTurn) { return statusEffectStackData.count; }
			//currentCount++;

			//API.Log(BepInEx.Logging.LogLevel.All, "It is happening");

			if (!canApply || !statusIds.Contains(statusEffectStackData.statusId)) { return statusEffectStackData.count; }
			return statusEffectStackData.count + additionalStacks[statusIds.IndexOf(statusEffectStackData.statusId)];
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
			API.Log(BepInEx.Logging.LogLevel.All, "Start of turn");
			canApply = true;
			return true;
		}

        public IEnumerator ApplyEffect(RelicEffectParams relicEffectParams)
        {
			yield break;
		}

        public bool TestEffect(EndOfTurnRelicEffectParams relicEffectParams)
        {
			return true;
		}

		public IEnumerator ApplyEffect(EndOfTurnRelicEffectParams relicEffectParams)
        {
			if (!relicEffectParams.saveManager.PreviewMode)
			{
				API.Log(BepInEx.Logging.LogLevel.All, "End of turn");
				canApply = false;
			}
			yield break;
		}
	}
}
