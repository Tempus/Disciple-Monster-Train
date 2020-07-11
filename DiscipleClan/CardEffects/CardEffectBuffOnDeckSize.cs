using System.Collections;

namespace DiscipleClan.CardEffects
{
    class CardEffectBuffOnDeckSize : CardEffectBase
    {
        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            int deckSize = cardEffectParams.cardManager.GetDrawPile().Count;
            foreach (var target in cardEffectParams.targets)
            {
                target.BuffDamage(cardEffectState.GetParamInt() * deckSize);
                target.BuffMaxHP(cardEffectState.GetAdditionalParamInt() * deckSize);
            }

            yield break;
        }
    }
}
