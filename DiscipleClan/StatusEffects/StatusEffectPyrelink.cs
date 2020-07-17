using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections;
using System.Collections.Generic;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectPyrelink : StatusEffectState
    {
        public const string statusId = "pyrelink";

        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            List<CharacterState> towerchars = new List<CharacterState>();
            inputTriggerParams.combatManager.GetHeroManager().AddCharactersInTowerToList(towerchars);
            foreach (var hero in towerchars)
            {
                hero.AddDeathSignal(OnEnemyDeath, true);
            }

            return true;
        }

        // If the Pyre kills something, I want to know about it!
        private IEnumerator OnEnemyDeath(CharacterDeathParams deathParams)
        {
            if (deathParams == null)
            {
                yield break;
            }
            CharacterState attackingCharacter = deathParams.attackingCharacter;
            SaveManager saveManager = deathParams.saveManager;
            if (attackingCharacter != null && attackingCharacter.IsPyreHeart())
            {
                var champion = this.GetAssociatedCharacter();
                var buffAmount = champion.GetStatusEffectStacks(GetStatusId());

                // We killed something, Let's buff
                if (!saveManager.PreviewMode)
                {
                    // attackingCharacter.GetCharacterUI().ShowEffectVFX(attackingCharacter, _srcRelicEffectData.GetAppliedVfx());
                    //API.Log(BepInEx.Logging.LogLevel.All, "This triggers when we're actually pyrelinking");
                    yield return champion.BuffMaxHP(buffAmount);
                }
                // We killed something imaginarily, we should preview here? Wait, how does the PYRE preview a kill?
                else
                {
                    //API.Log(BepInEx.Logging.LogLevel.All, "This triggers when we're previewing pyrelink");
                    //yield return champion.BuffMaxHP(buffAmount);
                    // attackingCharacter.PreviewHpOnPyreHeart(Mathf.Min(attackingCharacter.GetHP() + healAmount, saveManager.GetMaxTowerHP()));
                }
            }
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectPyrelink).AssemblyQualifiedName,
                StatusId = "pyrelink",
                DisplayCategory = StatusEffectData.DisplayCategory.Persistent,
                TriggerStage = StatusEffectData.TriggerStage.OnMonsterTeamTurnBegin,
                Icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Status/fire-dash.png"),
                IsStackable = true,
            }.Build();
        }
    }
}
