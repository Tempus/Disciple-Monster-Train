using HarmonyLib;
using Trainworks;
using Trainworks.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static Trainworks.Constants.VanillaStatusEffectIDs;

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
		}

		public void OnStatusEffectAddedApplyMultiplier(OnStatusEffectAddedRelicEffectParams relicEffectParams) { }

		public void OnStatusEffectAddedApplyAdder(OnStatusEffectAddedRelicEffectParams relicEffectParams)
		{
			if (lastStatus == relicEffectParams.statusId && !relicEffectParams.saveManager.PreviewMode && relicEffectParams.characterState != null)
			{
				canApply = false;
				NotifyRelicTriggered(relicEffectParams.relicManager, relicEffectParams.characterState);
			}
		}

		public void OnStatusEffectRemoved(OnStatusEffectAddedRelicEffectParams relicEffectParams) { }

		public int GetModifiedStatusEffectStacksFromMultiplier(StatusEffectStackData statusEffectStackData, CharacterState onCharacter)
		{
			return statusEffectStackData.count;
		}

		public int GetStatusEffectStacksToAdd(StatusEffectStackData statusEffectStackData, CharacterState onCharacter)
		{
			if (!canApply || (onCharacter == null) || statusEffectStackData == null || statusEffectStackData.statusId == null) { return 0; }

			ProviderManager.TryGetProvider<StatusEffectManager>(out StatusEffectManager statusManager);
			var status = statusManager.GetStatusEffectDataById(statusEffectStackData.statusId);

			if (status == null) { return 0; }

			bool validBuff = status.GetDisplayCategory() == StatusEffectData.DisplayCategory.Positive || statusEffectStackData.statusId == Armor || statusEffectStackData.statusId == Burnout;
			bool validDebuff = status.GetDisplayCategory() == StatusEffectData.DisplayCategory.Negative;

			if (validBuff   && onCharacter.GetTeamType() != Team.Type.Monsters) { return 0; }
			if (validDebuff && onCharacter.GetTeamType() == Team.Type.Monsters) { return 0; }
			if (!validBuff && !validDebuff) { return 0; }

			lastStatus = statusEffectStackData.statusId;
			return 1;
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
				canApply = false;
			}
			yield break;
		}
	}
}
