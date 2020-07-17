using MonsterTrainModdingAPI.Builders;
using System.Collections;
using System.Collections.Generic;

namespace DiscipleClan.CardEffects
{
    class CardEffectTimeStamp : CardEffectBase
    {
        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            
            CharacterState unit = cardEffectParams.targets[0];
            if (unit.PreviewMode)
                yield break;

            int damageBuff = unit.GetDamageBuff();
            int hpBuff = unit.GetMaxHP() - unit.GetSourceCharacterData().GetHealth();

            List<CharacterState.StatusEffectStack> statuses = new List<CharacterState.StatusEffectStack>();
            unit.GetStatusEffects(out statuses, true);

            CardDataBuilder cardDataBuilder = new CardDataBuilder()
            {
                CardID = "TimeStampInked",
                Name = "Time Stamp",
                Cost = 0,
                AssetPath = "Disciple/chrono/Card Assets/15924082478465092503139501393540.jpg",

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectBuffDamage",
                        TargetTeamType = Team.Type.Monsters | Team.Type.Heroes,
                        TargetMode = TargetMode.DropTargetCharacter,
                        ParamInt = damageBuff,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectBuffMaxHealth",
                        TargetTeamType = Team.Type.Monsters | Team.Type.Heroes,
                        TargetMode = TargetMode.DropTargetCharacter,
                        ParamInt = hpBuff,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetTeamType = Team.Type.Monsters | Team.Type.Heroes,
                        TargetMode = TargetMode.DropTargetCharacter,
                    },
                }
            };

            foreach (var status in statuses)
            {
                cardDataBuilder.EffectBuilders[2].AddStatusEffect(status.State.GetStatusId(), unit.GetStatusEffectStacks(status.State.GetStatusId()));
            }

            cardEffectParams.cardManager.AddCard(cardDataBuilder.Build(), CardPile.HandPile, 1, 1, false, false, null);

            yield break;
        }
    }
}
