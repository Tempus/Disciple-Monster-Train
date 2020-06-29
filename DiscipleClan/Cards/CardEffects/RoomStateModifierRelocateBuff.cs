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
    class RoomStateModifierRelocateBuff : RoomStateModifierBase, IRoomStateModifier, IRoomStateSpawnPointsChangedModifier
    {
        public CombatManager combatManager;
        public int buffAmount;
        public List<CardState> storedCards = new List<CardState>();

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            this.buffAmount = roomModifierData.GetParamInt();
            this.combatManager = GameObject.FindObjectOfType<CombatManager>().GetComponent<CombatManager>() as CombatManager;
        }

        public void SpawnPointChanged(SpawnPoint prevPoint, SpawnPoint newPoint, CardManager cardManager)
        {
			newPoint.GetCharacterState().BuffDamage(buffAmount);
		}
    }
}