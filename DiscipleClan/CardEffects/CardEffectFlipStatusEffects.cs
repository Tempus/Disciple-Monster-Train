using HarmonyLib;
using MonsterTrainModdingAPI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.CardEffects
{
	class CardEffectFlipStatusEffects : CardEffectBase
	{
		public static Dictionary<string, string> StatusFlips = new Dictionary<string, string>
		{
			["buff"] = "debuff",
			["regen"] = "poison",
			[Stealth] = Dazed,
			["spell shield"] = "spell weakness",
			["emberboost"] = "scorch",
			["spikes"] = "poison",
			["gravity"] = "rooted",
			[DamageShield] = "fragile",
			["lifesteal"] = "heal immunity",
		};

		public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
		{
			CharacterState.AddStatusEffectParams addStatusEffectParams = default(CharacterState.AddStatusEffectParams);
			addStatusEffectParams.sourceRelicState = cardEffectParams.sourceRelic;
			addStatusEffectParams.sourceCardState = cardEffectParams.playedCard;
			addStatusEffectParams.cardManager = cardEffectParams.cardManager;
			addStatusEffectParams.sourceIsHero = (cardEffectState.GetSourceTeamType() == Team.Type.Heroes);
			CharacterState.AddStatusEffectParams addStatusEffectParams2 = addStatusEffectParams;

			foreach (CharacterState character in cardEffectParams.targets)
			{
				List<CharacterState.StatusEffectStack> statusEffectStacks;
				character.GetStatusEffects(out statusEffectStacks);


				// True flips buffs into debuffs, false flips debuffs into buffs
                foreach (var status in statusEffectStacks)
                {
					if (cardEffectState.GetParamBool())
                    {
						if (StatusFlips.ContainsKey(status.State.GetStatusId()))
                        {
							int count = status.Count;
							character.RemoveStatusEffect(status.State.GetStatusId(), false, count);

							string debuff = StatusFlips.GetValueSafe(status.State.GetStatusId());
							API.Log(BepInEx.Logging.LogLevel.All, "Flipping " + status.State.GetStatusId() + " to " + debuff + " - " + count);
							character.AddStatusEffect(debuff, count);
						}
					}
					else
                    {
						if (StatusFlips.ContainsValue(status.State.GetStatusId()))
						{
							int count = status.Count;
							character.RemoveStatusEffect(status.State.GetStatusId(), false, count);

							string buff = StatusFlips.FirstOrDefault(x => x.Value == status.State.GetStatusId()).Key;
							API.Log(BepInEx.Logging.LogLevel.All, "Flipping " + status.State.GetStatusId() + " to " + buff + " - " + count);
							character.AddStatusEffect(buff, count);
						}
					}
				}
			}
			yield break;
		}
	}
}
