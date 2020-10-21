using HarmonyLib;
using Trainworks.Enums.MTTriggers;
using Trainworks.Managers;
using System.Collections.Generic;

namespace DiscipleClan.Triggers
{
    public static class OnShuffle
    {
        public static CharacterTrigger OnShuffleCharTrigger = new CharacterTrigger("OnShuffle");
        public static CardTrigger OnShuffleCardTrigger = new CardTrigger("OnShuffle");

        static OnShuffle()
        {
            CustomTriggerManager.AssociateTriggers(OnShuffleCardTrigger, OnShuffleCharTrigger);
        }
    }


    [HarmonyPatch(typeof(CardManager), "ShuffleDeck")]
    class OnShuffleTrigger
    {
        static void Postfix(CardManager __instance, bool initialShuffle)
        {
            MonsterManager monsterManager;
            ProviderManager.TryGetProvider<MonsterManager>(out monsterManager);

            List<CharacterState> units = new List<CharacterState>();
            monsterManager.AddCharactersInTowerToList(units);
            CustomTriggerManager.QueueAndRunTrigger(OnShuffle.OnShuffleCharTrigger, units.ToArray());
        }
    }
}