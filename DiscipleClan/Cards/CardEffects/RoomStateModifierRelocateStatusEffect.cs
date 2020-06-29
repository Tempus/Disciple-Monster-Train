using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DiscipleClan.Cards.CardEffects
{
    class RoomStateModifierRelocateStatusEffect : RoomStateModifierBase, IRoomStateModifier, IRoomStateSpawnPointsChangedModifier
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

        public void SpawnPointChanged(SpawnPoint prevPoint, SpawnPoint newPoint, CardManager cardManager)
        {
                foreach (var effect in effects)
                {
                    newPoint.GetCharacterState().AddStatusEffect(effect.statusId, effect.count);
                }
		}
    }
}