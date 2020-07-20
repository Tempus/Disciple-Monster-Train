using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
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

                EffectBuilders = new List<CardEffectDataBuilder>(),

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState"
                    }
                }
            };

            StatusEffectManager statMan;
            ProviderManager.TryGetProvider<StatusEffectManager>(out statMan);
            foreach (var status in statMan.GetAllStatusEffectsData().GetStatusEffectData())
            {
                if (status.GetDisplayCategory() != StatusEffectData.DisplayCategory.Persistent)
                {
                    var statTran = new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectTransferAllStatusEffects",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    };

                    statTran.AddStatusEffect(status.GetStatusId(), 1);
                    railyard.EffectBuilders.Add(statTran);
                }
            }

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Good_art.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
