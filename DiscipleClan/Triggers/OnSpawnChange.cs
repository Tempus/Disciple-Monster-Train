using HarmonyLib;
using Trainworks.Enums.MTTriggers;
using Trainworks.Managers;
using System.Collections.Generic;

namespace DiscipleClan.Triggers
{
    public static class OnSpawnChange
    {
        public static CharacterTrigger OnSpawnChangeCharTrigger = new CharacterTrigger("OnSpawnChange");
        public static CardTrigger OnSpawnChangeCardTrigger = new CardTrigger("OnSpawnChange");

        static OnSpawnChange()
        {
            CustomTriggerManager.AssociateTriggers(OnSpawnChangeCardTrigger, OnSpawnChangeCharTrigger);
        }
    }

    // Gotta patch in places to call this trigger from... gonna be OnSpawnPointChange

    [HarmonyPatch(typeof(RoomManager), "OnSpawnPointChanged")]
    class QueueOnEnchantUpdate
    {
        static void Postfix(RoomManager __instance, CharacterState characterState, SpawnPoint prevPoint, SpawnPoint newPoint)
        {

            List<CharacterState> chars = new List<CharacterState>();
            characterState.GetCharacterManager().AddCharactersInRoomToList(chars, characterState.GetCurrentRoomIndex());
            foreach (var unit in chars)
            {
                CustomTriggerManager.QueueTrigger(OnSpawnChange.OnSpawnChangeCharTrigger, unit);
            }
        }
    }
}