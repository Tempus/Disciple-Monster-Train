using Trainworks;
using Trainworks.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static Trainworks.Constants.VanillaClanIDs;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.CardEffects
{
    class CardEffectAddClassStatus : CardEffectBase
    {
		public static StatusEffectStackData GetStatusEffectStack(CardEffectState cardEffectState)
		{
			ClassData otherClass;
			bool exiled = false;
            StatusEffectStackData statusEffectStackData;

            var mainClass = ProviderManager.SaveManager.GetMainClass();
			var subClass = ProviderManager.SaveManager.GetSubClass();

			if (mainClass == DiscipleClan.clanRef)
            {
				otherClass = subClass;
				exiled = ProviderManager.SaveManager.GetSubChampionIndex() != 0;
			}
			else
            {
				otherClass = mainClass;
				exiled = ProviderManager.SaveManager.GetMainChampionIndex() != 0;
			}

			int Param = cardEffectState.GetParamInt();

			if (otherClass == null) { return null; }
            switch (otherClass.GetID())
            {
                case Hellhorned:
					if (!exiled)
	                    statusEffectStackData = new StatusEffectStackData { statusId = Rage, count = Param };
					else
						statusEffectStackData = new StatusEffectStackData { statusId = Armor, count = Param * 2 };
					break;
                case Awoken:
					if (!exiled)
						statusEffectStackData = new StatusEffectStackData { statusId = Spikes, count = Param };
					else
						statusEffectStackData = new StatusEffectStackData { statusId = Regen, count = Param };
					break;
                case Stygian:
					if (!exiled)
						statusEffectStackData = new StatusEffectStackData { statusId = SpellWeakness, count = Param / 2 };
					else
						statusEffectStackData = new StatusEffectStackData { statusId = Frostbite, count = Param };
					break;
                case Umbra:
					if (!exiled)
						statusEffectStackData = new StatusEffectStackData { statusId = DamageShield, count = Param / 2 };
					else
						statusEffectStackData = new StatusEffectStackData { statusId = Lifesteal, count = Param / 2 };
					break;
                case MeltingRemnant:
					if (!exiled)
						statusEffectStackData = new StatusEffectStackData { statusId = Burnout, count = Param + 1 };
					else
						statusEffectStackData = new StatusEffectStackData { statusId = Stealth, count = Param / 2 };
					break;
                default:
                    statusEffectStackData = new StatusEffectStackData { statusId = "gravity", count = Param / 2 };
                    break;
            }

            return statusEffectStackData;
		}

        public override string GetCardText(CardEffectState cardEffectState, RelicManager relicManager = null)
        {
            LocalizationUtil.GeneratedTextDisplay = LocalizationUtil.GeneratedTextDisplayType.Show;
            var status = GetStatusEffectStack(cardEffectState);
			if (status == null)
				return "Apply a status dependent on your paired clan.<br><i>(See tooltips)</i>";
			return "Apply " + StatusEffectManager.GetLocalizedName(status.statusId, status.count, true);
        }

		public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			StatusEffectStackData statusEffectStack = GetStatusEffectStack(cardEffectState);
			if (cardEffectParams.targets.Count <= 0)
			{
				return false;
			}
			if (statusEffectStack.statusId == Burnout)
            {
				if (cardEffectParams.targets[0].IsMiniboss() || cardEffectParams.targets[0].IsOuterTrainBoss())
					return false;
            }
			if (statusEffectStack == null)
			{
				return false;
			}
			if (cardEffectState.GetTargetMode() != TargetMode.DropTargetCharacter)
			{
				return true;
			}
			if (cardEffectParams.statusEffectManager.GetStatusEffectDataById(statusEffectStack.statusId).IsStackable())
			{
				return true;
			}
			foreach (CharacterState target in cardEffectParams.targets)
			{
				if (!target.HasStatusEffect(statusEffectStack.statusId))
				{
					return true;
				}
			}
			return false;
		}

		public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			StatusEffectStackData statusEffectStack = GetStatusEffectStack(cardEffectState);
			if (statusEffectStack == null)
			{
				yield break;
			}
			if (statusEffectStack.statusId == Burnout)
			{
				if (cardEffectParams.targets[0].IsMiniboss() || cardEffectParams.targets[0].IsOuterTrainBoss())
					yield break;
			}
			CharacterState.AddStatusEffectParams addStatusEffectParams = default(CharacterState.AddStatusEffectParams);
			addStatusEffectParams.sourceRelicState = cardEffectParams.sourceRelic;
			addStatusEffectParams.sourceCardState = cardEffectParams.playedCard;
			addStatusEffectParams.cardManager = cardEffectParams.cardManager;
			addStatusEffectParams.sourceIsHero = (cardEffectState.GetSourceTeamType() == Team.Type.Heroes);
			CharacterState.AddStatusEffectParams addStatusEffectParams2 = addStatusEffectParams;
			for (int num = cardEffectParams.targets.Count - 1; num >= 0; num--)
			{
				CharacterState characterState = cardEffectParams.targets[num];
				int count = statusEffectStack.count;
				characterState.AddStatusEffect(statusEffectStack.statusId, count, addStatusEffectParams2);
			}
		}

		//public override void GetTooltipsStatusList(CardEffectState cardEffectState, ref List<string> outStatusIdList)
		//{
		//	StatusEffectStackData statusEffectStack = GetStatusEffectStack(cardEffectState);
		//	if (statusEffectStack != null)
		//	{
		//		outStatusIdList.Add(statusEffectStack.statusId);
		//	} else
  //          {
		//		outStatusIdList.Add(Rage);
		//		outStatusIdList.Add(Regen);
		//		outStatusIdList.Add(SpellWeakness);
		//		outStatusIdList.Add(DamageShield);
		//		outStatusIdList.Add(Burnout);
		//		outStatusIdList.Add("gravity");
		//	}
		//}
		public string GetTooltipBaseKey(CardEffectState cardEffectState)
		{
			return "CardEffectAddClassStatus";
		}

	}
}
