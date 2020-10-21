using Trainworks;
using Trainworks.Builders;
using Trainworks.Managers;
using UnityEngine;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectPyreboost : StatusEffectState
    {
        public const string statusId = "pyreboost";
        public int lastBuff = 0;
        public SaveManager saveManager;
        public bool previewOnce = false;

        public void OnPyreAttackChange(int PyreAttack, int PyreNumAttacks)
        {
            CharacterState character = this.GetAssociatedCharacter();
            if (character == null) { return; }
            if (character.IsDead || character.IsDestroyed) { return; }

            try
            {
                character.DebuffDamage(lastBuff, null, fromStatusEffect: true);

                var multiplier = character.GetStatusEffectStacks(this.GetStatusId());
                
                character.BuffDamage(multiplier * PyreAttack * PyreNumAttacks, null, fromStatusEffect: true);
                lastBuff = multiplier * PyreAttack * PyreNumAttacks;
            }
            catch (System.Exception) {
            }
        }

        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            OnPyreAttackChange(saveManager.GetDisplayedPyreAttack(), saveManager.GetDisplayedPyreNumAttacks());
            return true;
        }

        public override void ModifyVisualDamage(ref int visualDamage, int damageApplied, int unmodifiedDamage, int damageSustained, int damageBlocked, CharacterState attacker, CharacterState target)
        {
            OnPyreAttackChange(saveManager.GetDisplayedPyreAttack(), saveManager.GetDisplayedPyreNumAttacks());
        }

        public override void OnStacksAdded(CharacterState character, int numStacksAdded)
        {
            if (ProviderManager.SaveManager.PreviewMode && previewOnce) { return; }
            if (character != null && numStacksAdded > 0)
            {
                saveManager = character.GetCombatManager().GetSaveManager();
                saveManager.pyreAttackChangedSignal.AddListener(OnPyreAttackChange);
                OnPyreAttackChange(saveManager.GetDisplayedPyreAttack(), saveManager.GetDisplayedPyreNumAttacks());
            }
            previewOnce = false;
            if (ProviderManager.SaveManager.PreviewMode) { previewOnce = true; }
        }

        public override void OnStacksRemoved(CharacterState character, int numStacksRemoved)
        {
            if (character != null && numStacksRemoved > 0)
            {
                OnPyreAttackChange(ProviderManager.SaveManager.GetDisplayedPyreAttack(), ProviderManager.SaveManager.GetDisplayedPyreNumAttacks());
            }
        }

        public override int GetEffectMagnitude(int stacks = 1)
        {
            return GetMagnitudePerStack() * stacks;
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
                StatusId = statusId,
                DisplayCategory = StatusEffectData.DisplayCategory.Persistent,
                TriggerStage = StatusEffectData.TriggerStage.OnMonsterTeamTurnBegin,
                IconPath = "chrono/Status/burning-embers.png",
            }.Build();
        }

    }
}
