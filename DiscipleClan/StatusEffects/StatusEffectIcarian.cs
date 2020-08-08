using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectIcarian : StatusEffectState
    {
        public const string statusId = "icarian";

        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            if (inputTriggerParams.associatedCharacter.GetCurrentRoomIndex() == 2 && inputTriggerParams.associatedCharacter.GetStatusEffectStacks("gravity") > 0)
                return false;

            List<CharacterState> characters = new List<CharacterState>();
            inputTriggerParams.combatManager.GetHeroManager().AddCharactersInRoomToList(characters, inputTriggerParams.associatedCharacter.GetCurrentRoomIndex());
            foreach (var character in characters)
            {
                if (character.GetStatusEffect("relentless") != null)
                    return false;
            }
            return true;
        }

        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            // Don't fly up during Relentless
            List<CharacterState> characters = new List<CharacterState>();
            inputTriggerParams.combatManager.GetHeroManager().AddCharactersInRoomToList(characters, inputTriggerParams.associatedCharacter.GetCurrentRoomIndex());
            foreach (var character in characters)
            {
                if (character.GetStatusEffect("relentless") != null)
                    yield break;
            }

            // Provider
            ProviderManager.TryGetProvider<RoomManager>(out inputTriggerParams.roomManager);

            // At end of turn, ascend and if we try to ascend into the Pyre then we kaboom and do something.
            if (inputTriggerParams.associatedCharacter.GetCurrentRoomIndex() == 2)
                yield return inputTriggerParams.associatedCharacter.Sacrifice(new CardState());
            else
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
                yield return bumper.Bump(cardEffectParams, 1);
            }

            yield break;
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectIcarian).AssemblyQualifiedName,
                StatusId = "icarian",
                DisplayCategory = StatusEffectData.DisplayCategory.Persistent,
                TriggerStage = StatusEffectData.TriggerStage.OnPostCombatRegen,
                Icon = CustomAssetManager.LoadSpriteFromPath("chrono/Status/icarus.png"),
            }.Build();
        }
    }
}
