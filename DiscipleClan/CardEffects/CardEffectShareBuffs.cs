using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.CardEffects
{
    class CardEffectShareBuffs : CardEffectBase
    {
        public CharacterState owner;
        public bool reflectDebuffs;
        public int flatIncrease = 0;
        public float multiplyIncrease = 1f;

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            owner = cardEffectParams.targets[0];
            owner.AddStatusEffectChangedListener(ShareStatusEffects);
            reflectDebuffs = cardEffectState.GetParamBool();
            flatIncrease = cardEffectState.GetParamInt();
            multiplyIncrease = cardEffectState.GetParamMultiplier();

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
                if (target != owner)
                    target.AddStatusEffect(statusId, (count * (int)multiplyIncrease) + flatIncrease);
            }
        }
    }
}
