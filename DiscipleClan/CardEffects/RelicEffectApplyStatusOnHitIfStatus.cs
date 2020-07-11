using System.Collections;

namespace DiscipleClan.CardEffects
{
    class RelicEffectApplyStatusOnHitIfStatus : RelicEffectBase, ICharacterActionRelicEffect, IRelicEffect, IStatusEffectRelicEffect
    {
        private Team.Type targetTeam;
        private StatusEffectStackData[] statusEffects;
        private CharacterTriggerData.Trigger trigger;

        public override bool CanShowNotifications => false;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            targetTeam = relicEffectData.GetParamSourceTeam();
            statusEffects = relicEffectData.GetParamStatusEffects();
            trigger = relicEffectData.GetParamTrigger();
        }

        public bool TestCharacterTriggerEffect(CharacterTriggerRelicEffectParams relicEffectParams)
        {
            CharacterState characterState = relicEffectParams.characterState;
            if (characterState.IsDestroyed)
            {
                return false;
            }
            if (characterState.GetTeamType() != targetTeam || characterState.HasStatusEffect("immune"))
            {
                return false;
            }
            if (relicEffectParams.combatManager.GetCombatPhase() == CombatManager.Phase.HeroTurn || relicEffectParams.combatManager.GetCombatPhase() == CombatManager.Phase.PreCombat || relicEffectParams.combatManager.GetCombatPhase() == CombatManager.Phase.BossActionPreCombat)
            {
                return false;
            }
            if (characterState.GetStatusEffectStacks("ambush") <= 0)
            {
                return false;
            }
            return true;
        }

        public IEnumerator ApplyCharacterTriggerEffect(CharacterTriggerRelicEffectParams relicEffectParams)
        {
            CharacterState characterState = relicEffectParams.characterState;
            int num = RandomManager.Range(0, statusEffects.Length, RngId.Battle);
            int numStacks = (statusEffects[num].count <= 0) ? 1 : statusEffects[num].count;
            CharacterState.AddStatusEffectParams addStatusEffectParams = default(CharacterState.AddStatusEffectParams);
            addStatusEffectParams.sourceRelicState = _srcRelicState;
            CharacterState.AddStatusEffectParams addStatusEffectParams2 = addStatusEffectParams;
            characterState.AddStatusEffect(statusEffects[num].statusId, numStacks, addStatusEffectParams2);
            string activatedDescription = GetActivatedDescription();
            activatedDescription = string.Format(activatedDescription, statusEffects[num]);
            if (_srcRelicData.CanShowNotifications)
            {
                relicEffectParams.relicManager.ShowRelicActivated(_srcRelicState.GetIcon(), activatedDescription, characterState.GetCharacterUI());
            }
            yield break;
        }

        public StatusEffectStackData[] GetStatusEffects()
        {
            return statusEffects;
        }
    }

}
