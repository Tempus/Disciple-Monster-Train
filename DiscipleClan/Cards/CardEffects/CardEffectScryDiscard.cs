namespace DiscipleClan.Cards.CardEffects
{
    class CardEffectScryDiscard :CardEffectScry
    {
        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
                cardEffectParams.cardManager.DiscardCard(new CardManager.DiscardCardParams { 
                    discardCard = chosenCardState,
                    triggeredByCard = true,
                    triggeredCard = cardEffectParams.playedCard,
                    wasPlayed = false,
            });
                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public override string GetTooltipBaseKey()
        {
            return "ScreenDeck_Select_CardEffectRecursion";
        }

    }
}
