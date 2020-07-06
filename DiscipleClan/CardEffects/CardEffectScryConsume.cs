namespace DiscipleClan.CardEffects
{
    class CardEffectScryConsume :CardEffectScry
    {
        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
				cardEffectParams.cardManager.MoveToStandByPile(chosenCardState, wasPlayed: false, wasExhausted: true, new RemoveFromStandByCondition(() => CardPile.ExhaustedPile), new CardManager.DiscardCardParams(), HandUI.DiscardEffect.Exhausted);
                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public override string GetTooltipBaseKey()
        {
            return "ScreenDeck_Select_CardEffectRecursion";
        }

    }
}
