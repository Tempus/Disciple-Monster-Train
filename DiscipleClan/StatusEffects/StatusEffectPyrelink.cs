using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using System.Collections;
using MonsterTrainModdingAPI;

namespace DiscipleClan.StatusEffects
{
	public class MTStatusEffect_Pyrelink : IMTStatusEffect { public string ID => "pyrelink"; }

	class StatusEffectPyrelink : StatusEffectState
    {
        public const string StatusId = "pyrelink";

		public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
		{
			List<CharacterState> towerchars = new List<CharacterState>();
			inputTriggerParams.combatManager.GetHeroManager().AddCharactersInTowerToList(towerchars);
            foreach (var hero in towerchars)
            {
				hero.AddDeathSignal(OnEnemyDeath, true);
            }

			return true;
		}

		// If the Pyre kills something, I want to know about it!
		private IEnumerator OnEnemyDeath(CharacterDeathParams deathParams)
		{
			if (deathParams == null)
			{
				yield break;
			}
			CharacterState attackingCharacter = deathParams.attackingCharacter;
			SaveManager saveManager = deathParams.saveManager;
			if (attackingCharacter != null && attackingCharacter.IsPyreHeart())
			{
				var buffAmount = GetEffectMagnitude();
				var champion = this.GetAssociatedCharacter();

				// We killed something, Let's buff
				if (!saveManager.PreviewMode)
				{
					// attackingCharacter.GetCharacterUI().ShowEffectVFX(attackingCharacter, _srcRelicEffectData.GetAppliedVfx());
					champion.BuffMaxHP(buffAmount);
				}
				// We killed something imaginarily, we should preview here? Wait, how does the PYRE preview a kill?
				else
				{
					API.Log(BepInEx.Logging.LogLevel.All, "This triggers when we're previewing pyrelink");
					// attackingCharacter.PreviewHpOnPyreHeart(Mathf.Min(attackingCharacter.GetHP() + healAmount, saveManager.GetMaxTowerHP()));
				}
			}
		}

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                statusEffectStateName = typeof(StatusEffectPyrelink).AssemblyQualifiedName,
                statusId = "pyrelink",
                displayCategory = StatusEffectData.DisplayCategory.Positive,
                triggerStage = StatusEffectData.TriggerStage.OnMonsterTeamTurnBegin,
				icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Clan Assets/clan_32.png"),
				isStackable = true,
			}.Build();
        }
    }
}
