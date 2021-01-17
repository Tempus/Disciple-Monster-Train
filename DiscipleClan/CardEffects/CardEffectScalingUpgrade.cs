using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DiscipleClan.Triggers;
using UnityEngine;
using HarmonyLib;

namespace DiscipleClan.CardEffects
{
    class CardEffectScalingUpgrade : CardEffectBase
    {
		public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			return cardEffectParams.targets.Count > 0;
		}

		public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
            foreach (CharacterState target in cardEffectParams.targets)
            {
                if (!OnGainEmber.energyData.TryGetValue(target, out int addEnergy))
                    addEnergy = 1;

                CardUpgradeState upgradeState = new CardUpgradeState();
                upgradeState.Setup(cardEffectState.GetParamCardUpgradeData());

                // Tweak the CardUpgradeState's stats
                Traverse.Create(upgradeState).Field("attackDamage").SetValue(upgradeState.GetAttackDamage() * addEnergy);
                Traverse.Create(upgradeState).Field("additionalHP").SetValue(upgradeState.GetAdditionalHP() * addEnergy);

                if (cardEffectParams.playedCard != null)
                {
                    foreach (CardTraitState traitState in cardEffectParams.playedCard.GetTraitStates())
                    {
                        traitState.OnApplyingCardUpgradeToUnit(cardEffectParams.playedCard, target, upgradeState, cardEffectParams.cardManager);
                    }
                }
                int attackDamage = upgradeState.GetAttackDamage();
                int additionalHP = upgradeState.GetAdditionalHP();
                string text = ((attackDamage != 0) ? GetAttackNotificationText(upgradeState) : null);
                string text2 = ((additionalHP != 0) ? GetHPNotificationText(upgradeState) : null);
                string text3 = string.Empty;
                if (text != null && text2 != null)
                {
                    text3 = string.Format("TextFormat_SpacedItems".Localize(), text, text2);
                }
                else if (text != null)
                {
                    text3 = text;
                }
                else if (text2 != null)
                {
                    text3 = text2;
                }
                if (text3 != null)
                {
                    NotifyHealthEffectTriggered(cardEffectParams.saveManager, cardEffectParams.popupNotificationManager, text3, target.GetCharacterUI());
                }
                yield return target.ApplyCardUpgrade(upgradeState);
                CardState spawnerCard = target.GetSpawnerCard();
                bool flag = target.HasStatusEffect("cardless");
                if (spawnerCard != null && !cardEffectParams.saveManager.PreviewMode && !flag)
                {
                    CardAnimator.CardUpgradeAnimationInfo type = new CardAnimator.CardUpgradeAnimationInfo(spawnerCard, upgradeState);
                    CardAnimator.DoAddRecentCardUpgrade.Dispatch(type);
                    spawnerCard.GetTemporaryCardStateModifiers().AddUpgrade(upgradeState);
                    spawnerCard.UpdateCardBodyText();
                    cardEffectParams.cardManager?.RefreshCardInHand(spawnerCard);
                }
            }
        }

		private string GetAttackNotificationText(CardUpgradeState upgradeState)
		{
			return CardEffectBuffDamage.GetNotificationText(upgradeState.GetAttackDamage());
		}

		private string GetHPNotificationText(CardUpgradeState upgradeState)
		{
			int additionalHP = upgradeState.GetAdditionalHP();
			if (additionalHP >= 0)
			{
				return CardEffectBuffMaxHealth.GetNotificationText(additionalHP);
			}
			return CardEffectDebuffMaxHealth.GetNotificationText(Mathf.Abs(additionalHP));
		}

		public override void GetTooltipsStatusList(CardEffectState cardEffectState, ref List<string> outStatusIdList)
		{
			GetTooltipsStatusList(cardEffectState.GetSourceCardEffectData(), ref outStatusIdList);
		}

		public static void GetTooltipsStatusList(CardEffectData cardEffectData, ref List<string> outStatusIdList)
		{
			foreach (StatusEffectStackData statusEffectUpgrade in cardEffectData.GetParamCardUpgradeData().GetStatusEffectUpgrades())
			{
				outStatusIdList.Add(statusEffectUpgrade.statusId);
			}
		}

		public string GetTipTooltipKey(CardEffectState cardEffectState)
		{
			if (cardEffectState.GetParamCardUpgradeData() != null && cardEffectState.GetParamCardUpgradeData().HasUnitStatUpgrade())
			{
				return "TipTooltip_StatChangesStick";
			}
			return null;
		}
	}
}
