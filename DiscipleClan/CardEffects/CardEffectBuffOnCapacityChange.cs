using HarmonyLib;
using MonsterTrainModdingAPI.Managers;
using System.Collections;
using System.Collections.Generic;

namespace DiscipleClan.CardEffects
{
    class CardEffectBuffOnCapacityChange : CardEffectBase
    {
        public static CharacterState target;
        public static int lastBuff = 0;
        public static int addDamage = 1;

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            target = cardEffectParams.targets[0];
            addDamage = cardEffectState.GetParamInt();
            lastBuff = 0;

            yield break;
        }

        public static void ChangeBuffs(RoomState room)
        {
            if (target == null) { return; }
            if (target.GetCurrentRoomIndex() != room.GetRoomIndex()) { return; }

            var capacityInfo = room.GetCapacityInfo(Team.Type.Monsters);
            int cap = capacityInfo.count;

            target.DebuffDamage(lastBuff);
            lastBuff = capacityInfo.count * addDamage;
            target.BuffDamage(capacityInfo.count * addDamage);
        }
    }

    [HarmonyPatch(typeof(RoomCapacityUI), "GetAndCompareCapacityInfo")]
    class CapacityChangeTrigger
    {
        static void Postfix(bool __result, RoomCapacityUI __instance, RoomState room)
        {
            CardEffectBuffOnCapacityChange.ChangeBuffs(room);
        }
    }

}
