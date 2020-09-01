using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    public class CardTraitScalingAddDamagePerCard : CardTraitState
    {
		public override int OnApplyingDamage(ApplyingDamageParameters damageParams)
		{
			CardStatistics cardStatistics = damageParams.combatManager.GetCardManager().GetCardStatistics();
			int baseDamage = GetBaseDamage(damageParams.damageSourceCard);
			int extraDamage = GetExtraDamage(damageParams.damageSourceCard);
			CharacterState attacker = damageParams.attacker;
			if (attacker != null)
			{
				attacker.SetAttackDamage(attacker.GetAttackDamage() + baseDamage + extraDamage);
			}
			return baseDamage + extraDamage;
		}

		private int GetExtraDamage(CardState thisCard)
		{
			CardStateModifiers cardStateModifiers = thisCard.GetCardStateModifiers();
			CardStateModifiers temporaryCardStateModifiers = thisCard.GetTemporaryCardStateModifiers();
			return CardStateModifiers.GetUpgradedStatValue(CardStateModifiers.GetUpgradedStatValue(0, CardStateModifiers.StatType.Damage, cardStateModifiers), CardStateModifiers.StatType.Damage, temporaryCardStateModifiers);
		}

		private int GetBaseDamage(CardState thisCard)
		{
			int paramInt = GetParamInt();

			CardManager cardManager;
			ProviderManager.TryGetProvider<CardManager>(out cardManager);

			if (cardManager == null) { return paramInt; }

			int statValue = 0;
			foreach (var card in cardManager.GetAllCards())
            {
				if (card.GetCardDataID() == GetCardTraitData().GetParamStr())
					statValue++;
            }

			return paramInt * statValue;
		}

		public override bool HasMultiWordDesc()
		{
			return true;
		}

		public override string GetCurrentEffectText(CardStatistics cardStats, SaveManager saveManager, RelicManager relicManager)
		{
			int extraDamage = GetExtraDamage(GetCard());
			if (cardStats == null)
			{
				if (extraDamage > 0)
				{
					return string.Format("CardTraitScalingAddDamage_ExtraDamage_XCostOutsideBattle_CardText".Localize(), extraDamage, "tempUpgradeHighlight");
				}
				return string.Empty;
			}
			int baseDamage = GetBaseDamage(GetCard());
			if (extraDamage > 0)
			{
				if (!CardStatistics.GetTrackedValueIsValidOutsideBattle(base.StatValueData.trackedValue) && !cardStats.GetIsInActiveBattle())
				{
					return string.Format("CardTraitScalingAddDamage_ExtraDamage_XCostOutsideBattle_CardText".Localize(), extraDamage, "tempUpgradeHighlight");
				}
				return string.Format("CardTraitScalingAddDamage_ExtraDamage_CardText".Localize(new LocalizedInteger(baseDamage, baseDamage + extraDamage)), extraDamage, "tempUpgradeHighlight");
			}
			if (!cardStats.GetStatValueShouldDisplayOnCardNow(base.StatValueData))
			{
				return string.Empty;
			}
			return "CardTraitScalingAddDamage_CurrentScaling_CardText".Localize(new LocalizedIntegers(baseDamage));
		}
	}
}
