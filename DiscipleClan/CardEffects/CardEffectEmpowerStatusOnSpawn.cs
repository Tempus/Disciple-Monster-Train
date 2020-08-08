using System.Collections;

namespace DiscipleClan.CardEffects
{
    class CardEffectEmpowerStatusOnSpawn : CardEffectBase
    {
        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            int energy = cardEffectParams.playerManager.GetEnergy();
            foreach (var target in cardEffectParams.targets)
            {
                foreach (var status in cardEffectState.GetParamStatusEffectStackData())
                {
                    target.AddStatusEffect(status.statusId, status.count * energy);
                }
            }

            yield break;
        }
    }
}
