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

namespace DiscipleClan.Cards.Triggers
{
    class MTCharacterTrigger_Relocate : MTCharacterTrigger<MTCharacterTrigger_Relocate> { }

    // Gotta patch in places to call this trigger from... gonna be OnSpawnPointChange

    [HarmonyPatch(typeof(RoomManager), "OnSpawnPointChanged")]
    class QueueRelocate
    {
        static void Prefix(RoomManager __instance, CharacterState characterState, SpawnPoint prevPoint, SpawnPoint newPoint)
        {
            if (newPoint == null) { return; }

            API.Log(BepInEx.Logging.LogLevel.All, "Moving to floor: " + newPoint.GetRoomOwner().GetRoomIndex());

            //CustomTriggerManager.QueueAndRunTrigger<MTCharacterTrigger_Relocate>(
            //    characterState,
            //    true,
            //    true,
            //    new CharacterState.FireTriggersData { 
            //        paramInt = newPoint.GetRoomOwner().GetRoomIndex(), // Destination Room?
            //    }, 1);

            CustomTriggerManager.QueueTrigger<MTCharacterTrigger_Relocate>(
                characterState,
                true,
                true,
                new CharacterState.FireTriggersData
                {
                    paramInt = newPoint.GetRoomOwner().GetRoomIndex(), // Destination Room?
                }, 
                1);
        }
    }
}