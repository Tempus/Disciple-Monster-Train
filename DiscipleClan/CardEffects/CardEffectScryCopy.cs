namespace DiscipleClan.CardEffects
{
    class CardEffectScryCopy :CardEffectScry
    {
        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
                CardData cardData = cardEffectParams.allGameData.FindCardData(chosenCardState.GetCardDataID());
                CardManager.AddCardUpgradingInfo addCardUpgradingInfo = new CardManager.AddCardUpgradingInfo();

                addCardUpgradingInfo.ignoreTempUpgrades = false;
                addCardUpgradingInfo.copyModifiersFromCard = chosenCardState;
                cardEffectParams.cardManager.AddCard(cardData, CardPile.HandPile, 1, 1, fromRelic: false, permanent: false, addCardUpgradingInfo);

                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public override string GetTooltipBaseKey()
        {
            return "ScreenDeck_Select_CardEffectRecursion";
        }

    }
}
