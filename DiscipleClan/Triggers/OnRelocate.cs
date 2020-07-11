using HarmonyLib;
using MonsterTrainModdingAPI.Enums.MTTriggers;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;

namespace DiscipleClan.Triggers
{
    public static class OnRelocate
    {
        public static CharacterTrigger OnRelocateCharTrigger = new CharacterTrigger("OnRelocate");
        public static CardTrigger OnRelocateCardTrigger = new CardTrigger("OnRelocate");

        static OnRelocate()
        {
            CustomTriggerManager.AssociateTriggers(OnRelocateCardTrigger, OnRelocateCharTrigger);
        }
    }

    // Gotta patch in places to call this trigger from... gonna be OnSpawnPointChange

    [HarmonyPatch(typeof(CharacterState), "MoveUpDownTrain")]
    class QueueRelocate
    {
        static void Postfix(CharacterState __instance, SpawnPoint destinationSpawnPoint, int delayIndex, int prevRoomIndex)
        {
            CustomTriggerManager.QueueTrigger(OnRelocate.OnRelocateCharTrigger, __instance, true, true,
                                                new CharacterState.FireTriggersData
                                                {
                                                    paramInt = destinationSpawnPoint.GetRoomOwner().GetRoomIndex() - prevRoomIndex
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