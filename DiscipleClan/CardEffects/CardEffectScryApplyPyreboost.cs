using Trainworks.Builders;

namespace DiscipleClan.CardEffects
{
    class CardEffectScryApplyPyreboost : CardEffectScry
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
                    if (!filter.FilterCard(chosenCardState, cardEffectParams.relicManager))
                    {
                        // If any of the filters matches, it doesn't get upgraded
                        return;
                    }
                }

                if (chosenCardState.GetCardType() == CardType.Monster)
                    chosenCardState.GetTemporaryCardStateModifiers().AddUpgrade(cardUpgradeState);
                if (chosenCardState.GetCardType() == CardType.Spell)
                    chosenCardState.AddTemporaryTrait(new CardTraitDataBuilder { TraitStateType = typeof(CardTraitPyreboost) }.Build(), cardEffectParams.cardManager);

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
