using DiscipleClan.Triggers;
using Trainworks.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RelicEffectEmberOnDivine : RelicEffectBase, IRelicEffect
    {
        int energyGain = 0;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            energyGain = relicEffectData.GetParamInt();
        }

        public void OnDivine()
        {
            PlayerManager playerManager;
            ProviderManager.TryGetProvider<PlayerManager>(out playerManager);

            playerManager.AddEnergy(energyGain);
        }

    }
}
