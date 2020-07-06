using System;
using System.Collections.Generic;
using System.Text;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using UnityEngine;

namespace DiscipleClan.StatusEffects
{
    public class MTStatusEffect_HideUntilBoss : IMTStatusEffect { public string ID => "hideuntilboss"; }

    class StatusEffectHideUntilBoss : StatusEffectState
    {
        public const string StatusId = "hideuntilboss";
        public CombatManager combatManager;
        public ScenarioData scenarioData;

        // This makes them unable to be targetted
        public override bool GetUnitIsTargetable(bool inCombat)
        {
            if (scenarioData.GetSpawnPattern().GetNumGroups() - combatManager.GetTurnCount() > 0)
            {
                return !inCombat;
            } else {
                return true;
            }
        }

        public override void OnStacksAdded(CharacterState character, int numStacksAdded)
        {
            combatManager = character.GetCombatManager();
            scenarioData = combatManager.GetSaveManager().GetCurrentScenarioData();
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                statusEffectStateName = typeof(StatusEffectHideUntilBoss).AssemblyQualifiedName,
                statusId = "hideuntilboss",
                displayCategory = StatusEffectData.DisplayCategory.Negative,
                removeStackAtEndOfTurn = true,
                icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
            }.Build();
        }
    }
}
