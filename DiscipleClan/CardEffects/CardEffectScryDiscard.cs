using System.Collections.Generic;
using static CardManager;

namespace DiscipleClan.CardEffects
{
    class CardEffectScryDiscard : CardEffectScry
    {
        public override string DescriptionKey { get { return "ScryDiscardInstructions"; } }

        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
                //cardEffectParams.cardManager.MoveToStandByPile(chosenCardState, wasPlayed: false, wasExhausted: false, new RemoveFromStandByCondition(() => CardPile.DiscardPile), new CardManager.DiscardCardParams(), HandUI.DiscardEffect.Default);

                cardEffectParams.cardManager.GetDrawPile().Remove(chosenCardState);
                cardEffectParams.cardManager.GetDiscardPile().Add(chosenCardState);

                cardEffectParams.relicManager.ApplyOnPostDiscardRelicEffects(chosenCardState);
                IEnumerable<CardTraitState> traitStates = chosenCardState.GetTraitStates();
                foreach (CardTraitState item in traitStates)
                {
                    item.OnCardDiscarded(new DiscardCardParams { discardCard = chosenCardState }, cardEffectParams.cardManager, cardEffectParams.relicManager, cardEffectParams.combatManager, cardEffectParams.roomManager, cardEffectParams.saveManager);
                }
                cardEffectParams.roomManager.GetRoom(cardEffectParams.roomManager.GetSelectedRoom()).UpdateCardManagerRoomStateModifiers(chosenCardState, drawn: false);
                chosenCardState.OnCardDiscarded();

                // I hate private funcitons so much
                int discardCount = cardEffectParams.cardManager.GetDiscardPile().Count;
                CardPileInformation cardPileInformation = default(CardPileInformation);
                cardPileInformation.deckCount = cardEffectParams.cardManager.GetDrawPile().Count;
                cardPileInformation.handCount = cardEffectParams.cardManager.GetHand().Count;
                cardPileInformation.discardCount = discardCount;
                cardPileInformation.exhaustedCount = cardEffectParams.cardManager.GetExhaustedPile().Count;
                cardPileInformation.eatenCount = cardEffectParams.cardManager.GetEatenPile().Count;
                CardPileInformation type = cardPileInformation;
                cardEffectParams.cardManager.cardPilesChangedSignal.Dispatch(type);


                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public override string GetTooltipBaseKey()
        {
            return "ScryDiscardTooltip";
        }

    }
}
