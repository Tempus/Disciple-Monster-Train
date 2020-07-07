using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using DiscipleClan.CardEffects;

// TODO - Icarian, Pyre attacks whole tower (we can fake it though)

namespace DiscipleClan.Cards.Pyrepact
{
    class Fireshaped
    {
        public static string IDName = "Fireshaped";
        public static string imgName = "Gorgonzola";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 0,
                Rarity = CollectableRarity.Rare,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, imgName + ".png");

            // Do this to complete
            railyard.BuildAndRegister();
        }

        // Builds the unit
        public static CharacterData BuildUnit()
        {
            // Monster card, so we build an attached unit
            CharacterDataBuilder characterDataBuilder = new CharacterDataBuilder
            {
                CharacterID = IDName,
                NameKey = IDName + "_Name",

                Size = 2,
                Health = 10,
                AttackDamage = 0,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnSpawn,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = typeof(CardEffectEmpowerOnSpawn).AssemblyQualifiedName,
                                TargetMode = TargetMode.Self,
                                ParamInt = 5,
                                AdditionalParamInt = 10,
                            },
                        }
                    },
                }
            };
            characterDataBuilder.AddStartingStatusEffect("icarian", 1);

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
