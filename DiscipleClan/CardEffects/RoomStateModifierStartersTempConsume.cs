using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RoomStateModifierStartersTempConsume : RoomStateModifierBase, IRoomStateModifier, IRoomStateRoomSelectedModifier, IRoomStateCardManagerModifier, IRoomStateSpawnPointsChangedModifier
    {
        public string ID = "RoomStateModifierStartersTempConsume";
        public List<CardState> cardsWeHaveModified = new List<CardState>();
        CardManager cardManager;

        public bool CanApplyInPreviewMode => false;

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            ProviderManager.TryGetProvider<CardManager>(out cardManager);
        }

        public void RoomSelectionChanged(bool roomSelected, CardManager cardManager)
        {
            if (roomSelected)
            {
                foreach (var card in cardManager.GetHand())
                {
                    cardManager.StartCoroutine(ApplyConsume(card));
                }
            }
            else
            {
                cardManager.StartCoroutine(RemoveConsume());
            }
        }

        public IEnumerator ApplyConsume(CardState card)
        {
            yield return new WaitForSeconds(0.01f);

            if ((card.GetRarityType() == CollectableRarity.Starter || card.GetDebugName().Contains("Starter")) && card.GetCardType() == CardType.Spell && !card.HasTrait(typeof(CardTraitExhaustState)))
            {
                cardManager.AddTemporaryTraitToCard(card, new CardTraitData { traitStateName = "CardTraitExhaustState" });
                cardManager.RefreshCardInHand(card);
                card.RefreshCardBodyTextLocalization();
                cardsWeHaveModified.Add(card);
            }

            yield break;
        }

        public new string GetDescriptionKey()
        {
            return ID + "_Desc";
        }

        public void CardDrawn(CardState cardState)
        {
            cardManager.StartCoroutine(ApplyConsume(cardState));
        }

        public void CardDiscarded(CardState cardState)
        {
        }

        public IEnumerator RemoveConsume()
        {
            //yield return new WaitForSeconds(0.2f);

            foreach (var card in cardsWeHaveModified)
            {
                cardManager.RemoveTemporaryTraitFromCard(card, new CardTraitData { traitStateName = "CardTraitExhaustState" });
                cardManager.RefreshCardInHand(card);
                card.RefreshCardBodyTextLocalization();
            }
            cardsWeHaveModified.Clear();
            yield break;
        }

        public void SpawnPointChanged(CharacterState characterState, SpawnPoint prevPoint, SpawnPoint newPoint, CardManager cardManager)
        {
            bool newSelected = newPoint?.GetRoomOwner().GetSelected() ?? false;
            bool oldIsSelected = prevPoint?.GetRoomOwner().GetSelected() ?? false;
            
            if (newSelected)
            {
                RoomSelectionChanged(true, cardManager);
            }
            else if (oldIsSelected || ((object)characterState != null && characterState.IsDead))
            {
                RoomSelectionChanged(false, cardManager);
            }
        }
    }
}