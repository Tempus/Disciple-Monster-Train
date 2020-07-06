using System.Collections.Generic;

namespace DiscipleClan.CardEffects
{
    class CardEffectScryConsumeFortune :CardEffectScry
    {
        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
				cardEffectParams.cardManager.MoveToStandByPile(chosenCardState, wasPlayed: false, wasExhausted: true, new RemoveFromStandByCondition(() => CardPile.ExhaustedPile), new CardManager.DiscardCardParams(), HandUI.DiscardEffect.Exhausted);

                switch (chosenCardState.GetRarityType())
                {
                    case CollectableRarity.Common:
                        cardEffectParams.saveManager.AdjustGold(5, isReward: false);
                        break;
                    case CollectableRarity.Uncommon:
                        cardEffectParams.saveManager.AdjustGold(10, isReward: false);
                        break;
                    case CollectableRarity.Rare:
                        cardEffectParams.saveManager.AdjustGold(20, isReward: false);
                        break;
                    case CollectableRarity.Champion:
                        cardEffectParams.saveManager.AdjustGold(50, isReward: false);
                        break;
                    case CollectableRarity.Starter:
                        cardEffectParams.saveManager.AdjustGold(-10, isReward: false);
                        break;
                    default:
                        break;
                }

                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public override string GetTooltipBaseKey()
        {
            return "ScreenDeck_Select_CardEffectRecursion";
        }

    }
}
