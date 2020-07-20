using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Managers;
using System.Collections;

namespace DiscipleClan.CardEffects
{
    public class CardTraitSuppressExhaust : CardTraitState { }

	[HarmonyPatch(typeof(RelicManager), "ApplyOnDiscardRelicEffects")]
	class DiscardSuppressPatch
	{
		static void Postfix(RelicManager __instance, CardManager.DiscardCardParams discardCardParams)
		{
			if (!discardCardParams.wasPlayed || !discardCardParams.discardCard.HasTrait(typeof(CardTraitExhaustState)))
			{
				return;
			}

			CombatManager combatManager;
			ProviderManager.TryGetProvider<CombatManager>(out combatManager);

			CardManager cardManager;
			ProviderManager.TryGetProvider<CardManager>(out cardManager);
			foreach (var card in cardManager.GetHand())
			{
				foreach (var trait in card.GetTraitStates())
				{
					CardTraitSuppressExhaust cardTraitSuppressExhaust;
					if ((cardTraitSuppressExhaust = (trait as CardTraitSuppressExhaust)) != null)
					{
						discardCardParams.outSuppressTraitOnDiscard = typeof(CardTraitExhaustState);
					}
				}
			}
		}
	}

	[HarmonyPatch(typeof(RelicManager), "InstantApplyOnDiscardRandomRelicEffects")]
    class PrediscardSuppressPatch
    {
        static void Postfix(RelicManager __instance, CardManager.DiscardCardParams discardCardParams)
        {
			if (!discardCardParams.wasPlayed || !discardCardParams.discardCard.HasTrait(typeof(CardTraitExhaustState)))
			{
				return;
			}

			CombatManager combatManager;
			ProviderManager.TryGetProvider<CombatManager>(out combatManager);

			CardManager cardManager;
			ProviderManager.TryGetProvider<CardManager>(out cardManager);
			foreach (var card in cardManager.GetHand())
			{
				foreach (var trait in card.GetTraitStates())
				{
					CardTraitSuppressExhaust cardTraitSuppressExhaust;
					if ((cardTraitSuppressExhaust = (trait as CardTraitSuppressExhaust)) != null)
					{
						discardCardParams.outSuppressTraitOnDiscard = typeof(CardTraitExhaustState);
					}
				}
			}
		}
    }
}


