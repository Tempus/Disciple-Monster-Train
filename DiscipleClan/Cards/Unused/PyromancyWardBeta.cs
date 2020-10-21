using DiscipleClan.CardEffects;
using Trainworks.Builders;
using Trainworks.Managers;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Unused
{
    class PyromancyWardBeta
    {
        public static string IDName = "PyromancyWardBeta";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Rare,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateType = typeof(CardEffectAddWard),
                        ParamStr = "WardStatePyrebound",
                        TargetMode = TargetMode.Room,
                    },
                },
                //TriggerBuilders = new List<CardTriggerEffectDataBuilder>
                //{
                //     new CardTriggerEffectDataBuilder
                //     {
                //         Trigger = CardTriggerType.OnCast,
                //         CardEffectBuilders = new List<CardEffectDataBuilder>
                //         {
                //            new CardEffectDataBuilder
                //            {
                //                EffectStateName = "CardEffectSpawnMonster",
                //                ParamInt = 7,
                //                ParamCharacterData = BuildFillerUnit(),
                //            }
                //         }
                //     }
                //},
                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState",
                    },
                    new CardTraitDataBuilder
                    {
                        TraitStateName = typeof(CardTraitRequireFreeCapacity).AssemblyQualifiedName,
                        ParamInt = 4
                    },
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Webcam.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }

        public static CharacterData BuildFillerUnit()
        {
            // Monster card, so we build an attached unit
            CharacterDataBuilder characterDataBuilder = new CharacterDataBuilder
            {
                CharacterID = "Slag",
                NameKey = "Slag" + "_Name",

                Size = 1,
                Health = 1,
                AttackDamage = 0,
                CanAttack = false,
                PriorityDraw = false,
                CanBeHealed = false,
                StartingStatusEffects = new StatusEffectStackData[] {
                    new StatusEffectStackData { count=1, statusId="fragile" },
                    new StatusEffectStackData { count=1, statusId="cardless" },
                },
                StatusEffectImmunities = new string[] { "pyreboost" },
            };

            Utils.AddUnitImg(characterDataBuilder, "rocka.png");
            characterDataBuilder.SubtypeKeys = new List<string> { "ChronoSubtype_Ward" };
            return characterDataBuilder.BuildAndRegister();
        }
    }
}