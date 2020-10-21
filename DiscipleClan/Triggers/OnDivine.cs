using Trainworks.Enums.MTTriggers;
using Trainworks.Managers;

namespace DiscipleClan.Triggers
{
    public static class OnDivine
    {
        public static CharacterTrigger OnDivineCharTrigger = new CharacterTrigger("OnDivine");
        public static CardTrigger OnDivineCardTrigger = new CardTrigger("OnDivine");

        static OnDivine()
        {
            CustomTriggerManager.AssociateTriggers(OnDivineCardTrigger, OnDivineCharTrigger);
        }
        public interface IRelicEffectOnDivine
        {
            int OnDivine(int divineAmount);
        }
    }
}