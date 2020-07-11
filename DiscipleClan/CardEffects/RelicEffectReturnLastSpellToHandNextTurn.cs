using MonsterTrainModdingAPI.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RelicEffectReturnLastSpellToHandNextTurn : RelicEffectBase, ICardPlayedRelicEffect, IRelicEffect, IStartOfPlayerTurnBeforeDrawRelicEffect
    {
        public List<CardState> cardsPlayed = new List<CardState>();

        public IEnumerator ApplyEffect(RelicEffectParams relicEffectParams)
        {
            if (cardsPlayed.Count > 0)
            {
                var corountine = RedrawCard(cardsPlayed.Last(), relicEffectParams.cardManager.GetHand().Count);
                relicEffectParams.cardManager.StartCoroutine(corountine);
            }

            cardsPlayed.Clear();

            yield break;
        }
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
            if (relicEffectParams.cardState.GetCardType() == CardType.Spell)
                if (relicEffectParams.cardState.GetDiscardEffectWhenPlayed(relicEffectParams.relicManager, null) == HandUI.DiscardEffect.Default)
                    cardsPlayed.Add(relicEffectParams.cardState);

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
