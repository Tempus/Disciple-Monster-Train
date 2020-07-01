using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.Cards.CardEffects
{
    class CardEffectAddTempUpgrade : CardEffectBase
    {
        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams)
        {
            CardUpgradeState cardUpgradeState = new CardUpgradeState();
            cardUpgradeState.Setup(cardEffectState.GetParamCardUpgradeData());

            foreach (var card in cardEffectParams.targetCards)
            {
                CardAnimator.CardUpgradeAnimationInfo type = new CardAnimator.CardUpgradeAnimationInfo(card, cardUpgradeState);
                CardAnimator.DoAddRecentCardUpgrade.Dispatch(type);
                card.GetTemporaryCardStateModifiers().AddUpgrade(cardUpgradeState);
                card.UpdateCardBodyText();
                cardEffectParams.cardManager.RefreshCardInHand(card);
            }

            yield break;
        }
    }
}
