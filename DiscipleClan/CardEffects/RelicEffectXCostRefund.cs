using System.Collections;

namespace DiscipleClan.CardEffects
{
    class RelicEffectXCostRefund : RelicEffectBase, ICardPlayedRelicEffect, IRelicEffect
    {
        public int refund = 0;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            refund = relicEffectData.GetParamInt();
        }

        public IEnumerator OnCardPlayed(CardPlayedRelicEffectParams relicEffectParams)
        {
            if (relicEffectParams.cardState.IsConsumeRemainingEnergyCostType())
                relicEffectParams.playerManager.AddEnergy(refund);

            yield break;
        }

        public bool TestCardPlayed(CardPlayedRelicEffectParams relicEffectParams)
        {
            return true;
        }
    }
}
