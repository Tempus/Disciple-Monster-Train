using HarmonyLib;
using Trainworks.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    [HarmonyPatch(typeof(RelicEffectBase), "AppendConditionCountDisplay")]
    public static class RelicEffectRewind_EffectCounterTweaks1
    {
        static bool Prefix(RelicEffectBase __instance, ref StringBuilder stringBuilder, CardStatistics cardStatistics)
        {
            RelicEffectRewind relicEffect;
            if ((relicEffect = __instance as RelicEffectRewind) != null)
            {
                // When we find our relic type, we overwrite this entire method to bypass CardStatistics which can't track our custom relic triggers correctly
                string countDisplayString = relicEffect._counter.ToString() + "/1";
                if (!string.IsNullOrWhiteSpace(countDisplayString))
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(", ");
                    }
                    stringBuilder.Append(countDisplayString);
                }
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CardManager), "DrawHand")]
    public static class RelicEffectRewind_EffectCounterTweaks2
    {
        static void Postfix(CardManager __instance, int handSize)
        {
            // We have to reset the counter for all relics of our relic type here
            ProviderManager.TryGetProvider(out RelicManager relicManager);
            if (relicManager == null)
                return;

            foreach (RelicEffectRewind relicState in relicManager.GetRelicEffects<RelicEffectRewind>())
                relicState._counter = 0;
        }
    }

    class RelicEffectRewind : RelicEffectBase, ICardPlayedRelicEffect, ICardModifierRelicEffect, IRelicEffect
    {
        public int _counter = 0;
        private bool _firstCard = false;

        public static IEnumerator RedrawCard(CardState cardState, int cardIndex)
        {
            yield return new WaitForSeconds(0.2f);

            ProviderManager.TryGetProvider(out CardManager cardManager);
            cardManager.DrawSpecificCard(cardState, 0f, HandUI.DrawSource.Discard, cardState, cardIndex);
            cardManager.GetDiscardPile().Remove(cardState);
        }

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
        }

        public IEnumerator OnCardPlayed(CardPlayedRelicEffectParams relicEffectParams)
        {
            var coroutine = RedrawCard(relicEffectParams.cardState, relicEffectParams.cardManager.GetHand().Count);

            // We cache this as soon as it's true and then flip it back once the effect has triggered
            if (_counter == 0)
                _firstCard = true;

            if (_firstCard && relicEffectParams.cardState.GetCardType() == CardType.Spell)
            {
                // Check for Consume cards played and omit them from both the effect and the "was this the first card played" tracking
                bool doesConsume = false;
                foreach (CardTraitState traitState in relicEffectParams.cardState.GetTraitStates())
                {
                    CardTraitExhaustState cardTraitExhaustState;
                    if ((cardTraitExhaustState = traitState as CardTraitExhaustState) != null && cardTraitExhaustState.WillExhaustOnNextPlay(relicEffectParams.cardState, relicEffectParams.relicManager))
                        doesConsume = true;
                }

                if (!doesConsume)
                {
                    if (relicEffectParams.cardState.GetDiscardEffectWhenPlayed(relicEffectParams.relicManager, null) == HandUI.DiscardEffect.Default)
                    {
                        _counter++;
                        _firstCard = false;
                        relicEffectParams.cardManager.StartCoroutine(coroutine);
                    }
                }
            }

            yield break;
        }

        public override void OnTrigger(CardStatistics cardStatistics)
        {
            // We override this for this relic to handle the manual counting we do for when the effect has triggered (for the UI)
            // Don't run the base class's implementation with this RelicEffect
        }

        public bool TestCardPlayed(CardPlayedRelicEffectParams relicEffectParams)
        {
            return true;
        }

        public bool TestEffect(RelicEffectParams relicEffectParams)
        {
            return true;
        }

        public bool ApplyCardStateModifiers(CardState cardState, SaveManager saveManager, CardManager cardManager, RelicManager relicManager)
        {
            // Unused. We use this interface mostly to trick the game into putting the tooltip we want onto the relic with minimal effort.
            return false;
        }

        public bool GetTooltip(out string title, out string body)
        {
            title = CardTraitData.GetTraitCardTextLocalizationKey("CardTraitExhaustState").Localize();
            body = CardTraitData.GetTraitTooltipTextLocalizationKey("CardTraitExhaustState").Localize();
            return true;
        }
    }
}
