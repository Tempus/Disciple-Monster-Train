using Trainworks.Managers;
using System.Collections;
using System.Collections.Generic;
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
          bool fromPlayedCard,
          CombatManager.ApplyPreEffectsVfxAction onPreEffectsFiredVfx,
          CombatManager.ApplyEffectsAction onEffectsFired)
        {
            if (cardState.GetCardType() != CardType.Spell) { return; }

            // Gotta check and make sure it's not a consume or purge card. Avoid crashes
            RelicManager relicManager;
            ProviderManager.TryGetProvider<RelicManager>(out relicManager);
            if (cardState.GetDiscardEffectWhenPlayed(relicManager, null) != HandUI.DiscardEffect.Default) { return; }

            // Make sure we're on the same floor as the Owner
            MonsterManager monsterManager;
            ProviderManager.TryGetProvider<MonsterManager>(out monsterManager);
            List<CharacterState> units = new List<CharacterState>();
            monsterManager.AddCharactersInRoomToList(units, roomIndex);

            bool ret = true;
            foreach (var unit in units)
            {
                foreach (var mod in unit.GetRoomStateModifiers())
                {
                    if (mod is RoomStateModifierRewind)
                    {
                        ret = false;
                    }
                }
            }
            if (ret) { return; }

            // Get the provider and do the effect
            CardManager cardManager;
            ProviderManager.TryGetProvider<CardManager>(out cardManager);

            if (cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Spell) < numOfCards)
            {
                var corountine = RedrawCard(cardState, cardManager.GetHand().Count);
                cardManager.StartCoroutine(corountine);
            }
        }
        public static IEnumerator RedrawCard(CardState cardState, int cardIndex)
        {
            yield return new WaitForSeconds(0.2f);

            CardManager cardManager;
            ProviderManager.TryGetProvider<CardManager>(out cardManager);
            cardManager.DrawSpecificCard(cardState, 0f, HandUI.DrawSource.Discard, cardState, cardIndex);
            cardManager.GetDiscardPile().Remove(cardState);
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