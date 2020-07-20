using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RoomStateModifierRefund : RoomStateModifierBase, IRoomStateModifier, IRoomStateCardPlayedModifier
    {
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
            API.Log(BepInEx.Logging.LogLevel.All, "Refunding function for " + cardState.GetDebugName());
            if (cardState.HasTrait(typeof(CardTraitExhaustState)))
            {
                API.Log(BepInEx.Logging.LogLevel.All, "We exhaust.");
                int refund = refundAmount;
                if (cardState.GetCostWithoutAnyModifications() < refund)
                {
                    refund = cardState.GetCostWithoutAnyModifications();
                }

                API.Log(BepInEx.Logging.LogLevel.All, "Refunding " + refund + " for " + cardState.GetDebugName());
                playerManager.AddEnergy(refund);
            }
        }

        public new string GetDescriptionKey()
        {
            return ID + "_Desc";
        }
    }

    [HarmonyPatch(typeof(CardManager), "OnCardPlayed")]
    class OnCardPlayedPatch
    {
        static void Prefix(CardManager __instance, CardState playCard, int selectedRoom, RoomState roomState, SpawnPoint dropLocation, CharacterState characterSummoned, List<CharacterState> targets, bool discardCard)
        {
            //if (ProviderManager.SaveManager.PreviewMode) { return; }
            API.Log(BepInEx.Logging.LogLevel.All, "We played " + playCard.GetDebugName());

            int roomindex = selectedRoom;
            if (roomindex == -1)
            {
                RoomManager roomManager;
                ProviderManager.TryGetProvider<RoomManager>(out roomManager);
                roomindex = roomManager.GetSelectedRoom();
            }
            __instance.StartCoroutine(TriggerOnCardPlayed(playCard, roomindex, targets));
        }

        static IEnumerator TriggerOnCardPlayed(CardState cardState, int roomIndex, List<CharacterState> targets)
        {
            API.Log(BepInEx.Logging.LogLevel.All, "Waiting");

            yield return new WaitForSeconds(0.2f);
            API.Log(BepInEx.Logging.LogLevel.All, "Here we go!");

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
        }
    }

    public interface IRoomStateCardPlayedModifier
    {
        void OnCardPlayed(CardState cardState, int roomIndex, List<CharacterState> targets);
    }

}