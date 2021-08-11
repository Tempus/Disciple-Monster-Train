﻿using Trainworks.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;

namespace DiscipleClan.CardEffects
{
    public sealed class CardEffectChooseDraw : CardEffectBase
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
            var pileDiscard = Traverse.Create(cardEffectParams.cardManager).Field<List<CardState>>("pileDiscard");
            var pileDeck = Traverse.Create(cardEffectParams.cardManager).Field<List<CardState>>("pileDeck");

            if (pileDeck.Value.Count == 0)
            {
                List<CardState> list = pileDeck.Value;
                pileDeck.Value = pileDiscard.Value;
                pileDiscard.Value = list;
                cardEffectParams.cardManager.ShuffleDeck();
                ProviderManager.TryGetProvider<SoundManager>(out SoundManager soundManager);
                soundManager.PlaySfx("UI_PlayCardShuffle");
            }
            if (pileDeck.Value.Count > 0)
                yield return (object)this.HandleChooseCard(cardEffectState, cardEffectParams);

            yield break;
        }

        private IEnumerator HandleChooseCard(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, true, (ScreenManager.ScreenActiveCallback)(screen =>
            {
                DeckScreen deckScreen = screen as DeckScreen;
                deckScreen.Setup(new DeckScreen.Params()
                {
                    mode = DeckScreen.Mode.CardEffectSelection,
                    targetMode = cardEffectState.GetTargetMode(),
                    showCancel = false,
                    titleKey = cardEffectState.GetParentCardState().GetTitleKey(),
                    instructionsKey = "SeekCopyInstructions",
                    numCardsSelectable = cardEffectState.GetParamInt(),
                });
                deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
                {
                    cardEffectParams.cardManager.DrawSpecificCard(chosenCardState, 0.0f, this.GetDrawSource(cardEffectState.GetTargetMode()), cardEffectParams.playedCard, 1, 1);
                    cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
                }));
            }));
            yield break;
        }

        //public string GetTooltipBaseKey(CardEffectState cardEffectState)
        //{
        //    return "Choose a card from your deck to draw to your hand.";
        //}

        private HandUI.DrawSource GetDrawSource(TargetMode targetMode)
        {
            switch (targetMode)
            {
                case TargetMode.Discard:
                    return HandUI.DrawSource.Discard;
                case TargetMode.Exhaust:
                    return HandUI.DrawSource.Consume;
                case TargetMode.DrawPile:
                    return HandUI.DrawSource.Deck;
                default:
                    return HandUI.DrawSource.Deck;
            }
        }
    }
}
