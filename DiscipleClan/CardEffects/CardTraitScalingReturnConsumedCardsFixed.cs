using System;
using System.Collections;
using System.Text;

namespace DiscipleClan.CardEffects
{
    class CardTraitScalingReturnConsumedCardsFixed : CardTraitState
    {
		// This exists because the base form implemented in the game (but unused) is broken and causes problems with CardStatistics counting
		public override IEnumerator OnCardDiscarded(CardManager.DiscardCardParams discardCardParams, CardManager cardManager, RelicManager relicManager, CombatManager combatManager, RoomManager roomManager, SaveManager saveManager)
		{
			if (discardCardParams.wasPlayed)
			{
				int additionalCardCount = GetAdditionalCardCount(cardManager.GetCardStatistics());
				for (int i = 0; i < additionalCardCount; i++)
				{
					if (!cardManager.RestoreNextExhaustedCard())
						break;
				}
			}
			yield break;
		}

		private int GetAdditionalCardCount(CardStatistics cardStatistics)
		{
			int statValue = cardStatistics.GetStatValue(base.StatValueData);
			return GetParamInt() * statValue;
		}

		public override bool HasMultiWordDesc()
		{
			return true;
		}

		public override string GetCurrentEffectText(CardStatistics cardStatistics, SaveManager saveManager, RelicManager relicManager)
		{
			if (cardStatistics != null && cardStatistics.GetStatValueShouldDisplayOnCardNow(base.StatValueData))
			{
				return "CardTraitScalingReturnConsumedCards_CurrentScaling_CardText".Localize(new LocalizedIntegers(GetAdditionalCardCount(cardStatistics)));
			}
			return string.Empty;
		}
		
	}
}
