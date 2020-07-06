using System;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.CardEffects
{
    public class CardTraitFreebie : CardTraitState
    {
        public override int GetModifiedCost(int cost, CardState thisCard, CardStatistics cardStats, MonsterManager monsterManager)
        {
             return GetParamInt();
        }
    }
}
