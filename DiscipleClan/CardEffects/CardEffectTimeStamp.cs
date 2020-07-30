using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class CardEffectTimeStamp : CardEffectBase
    {
        public static int IDOffset = 1;
        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            
            CharacterState unit = cardEffectParams.targets[0];
            if (unit.PreviewMode)
                yield break;

            int damageBuff = unit.GetAttackDamageWithoutStatusEffectBuffs() - unit.GetSourceCharacterData().GetAttackDamage();
            int hpBuff = unit.GetMaxHP() - unit.GetSourceCharacterData().GetHealth();
            int sizeBuff = unit.GetSize() - unit.GetSourceCharacterData().GetSize();

            List<CharacterState.StatusEffectStack> statuses = new List<CharacterState.StatusEffectStack>();
            unit.GetStatusEffects(out statuses, true);

            // Description builder
            string desc = "";
            if (statuses.Count > 0) 
            {
                desc += "Apply <b>";
                foreach (var status in statuses)
                {
                    desc += "" + status.State.GetDisplayName(true);
                    if (status.State.ShowStackCount())
                        desc += " " + status.Count;
                    desc += ", ";
                }
                desc = desc.Remove(desc.Length - 2);
                desc += ".</b>";
            }
            if (desc != "" && (damageBuff > 0 || hpBuff > 0 || sizeBuff > 0))
                desc += "<br>";
            if (damageBuff > 0)
                desc += "[enhance] with +" + damageBuff + "[attack]";
            if (hpBuff > 0)
            {
                if (damageBuff == 0)
                    desc += "[enhance] with +";
                else
                    desc += " and +";
                desc += hpBuff + "[health]";
            }
            if (sizeBuff > 0)
            {
                if (damageBuff == 0 && hpBuff == 0)
                    desc += "[enhance] with +";
                else
                    desc += " and +";
                desc += sizeBuff + "[capacity]";
            }
            if (desc != "" && (damageBuff > 0 || hpBuff > 0 || sizeBuff > 0))
                desc += ".";

            // New Card Data
            CardDataBuilder cardDataBuilder = new CardDataBuilder()
            {
                CardID = "TimeStampInked" + IDOffset,
                Name = unit.GetSourceCharacterData().GetNameKey().Localize() + " Stamp",
                Description = desc,
                OverrideDescriptionKey = "TimeStampInked" + IDOffset + "_CardText",
                Cost = 0,
                AssetPath = "Disciple/chrono/Card Assets/15924082478465092503139501393540.jpg",
                EffectBuilders = new List<CardEffectDataBuilder>(),
                TraitBuilders = new List<CardTraitDataBuilder>()
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateType = typeof(CardTraitExhaustState),
                    }
                }
            };

            if (damageBuff > 0)
            {
                cardDataBuilder.EffectBuilders.Add(new CardEffectDataBuilder
                {
                    EffectStateName = "CardEffectBuffDamage",
                    TargetTeamType = Team.Type.Monsters | Team.Type.Heroes,
                    TargetMode = TargetMode.DropTargetCharacter,
                    TargetIgnoreBosses = true,
                    ParamInt = damageBuff,
                });
            }

            if (hpBuff > 0)
            {
                cardDataBuilder.EffectBuilders.Add(new CardEffectDataBuilder
                {
                    EffectStateName = "CardEffectBuffMaxHealth",
                    TargetTeamType = Team.Type.Monsters | Team.Type.Heroes,
                    TargetMode = TargetMode.DropTargetCharacter,
                    ParamInt = hpBuff,
                });
            }

            if (sizeBuff > 0)
            {
                cardDataBuilder.EffectBuilders.Add(new CardEffectDataBuilder
                {
                    EffectStateName = "CardEffectAddTempCardUpgradeToUnits",
                    TargetMode = TargetMode.DropTargetCharacter,
                    TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    ParamCardUpgradeData = new CardUpgradeDataBuilder
                    {
                        BonusSize = sizeBuff,
                        HideUpgradeIconOnCard = true,
                    }.Build(),
                });
            }

            if (statuses.Count > 0)
            {
                foreach (var status in statuses)
                {
                    var statbuilder = new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetTeamType = Team.Type.Monsters | Team.Type.Heroes,
                        TargetMode = TargetMode.DropTargetCharacter,
                    };

                    statbuilder.AddStatusEffect(status.State.GetStatusId(), unit.GetStatusEffectStacks(status.State.GetStatusId()));
                    cardDataBuilder.EffectBuilders.Add(statbuilder);
                }
            }

            IDOffset++;
            cardEffectParams.cardManager.AddCard(cardDataBuilder.Build(), CardPile.HandPile, 1, 1, false, false, null);

            yield break;
        }
    }
}
