using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.CardEffects
{
    class CardTriggerEffectBuffSpellDamageForEveryCardOfType : ICardTriggerEffect
    {
		public IEnumerator ApplyTriggerEffect(CardTriggerState triggerEffectState, CardTriggerEventParams triggerParams)
		{
			int paramInt = triggerEffectState.GetParamInt();

            foreach (var card in triggerParams.cardManager.GetAllCards())
            {
				if (card.GetCardDataID() == triggerParams.playedCard.GetCardDataID())
                {
					card.GetCardStateModifiers().IncrementAdditionalDamage(paramInt);
					card.UpdateCardBodyText();
				}
			}
			triggerParams.cardManager.RefreshHandCards();
			yield break;
		}

		public IEnumerator ApplyTriggerEffectPreviewMode(CardTriggerState triggerEffectState, CardTriggerEventParams triggerParams)
		{
			yield break;
		}

		private string GetNotificationText(int additionalAmount)
		{
			return LocalizationUtil.LocalizeWithNumber("CardUpgrade_AttackDamage", additionalAmount);
		}
	}
}
