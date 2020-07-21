using HarmonyLib;
using MonsterTrainModdingAPI;
using System.Collections;
using System.Collections.Generic;

namespace DiscipleClan.CardEffects
{
    class RelicEffectApplyStatusOnHitIfStatus : RelicEffectBase, ICharacterActionRelicEffect, IRelicEffect, IStatusEffectRelicEffect, IStartOfPlayerTurnAfterDrawRelicEffect
    {
        private Team.Type targetTeam;
        private StatusEffectStackData[] statusEffects;
        private CharacterTriggerData.Trigger trigger;
        private List<CharacterState> _targets = new List<CharacterState>();
        public List<CharacterState> alreadyGone = new List<CharacterState>();

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
            if (relicEffectParams.trigger != CharacterTriggerData.Trigger.OnAttacking)
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
            API.Log(BepInEx.Logging.LogLevel.All, "We've gotten past all the checks!");

            TargetMode targetMode = TargetMode.LastAttackedCharacter;
            if (characterState.GetStatusEffectStacks("sweep") > 0)
            {
                targetMode = TargetMode.Room;
            }

            API.Log(BepInEx.Logging.LogLevel.All, "Target mode is " + targetMode.ToString());

            try
            {
                TargetHelper.CollectTargetsData collectTargetsData = default(TargetHelper.CollectTargetsData);
                collectTargetsData.targetMode = targetMode;
                collectTargetsData.targetModeStatusEffectsFilter = new List<string>();
                collectTargetsData.targetModeHealthFilter = CardEffectData.HealthFilter.Both;
                collectTargetsData.targetTeamType = Team.Type.Heroes;
                collectTargetsData.roomIndex = characterState.GetSpawnPoint().GetRoomOwner().GetRoomIndex();
                collectTargetsData.heroManager = relicEffectParams.heroManager;
                collectTargetsData.monsterManager = relicEffectParams.monsterManager;
                collectTargetsData.roomManager = relicEffectParams.roomManager;
                collectTargetsData.inCombat = false;
                collectTargetsData.isTesting = true;
                collectTargetsData.selfTarget = characterState;

                _targets.Clear();
                TargetHelper.CollectTargets(collectTargetsData, ref _targets);
                API.Log(BepInEx.Logging.LogLevel.All, "Targets " + _targets.Count + " are " + _targets.Join());

                return _targets.Count > 0;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public IEnumerator ApplyCharacterTriggerEffect(CharacterTriggerRelicEffectParams relicEffectParams)
        {
            CharacterState characterState = relicEffectParams.characterState;

            API.Log(BepInEx.Logging.LogLevel.All, "Applying Daze for the attack of " + characterState.name);
            if (alreadyGone.Contains(characterState)) { yield break; }

            API.Log(BepInEx.Logging.LogLevel.All, "We haven't attacked before: " + characterState.name);
            foreach (var target in _targets)
            {
                target.AddStatusEffect(statusEffects[0].statusId, 1);
                string activatedDescription = GetActivatedDescription();
                activatedDescription = string.Format(activatedDescription, statusEffects[0]);
                if (_srcRelicData.CanShowNotifications)
                {
                    relicEffectParams.relicManager.ShowRelicActivated(_srcRelicState.GetIcon(), activatedDescription, target.GetCharacterUI());
                }
            }
            API.Log(BepInEx.Logging.LogLevel.All, "They should be dazed now: " + _targets.Join());

            alreadyGone.Add(characterState);
            yield break;
        }

        public StatusEffectStackData[] GetStatusEffects()
        {
            return statusEffects;
        }

        public bool TestEffect(RelicEffectParams relicEffectParams)
        {
            return true;
        }

        public IEnumerator ApplyEffect(RelicEffectParams relicEffectParams)
        {
            alreadyGone.Clear();
            yield break;
        }
    }
}