using MonsterTrainModdingAPI.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RelicEffectRewind : RelicEffectBase, ICardPlayedRelicEffect, IRelicEffect
    {
        public static IEnumerator RedrawCard(CardState cardState, int cardIndex)
        {
            yield return new WaitForSeconds(0.2f);

            CardManager cardManager;
            ProviderManager.TryGetProvider<CardManager>(out cardManager);
            cardManager.DrawSpecificCard(cardState, 0f, HandUI.DrawSource.Discard, cardState, cardIndex);
            cardManager.GetDiscardPile().Remove(cardState);
        }

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
        }

        public IEnumerator OnCardPlayed(CardPlayedRelicEffectParams relicEffectParams)
        {
            var corountine = RedrawCard(relicEffectParams.cardState, relicEffectParams.cardManager.GetHand().Count);

            if (relicEffectParams.cardState.GetCardType() == CardType.Spell)
                if (relicEffectParams.cardState.GetDiscardEffectWhenPlayed(relicEffectParams.relicManager, null) == HandUI.DiscardEffect.Default)
                    if (relicEffectParams.cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Spell) <= 1)
                        relicEffectParams.cardManager.StartCoroutine(corountine);

            yield break;
        }

        public bool TestCardPlayed(CardPlayedRelicEffectParams relicEffectParams)
        {
            return true;
        }

        public bool TestEffect(RelicEffectParams relicEffectParams)
        {
            return true;
        }
    }
}
