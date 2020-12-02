using HarmonyLib;
using Trainworks;
using Trainworks.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static CharacterState;
using static Trainworks.Constants.VanillaStatusEffectIDs;

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
        public int targeting = 0;

        public static CardEffectShareBuffs instance;

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            owner = cardEffectParams.targets[0];
            reflectDebuffs = cardEffectState.GetParamBool();
            flatIncrease = cardEffectState.GetParamInt();
            multiplyIncrease = cardEffectState.GetParamMultiplier();
            targeting = cardEffectState.GetAdditionalParamInt();
            cardEffectState.SetShouldOverrideTriggerUI(true);

            instance = this;

            yield break;
        }

        public void ShareStatusEffects(string statusId, int count)
        {
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);
            ProviderManager.TryGetProvider<CardManager>(out CardManager CardManager);

            List<CharacterState> targets = new List<CharacterState>();

            ProviderManager.TryGetProvider<StatusEffectManager>(out StatusEffectManager statusEffectManager);
            var statusData = statusEffectManager.GetStatusEffectDataById(statusId);

            //if (statusData.GetDisplayCategory() == StatusEffectData.DisplayCategory.Positive)
            //    roomManager.GetRoom(owner.GetCurrentRoomIndex()).AddCharactersToList(targets, Team.Type.Monsters);

            //if (statusData.GetDisplayCategory() == StatusEffectData.DisplayCategory.Negative && reflectDebuffs)
            //    roomManager.GetRoom(owner.GetCurrentRoomIndex()).AddCharactersToList(targets, Team.Type.Heroes);

            var collector = new TargetHelper.CollectTargetsData
            {
                cardManager = CardManager,
                heroManager = ProviderManager.CombatManager.GetHeroManager(),
                ignoreDead = true,
                monsterManager = ProviderManager.CombatManager.GetMonsterManager(),
                roomIndex = owner.GetCurrentRoomIndex(),
                combatManager = ProviderManager.CombatManager,
                roomManager = roomManager,
                skipCharacters = new List<CharacterState> { owner },
                targetModeStatusEffectsFilter = new List<string>(),
            };

            int statCount = (int)Math.Ceiling(count * multiplyIncrease);

            if (statusData.GetDisplayCategory() == StatusEffectData.DisplayCategory.Positive || statusId == Armor || statusId == Burnout)
                //roomManager.GetRoom(owner.GetCurrentRoomIndex()).AddCharactersToList(targets, Team.Type.Monsters);
                collector.targetTeamType = Team.Type.Monsters;

            if (statusData.GetDisplayCategory() == StatusEffectData.DisplayCategory.Negative && reflectDebuffs)
            {
                //roomManager.GetRoom(owner.GetCurrentRoomIndex()).AddCharactersToList(targets, Team.Type.Heroes);
                collector.targetTeamType = Team.Type.Heroes;
                owner.RemoveStatusEffect(statusId, false, 999);
                statCount = (int)Math.Ceiling(count * (multiplyIncrease + 1f));
            }

            // Random
            if (targeting == 0)
            {
                collector.targetMode = TargetMode.RandomInRoom;
            }
            // Front
            else if (targeting == 1)
            {
                collector.targetMode = TargetMode.FrontInRoom;
            }
            // Everyone
            else
            {
                collector.targetMode = TargetMode.Room;
            }

            TargetHelper.CollectTargets(collector, ref targets);
            //int statCount = Math.Min(count, flatIncrease);

            foreach (var target in targets)
            {
                if (statCount > 0)
                {
                    target.AddStatusEffect(statusId, statCount);
                }
            }
        }

        public override string GetCardText(CardEffectState cardEffectState, RelicManager relicManager = null)
        {
            return "GET CARD TEXT";
        }

        public override string GetHintText(CardEffectState cardEffectState, CharacterState selfTarget)
        {
            return "GET HINT TEXT";
        }

        public override string GetDescriptionAsTrait(CardEffectState cardEffectState)
        {
            return "GET TRAIT TEXT";
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
