using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Pyrepact
{
    class Flashfire
    {
        public static string IDName = "Flashfire";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 3,
                Rarity = CollectableRarity.Common,
                Targetless = true,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Heroes,
                        AdditionalTooltips = new AdditionalTooltipData[] {
                                    new AdditionalTooltipData {
                                        isTriggerTooltip = true,
                                        titleKey = "Pyreboost_TooltipTitle",
                                        descriptionKey = "Pyreboost_TooltipText",
                                    },
                                },
                    }
                },
                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateType = typeof(CardTraitPyreboost),
                    }
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Flashfire.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
