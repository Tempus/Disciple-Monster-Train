using System.Collections;

namespace DiscipleClan.CardEffects
{
    class CardEffectPyreAttackDispatch : CardEffectBase
    {
        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams)
        {
            int pyredamage = cardEffectParams.roomManager.GetPyreRoom().GetPyreHeart().GetAttackDamage();
            cardEffectParams.saveManager.pyreAttackChangedSignal.Dispatch(pyredamage, 1 + ((cardEffectParams.relicManager != null) ? cardEffectParams.relicManager.GetPyreStatusEffectCount("multistrike") : 0));

            yield break;
        }
    }
}
