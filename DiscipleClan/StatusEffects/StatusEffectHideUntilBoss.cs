using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectHideUntilBoss : StatusEffectState
    {
        public const string StatusId = "hideuntilboss";

        // This makes them unable to be targetted
        public override bool GetUnitIsTargetable(bool inCombat)
        {
            CombatManager combatManager;
            ProviderManager.TryGetProvider<CombatManager>(out combatManager);
            if (combatManager.GetSaveManager().GetCurrentScenarioData().GetSpawnPattern().GetNumGroups() - combatManager.GetTurnCount() > 0)
            {
                return !inCombat;
            }
            else
            {
                return true;
            }
        }

        //public override void OnStacksAdded(CharacterState character, int numStacksAdded)
        //{

        //}

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                statusEffectStateName = typeof(StatusEffectHideUntilBoss).AssemblyQualifiedName,
                statusId = "hideuntilboss",
                displayCategory = StatusEffectData.DisplayCategory.Persistent,
                icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Status/time-trap.png"),
            }.Build();
        }
    }
}
