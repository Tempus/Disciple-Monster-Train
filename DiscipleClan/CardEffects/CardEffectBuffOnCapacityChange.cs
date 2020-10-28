using HarmonyLib;
using Trainworks;
using Trainworks.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class CardEffectBuffOnCapacityChange : CardEffectBase
    {
        public static CharacterState target;
        public static int lastCap = 0;
        public static int addDamage = 1;

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            target = cardEffectParams.targets[0];
            addDamage = cardEffectState.GetParamInt();
            lastCap = 0;

            yield break;
        }

        public static void ChangeBuffs(RoomState room)
        {
            if (target == null) { return; }
            if (target.GetCurrentRoomIndex() != room.GetRoomIndex()) { return; }

            var capacityInfo = room.GetCapacityInfo(Team.Type.Monsters);
            int diff = capacityInfo.count - lastCap;

            if (diff == 0)
                return;

            if (ProviderManager.SaveManager.PreviewMode)
                return;

            if (diff > 0)
                target.BuffDamage(diff * addDamage);
            if (diff < 0)
                target.DebuffDamage(Mathf.Abs(diff * addDamage));

            if (!ProviderManager.SaveManager.PreviewMode)
                lastCap = capacityInfo.count;
        }
    }

    [HarmonyPatch(typeof(RoomCapacityUI), "GetAndCompareCapacityInfo")]
    class CapacityChangeTriggerOnUIUpdate
    {
        static void Postfix(bool __result, RoomCapacityUI __instance, RoomState room)
        {
            CardEffectBuffOnCapacityChange.ChangeBuffs(room);
        }
    }

    [HarmonyPatch(typeof(RoomManager), "OnSpawnPointChanged")]
    class CapacityChangeTriggerOnSpawnUpdate
    {
        static void Postfix(RoomManager __instance, CharacterState characterState, SpawnPoint prevPoint, SpawnPoint newPoint)
        {
            if (prevPoint != null) 
                CardEffectBuffOnCapacityChange.ChangeBuffs(prevPoint.GetRoomOwner());
            if (newPoint != null)
                CardEffectBuffOnCapacityChange.ChangeBuffs(newPoint.GetRoomOwner());
        }
    }

}
