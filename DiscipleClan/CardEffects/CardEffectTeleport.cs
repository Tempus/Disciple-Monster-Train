using HarmonyLib;
using Trainworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardEffectBump;
using Trainworks.Managers;

namespace ShinyShoe
{
    public class CardEffectTeleport : CardEffectBase, ICardEffectStatuslessTooltip
    {
        private CardEffectBump bumper;
        
        public override void Setup(CardEffectState cardEffectState)
        {
            base.Setup(cardEffectState);
            this.bumper = new CardEffectBump();
        }

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            if (cardEffectParams.targets.Count == 0) { yield break; }

            var availableFloors = GetAvailableFloors(cardEffectParams.targets[0], cardEffectParams.roomManager);
            int chosenFloor = 0;
            if (availableFloors.Count > 0)
            {
                RngId rngId = cardEffectParams.saveManager.PreviewMode ? RngId.BattleTest : RngId.Battle;
                chosenFloor = availableFloors[RandomManager.Range(0, availableFloors.Count, rngId)];                
            }

            yield return bumper.Bump(cardEffectParams, chosenFloor - cardEffectParams.targets[0].GetCurrentRoomIndex());

            yield break;
        }

        public List<int> GetAvailableFloors(CharacterState target, RoomManager roomManager)
        {
            int currentFloor = target.GetCurrentRoomIndex();
            List<int> availableFloors = new List<int>();

            // Check all rooms to see if they're available
            for (int i = 0; i < roomManager.GetNumRooms(); i++)
            {
                var room = roomManager.GetRoom(i);

                // Room doesn't exist
                if (room == null)
                    continue;

                // Room is destroyed
                if (room.IsDestroyedOrInactive())
                    continue;

                if (!room.IsRoomEnabled())
                    continue;

                // We're already in the room
                if (i == currentFloor)
                    continue;

                // Room is full
                if (target.GetTeamType() == Team.Type.Heroes && room.GetRemainingSpawnPointCount(Team.Type.Heroes) == 0)
                    continue;

                if (target.GetTeamType() == Team.Type.Monsters && room.GetRemainingSpawnPointCount(Team.Type.Monsters) == 0)
                    continue;

                // It's a monster, and this is the Pyre room where we can't go.
                if (target.GetTeamType() == Team.Type.Monsters && room.GetIsPyreRoom())
                    continue;

                // It's the outer boss, and we're not in relentless yet, and this is the Pyre room where we can't go.
                if (target.IsOuterTrainBoss() && room.GetIsPyreRoom() && target.GetBossState().GetCurrentAttackPhase() == BossState.AttackPhase.Relentless)
                    continue;

                // Looks like this is a valid room!
                availableFloors.Add(i);
            }

            return availableFloors;
        }

        public string GetTooltipBaseKey(CardEffectState cardEffectState)
        {
            return "CardEffectTeleport";
        }
    }
}
