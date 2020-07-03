using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Enums.MTTriggers;
using MonsterTrainModdingAPI.Managers;
using UnityEngine;

namespace DiscipleClan.Cards.Triggers
{
    class MTCharacterTrigger_Relocate : MTCharacterTrigger<MTCharacterTrigger_Relocate> { }

    // Gotta patch in places to call this trigger from... gonna be OnSpawnPointChange

    [HarmonyPatch(typeof(CharacterState), "MoveUpDownTrain")]
    class QueueRelocate
    {
        static void Postfix(CharacterState __instance)
        {
            API.Log(BepInEx.Logging.LogLevel.All, "Moving to floor: " + __instance.GetCurrentRoomIndex());

            //CustomTriggerManager.QueueAndRunTrigger<MTCharacterTrigger_Relocate>(
            //    characterState,
            //    true,
            //    true,
            //    new CharacterState.FireTriggersData { 
            //        paramInt = newPoint.GetRoomOwner().GetRoomIndex(), // Destination Room?
            //    }, 1);

            CustomTriggerManager.QueueTrigger<MTCharacterTrigger_Relocate>(
                __instance,
                true,
                true,
                new CharacterState.FireTriggersData
                {
                    paramInt = __instance.GetCurrentRoomIndex(), // Destination Room?
                }, 
                1);

            List<CharacterState> chars = new List<CharacterState>();
            __instance.GetCharacterManager().AddCharactersInRoomToList(chars, __instance.GetCurrentRoomIndex());
            foreach (var unit in chars)
            {
                if (unit == __instance) { continue; }
                foreach (IRoomStateModifier roomStateModifier in unit.GetRoomStateModifiers())
                {
                    IRoomStateSpawnPointsModifiedModifier roomStateSpawnPointsChangedModifier;
                    if ((roomStateSpawnPointsChangedModifier = (roomStateModifier as IRoomStateSpawnPointsModifiedModifier)) != null)
                    {
                        roomStateSpawnPointsChangedModifier.SpawnPointModifier(__instance);
                    }
                }
            }
        }
    }
    public interface IRoomStateSpawnPointsModifiedModifier
    {
        void SpawnPointModifier(CharacterState characterState);
    }
}