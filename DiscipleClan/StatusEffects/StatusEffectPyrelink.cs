using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections;
using System.Collections.Generic;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectPyrelink : StatusEffectState
    {
        public const string StatusId = "pyrelink";

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
                var buffAmount = GetEffectMagnitude();
                var champion = this.GetAssociatedCharacter();

                // We killed something, Let's buff
                if (!saveManager.PreviewMode)
                {
                    // attackingCharacter.GetCharacterUI().ShowEffectVFX(attackingCharacter, _srcRelicEffectData.GetAppliedVfx());
                    API.Log(BepInEx.Logging.LogLevel.All, "This triggers when we're actually pyrelinking");
                    yield return champion.BuffMaxHP(buffAmount);
                }
                // We killed something imaginarily, we should preview here? Wait, how does the PYRE preview a kill?
                else
                {
                    API.Log(BepInEx.Logging.LogLevel.All, "This triggers when we're previewing pyrelink");
                    //champion.BuffMaxHP(buffAmount);
                    // attackingCharacter.PreviewHpOnPyreHeart(Mathf.Min(attackingCharacter.GetHP() + healAmount, saveManager.GetMaxTowerHP()));
                }
            }
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                statusEffectStateName = typeof(StatusEffectPyrelink).AssemblyQualifiedName,
                statusId = "pyrelink",
                displayCategory = StatusEffectData.DisplayCategory.Persistent,
                triggerStage = StatusEffectData.TriggerStage.OnMonsterTeamTurnBegin,
                icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Status/fire-dash.png"),
                isStackable = true,
            }.Build();
        }
    }
}
