using DiscipleClan.Cards.Prophecy;
using DiscipleClan.Triggers;
using HarmonyLib;
using Trainworks.Builders;
using Trainworks.Managers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DiscipleClan.CardEffects
{
    class CardEffectScry : CardEffectBase
    {
        public CardEffectData.CardSelectionMode cardSelectionMode;
        public int cardsToScry = 0;

        public virtual string DescriptionKey { get { return "ScryInstructions"; } }

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
                cardsToScry = cardEffectState.GetParamInt();
                if (cardEffectState.GetParamBool())
                {
                    int empowerMultiplier = cardEffectState.GetParamInt();
                    if (empowerMultiplier == 0)
                        empowerMultiplier = 1;

                    cardsToScry = cardEffectParams.playerManager.GetEnergy() * empowerMultiplier;
                }

                // Fire the trigger!
                MonsterManager monsterManager;
                ProviderManager.TryGetProvider<MonsterManager>(out monsterManager);

                List<CharacterState> units = new List<CharacterState>();
                monsterManager.AddCharactersInTowerToList(units);

                CustomTriggerManager.QueueAndRunTrigger(OnDivine.OnDivineCharTrigger, units.ToArray(), true, true, null, 1);

                CardManager cardManager;
                ProviderManager.TryGetProvider<CardManager>(out cardManager);
                foreach (var card in cardManager.GetHand())
                {
                    CustomTriggerManager.FireCardTriggers(OnDivine.OnDivineCardTrigger, card, -1, true, null, 1, null);
                }

                foreach (var relic in ProviderManager.SaveManager.GetCollectedRelics())
                {
                    foreach (var effect in relic.GetEffectsOfType<RelicEffectEmberOnDivine>())
                    {
                        if (effect == null)
                            continue;
                        effect.OnDivine();
                    }
                }

                // Check for the relic!
                if (cardEffectParams.saveManager.GetRelicCount("SeersBoostDivine") > 0)
                {
                    foreach (var card in cardManager.GetAllCards())
                    {
                        if (card.GetSpawnCharacterData() != null)
                        { 
                            foreach (var subtype in card.GetSpawnCharacterData().GetSubtypes())
                            {
                                if (subtype.Key == "ChronoSubtype_Seer")
                                    cardsToScry++;
                            }
                        }
                    }
                }

                yield return (object)this.HandleChooseCard(cardEffectState, cardEffectParams);
            }
        }

        public virtual IEnumerator HandleChooseCard(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            // Generate the Scryed Cards
            // Param Int -> Cards to scry, Additional Param Int -> cards to choose
            List<CardState> drawPile = cardEffectParams.cardManager.GetDrawPile();
            List<CardState> scryedCards = new List<CardState>();
            CardState priorityUnitDraw = DivinePriorityUnit(drawPile);

            // Adjust Divine for priority draw
            if (priorityUnitDraw != null)
                scryedCards.Add(priorityUnitDraw);

            // Draw Piles are dumb?
            drawPile.Reverse();

            // Draw normally
            int drawSize = scryedCards.Count;
            for (int i = 0; i < Math.Min(cardsToScry - drawSize, drawPile.Count); i++)
            {
                if (drawPile[i] != priorityUnitDraw)
                    scryedCards.Add(drawPile[i]);
            }

            // Draw Piles are dumb? Yep
            drawPile.Reverse();

            // Hardcoded Divine interaction to always divine a card with ID "ChosenOne"
            foreach (var card in drawPile)
            {
                if (card.GetID() == "ChosenOne")
                {
                    if (!scryedCards.Contains(card))
                        scryedCards.Add(card);
                }
            }
            foreach (var card in cardEffectParams.cardManager.GetDiscardPile())
            {
                if (card.GetID() == "ChosenOne")
                {
                    if (!scryedCards.Contains(card))
                        scryedCards.Add(card);
                }
            }

            cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, true, screen =>
            {
                DeckScreen deckScreen = screen as DeckScreen;
                deckScreen.Setup(new DeckScreen.Params()
                {
                    mode = DeckScreen.Mode.CardEffectSelection,
                    showCancel = true,
                    targetMode = TargetMode.DrawPile,
                    titleKey = cardEffectState.GetParentCardState().GetTitleKey(),
                    instructionsKey = DescriptionKey,
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

        public CardState DivinePriorityUnit(List<CardState> drawPile)
        {
            for (int i = 0; i < drawPile.Count; i++)
            {
                CardState cardState = drawPile[i];
                CharacterData spawnCharacterData = cardState.GetSpawnCharacterData();
                if (!(spawnCharacterData != null))
                {
                    continue;
                }
                foreach (SubtypeData subtype in spawnCharacterData.GetSubtypes())
                {
                    if (subtype.Key == "SubtypesData_Chosen")
                    {
                        return cardState;
                    }
                }
            }
            return null;
        }

        public HandUI.DrawSource GetDrawSource(TargetMode targetMode)
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
