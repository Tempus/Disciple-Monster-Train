using System;
using System.Collections.Generic;
using System.Text;
using ShinyShoe.Logging;
using System.Collections;
using Trainworks.Managers;

namespace DiscipleClan.CardEffects
{
    public class CardEffectAddPyreStatusEmpowered : CardEffectBase
	{
		public static StatusEffectStackData GetStatusEffectStack(CardEffectState cardEffectState)
		{
			return GetStatusEffectStack(cardEffectState.GetSourceCardEffectData());
		}

		protected static StatusEffectStackData GetStatusEffectStack(CardEffectData cardEffectData)
		{
			StatusEffectStackData[] paramStatusEffects = cardEffectData.GetParamStatusEffects();
			if (paramStatusEffects == null || paramStatusEffects.Length == 0)
			{
				Log.Error(LogGroups.Gameplay, "cardEffectData.GetParamStatusEffects() yielded no results.");
				return null;
			}

			var stat = paramStatusEffects[0];

			// Empower it
			PlayerManager playerManager;
			ProviderManager.TryGetProvider<PlayerManager>(out playerManager);

			stat.count = playerManager.GetEnergy();

			return stat;
		}

		public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			StatusEffectStackData statusEffectStack = GetStatusEffectStack(cardEffectState);
			if (statusEffectStack == null)
			{
				return false;
			}
			if (cardEffectState.GetTargetMode() != TargetMode.DropTargetCharacter)
			{
				return true;
			}
			if (cardEffectParams.targets.Count <= 0)
			{
				return false;
			}
			if (cardEffectParams.statusEffectManager.GetStatusEffectDataById(statusEffectStack.statusId).IsStackable())
			{
				return true;
			}
			ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);
			CharacterState target = roomManager.GetPyreRoom().GetPyreHeart();

			if (!target.HasStatusEffect(statusEffectStack.statusId))
			{
				return true;
			}
			return false;
		}

		public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);
			CharacterState target = roomManager.GetPyreRoom().GetPyreHeart();

			if (cardEffectParams.saveManager.PreviewMode) { yield break; }

			StatusEffectStackData statusEffectStack = GetStatusEffectStack(cardEffectState);
			if (statusEffectStack == null)
			{
				yield break;
			}
			int intInRange = cardEffectState.GetIntInRange();
			CharacterState.AddStatusEffectParams addStatusEffectParams = default(CharacterState.AddStatusEffectParams);
			addStatusEffectParams.sourceRelicState = cardEffectParams.sourceRelic;
			addStatusEffectParams.sourceCardState = cardEffectParams.playedCard;
			addStatusEffectParams.cardManager = cardEffectParams.cardManager;
			addStatusEffectParams.sourceIsHero = (cardEffectState.GetSourceTeamType() == Team.Type.Heroes);
			CharacterState.AddStatusEffectParams addStatusEffectParams2 = addStatusEffectParams;

			RngId rngId = cardEffectParams.saveManager.PreviewMode ? RngId.BattleTest : RngId.Battle;
			PlayerManager playerManager;
			ProviderManager.TryGetProvider<PlayerManager>(out playerManager);

			target.AddStatusEffect(statusEffectStack.statusId, playerManager.GetEnergy(), addStatusEffectParams2);
		}

		public override void GetTooltipsStatusList(CardEffectState cardEffectState, ref List<string> outStatusIdList)
		{
			GetTooltipsStatusList(cardEffectState.GetSourceCardEffectData(), ref outStatusIdList);
		}

		public static void GetTooltipsStatusList(CardEffectData cardEffectData, ref List<string> outStatusIdList)
		{
			StatusEffectStackData statusEffectStack = GetStatusEffectStack(cardEffectData);
			if (statusEffectStack != null)
			{
				outStatusIdList.Add(statusEffectStack.statusId);
			}
		}
	}
}
