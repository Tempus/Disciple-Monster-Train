using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectPastGlory : StatusEffectState
    {
        public const string statusId = "pastglory";
        public int lastBuff = 0;

        public override void ModifyVisualDamage(ref int visualDamage, int damageApplied, int unmodifiedDamage, int damageSustained, int damageBlocked, CharacterState attacker, CharacterState target)
        {
            attacker.DebuffDamage(lastBuff);

            lastBuff = GetEffectMagnitude(0);
            attacker.BuffDamage(GetEffectMagnitude(0));
        }

        public override int GetEffectMagnitude(int stacks)
        {
            CardManager cardManager;
            ProviderManager.TryGetProvider<CardManager>(out cardManager);

            return (cardManager.GetDiscardPile().Count + cardManager.GetExhaustedPile().Count) * GetParamInt();
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectPastGlory).AssemblyQualifiedName,
                StatusId = "pastglory",
                DisplayCategory = StatusEffectData.DisplayCategory.Persistent,
                IconPath = "chrono/Status/card-discard.png",
                ParamInt = 2,
            }.Build();
        }
    }
}
