using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectGravity : StatusEffectState
    {
        public const string statusId = "gravity";
        public bool shouldDie = false;

        [HarmonyPatch(typeof(CharacterState), "GetMovementSpeed")]
        class GravityNoMove
        {
            // Creates and registers card data for each card class
            static void Postfix(ref int __result, CharacterState __instance)
            {
                if (__instance.GetStatusEffectStacks("gravity") > 0 && !__instance.IsMiniboss() && !__instance.IsOuterTrainBoss()) 
                {
                    __result = 0;
                    __instance.RemoveStatusEffect("gravity", false, 1, true);
                }
            }
        }

        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            ProviderManager.TryGetProvider<RoomManager>(out inputTriggerParams.roomManager);

            // At end of turn, descend, and if we did Descend remove a stack of gravity.
            if (canMove(inputTriggerParams))
            {
                var cardEffectParams = new CardEffectParams
                {
                     saveManager = inputTriggerParams.combatManager.GetSaveManager(),
                     combatManager = inputTriggerParams.combatManager,
                     heroManager = inputTriggerParams.combatManager.GetHeroManager(),
                     roomManager = inputTriggerParams.roomManager,
                     targets = new List<CharacterState> { inputTriggerParams.associatedCharacter },
                };
                var bumper = new CardEffectBump();
                yield return bumper.Bump(cardEffectParams, -1);
                yield return Descend(inputTriggerParams.associatedCharacter);
            }
        }

        public IEnumerator Descend(CharacterState target)
        {
            target.RemoveStatusEffect("gravity", false, 1, true);
            yield break;
        }

        public bool canMove(InputTriggerParams inputTriggerParams)
        {
            int currentRoom = inputTriggerParams.associatedCharacter.GetCurrentRoomIndex();
            if (currentRoom > 0)
                if (inputTriggerParams.roomManager.GetRoom(currentRoom - 1).IsRoomEnabled())
                    return true;

            return false;
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectGravity).AssemblyQualifiedName,
                StatusId = "gravity",
                DisplayCategory = StatusEffectData.DisplayCategory.Positive,
                TriggerStage = StatusEffectData.TriggerStage.OnPostRoomCombat,
                IsStackable = true,
                IconPath = "chrono/Status/weight.png",
            }.Build();
        }
    }
}
