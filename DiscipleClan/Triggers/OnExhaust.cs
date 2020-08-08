using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Enums.MTTriggers;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;

namespace DiscipleClan.Triggers
{
    public static class OnExhaust
    {
        public static CharacterTrigger OnExhaustCharTrigger = new CharacterTrigger("OnExhaust");
        public static CardTrigger OnExhaustCardTrigger = new CardTrigger("OnExhaust");

        static OnExhaust()
        {
            CustomTriggerManager.AssociateTriggers(OnExhaustCardTrigger, OnExhaustCharTrigger);
        }
    }

    [HarmonyPatch(typeof(CardManager), "OnCardPlayed")]
    class QueueOnExhaustUpdate
    {
        static void Prefix(CardManager __instance, CardState playCard, int selectedRoom, RoomState roomState, SpawnPoint dropLocation, CharacterState characterSummoned, List<CharacterState> targets, bool discardCard)
        {
            if (playCard.HasTrait(typeof(CardTraitExhaustState)))
            {
                API.Log(BepInEx.Logging.LogLevel.All, "Exhaust trait found: " + roomState);
                ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);
                roomManager.GetSelectedRoom();
                int roomindex = roomManager.GetSelectedRoom();
                if (roomindex != -1)
                {
                    API.Log(BepInEx.Logging.LogLevel.All, "Room: " + selectedRoom);

                    List<CharacterState> charList = new List<CharacterState>();
                    ProviderManager.CombatManager.GetMonsterManager().AddCharactersInRoomToList(charList, roomManager.GetSelectedRoom());
                    foreach (var unit in charList)
                    {
                        CustomTriggerManager.QueueTrigger(OnExhaust.OnExhaustCharTrigger, unit);
                    }
                }
            }
        }
    }
}