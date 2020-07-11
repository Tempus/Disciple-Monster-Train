using System.Collections;

namespace DiscipleClan.CardEffects
{
    class CardEffectEmpowerOnSpawn : CardEffectBase
    {
        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            int energy = cardEffectParams.playerManager.GetEnergy();
            foreach (var target in cardEffectParams.targets)
            {
                target.BuffDamage(cardEffectState.GetParamInt() * energy);
                target.BuffMaxHP(cardEffectState.GetAdditionalParamInt() * energy);
            }

            yield break;
        }
    }
}
