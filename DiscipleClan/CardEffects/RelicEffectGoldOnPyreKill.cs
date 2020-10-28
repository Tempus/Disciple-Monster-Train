using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Trainworks.Managers;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
	public sealed class RelicEffectGoldOnPyreKill : RelicEffectBase, IStartOfPlayerTurnBeforeDrawRelicEffect, ITurnTimingRelicEffect, IRelicEffect
	{
		private int goldAmount;

		public override bool CanApplyInPreviewMode => true;

		public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
		{
			base.Initialize(relicState, relicData, relicEffectData);
			goldAmount = relicEffectData.GetParamInt();
		}

		public bool TestEffect(RelicEffectParams relicEffectParams)
		{
			return false;
		}

		public IEnumerator ApplyEffect(RelicEffectParams relicEffectParams)
		{
			yield break;
		}

		public override IEnumerator OnCharacterAdded(CharacterState character, CardState fromCard, RelicManager relicManager, SaveManager saveManager, PlayerManager playerManager, RoomManager roomManager, CombatManager combatManager, CardManager cardManager)
		{
			if (character.GetTeamType() == Team.Type.Heroes)
			{
				character.AddDeathSignal(OnEnemyDeath);
			}
			yield break;
		}

		public override string GetActivatedDescription()
		{
			return null;
		}

		private IEnumerator OnEnemyDeath(CharacterDeathParams deathParams)
		{
			if (deathParams == null)
			{
				yield break;
			}
			CharacterState attackingCharacter = deathParams.attackingCharacter;
			RelicManager relicManager = deathParams.relicManager;
			SaveManager saveManager = deathParams.saveManager;
			if (attackingCharacter != null && attackingCharacter.IsPyreHeart())
			{
				if (!saveManager.PreviewMode)
				{
					//attackingCharacter.GetCharacterUI().ShowEffectVFX(attackingCharacter, _srcRelicEffectData.GetAppliedVfx());
					if (!ProviderManager.TryGetProvider<PlayerManager>(out PlayerManager playerManager)) { yield break; }
					playerManager.AdjustGold(goldAmount, isReward: false);
					deathParams.deadCharacter.ShowNotification("HudNotification_TreasureHeroTriggered".Localize(new LocalizedInteger(deathParams.saveManager.GetAdjustedGoldAmount(goldAmount, isReward: false))), PopupNotificationUI.Source.General);

					NotifyRelicTriggered(relicManager, attackingCharacter);
				}
			}
		}
	}
}
