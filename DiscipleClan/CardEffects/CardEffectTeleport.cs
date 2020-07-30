using HarmonyLib;
using MonsterTrainModdingAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardEffectBump;

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
                chosenFloor = availableFloors[RandomManager.Range(0, availableFloors.Count, RngId.Battle)];
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
                if (target.GetTeamType() == Team.Type.Heroes && room.GetFirstEmptyHeroPoint() == null)
                    continue;

                if (target.GetTeamType() == Team.Type.Monsters && room.GetFirstEmptyMonsterPoint() == null)
                    continue;

                // It's a monster, and this is the Pyre room where we can't go.
                if (target.GetTeamType() == Team.Type.Monsters && room.GetIsPyreRoom())
                    continue;

                // Looks like this is a valid room!
                availableFloors.Add(i);
            }

            return availableFloors;
        }

        public string GetTooltipBaseKey(CardEffectState cardEffectState)
        {
            return "CardEffectTeleport_TooltipTitle";
        }
    }
}
