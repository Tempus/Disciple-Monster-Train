using HarmonyLib;

namespace DiscipleClan.CardEffects
{
    class CardEffectScryCopy : CardEffectScry
    {
        public override string DescriptionKey { get { return "ScryCopyInstructions"; } }

        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
                CardData cardData = cardEffectParams.allGameData.FindCardData(chosenCardState.GetCardDataID());
                if (cardData.GetID() == cardEffectParams.playedCard.GetCardDataID())
                {
                    Traverse.Create(cardEffectParams.cardManager).Field<HandUI>("handUI").Value.ShowCardSelectionErrorMessage("Revelation can not copy itself.", useCenterPositioning: true);
                    return;
                }
                
                CardManager.AddCardUpgradingInfo addCardUpgradingInfo = new CardManager.AddCardUpgradingInfo();

                addCardUpgradingInfo.ignoreTempUpgrades = true;
                addCardUpgradingInfo.copyModifiersFromCard = chosenCardState;
                cardEffectParams.cardManager.AddCard(cardData, CardPile.HandPile, 1, 1, fromRelic: false, permanent: false, addCardUpgradingInfo);

                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public override string GetTooltipBaseKey()
        {
            return "ScryCopyTooltip";
        }

    }
}
