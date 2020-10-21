using Trainworks.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using System.Collections;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace DiscipleClan.CardEffects
{
    class WardStatePyrebound : WardState
    {
        public WardStatePyrebound() 
        {
            ID = "Pyrebound";
            tooltipTitleKey = "PyromancyWardBeta_Name";
            tooltipBodyKey = "PyromancyWardBeta_Desc";

            var localPath = Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).LocalPath);
            wardIcon = CustomAssetManager.LoadSpriteFromPath(Path.Combine(localPath, "chrono/Unit Assets/SwordWard.png"));
			saveManager = ProviderManager.SaveManager;
			Setup();
		}

        public override void OnTriggerNow(List<CharacterState> targets)
        {
			ApplyEffectInternal();
		}

		// Enchanting starts here

		private enum EnchanterStateNextAction
		{
			NoAction,
			AddStatusEffect,
			RemoveStatusEffect
		}

		private class EnchantedState
		{
			public bool isEnchanted;

			public EnchanterStateNextAction nextStateAction;

			public EnchantedState()
			{
			}

			public EnchantedState(EnchantedState other)
			{
				isEnchanted = other.isEnchanted;
				nextStateAction = other.nextStateAction;
			}

			public override string ToString()
			{
				return $"(Applied? {isEnchanted} Action? {nextStateAction})";
			}
		}

		private Dictionary<CharacterState, EnchantedState> _primaryEnchantedTargets;
		private Dictionary<CharacterState, EnchantedState> _previewEnchantedTargets;
		private StatusEffectStackData statusEffect;
		private SaveManager saveManager;

		private Dictionary<CharacterState, EnchantedState> EnchantedTargets
		{
			get
			{
				if (saveManager != null)
				{
					if (saveManager.PreviewMode)
					{
						return _previewEnchantedTargets;
					}
					return _primaryEnchantedTargets;
				}
				return _primaryEnchantedTargets;
			}
		}

		public void Setup()
		{
			_primaryEnchantedTargets = new Dictionary<CharacterState, EnchantedState>();
			_previewEnchantedTargets = new Dictionary<CharacterState, EnchantedState>();
			statusEffect = new StatusEffectStackData { count = 1, statusId = "pyreboost" };
		}

		private void ApplyEffectInternal()
		{
			if (saveManager.PreviewMode)
			{
				_previewEnchantedTargets.Clear();
				foreach (KeyValuePair<CharacterState, EnchantedState> primaryEnchantedTarget in _primaryEnchantedTargets)
				{
					_previewEnchantedTargets.Add(primaryEnchantedTarget.Key, new EnchantedState(primaryEnchantedTarget.Value));
				}
			}
			foreach (CharacterState target in GetRoomTargets(Team.Type.Monsters))
			{
				if (!EnchantedTargets.ContainsKey(target))
				{
					EnchantedTargets.Add(target, new EnchantedState());
				}
			}
			foreach (KeyValuePair<CharacterState, EnchantedState> enchantedTarget in EnchantedTargets)
			{
				CharacterState key = enchantedTarget.Key;
				enchantedTarget.Value.nextStateAction = (IsEnchantmentValidForTarget(key) ? EnchanterStateNextAction.AddStatusEffect : EnchanterStateNextAction.RemoveStatusEffect);
			}
			BalanceData.TimingData activeTiming = saveManager.GetActiveTiming();
			foreach (KeyValuePair<CharacterState, EnchantedState> enchantedTarget2 in EnchantedTargets)
			{
				CharacterState key2 = enchantedTarget2.Key;
				EnchantedState value = enchantedTarget2.Value;
				if (!key2.IsDead && !key2.IsDestroyed)
				{
					if (value.nextStateAction == EnchanterStateNextAction.AddStatusEffect && !value.isEnchanted)
					{
						CharacterState.AddStatusEffectParams addStatusEffectParams = default(CharacterState.AddStatusEffectParams);
						addStatusEffectParams.fromEffectType = typeof(CardEffectEnchant);
						addStatusEffectParams.sourceIsHero = (key2.GetTeamType() == Team.Type.Heroes);
						CharacterState.AddStatusEffectParams addStatusEffectParams2 = addStatusEffectParams;
						key2.AddStatusEffect(statusEffect.statusId, statusEffect.count, addStatusEffectParams2);
						value.isEnchanted = true;
					}
					else if (value.nextStateAction == EnchanterStateNextAction.RemoveStatusEffect && value.isEnchanted)
					{
						key2.RemoveStatusEffect(statusEffect.statusId, removeAtEndOfTurn: false, statusEffect.count, !saveManager.PreviewMode, null, typeof(CardEffectEnchant));
						value.isEnchanted = false;
					}
					value.nextStateAction = EnchanterStateNextAction.NoAction;
				}
			}
		}

		private bool IsEnchantmentValidForTarget(CharacterState target)
		{
			if (((object)target == null || !target.IsDestroyed) && (object)target != null && target.IsAlive)
			{
				return this.floor == target.GetCurrentRoomIndex();
			}
			return false;
		}

		public IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			ApplyEffectInternal();
			yield break;
		}

		public void OnSpawnPointsChanged()
		{
			ApplyEffectInternal();
		}

	}

    [HarmonyPatch(typeof(RoomManager), "UpdateEnchantments")]
    class PyreboostWardTrigger
    {
        static void Postfix(RoomManager __instance)
        {
             WardManager.TriggerWardsNow("Pyrebound", 0, null);
			 WardManager.TriggerWardsNow("Pyrebound", 1, null);
 			 WardManager.TriggerWardsNow("Pyrebound", 2, null);
		}
	}
}
