using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System.Collections;
using UnityEngine;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectAdapted : StatusEffectState
    {
        public const string statusId = "adapted";

		public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
		{
			if (inputTriggerParams.damage == 0)
			{
				return false;
			}
			CharacterState attacked = inputTriggerParams.attacked;
			if (attacked == null)
			{
				return false;
			}

			int attack = attacked.GetAttackDamage();
			int min = Mathf.Min(attacked.GetAttackDamage(), inputTriggerParams.damage);

			attacked.DebuffDamage(min);

			outputTriggerParams.damage = inputTriggerParams.damage - min;
			//outputTriggerParams.visualDamage = inputTriggerParams.damage - min;
			//outputTriggerParams.damageBlocked = min;
			return true;
		}

        //protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        //{
        //    CharacterState attacked = inputTriggerParams.attacked;
        //    if (!(attacked == null))
        //    {
        //        attacked.ShowNotification("StatusEffect_Fragile_NotificationText".Localize(), PopupNotificationUI.Source.General);
        //    }
        //    yield break;
        //}

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectAdapted).AssemblyQualifiedName,
                StatusId = statusId,
                DisplayCategory = StatusEffectData.DisplayCategory.Persistent,
                TriggerStage = StatusEffectData.TriggerStage.OnPreAttacked,
                IsStackable = false,
                IconPath = "chrono/Status/weight.png",
            }.Build();
        }
    }
}
