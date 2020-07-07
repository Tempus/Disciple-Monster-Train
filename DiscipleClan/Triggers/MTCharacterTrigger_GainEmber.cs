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

namespace DiscipleClan.Triggers
{
    class MTCharacterTrigger_GainEmber : MTCharacterTrigger<MTCharacterTrigger_GainEmber> { }


    [HarmonyPatch(typeof(PlayerManager), "AddEnergy")]
    class EnergyAddTrigger
    {
        static void Postfix(PlayerManager __instance, int addEnergy)
        {
            API.Log(BepInEx.Logging.LogLevel.All, "Added Energy: " + addEnergy);
            MonsterManager monsterManager;
            ProviderManager.TryGetProvider<MonsterManager>(out monsterManager);

            List<CharacterState> units = new List<CharacterState>();
            monsterManager.AddCharactersInTowerToList(units);
            foreach (var character in units)
            {
                CustomTriggerManager.QueueTrigger<MTCharacterTrigger_GainEmber>(character, true, true, null, addEnergy);
            }
        }
    }
}