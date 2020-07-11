namespace DiscipleClan.CardEffects
{
    class CardEffectScryApplyUpgrade : CardEffectScry
    {
        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
                CardUpgradeState cardUpgradeState = new CardUpgradeState();
                cardUpgradeState.Setup(cardEffectState.GetParamCardUpgradeData());

                foreach (CardUpgradeMaskData filter in cardUpgradeState.GetFilters())
                {
                    if (!filter.FilterCard(chosenCardState))
                    {
                        // If any of the filters matches, it doesn't get upgraded
                        return;
                    }
                }
                chosenCardState.GetTemporaryCardStateModifiers().AddUpgrade(cardUpgradeState);
                chosenCardState.UpdateCardBodyText();

                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public override string GetTooltipBaseKey()
        {
            return "ScreenDeck_Select_CardEffectRecursion";
        }

    }
}
