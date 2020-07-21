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
                NotifyHealthEffectTriggered(cardEffectParams.saveManager, cardEffectParams.popupNotificationManager, GetActivatedDescription(cardEffectState), target.GetCharacterUI());

                target.BuffDamage(cardEffectState.GetParamInt() * energy);
                yield return target.BuffMaxHP(cardEffectState.GetAdditionalParamInt() * energy, false);
            }

            yield break;
        }
    }
}
