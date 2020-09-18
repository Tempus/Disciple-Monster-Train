using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Shifter
{
    class Snecko
    {
        public static string IDName = "Snecko";
        public static string imgName = "Galilizard";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
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
                SubtypeKeys = new List<string> { "ChronoSubtype_Pythian" },

                Size = 3,
                Health = 30,
                AttackDamage = 15,

                //TriggerBuilders = new List<CharacterTriggerDataBuilder>
                //{
                //    new CharacterTriggerDataBuilder
                //    {
                //        Trigger = CharacterTriggerData.Trigger.PostCombat,
                //        EffectBuilders = new List<CardEffectDataBuilder>
                //        {
                //            new CardEffectDataBuilder
                //            {
                //                EffectStateName = typeof(ShinyShoe.CardEffectTeleport).AssemblyQualifiedName,
                //                TargetMode = TargetMode.Self,
                //                TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                //            }
                //        }
                //    },
                //}
            };
            characterDataBuilder.AddStartingStatusEffect("gravity", 12);

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
