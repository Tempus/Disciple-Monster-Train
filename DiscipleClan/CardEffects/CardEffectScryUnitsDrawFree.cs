using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using System.Collections;
using System.Collections.Generic;

namespace DiscipleClan.CardEffects
{
    class CardEffectScryUnitsDrawFree : CardEffectScry
    {
        public override IEnumerator HandleChooseCard(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            List<CardState> toProcessCards = new List<CardState>();
            toProcessCards.AddRange(cardEffectParams.cardManager.GetDrawPile());

            List<CardState> scryedCards = new List<CardState>();

            int num = 0;
            int intInRange = cardsToScry;
            for (int i = 0; i < toProcessCards.Count; i++)
            {
                if (num >= intInRange)
                {
                    break;
                }
                scryedCards.Add(toProcessCards[i]);
            }
            List<CardState> drawPile = cardEffectParams.cardManager.GetDrawPile();
            drawPile.AddRange(cardEffectParams.cardManager.GetDiscardPile());

            foreach (var card in drawPile)
            {
                if (card.GetID() == "ChosenOne")
                {
                    if (!scryedCards.Contains(card))
                        scryedCards.Add(card);
                }
            }

            // Generate the Scryed Cards
            // Param Int -> Cards to scry, Additional Param Int -> cards to choose

            cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, true, screen =>
            {
                DeckScreen deckScreen = screen as DeckScreen;
                deckScreen.Setup(new DeckScreen.Params()
                {
                    mode = DeckScreen.Mode.CardSelection,
                    showCancel = true,
                    targetMode = TargetMode.Deck,
                    titleKey = cardEffectState.GetParentCardState().GetTitleKey(),
                    instructionsKey = GetTooltipBaseKey(),
                    numCardsSelectable = cardEffectState.GetAdditionalParamInt(),
                });

                // Reset the card List to the scryed cards
                AccessTools.Field(typeof(DeckScreen), "cardStates").SetValue(screen, scryedCards);
                AccessTools.Method(typeof(DeckScreen), "ApplyStateToCachedCards").Invoke(screen, new object[] { scryedCards, cardEffectParams.cardManager.GetCardStatistics(), cardEffectParams.cardManager.GetMonsterManager(), cardEffectParams.cardManager.GetHeroManager(), null });

                AddDelegate(cardEffectState, cardEffectParams, deckScreen);
            });

            yield break;
        }

        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
                cardEffectParams.cardManager.DrawSpecificCard(chosenCardState, 0.0f, this.GetDrawSource(cardEffectState.GetTargetMode()), cardEffectParams.playedCard, 1, 1);
                chosenCardState.AddTemporaryTrait(new CardTraitDataBuilder { TraitStateName = "CardTraitFreebie", ParamInt = -99 }.Build(), cardEffectParams.cardManager);
                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }
        public override string GetTooltipBaseKey()
        {
            return "ScreenDeck_Select_CardEffectRecursion";
        }
    }
}
