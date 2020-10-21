using HarmonyLib;
using Trainworks;
using Trainworks.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static CharacterState;

namespace DiscipleClan.CardEffects
{
    // Triggers on losing a stack, whoops. That's not right.
    // Adds +1 to stacks from everyone else, that's not right either.

    class CardEffectShareBuffs : CardEffectBase
    {
        public CharacterState owner;
        public bool reflectDebuffs;
        public int flatIncrease = 0;
        public float multiplyIncrease = 1f;

        public static CardEffectShareBuffs instance;

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            owner = cardEffectParams.targets[0];
            reflectDebuffs = cardEffectState.GetParamBool();
            flatIncrease = cardEffectState.GetParamInt();
            multiplyIncrease = cardEffectState.GetParamMultiplier();
            instance = this;

            yield break;
        }

        public void ShareStatusEffects(string statusId, int count)
        {
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);

            List<CharacterState> targets = new List<CharacterState>();

            ProviderManager.TryGetProvider<StatusEffectManager>(out StatusEffectManager statusEffectManager);
            var statusData = statusEffectManager.GetStatusEffectDataById(statusId);

            if (statusData.GetDisplayCategory() == StatusEffectData.DisplayCategory.Positive)
                roomManager.GetRoom(owner.GetCurrentRoomIndex()).AddCharactersToList(targets, Team.Type.Monsters);

            if (statusData.GetDisplayCategory() == StatusEffectData.DisplayCategory.Negative && reflectDebuffs)
                roomManager.GetRoom(owner.GetCurrentRoomIndex()).AddCharactersToList(targets, Team.Type.Heroes);

            foreach (var target in targets)
            {
                if (target != owner && count > 0)
                {
                    int statCount = Math.Min(count, flatIncrease);
                    target.AddStatusEffect(statusId, statCount);
                }
            }
        }
    }

    // Reset the Ward List
    [HarmonyPatch(typeof(CharacterState), "AddStatusEffect", new Type[] { typeof(string), typeof(int), typeof(AddStatusEffectParams) })]
    class OnStatusEffect
    {
        static void Postfix(CharacterState __instance, string statusId, int numStacks, AddStatusEffectParams addStatusEffectParams)
        {
            if (CardEffectShareBuffs.instance == null) { return; }
            if (CardEffectShareBuffs.instance.owner == __instance)
            {
                CardEffectShareBuffs.instance.ShareStatusEffects(statusId, numStacks);
            }
        }
    }
}
