using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using UnityEngine;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectPyreboost : StatusEffectState
    {
        public const string statusId = "pyreboost";
        public int lastBuff = 0;
        public SaveManager saveManager;

        public void OnPyreAttackChange(int PyreAttack, int PyreNumAttacks)
        {
            var character = this.GetAssociatedCharacter();

            character.DebuffDamage(lastBuff, null, fromStatusEffect: true);

            var multiplier = character.GetStatusEffectStacks(this.GetStatusId());
            int pyreAttack = saveManager.GetDisplayedPyreAttack();
            
            character.BuffDamage(multiplier * pyreAttack, null, fromStatusEffect: true);
            lastBuff = multiplier * pyreAttack;
        }

        public override void OnStacksAdded(CharacterState character, int numStacksAdded)
        {
            if (character != null && numStacksAdded > 0)
            {
                saveManager = character.GetCombatManager().GetSaveManager();
                saveManager.pyreAttackChangedSignal.AddListener(OnPyreAttackChange);
                OnPyreAttackChange(0, 1);
            }
        }

        public override void OnStacksRemoved(CharacterState character, int numStacksRemoved)
        {
            if (character != null && numStacksRemoved > 0)
            {
                OnPyreAttackChange(0, 1);

                if (character.GetStatusEffectStacks(GetStatusId()) <= 0)
                    character.DebuffDamage(lastBuff, null, fromStatusEffect: true);
            }
        }

        //protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        //{
        //          // Do damage to the pyre here!
        //          yield break;
        //}

        private int DamageValue(int stacks)
        {
            return GetMagnitudePerStack() * stacks;
        }

        public override int GetEffectMagnitude(int stacks = 1)
        {
            return DamageValue(stacks);
        }

        public override int GetMagnitudePerStack()
        {
            return ProviderManager.SaveManager.GetDisplayedPyreAttack(); ;
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectPyreboost).AssemblyQualifiedName,
                StatusId = "pyreboost",
                DisplayCategory = StatusEffectData.DisplayCategory.Positive,
                TriggerStage = StatusEffectData.TriggerStage.OnDeath,
                Icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Status/burning-embers.png"),
            }.Build();
        }

    }
}
