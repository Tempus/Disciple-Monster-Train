using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.CardEffects
{
    class CardEffectEntropyBuffDamage : CardEffectBase
    {
        private const string PositiveEffectKey = "TextFormat_Add";
        private const string NegativeEffectKey = "TextFormat_Default";
        private int buffAmount;

        public override IEnumerator ApplyEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            this.buffAmount = cardEffectParams.playerManager.GetEnergy() * cardEffectState.GetParamInt();

            cardEffectParams.playedCard.GetCardStateModifiers().IncrementAdditionalDamage(this.buffAmount);
            cardEffectParams.playedCard.UpdateCardBodyText((SaveManager)null);
            CardManager cardManager = cardEffectParams.cardManager;
            if (cardManager != null)
            {
                cardManager.RefreshCardInHand(cardEffectParams.playedCard, true);
            }

            yield break;
        }
    }
}