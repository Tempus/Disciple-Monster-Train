using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DiscipleClan.Cards.CardEffects
{
    class RoomStateModifierRewind : RoomStateModifierBase, IRoomStateModifier
    {
        public CardManager cardManager;
        public int numOfCards;
        public List<CardState> storedCards = new List<CardState>();

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            this.numOfCards = roomModifierData.GetParamInt();
            this.cardManager = GameObject.FindObjectOfType<CardManager>().GetComponent<CardManager>() as CardManager;
            this.cardManager.OnCardPlayedCallback += new CardManager.OnCardPlayedEvent(this.OnPlayedCard);
        }

        private void OnPlayedCard(
              CardState cardState,
              int roomIndex,
              SpawnPoint dropLocation,
              CombatManager.ApplyPreEffectsVfxAction onPreEffectsFiredVfx,
              CombatManager.ApplyEffectsAction onEffectsFired)
        {
            cardManager = GameObject.FindObjectOfType<CardManager>().GetComponent<CardManager>() as CardManager;
            API.Log(BepInEx.Logging.LogLevel.All, "Rewind: Card Discarded");

            if (cardState.GetCardType() != CardType.Spell) { return; }

            if (cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Spell) +
                cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Monster) <= numOfCards)
            {
                API.Log(BepInEx.Logging.LogLevel.All, "Rewind: Queing the action");
                //storedCards.Add(cardState);
                cardManager.DrawSpecificCard(cardState, 0.2f, HandUI.DrawSource.Discard, cardState);
                cardManager.RefreshHandCards();
            }
        }

        public void DrawStoredCard()
        {
            //cardManager.DrawSpecificCard(cardState);
        }
        new public string GetDescriptionKey()
        {
            return "RoomStateModifierRelocateRewind_Desc";
        }

        new public string GetExtraTooltipTitleKey()
        {
            return "RoomStateModifierRelocateRewind_TooltipTitle";
        }

        new public string GetExtraTooltipBodyKey()
        {
            return "RoomStateModifierRelocateRewind_TooltipBody";
        }


    }
}