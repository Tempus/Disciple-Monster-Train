using HarmonyLib;
using Trainworks;
using Trainworks.Builders;
using Trainworks.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    #region Interface Definition
    [HarmonyPatch(typeof(CardManager), "OnCardPlayed")]
    class OnCardPlayedPatch
    {
        static void Prefix(CardManager __instance, CardState playCard, int selectedRoom, RoomState roomState, SpawnPoint dropLocation, CharacterState characterSummoned, List<CharacterState> targets, bool discardCard)
        {
            int roomindex = selectedRoom;
            if (roomindex == -1)
            {
                RoomManager roomManager;
                ProviderManager.TryGetProvider<RoomManager>(out roomManager);
                roomindex = roomManager.GetSelectedRoom();

                List<CharacterState> charList = new List<CharacterState>();
                ProviderManager.CombatManager.GetMonsterManager().AddCharactersInTowerToList(charList);
                foreach (var unit in charList)
                {
                    if (unit.IsChampion())
                        if (playCard.CharacterInRoomAtTimeOfCardPlay(unit))
                            roomindex = unit.GetCurrentRoomIndex();
                }
            }
            __instance.StartCoroutine(TriggerOnCardPlayed(playCard, roomindex, targets));
        }

        static IEnumerator TriggerOnCardPlayed(CardState cardState, int roomIndex, List<CharacterState> targets)
        {
            //yield return new WaitForSeconds(0.2f);

            List<CharacterState> chars = new List<CharacterState>();
            ProviderManager.CombatManager.GetMonsterManager().AddCharactersInRoomToList(chars, roomIndex);
            foreach (var unit in chars)
            {
                foreach (IRoomStateModifier roomStateModifier in unit.GetRoomStateModifiers())
                {
                    IRoomStateCardPlayedModifier roomStateCardPlayedModifier;
                    if ((roomStateCardPlayedModifier = (roomStateModifier as IRoomStateCardPlayedModifier)) != null)
                    {
                        roomStateCardPlayedModifier.OnCardPlayed(cardState, roomIndex, targets);
                    }
                }
            }

            yield break;
        }
    }

    [HarmonyPatch(typeof(CardManager), "DiscardCard")]
    class OnCardDiscardedPatch
    {
        static void Postfix(CardManager.DiscardCardParams discardCardParams, bool fromNaturalPlay)
        {
            ProviderManager.TryGetProvider(out SaveManager saveManager);
            ProviderManager.TryGetProvider(out RoomManager roomManager);
            if (saveManager == null || roomManager == null)
                return;

            if (saveManager.PreviewMode)
                return;

            RoomState room = roomManager.GetRoom(roomManager.GetSelectedRoom());
            List<CharacterState> toProcessCharacters = new List<CharacterState>();
            room.AddCharactersToList(toProcessCharacters, Team.Type.Heroes | Team.Type.Monsters);

            foreach (CharacterState toProcessCharacter in toProcessCharacters)
            {
                foreach (IRoomStateModifier roomStateModifier in toProcessCharacter.GetRoomStateModifiers())
                {
                    IRoomStateCardDiscardedAdvancedModifier mod;
                    if ((mod = roomStateModifier as IRoomStateCardDiscardedAdvancedModifier) != null)
                    {
                        mod.OnCardDiscarded(discardCardParams, roomManager.GetSelectedRoom());
                    }
                }
            }
        }
    }

    public interface IRoomStateCardPlayedModifier
    {
        void OnCardPlayed(CardState cardState, int roomIndex, List<CharacterState> targets);
    }

    public interface IRoomStateCardDiscardedAdvancedModifier
    {
        void OnCardDiscarded(CardManager.DiscardCardParams cardParams, int roomIndex);
    }
    #endregion

    #region Diligent Seraph Workaround
    [HarmonyPatch(typeof(CardManager), "RemoveTemporaryTraitFromCard")]
    class RoomStateModifierStartersConsumeRebate_DiligentSeraphFix
    {
        static bool Prefix(CardManager __instance, CardState cardState, CardTraitData cardTraitData)
        {
            // With the base value set, we now have to override the result to not remove Consume if all of the following are true:
            //  - Card already has the Consume temporary Trait
            //  - Trait Data to be removed is of type Consume
            //  - RoomStateModifierStartersConsumeRebate is present on the selected floor
            //  - The card is a starter
            // We do this because we don't want to conflict with the Discard remove consume handled via the RoomModifierData itself
            if (cardState != null && cardState.HasTrait(typeof(CardTraitExhaustState)))
            {
                if (cardTraitData != null && cardTraitData.GetTraitStateName() == "CardTraitExhaustState")
                {
                    // Trait matches, so search RoomModifierData
                    ProviderManager.TryGetProvider(out RoomManager roomManager);
                    if (roomManager == null)
                        return true;

                    RoomState room = roomManager.GetRoom(roomManager.GetSelectedRoom());
                    List<CharacterState> toProcessCharacters = new List<CharacterState>();
                    room.AddCharactersToList(toProcessCharacters, Team.Type.Heroes | Team.Type.Monsters);

                    foreach (CharacterState toProcessCharacter in toProcessCharacters)
                    {
                        foreach (IRoomStateModifier roomStateModifier in toProcessCharacter.GetRoomStateModifiers())
                        {
                            RoomStateModifierStartersConsumeRebate stateOfType;
                            if ((stateOfType = roomStateModifier as RoomStateModifierStartersConsumeRebate) != null)
                            {
                                // Room Modifier matches, so now check if it's a starter
                                if (RoomStateModifierStartersConsumeRebate.IsCardStarterSpell(cardState))
                                {
                                    // In the case where we're saying "we found a card that should still have the Consume trait", we want to add it to the list
                                    // This tells the RoomModifierData that it's a card it should be tracking now that Diligent's effect has worn off
                                    // We make sure it's not already in the list because multiple things could be played on this floor after Diligent's effect wears off
                                    if (!stateOfType.cardsWeHaveModified.Contains(cardState))
                                        stateOfType.cardsWeHaveModified.Add(cardState);

                                    // Is a starter, so override and force it to not remove the trait
                                    return stateOfType.allowRemovals;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
    #endregion

    class RoomStateModifierStartersConsumeRebate : RoomStateModifierBase, IRoomStateModifier, IRoomStateRoomSelectedModifier, IRoomStateCardManagerModifier, IRoomStateSpawnPointsChangedModifier, IRoomStateCardPlayedModifier, IRoomStateCardDiscardedAdvancedModifier
    {
        public string ID = "RoomStateModifierStartersConsumeRebate";
        public int refundAmount = 0;
        public bool allowRemovals = false;
        public List<CardState> cardsWeHaveModified = new List<CardState>();

        public RoomManager roomManager;
        public PlayerManager playerManager;
        public CardManager cardManager;

        public bool CanApplyInPreviewMode => false;

        public static bool IsCardStarterSpell(CardState card)
        {
            foreach (var clan in ProviderManager.SaveManager.GetAllGameData().GetAllClassDatas())
            {
                for (int i = 0; i < 2; i++)
                {
                    // If we find any match, return true
                    if (card.GetCardDataID() == clan.GetChampionData(i).starterCardData.GetID() && card.GetCardType() == CardType.Spell)
                        return true;
                }
            }
            return false;
        }

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            refundAmount = roomModifierData.GetParamInt();
            this.roomManager = roomManager;
            ProviderManager.TryGetProvider<PlayerManager>(out playerManager);
            this.cardManager = GameObject.FindObjectOfType<CardManager>().GetComponent<CardManager>() as CardManager;
        }

        public void OnCardPlayed(CardState cardState, int roomIndex, List<CharacterState> targets)
        {
            // Perform the Rebate first
            if (cardState.HasTrait(typeof(CardTraitExhaustState)))
            {
                int refund = refundAmount;
                int originalCost = cardState.GetCostWithoutAnyModifications();

                // Check if the exhausted card was X-cost
                if (cardState.IsConsumeRemainingEnergyCostType())
                {
                    CardStatistics.StatValueData statValueData = default(CardStatistics.StatValueData);
                    statValueData.cardState = cardState;
                    statValueData.trackedValue = CardStatistics.TrackedValueType.PlayedCost;
                    statValueData.entryDuration = CardStatistics.EntryDuration.ThisBattle;
                    originalCost = ProviderManager.CombatManager.GetCardManager().GetCardStatistics().GetStatValue(statValueData);
                }

                if (originalCost < refund)
                    refund = originalCost;
                
                playerManager.AddEnergy(refund);
            }

            // Check all cards in-hand and add any new entries to the tracked list
            foreach (var card in cardManager.GetHand())
            {
                cardManager.StartCoroutine(ApplyConsumeToCard(card));
            }
        }

        public void OnCardDiscarded(CardManager.DiscardCardParams cardParams, int roomIndex)
        {
            // We handle played cards elsewhere
            if (!cardParams.wasPlayed)
            {
                cardManager.StartCoroutine(RemoveConsumeFromCard(cardParams.discardCard, true));
            }
        }

        public new string GetExtraTooltipBodyKey()
        {
            return string.Empty;
        }

        public new string GetExtraTooltipTitleKey()
        {
            return string.Empty;
        }

        public void CardDrawn(CardState cardState)
        {
            cardManager.StartCoroutine(ApplyConsumeToCard(cardState));
        }

        public void CardDiscarded(CardState cardState)
        {
            // We do everything in OnCardDiscarded instead due to better argument availability
        }

        public IEnumerator ApplyConsumeToCard(CardState card)
        {
            yield return new WaitForSeconds(0.1f);

            // If the card already has Consume for any reason, we pass on modifying it and will try again after we play a card
            if (!cardsWeHaveModified.Contains(card) && !card.HasTrait(typeof(CardTraitExhaustState)) && IsCardStarterSpell(card))
            {
                cardManager.AddTemporaryTraitToCard(card, new CardTraitData { traitStateName = "CardTraitExhaustState" });
                cardManager.RefreshCardInHand(card);
                card.RefreshCardBodyTextLocalization();
                cardsWeHaveModified.Add(card);
            }
            yield break;
        }

        public IEnumerator RemoveConsumeFromCard(CardState card, bool doRemove)
        {
            if (cardsWeHaveModified.Contains(card))
            {
                allowRemovals = true;
                cardManager.RemoveTemporaryTraitFromCard(card, new CardTraitData { traitStateName = "CardTraitExhaustState" });
                allowRemovals = false;
                cardManager.RefreshCardInHand(card);
                card.RefreshCardBodyTextLocalization();

                if (doRemove)
                    cardsWeHaveModified.Remove(card);
            }
            yield break;
        }

        public void RoomSelectionChanged(bool roomSelected, CardManager cardManager)
        {
            if (roomSelected)
            {
                foreach (var card in cardManager.GetHand())
                {
                    cardManager.StartCoroutine(ApplyConsumeToCard(card));
                }
            }
            else
            {
                foreach (var card in cardsWeHaveModified)
                {
                    cardManager.StartCoroutine(RemoveConsumeFromCard(card, false));
                }
                cardsWeHaveModified.Clear();
            }
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