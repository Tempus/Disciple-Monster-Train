using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Shifter
{
    class PowerWardBeta
    {
        public static string IDName = "PowerWardBeta";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Uncommon,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateType = typeof(CardEffectAddWard),
                        ParamStr = "WardStatePower",
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Monsters,
                        ParamInt = 5,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateType = typeof(CardEffectAddTempCardUpgradeToUnits),
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Monsters,
                        ParamCardUpgradeData = new CardUpgradeDataBuilder
                        {
                            BonusDamage = 5,
                        }.Build()
                    }

                },
                
                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState",
                    },
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Webcam.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
