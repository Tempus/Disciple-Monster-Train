using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectPastGlory : StatusEffectState
    {
        public const string StatusId = "pastglory";
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
                statusEffectStateName = typeof(StatusEffectPastGlory).AssemblyQualifiedName,
                statusId = "pastglory",
                displayCategory = StatusEffectData.DisplayCategory.Persistent,
                icon = CustomAssetManager.LoadSpriteFromPath("Disciple/chrono/Status/card-discard.png"),
                paramInt = 2,
            }.Build();
        }
    }
}
