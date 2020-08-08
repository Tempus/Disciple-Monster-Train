using System;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.CardEffects
{
    class CardTraitScalingEveryStatusEffect : CardTraitState
    {
		public override int OnStatusEffectApplied(CharacterState affectedCharacter, CardState thisCard, CardManager cardManager, RelicManager relicManager, string statusId, int sourceStacks = 0)
		{
			int statValue = cardManager.GetCardStatistics().GetStatValue(base.StatValueData);
			int multiplier = GetParamInt();
			if (thisCard.HasTrait(typeof(CardTraitJuice)))
				multiplier *= 2;

			return statValue * multiplier;
		}

		public override bool HasMultiWordDesc()
		{
			return true;
		}

		//public override string GetCurrentEffectText(CardStatistics cardStatistics, SaveManager saveManager, RelicManager relicManager)
		//{
		//	if (cardStatistics != null && cardStatistics.GetStatValueShouldDisplayOnCardNow(base.StatValueData))
		//	{
		//		int num = GetAdditionalStacks(cardStatistics, relicManager, "");
		//		if (GetCard() != null && GetCard().HasTrait(typeof(CardTraitJuice)))
		//		{
		//			num *= 2;
		//		}
		//		return string.Format("CardTraitScalingAddStatusEffect_CurrentScaling_CardText".Localize(), num);
		//	}
		//	return string.Empty;
		//}
	}
}
