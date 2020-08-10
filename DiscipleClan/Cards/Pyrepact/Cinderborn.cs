using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;


namespace DiscipleClan.Cards.Pyrepact
{
    class Cinderborn
    {
        public static string IDName = "Cinderborn";
        public static string imgName = "Clocktopus";
        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
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
                SubtypeKeys = new List<string> { "ChronoSubtype_Eternal" },

                Size = 2,
                Health = 10,
                AttackDamage = 4,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = OnGainEmber.OnGainEmberCharTrigger.GetEnum(),
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectBuffDamage",
                                ParamInt = 2,
                                TargetMode = TargetMode.Self,
                                TargetTeamType = Team.Type.Monsters,
                            }
                        }
                    }
                }
            };
            // Unit art asset, complex stuff!
            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}