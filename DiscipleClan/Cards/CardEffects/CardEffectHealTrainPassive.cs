using MonsterTrainModdingAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DiscipleClan.Cards.CardEffects
{
    class CardEffectHealTrainPassive : CardEffectBase
    {
		public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			int num = 0;
			CardState parentCardState = cardEffectState.GetParentCardState();
			if (parentCardState != null)
			{
				CardTraitState.ApplyingHealParameters healParameters = default(CardTraitState.ApplyingHealParameters);
				healParameters.combatManager = cardEffectParams.combatManager;
				foreach (CardTraitState traitState in parentCardState.GetTraitStates())
				{
					healParameters.healAmount = num;
					num = traitState.OnApplyingTrainHeal(healParameters);
				}
			}
			num += cardEffectState.GetParamInt();
			PyreRoomState pyreRoom = cardEffectParams.roomManager.GetPyreRoom();
			CharacterState characterState = pyreRoom?.GetPyreHeart();
			if (characterState != null)
			{
				num = Mathf.Min(cardEffectParams.saveManager.GetMaxTowerHP() - cardEffectParams.playerManager.GetTowerHP(), num);
				characterState.GetCharacterUI().ApplyStateToUI(pyreRoom.GetPyreHeart(), cardEffectParams.popupNotificationManager, num, doingDamage: false);
			}
			cardEffectParams.playerManager.HealTowerHP(num);

			yield break;
		}
	}
}
