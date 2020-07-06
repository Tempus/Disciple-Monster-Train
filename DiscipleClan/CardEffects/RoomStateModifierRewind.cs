using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RoomStateModifierRewind : RoomStateModifierBase, IRoomStateModifier
    {
        public static int numOfCards = 0;
        public List<CardState> storedCards = new List<CardState>();
        public CardManager.OnCardPlayedEvent callback = new CardManager.OnCardPlayedEvent(OnPlayedCard);

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
        
            numOfCards = roomModifierData.GetParamInt();

            var cardManager = GameObject.FindObjectOfType<CardManager>().GetComponent<CardManager>() as CardManager;
            cardManager.OnCardPlayedCallback -= callback;
            cardManager.OnCardPlayedCallback += callback;
        }

        public static void OnPlayedCard(
          CardState cardState,
          int roomIndex,
          SpawnPoint dropLocation,
          CombatManager.ApplyPreEffectsVfxAction onPreEffectsFiredVfx,
          CombatManager.ApplyEffectsAction onEffectsFired)
        {
            if (cardState.GetCardType() != CardType.Spell) { return; }

            // Gotta check and make sure it's not a consume or purge card. Avoid crashes
            RelicManager relicManager;
            ProviderManager.TryGetProvider<RelicManager>(out relicManager);
            if (cardState.GetDiscardEffectWhenPlayed(relicManager, null) != HandUI.DiscardEffect.Default) { return; }

            // Get the provider
            CardManager cardManager;
            ProviderManager.TryGetProvider<CardManager>(out cardManager);

            if (cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Spell) < numOfCards)
            {
                var corountine = WaitAndPrint(cardState);
                cardManager.StartCoroutine(corountine);
            }
        }
        public static IEnumerator WaitAndPrint(CardState cardState)
        {
            yield return new WaitForSeconds(0.2f);

            CardManager cardManager;
            ProviderManager.TryGetProvider<CardManager>(out cardManager);
            cardManager.DrawSpecificCard(cardState, 0f, HandUI.DrawSource.Discard, cardState);
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