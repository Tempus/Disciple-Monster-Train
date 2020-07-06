using System;
using System.Collections.Generic;
using System.Text;
using DiscipleClan.CardEffects;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using ShinyShoe;

namespace DiscipleClan.Cards.Spells
{
    class Dilation
    {
        public static string IDName = "Dilation";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CostType = CardData.CostType.ConsumeRemainingEnergy,
                Rarity = CollectableRarity.Uncommon,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddTempCardUpgradeToUnits",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                        ParamCardUpgradeData = new CardUpgradeDataBuilder
                        {
                            bonusDamage = 10,
                            bonusHP = 5,
                            bonusSize = 1,
                            hideUpgradeIconOnCard = true,
                        }.Build(),
                    },
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitMultiplyCharacterUpgrade",
                         ParamUseScalingParams = true,
                         ParamInt = 1,
                    },
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitExhaustState",
                    },
                }
            };

            railyard.EffectBuilders[0].AddStatusEffect(typeof(MTStatusEffect_Armor), 0);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "image0.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
