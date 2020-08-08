using System.Collections;
using System.Collections.Generic;

namespace DiscipleClan.CardEffects
{
	class CardEffectIncreaseStatusEffects : CardEffectBase
	{
		public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			if (cardEffectState.GetTargetMode() != TargetMode.DropTargetCharacter)
			{
				return true;
			}
			if (cardEffectParams.targets.Count <= 0)
			{
				return false;
			}
			return false;
		}

		public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			int intInRange = cardEffectState.GetIntInRange();

			CharacterState.AddStatusEffectParams addStatusEffectParams = default(CharacterState.AddStatusEffectParams);
			addStatusEffectParams.sourceRelicState = cardEffectParams.sourceRelic;
			addStatusEffectParams.sourceCardState = cardEffectParams.playedCard;
			addStatusEffectParams.cardManager = cardEffectParams.cardManager;
			addStatusEffectParams.sourceIsHero = false;
			CharacterState.AddStatusEffectParams addStatusEffectParams2 = addStatusEffectParams;

			StatusEffectData.DisplayCategory buffOrDebuff = StatusEffectData.DisplayCategory.Negative;
			if (cardEffectState.GetParamBool())
				buffOrDebuff = StatusEffectData.DisplayCategory.Positive;

			foreach (CharacterState character in cardEffectParams.targets)
			{
				List<CharacterState.StatusEffectStack> statusEffectStacks;
				character.GetStatusEffects(out statusEffectStacks);

                foreach (var status in statusEffectStacks)
                {
					if (status.State.GetDisplayCategory() == buffOrDebuff)
						character.AddStatusEffect(status.State.GetStatusId(), 0, addStatusEffectParams2);
				}
			}
			yield break;
		}
	}
}
