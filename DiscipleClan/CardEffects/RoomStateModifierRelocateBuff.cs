using DiscipleClan.Triggers;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RoomStateModifierRelocateBuff : RoomStateModifierBase, IRoomStateModifier, IRoomStateSpawnPointsModifiedModifier
    {
        public CombatManager combatManager;
        public RoomManager roomManager;
        public int buffAmount;
        public List<CardState> storedCards = new List<CardState>();

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            this.buffAmount = roomModifierData.GetParamInt();
            this.combatManager = GameObject.FindObjectOfType<CombatManager>().GetComponent<CombatManager>() as CombatManager;
            this.roomManager = roomManager;
        }

        public void SpawnPointModifier(CharacterState characterState)
        {
            characterState.BuffDamage(buffAmount);
        }

        new public string GetDescriptionKey()
        {
            return "RoomStateModifierRelocateBuff_Desc";
        }

        new public string GetExtraTooltipTitleKey()
        {
            return "RoomStateModifierRelocateBuff_TooltipTitle";
        }

        new public string GetExtraTooltipBodyKey()
        {
            return "RoomStateModifierRelocateBuff_TooltipBody";
        }
    }
}