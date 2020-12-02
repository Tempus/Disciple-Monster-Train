using Trainworks.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using System.Collections;
using UnityEngine;
using System.Reflection;
using System.IO;
using Trainworks.Builders;
using Trainworks;

namespace DiscipleClan.CardEffects
{
    class WardStatePower : WardState
    {
		public CardUpgradeState upgradeState;

		public WardStatePower() 
        {
            ID = "Power";
            tooltipTitleKey = "PowerWardBeta_Name";
            tooltipBodyKey = "PowerWardBeta_Desc";

            var localPath = Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath);
            wardIcon = CustomAssetManager.LoadSpriteFromPath(Path.Combine(localPath, "Unit Assets/PowerWard.png"));

			CardUpgradeData upgrade = new CardUpgradeDataBuilder
			{
				BonusDamage = power
			}.Build();

			upgradeState = new CardUpgradeState();
			upgradeState.Setup(upgrade);
		}

		public override void OnTriggerNow(List<CharacterState> targets)
        {
			foreach (var unit in targets)
			{
				if (unit.GetTeamType() == Team.Type.Monsters)
				{
					Traverse.Create(unit).Field("_primaryStateInformation").Property<int>("AttackDamage").Value += power;
					unit.StartCoroutine(ShowDelayedNotification(unit));

					CardState spawnerCard = unit.GetSpawnerCard();
					if (spawnerCard != null && !ProviderManager.SaveManager.PreviewMode && !unit.HasStatusEffect("cardless"))
					{
						CardAnimator.CardUpgradeAnimationInfo type = new CardAnimator.CardUpgradeAnimationInfo(spawnerCard, upgradeState);
						CardAnimator.DoAddRecentCardUpgrade.Dispatch(type);
						spawnerCard.GetTemporaryCardStateModifiers().AddUpgrade(upgradeState);
						spawnerCard.UpdateCardBodyText();
						ProviderManager.TryGetProvider<CardManager>(out CardManager cardManager);
						cardManager?.RefreshCardInHand(spawnerCard);
					}
				}
			}
		}

		public IEnumerator ShowDelayedNotification(CharacterState unit)
        {
			yield return new WaitForSeconds(0.3f);
            unit.ShowNotification(CardEffectBuffDamage.GetNotificationText(power), PopupNotificationUI.Source.General);

			ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);
			yield return roomManager.GetRoomUI().SetSelectedRoom(unit.GetCurrentRoomIndex());
		}
	}
}
