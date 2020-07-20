﻿namespace DiscipleClan.CardEffects
{
    class CardEffectScryDiscard : CardEffectScry
    {
        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
                cardEffectParams.cardManager.MoveToStandByPile(chosenCardState, wasPlayed: false, wasExhausted: false, new RemoveFromStandByCondition(() => CardPile.DiscardPile), new CardManager.DiscardCardParams(), HandUI.DiscardEffect.Default);
                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public override string GetTooltipBaseKey()
        {
            return "ScreenDeck_Select_CardEffectRecursion";
        }

    }
}