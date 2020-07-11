using DiscipleClan.Triggers;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RoomStateModifierRelocateDamage : RoomStateModifierBase, IRoomStateModifier, IRoomStateSpawnPointsModifiedModifier
    {
        public CombatManager combatManager;
        public int damage;
        public List<CardState> storedCards = new List<CardState>();

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            this.damage = roomModifierData.GetParamInt();
            this.combatManager = GameObject.FindObjectOfType<CombatManager>().GetComponent<CombatManager>() as CombatManager;
        }

        public void SpawnPointModifier(CharacterState characterState)
        {
            combatManager.ApplyDamageToTarget(this.damage, characterState, new CombatManager.ApplyDamageToTargetParameters
            {
                damageType = Damage.Type.DirectAttack,
            });
        }

        new public string GetDescriptionKey()
        {
            return "RoomStateModifierRelocateDamage_Desc";
        }

        new public string GetExtraTooltipTitleKey()
        {
            return "RoomStateModifierRelocateDamage_TooltipTitle";
        }

        new public string GetExtraTooltipBodyKey()
        {
            return "RoomStateModifierRelocateDamage_TooltipBody";
        }
    }
}