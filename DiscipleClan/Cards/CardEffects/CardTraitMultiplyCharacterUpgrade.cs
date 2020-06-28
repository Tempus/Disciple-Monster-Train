using System;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.Cards.CardEffects
{
    public class CardTraitMultiplyCharacterUpgrade : CardTraitState
    {
        public override void OnApplyingCardUpgradeToUnit(
          CardState thisCard,
          CharacterState targetUnit,
          CardUpgradeState upgradeState,
          CardManager cardManager)
        {
            for (int i = 0; i < this.GetParamInt() * cardManager.GetCardStatistics().GetStatValue(this.StatValueData); i++)
            {
                targetUnit.ApplyCardUpgrade(upgradeState);
            }
        }

        //public override string GetCurrentEffectText(
        //  CardStatistics cardStatistics,
        //  SaveManager saveManager,
        //  RelicManager relicManager)
        //{
        //    return cardStatistics.GetStatValueShouldDisplayOnCardNow(this.StatValueData) ? string.Format("CardTraitScalingUpgradeUnitAttack_CurrentScaling_CardText".Localize((ILocalizationParameterContext)null), (object)this.GetAdditionalDamage(cardStatistics)) : string.Empty;
        //}
    }
}
