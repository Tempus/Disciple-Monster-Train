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
                NotifyHealthEffectTriggered(cardEffectParams.saveManager, cardEffectParams.popupNotificationManager, GetActivatedDescription(cardEffectState), target.GetCharacterUI());

                target.BuffDamage(cardEffectState.GetParamInt() * deckSize);
                yield return target.BuffMaxHP(cardEffectState.GetParamInt() * deckSize, false);
            }

            yield break;
        }
    }
}
