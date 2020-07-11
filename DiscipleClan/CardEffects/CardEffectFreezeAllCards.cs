using System.Collections;
using System.Collections.Generic;

namespace DiscipleClan.CardEffects
{
    public class CardEffectFreezeAllCards : CardEffectBase, ICardEffectStatuslessTooltip
    {
        public override bool CanPlayAfterBossDead
        {
            get
            {
                return false;
            }
        }

        public override bool CanApplyInPreviewMode
        {
            get
            {
                return false;
            }
        }

        public override bool TestEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams)
        {
            List<CardState> hand = cardEffectParams.cardManager.GetHand(true);
            CardEffectFreezeRandomCard.FilterCards(hand);
            return hand.Count > 0;
        }

        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams)
        {
            List<CardState> cards = cardEffectParams.cardManager.GetAllCards();
            CardEffectFreezeRandomCard.FilterCards(cards);

            foreach (var card in cards)
            {
                CardTraitData cardTraitData = new CardTraitData();
                cardTraitData.Setup("CardTraitFreeze");
                cardEffectParams.cardManager.AddTemporaryTraitToCard(card, cardTraitData, true, false);
            }
            yield break;
        }

        public string GetTooltipBaseKey(CardEffectState cardEffectState)
        {
            return "CardEffectFreezeCard";
        }

        public static void FilterCards(List<CardState> listOfCards)
        {
            if (listOfCards == null || listOfCards.Count == 0)
                return;
            for (int index = listOfCards.Count - 1; index >= 0; --index)
            {
                bool flag = listOfCards[index].HasTrait(typeof(CardTraitFreeze));
                switch (listOfCards[index].GetCardType())
                {
                    case CardType.Blight:
                    case CardType.Junk:
                        flag = true;
                        break;
                }
                if (flag)
                    listOfCards.RemoveAt(index);
            }
        }
    }
}
