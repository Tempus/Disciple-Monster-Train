using DiscipleClan.Triggers;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.CardEffects
{
    class RelicEffectPyreDamageOnEmber : RelicEffectBase, IRelicEffectOnGainEmber, IRelicEffect
    {
        private int buffAmount;

        public override void Initialize(RelicState relicState, RelicData srcRelicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, srcRelicData, relicEffectData);
            buffAmount = relicEffectData.GetParamInt();
        }

        public void OnGainEmber(int addAmount)
        {
            RoomManager roomManager;
            ProviderManager.TryGetProvider<RoomManager>(out roomManager);

            RelicManager relicManager;
            ProviderManager.TryGetProvider<RelicManager>(out relicManager);

            if (roomManager == null || roomManager.GetPyreRoom() == null || roomManager.GetPyreRoom().GetPyreHeart() == null)
                return;

            roomManager.GetPyreRoom().GetPyreHeart().BuffDamage(addAmount * buffAmount);
            CustomCardManager.SaveManager.pyreAttackChangedSignal.Dispatch(CustomCardManager.SaveManager.GetDisplayedPyreAttack(), CustomCardManager.SaveManager.GetDisplayedPyreNumAttacks());
            NotifyRelicTriggered(relicManager, roomManager.GetPyreRoom().GetPyreHeart());
        }
    }
}
