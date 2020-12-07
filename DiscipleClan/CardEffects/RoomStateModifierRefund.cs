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
    class RoomStateModifierRefund : RoomStateModifierBase, IRoomStateModifier, IRoomStateCardPlayedModifier
    {
        // IRoomStateCardPlayedModifier is defined in RoomStateModifierStartersConsumeRebate

        public string ID = "RoomModifierRefund";
        public int refundAmount = 0;
        public RoomManager roomManager;
        public PlayerManager playerManager;
        public CardManager cardManager;

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
            if (cardState.HasTrait(typeof(CardTraitExhaustState)))
            {
                int refund = refundAmount;
                int originalCost = cardState.GetCostWithoutAnyModifications();

                if (cardState.IsConsumeRemainingEnergyCostType())
                {
                    CardStatistics.StatValueData statValueData = default(CardStatistics.StatValueData);
                    statValueData.cardState = cardState;
                    statValueData.trackedValue = CardStatistics.TrackedValueType.PlayedCost;
                    statValueData.entryDuration = CardStatistics.EntryDuration.ThisBattle;
                    originalCost = ProviderManager.CombatManager.GetCardManager().GetCardStatistics().GetStatValue(statValueData);
                }

                if (originalCost < refund)
                {
                    refund = originalCost;
                }

                playerManager.AddEnergy(refund);
            }
        }

        public new string GetDescriptionKey()
        {
            return ID + "_Desc";
        }
    }
}