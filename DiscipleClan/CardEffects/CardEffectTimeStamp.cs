using Trainworks;
using Trainworks.Builders;
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
            // Grab the information on stats and statuses. Won't read unit upgrades applied to heroes due to Ascension shenanigans and shitty tracking from PrimaryStateInformation.
            CharacterState unit = cardEffectParams.targets[0];
            if (unit.PreviewMode)
                yield break;

            int damageBuff = 0;
            int hpBuff = 0;
            int sizeBuff = 0;

            if (unit.GetTeamType() == Team.Type.Monsters)
            {
                damageBuff = unit.GetAttackDamageWithoutStatusEffectBuffs() - unit.GetSourceCharacterData().GetAttackDamage();
                hpBuff = unit.GetMaxHP() - unit.GetSourceCharacterData().GetHealth();
                sizeBuff = unit.GetSize() - unit.GetSourceCharacterData().GetSize();
            }

            unit.GetStatusEffects(out List<CharacterState.StatusEffectStack> statuses, true);
            List<StatusEffectStackData> statusList = new List<StatusEffectStackData>();
            foreach (var status in statuses)
            {
                statusList.Add(new StatusEffectStackData { statusId=status.State.GetStatusId(), count=status.Count });
            }

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
                ClanID = DiscipleClan.clanRef.GetID(),
                Name = unit.GetSourceCharacterData().GetNameKey().Localize() + " Stamp",
                Description = desc,
                OverrideDescriptionKey = "TimeStampInked" + IDOffset + "_CardText",
                Cost = 0,
                AssetPath = "chrono/Card Assets/15924082478465092503139501393540.jpg",
                EffectBuilders = new List<CardEffectDataBuilder> 
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddTempCardUpgradeToUnits",
                        ParamCardUpgradeData = new CardUpgradeDataBuilder
                        {
                            BonusDamage = damageBuff,
                            BonusHP = hpBuff,
                            BonusSize = sizeBuff,
                            StatusEffectUpgrades = statusList,
                        }.Build(),
                        TargetMode = TargetMode.DropTargetCharacter,
                    }
                },
                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateType = typeof(CardTraitExhaustState),
                    }
                }
            };

            IDOffset++;
            cardEffectParams.cardManager.AddCard(cardDataBuilder.Build(), CardPile.HandPile, 1, 1, false, false, null);

            yield break;
        }
    }
}
