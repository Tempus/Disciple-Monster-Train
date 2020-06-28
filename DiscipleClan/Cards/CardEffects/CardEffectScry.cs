using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DiscipleClan.Cards.CardEffects
{
    class CardEffectScry : CardEffectBase
    {
        public CardEffectData.CardSelectionMode cardSelectionMode;

        public override bool CanPlayAfterBossDead
        {
            get
            {
                return false;
            }
        }

        public override bool CanApplyInPreviewMode
        {
            get
            {
                return this.cardSelectionMode == CardEffectData.CardSelectionMode.RandomToRoom;
            }
        }

        public override bool TestEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            this.cardSelectionMode = cardEffectState.GetTargetCardSelectionMode();
            return base.TestEffect(cardEffectState, cardEffectParams);
        }

        public override IEnumerator ApplyEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            if (cardEffectParams.targetCards.Count > 0)
            {
                yield return (object)this.HandleChooseCard(cardEffectState, cardEffectParams);
            }
        }

        private IEnumerator HandleChooseCard(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            // Generate the Scryed Cards
            // Param Int -> Cards to scry, Additional Param Int -> cards to choose
            List<CardState> drawPile = cardEffectParams.cardManager.GetDrawPile();
            var scryedCards = drawPile.Skip(drawPile.Count - cardEffectState.GetParamInt()).Take(cardEffectState.GetParamInt()).ToList();

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

        public virtual void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
                cardEffectParams.cardManager.DrawSpecificCard(chosenCardState, 0.0f, this.GetDrawSource(cardEffectState.GetTargetMode()), cardEffectParams.playedCard, 1, 1);
                chosenCardState.AddTemporaryTrait(new CardTraitDataBuilder { TraitStateName = "CardTraitExhaustState" }.Build(), cardEffectParams.cardManager);
                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public virtual string GetTooltipBaseKey()
        {
            return "ScreenDeck_Select_CardEffectRecursion";
        }

        private HandUI.DrawSource GetDrawSource(TargetMode targetMode)
        {
            switch (targetMode)
            {
                case TargetMode.Discard:
                    return HandUI.DrawSource.Discard;
                case TargetMode.Exhaust:
                    return HandUI.DrawSource.Consume;
                case TargetMode.Deck:
                    return HandUI.DrawSource.Deck;
                default:
                    return HandUI.DrawSource.Deck;
            }
        }
    }
}
