using Trainworks.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.CardEffects
{
    class CardEffectHalveDamageDebuff : CardEffectBase
    {
		private static readonly string ActivatedKey = "CardEffectBuffDamage_Activated";

		private const string PositiveEffectKey = "TextFormat_Add";

		private const string NegativeEffectKey = "TextFormat_Default";

		private int buffAmount;

		public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			foreach (CharacterState target in cardEffectParams.targets)
			{
				if (!TestEffectOnTarget(cardEffectState, cardEffectParams, target))
				{
					continue;
				}
				buffAmount = -(target.GetAttackDamageWithoutStatusEffectBuffs() / 2);
				if (cardEffectState.GetParentCardState() != null)
				{
					CardTraitState.ApplyingDamageParameters applyingDamageParameters = default(CardTraitState.ApplyingDamageParameters);
					applyingDamageParameters.damage = buffAmount;
					applyingDamageParameters.damageType = Damage.Type.Default;
					applyingDamageParameters.combatManager = cardEffectParams.combatManager;
					CardTraitState.ApplyingDamageParameters damageParams = applyingDamageParameters;
					foreach (CardTraitState traitState in cardEffectState.GetParentCardState().GetTraitStates())
					{
						damageParams.damage = buffAmount;
						buffAmount = traitState.OnApplyingBuffDamageToUnit(cardEffectParams.cardManager, damageParams);
					}
				}

				CardUpgradeData upgrade = new CardUpgradeDataBuilder
				{
					BonusDamage = -buffAmount
				}.Build();

				var upgradeState = new CardUpgradeState();
				upgradeState.Setup(upgrade);

				target.ApplyCardUpgrade(upgradeState);
				
				NotifyHealthEffectTriggered(cardEffectParams.saveManager, cardEffectParams.popupNotificationManager, GetActivatedDescription(cardEffectState), target.GetCharacterUI());
				if (!cardEffectParams.saveManager.PreviewMode && target.IsPyreHeart() && cardEffectState.GetTargetMode() == TargetMode.Pyre)
				{
					cardEffectParams.saveManager.pyreAttackChangedSignal.Dispatch(cardEffectParams.saveManager.GetDisplayedPyreAttack(), cardEffectParams.saveManager.GetDisplayedPyreNumAttacks());
				}
			}
			yield break;
		}

		public override string GetActivatedDescription(CardEffectState cardEffectState)
		{
			return GetNotificationText(buffAmount);
		}

		public static string GetNotificationText(int amount)
		{
			if (ActivatedKey.HasTranslation())
			{
				string key = (amount >= 0) ? "TextFormat_Add" : "TextFormat_Default";
				return string.Format(ActivatedKey.Localize(), string.Format(key.Localize(), amount));
			}
			return null;
		}

		public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			foreach (CharacterState target in cardEffectParams.targets)
			{
				if (TestEffectOnTarget(cardEffectState, cardEffectParams, target))
				{
					return true;
				}
			}
			return false;
		}

		public override bool TestEffectOnTarget(CardEffectState cardEffectState, CardEffectParams cardEffectParams, CharacterState target)
		{
			if (target.IsPyreHeart())
			{
				if (cardEffectParams.saveManager.PreviewMode && !cardEffectParams.GetSelectedRoom().GetIsPyreRoom())
				{
					return false;
				}
				return true;
			}
			if (target.IsAlive && target.GetCanAttack())
			{
				return true;
			}
			return false;
		}
	}
}
