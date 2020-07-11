namespace DiscipleClan.CardEffects
{
    public class CardTraitMultiplyCharacterUpgrade : CardTraitState
    {
        public override void OnApplyingCardUpgradeToUnit(CardState thisCard, CharacterState targetUnit, CardUpgradeState upgradeState, CardManager cardManager)
        {
            int multiplier = GetMultiplier(cardManager.GetCardStatistics());
            upgradeState.SetAttackDamage(upgradeState.GetAttackDamage() * multiplier);
            upgradeState.SetAdditionalHP(upgradeState.GetAdditionalHP() * multiplier);
            upgradeState.SetAdditionalSize(upgradeState.GetAdditionalSize() * multiplier);
        }

        private int GetMultiplier(CardStatistics cardStatistics)
        {
            CardStatistics.StatValueData statValueData = default(CardStatistics.StatValueData);
            statValueData.cardState = GetCard();
            statValueData.trackedValue = GetParamTrackedValue();
            statValueData.entryDuration = GetParamEntryDuration();
            statValueData.cardTypeTarget = GetParamCardType();
            statValueData.paramSubtype = GetParamSubtype();
            CardStatistics.StatValueData statValueData2 = statValueData;
            int statValue = cardStatistics.GetStatValue(statValueData2);
            return statValue;
        }

        public override string GetCurrentEffectText(CardStatistics cardStatistics, SaveManager saveManager, RelicManager relicManager)
        {
            //if (cardStatistics.GetStatValueShouldDisplayOnCardNow(base.StatValueData))
            //{
            //	return string.Format("CardTraitScalingUpgradeUnitAttack_CurrentScaling_CardText".Localize(), GetMultiplier(cardStatistics));
            //}
            return string.Empty;
        }
    }
}
