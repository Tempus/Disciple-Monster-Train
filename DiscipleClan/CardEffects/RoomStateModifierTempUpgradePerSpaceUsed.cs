using HarmonyLib;
using DiscipleClan.Triggers;
using System.Collections.Generic;
using Trainworks.Builders;
using Trainworks.Managers;
using UnityEngine;
using System;

namespace DiscipleClan.CardEffects
{
    #region Interface Definition
    public interface IRoomStateSpawnPointsModifier
    {
        bool CanApplyInPreviewMode { get; }

        void SetSpawnPoint(CharacterState character, SpawnPoint setSpawnPoint, bool animate, bool setPosition, Action onCharacterReachedPoint, float delay);

        void ShiftSpawnPoints(CharacterState character, RoomState room, Team.Type team, int pivotIndex);
    }

    // These tweaks hook up our custom interface's methods to get run with appropriate arguments
    [HarmonyPatch(typeof(RoomState), "ShiftSpawnPoints")]
    class RoomStateModifierTempUpgradePerSpaceUsed_Tweaks1
    {
        static void Postfix(RoomState __instance, Team.Type team, int pivotIndex)
        {
            List<CharacterState> toProcessCharacters = new List<CharacterState>();
            __instance.AddCharactersToList(toProcessCharacters, Team.Type.Monsters);

            foreach (CharacterState toProcessCharacter in toProcessCharacters)
            {
                foreach (IRoomStateModifier roomStateModifier in toProcessCharacter.GetRoomStateModifiers())
                {
                    IRoomStateSpawnPointsModifier roomStateShiftPointOccupantsModifier;
                    if ((roomStateShiftPointOccupantsModifier = roomStateModifier as IRoomStateSpawnPointsModifier) != null)
                    {
                        roomStateShiftPointOccupantsModifier.ShiftSpawnPoints(toProcessCharacter, __instance, team, pivotIndex);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(CharacterState), "SetSpawnPoint")]
    class RoomStateModifierTempUpgradePerSpaceUsed_Tweaks2
    {
        static void Postfix(CharacterState __instance, SpawnPoint setSpawnPoint, bool animate, bool setPosition, Action onCharacterReachedPoint, float delay)
        {
            ProviderManager.TryGetProvider(out RoomManager roomManager);
            if (roomManager == null)
                return;

            if (setSpawnPoint == null)
                return;

            // Trigger this for both __instance's room and setSpawnPoint's room, but don't repeat count
            List<CharacterState> toProcessCharacters = new List<CharacterState>();
            setSpawnPoint.GetRoomOwner().AddCharactersToList(toProcessCharacters, Team.Type.Monsters);
            if (setSpawnPoint.GetRoomOwner() != roomManager.GetRoom(__instance.GetCurrentRoomIndex()))
                roomManager.GetRoom(__instance.GetCurrentRoomIndex()).AddCharactersToList(toProcessCharacters, Team.Type.Monsters);

            foreach (CharacterState toProcessCharacter in toProcessCharacters)
            {
                foreach (IRoomStateModifier roomStateModifier in toProcessCharacter.GetRoomStateModifiers())
                {
                    IRoomStateSpawnPointsModifier roomStateShiftPointOccupantsModifier;
                    if ((roomStateShiftPointOccupantsModifier = roomStateModifier as IRoomStateSpawnPointsModifier) != null)
                    {
                        roomStateShiftPointOccupantsModifier.SetSpawnPoint(toProcessCharacter, setSpawnPoint, animate, setPosition, onCharacterReachedPoint, delay);
                    }
                }
            }
        }
    }
    #endregion

    class RoomStateModifierTempUpgradePerSpaceUsed : RoomStateModifierBase, IRoomStateModifier, IRoomStateSpawnPointsModifier
    {
        private int _baseAttack;
        private int _buffAttack;

        public bool CanApplyInPreviewMode => true;

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);

            // To save time, we only implement an attack buff component here
            // Via hashing of the param int, we could expand this to health later
            _baseAttack = GetParamInt();
            _buffAttack = 0;
        }

        private string GetAttackNotificationText(int additionalDamage)
        {
            return CardEffectBuffDamage.GetNotificationText(additionalDamage);
        }

        private string GetHPNotificationText(int additionalHP)
        {
            if (additionalHP >= 0)
            {
                return CardEffectBuffMaxHealth.GetNotificationText(additionalHP);
            }
            return CardEffectDebuffMaxHealth.GetNotificationText(Mathf.Abs(additionalHP));
        }

        private void NotifyHealthEffectTriggered(SaveManager saveManager, PopupNotificationManager popupNotificationManager, string text, CharacterUI source)
        {
            if (popupNotificationManager == null || saveManager == null)
                return;

            if (!saveManager.PreviewMode && !text.IsNullOrEmpty())
            {
                PopupNotificationUI.NotificationData notificationData = default(PopupNotificationUI.NotificationData);
                notificationData.text = text;
                notificationData.colorType = ColorDisplayData.ColorType.Default;
                notificationData.source = PopupNotificationUI.Source.General;
                notificationData.forceUseCountLabel = true;
                PopupNotificationUI.NotificationData notificationData2 = notificationData;
                popupNotificationManager.ShowNotification(notificationData2, source);
            }
        }

        public void ApplyUpgradeVisuals(CharacterState target, int attackBuff, int healthBuff)
        {
            if (!target.PreviewMode)
            {
                int attackDamage = attackBuff;
                int additionalHP = healthBuff;

                string text = ((attackDamage != 0) ? GetAttackNotificationText(attackBuff) : null);
                string text2 = ((additionalHP != 0) ? GetHPNotificationText(healthBuff) : null);
                string text3 = string.Empty;
                if (text != null && text2 != null)
                {
                    text3 = string.Format("TextFormat_SpacedItems".Localize(), text, text2);
                }
                else if (text != null)
                {
                    text3 = text;
                }
                else if (text2 != null)
                {
                    text3 = text2;
                }
                if (text3 != null)
                {
                    ProviderManager.TryGetProvider(out SaveManager saveManager);
                    ProviderManager.TryGetProvider(out PopupNotificationManager popupNotificationManager);

                    if (saveManager != null && popupNotificationManager != null)
                        NotifyHealthEffectTriggered(saveManager, popupNotificationManager, text3, target.GetCharacterUI());
                }
            }
        }

        public void ShiftSpawnPoints(CharacterState character, RoomState room, Team.Type team, int pivotIndex)
        {
            // We disallow changes to stats during combat phases
            ProviderManager.TryGetProvider(out CombatManager combatManager);
            if (combatManager == null || combatManager.GetCombatPhase() == CombatManager.Phase.Combat)
                return;

            // We recalculate the space used on the target floor to update the upgrade's data
            int spaceUsed = 0;
            List<CharacterState> charList = new List<CharacterState>();
            ProviderManager.CombatManager.GetMonsterManager().AddCharactersInRoomToList(charList, room.GetRoomIndex());
            foreach (var unit in charList)
            {
                if (unit.IsAlive)
                    spaceUsed += unit.GetSize();
            }

            // Make sure we only push any changes if there's a change to push to prevent random sound effects
            int upgradeAttack = spaceUsed * _baseAttack;
            if (upgradeAttack - _buffAttack != 0)
            {
                // Affect the stats of the target directly
                if (_buffAttack > 0)
                    character.DebuffDamage(_buffAttack);

                character.BuffDamage(upgradeAttack);
                if (!character.PreviewMode)
                {
                    ApplyUpgradeVisuals(character, upgradeAttack - _buffAttack, 0);
                    _buffAttack = upgradeAttack;
                }
            }
        }

        public void SetSpawnPoint(CharacterState character, SpawnPoint setSpawnPoint, bool animate, bool setPosition, Action onCharacterReachedPoint, float delay)
        {
            // Unused Here
        }

        new public string GetExtraTooltipTitleKey()
        {
            return string.Empty;
        }

        new public string GetExtraTooltipBodyKey()
        {
            return string.Empty;
        }
    }
}