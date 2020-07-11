using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Chronolock
{
    class Pendulum
    {
        public static string IDName = "Pendulum";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 3,
                Rarity = CollectableRarity.Rare,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectTransferAllStatusEffects",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    }
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState"
                    }
                }
            };

            railyard.EffectBuilders[0].AddStatusEffect("buff", 1);
            railyard.EffectBuilders[0].AddStatusEffect("damage shield", 1);
            railyard.EffectBuilders[0].AddStatusEffect("dazed", 1);
            railyard.EffectBuilders[0].AddStatusEffect("debuff", 1);
            railyard.EffectBuilders[0].AddStatusEffect("lifesteal", 1);
            railyard.EffectBuilders[0].AddStatusEffect("poison", 1);
            railyard.EffectBuilders[0].AddStatusEffect("regen", 1);
            railyard.EffectBuilders[0].AddStatusEffect("rooted", 1);
            railyard.EffectBuilders[0].AddStatusEffect("silenced", 1);
            railyard.EffectBuilders[0].AddStatusEffect("scorch", 1);
            railyard.EffectBuilders[0].AddStatusEffect("spell shield", 1);
            railyard.EffectBuilders[0].AddStatusEffect("spell weakness", 1);
            railyard.EffectBuilders[0].AddStatusEffect("spikes", 1);
            railyard.EffectBuilders[0].AddStatusEffect("trample", 1);

            railyard.EffectBuilders[0].AddStatusEffect("pyreboost", 1);
            railyard.EffectBuilders[0].AddStatusEffect("gravity", 1);
            railyard.EffectBuilders[0].AddStatusEffect("emberboost", 1);
            railyard.EffectBuilders[0].AddStatusEffect("chronolock", 1);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Good_art.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
