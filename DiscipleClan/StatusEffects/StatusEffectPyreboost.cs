using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.StatusEffects
{
	public class MTStatusEffect_Pyreboost : IMTStatusEffect { public string ID => "pyreboost"; }

	class StatusEffectPyreboost : StatusEffectState
    {
        public const string StatusId = "pyreboost";

		public override void OnStacksAdded(CharacterState character, int numStacksAdded)
		{
			if (character != null && numStacksAdded > 0)
			{
				character.BuffDamage(DamageValue(numStacksAdded), null, fromStatusEffect: true);
			}
		}

		public override void OnStacksRemoved(CharacterState character, int numStacksRemoved)
		{
			if (character != null && numStacksRemoved > 0)
			{
				character.DebuffDamage(DamageValue(numStacksRemoved), null, fromStatusEffect: true);
			}
		}

		//protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
		//{
  //          // Do damage to the pyre here!
  //          yield break;
		//}

		private int DamageValue(int stacks)
		{
			return GetMagnitudePerStack() * stacks;
		}

		public override int GetEffectMagnitude(int stacks = 1)
		{
			return DamageValue(stacks);
		}

		public override int GetMagnitudePerStack()
		{
			var pyreroom = GameObject.FindObjectOfType<PyreRoomState>() as PyreRoomState;
			int pyreAttack = 0;
			pyreroom.TryGetPyreAttack(out pyreAttack);
			return pyreAttack;
		}

		public static void Make()
        {
            new StatusEffectDataBuilder
            {
                statusEffectStateName = typeof(StatusEffectPyreboost).AssemblyQualifiedName,
                statusId = "pyreboost",
                displayCategory = StatusEffectData.DisplayCategory.Positive,
                triggerStage = StatusEffectData.TriggerStage.OnDeath,
				icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
			}.Build();
        }

    }
}
