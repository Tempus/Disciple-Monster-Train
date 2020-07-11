using DiscipleClan.Triggers;
using MonsterTrainModdingAPI;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RoomStateModifierRelocateStatusEffect : RoomStateModifierBase, IRoomStateModifier, IRoomStateSpawnPointsModifiedModifier
    {
        public CombatManager combatManager;
        public StatusEffectStackData[] effects;
        public List<CardState> storedCards = new List<CardState>();

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            this.effects = roomModifierData.GetParamStatusEffects();
            this.combatManager = GameObject.FindObjectOfType<CombatManager>().GetComponent<CombatManager>() as CombatManager;
        }

        public void SpawnPointModifier(CharacterState characterState)
        {
            if (effects.Length == 0)
            {
                API.Log(BepInEx.Logging.LogLevel.All, "No status effects!");
                return;
            }

            foreach (var effect in effects)
            {
                characterState.AddStatusEffect(effect.statusId, effect.count);
            }
        }
        new public string GetDescriptionKey()
        {
            return "RoomStateModifierRelocateStatusEffect_Desc";
        }

        new public string GetExtraTooltipTitleKey()
        {
            return "RoomStateModifierRelocateStatusEffect_TooltipTitle";
        }

        new public string GetExtraTooltipBodyKey()
        {
            return "RoomStateModifierRelocateStatusEffect_TooltipBody";
        }

    }
}