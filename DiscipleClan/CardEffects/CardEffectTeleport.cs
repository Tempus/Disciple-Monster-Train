using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShinyShoe
{
    public class CardEffectTeleport : CardEffectBase, ICardEffectStatuslessTooltip
    {
        private static readonly Dictionary<CardEffectTeleport.BumpError, string> BumpErrorToMessageKey = new Dictionary<CardEffectTeleport.BumpError, string>()
        {
            [CardEffectTeleport.BumpError.None] = string.Empty,
            [CardEffectTeleport.BumpError.NoRoom] = "BumpError_NoRoom",
            [CardEffectTeleport.BumpError.SameRoom] = "BumpError_SameRoom",
            [CardEffectTeleport.BumpError.FurnaceRoom] = "BumpError_FurnaceRoom",
            [CardEffectTeleport.BumpError.DestroyedRoom] = "BumpError_DestroyedRoom",
            [CardEffectTeleport.BumpError.FullRoom] = "BumpError_FullRoom",
            [CardEffectTeleport.BumpError.BossFurnaceRoom] = "BumpError_BossFurnaceRoom",
            [CardEffectTeleport.BumpError.ImmobileCharacter] = "BumpError_ImmobileCharacter"
        };
        private Dictionary<CharacterState, CharacterState.SpawnPointAscensionData> ascendingCharacters;
        private CardEffectState cachedState;

        public static string GetErrorMessage(CardEffectTeleport.BumpError bumpError)
        {
            string key = string.Empty;
            if (CardEffectTeleport.BumpErrorToMessageKey.TryGetValue(bumpError, out key))
                key = key.Localize((ILocalizationParameterContext)null);
            return key;
        }

        public override void Setup(CardEffectState cardEffectState)
        {
            this.cachedState = cardEffectState;
            base.Setup(cardEffectState);
        }

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            this.ascendingCharacters = this.ascendingCharacters ?? new Dictionary<CharacterState, CharacterState.SpawnPointAscensionData>();
            this.ascendingCharacters.Clear();
            List<CharacterState> characters = new List<CharacterState>((IEnumerable<CharacterState>)cardEffectParams.targets);
            RoomManager roomManager = cardEffectParams.roomManager;
            HeroManager heroManager = cardEffectParams.heroManager;
            CombatManager combatManager = cardEffectParams.combatManager;
            int i;
            int bumpAmount = 0;
            CharacterState target1;
            SpawnPoint oldSpawnPoint;
            SpawnPoint newSpawnPoint;
            for (i = 0; i < characters.Count; ++i)
            {
                target1 = characters[i];
                Team.Type teamType = target1.GetTeamType();
                CardEffectTeleport.BumpError bumpError = CardEffectTeleport.BumpError.None;

                // Return if 2 floors are gone and we're friendly
                RoomState room = roomManager.GetRoom(1);
                if (!room.IsRoomEnabled() && target1.GetTeamType() == Team.Type.Monsters)
                {
                    bumpError = CardEffectTeleport.BumpError.DestroyedRoom;
                    target1.ShowNotification(CardEffectTeleport.GetErrorMessage(bumpError), PopupNotificationUI.Source.General, (RelicState)null);
                    break;
                }

                // This should randomly teleport you to an available floor
                while (bumpAmount == 0)
                {
                    bumpAmount = RandomManager.Range(0, target1.GetTeamType() == Team.Type.Monsters ? 3 : 4, RngId.Battle) - target1.GetCurrentRoomIndex();
                    // a non destroyed one lol
                    if (!roomManager.GetRoom(1).IsRoomEnabled())
                        bumpAmount = 0;
                }

                oldSpawnPoint = target1.GetSpawnPoint(false);
                newSpawnPoint = (SpawnPoint)null;
                if (target1.HasStatusEffect("immobile"))
                {
                    bumpError = CardEffectTeleport.BumpError.ImmobileCharacter;
                }
                else
                {
                    if (target1.HasStatusEffect("rooted"))
                    {
                        target1.RemoveStatusEffect("rooted", false, 1, true, cardEffectParams.sourceRelic, (System.Type)null);
                        continue;
                    }
                    if (!target1.IsPyreHeart())
                        newSpawnPoint = this.FindBumpSpawnPoint(target1, bumpAmount, roomManager, out bumpError);
                }
                if (bumpError != CardEffectTeleport.BumpError.None)
                    target1.ShowNotification(CardEffectTeleport.GetErrorMessage(bumpError), PopupNotificationUI.Source.General, (RelicState)null);
                if (newSpawnPoint != null)
                {
                    if (target1.IsOuterTrainBoss())
                        yield return (object)heroManager.ForceMoveBoss(target1, oldSpawnPoint.GetRoomOwner(), newSpawnPoint.GetRoomOwner(), CardEffectTeleport.GetBumpDirection(bumpAmount));
                    else if (!cardEffectParams.saveManager.PreviewMode)
                    {
                        oldSpawnPoint.SetCharacterState((CharacterState)null);
                        newSpawnPoint.SetCharacterState(target1);
                        oldSpawnPoint.GetRoomOwner()?.UpdateSpawnPointPositions(teamType, -1, false, false);
                        newSpawnPoint.GetRoomOwner()?.UpdateSpawnPointPositions(teamType, -1, false, false);
                        target1.SetSpawnPoint(newSpawnPoint, false, false, (Action)null, 0.0f);
                    }
                    this.ascendingCharacters.Add(target1, new CharacterState.SpawnPointAscensionData(oldSpawnPoint, newSpawnPoint));
                }
                if (!cardEffectParams.saveManager.PreviewMode)
                    target1.GetCharacterUI().SetHighlightVisible(false, SelectionStyle.DEFAULT);
                target1 = (CharacterState)null;
                oldSpawnPoint = (SpawnPoint)null;
                newSpawnPoint = (SpawnPoint)null;
            }
            if (cardEffectParams.saveManager.PreviewMode)
            {
                foreach (KeyValuePair<CharacterState, CharacterState.SpawnPointAscensionData> ascendingCharacter in this.ascendingCharacters)
                {
                    CharacterState key = ascendingCharacter.Key;
                    CharacterState.SpawnPointAscensionData pointAscensionData = ascendingCharacter.Value;
                    int bumpDirection = pointAscensionData.newSpawnPoint.GetRoomOwner().GetRoomIndex() - pointAscensionData.oldSpawnPoint.GetRoomOwner().GetRoomIndex();
                    key.SetPreviewBumpAmount(bumpDirection);
                }
            }
            else
            {
                foreach (KeyValuePair<CharacterState, CharacterState.SpawnPointAscensionData> ascendingCharacter in this.ascendingCharacters)
                {
                    CharacterState key = ascendingCharacter.Key;
                    CharacterState.SpawnPointAscensionData pointAscensionData = ascendingCharacter.Value;
                    SpawnPoint oldSpawnPoint1 = pointAscensionData.oldSpawnPoint;
                    SpawnPoint newSpawnPoint1 = pointAscensionData.newSpawnPoint;
                    Team.Type teamType = key.GetTeamType();
                    if (!key.IsOuterTrainBoss())
                    {
                        oldSpawnPoint1.GetRoomOwner()?.UpdateSpawnPointPositions(teamType, -1, false, true);
                        newSpawnPoint1.GetRoomOwner()?.UpdateSpawnPointPositions(teamType, -1, false, true);
                    }
                }
                roomManager.AllowEnchantmentUpdates = false;
                int targetIndex = 0;
                foreach (KeyValuePair<CharacterState, CharacterState.SpawnPointAscensionData> ascendingCharacter in this.ascendingCharacters)
                {
                    target1 = ascendingCharacter.Key;
                    CharacterState.SpawnPointAscensionData pointAscensionData = ascendingCharacter.Value;
                    newSpawnPoint = pointAscensionData.oldSpawnPoint;
                    oldSpawnPoint = pointAscensionData.newSpawnPoint;
                    target1.ChatterClearedSignal.Dispatch(true);
                    if (target1.HasStatusEffect("relentless"))
                        yield return (object)roomManager.GetRoomUI().SetSelectedRoom(oldSpawnPoint.GetRoomOwner().GetRoomIndex(), false);
                    if (!target1.IsOuterTrainBoss())
                    {
                        target1.MoveUpDownTrain(oldSpawnPoint, targetIndex, newSpawnPoint.GetRoomOwner().GetRoomIndex(), (Action)null, true);
                        ++targetIndex;
                        target1 = (CharacterState)null;
                        newSpawnPoint = (SpawnPoint)null;
                        oldSpawnPoint = (SpawnPoint)null;
                    }
                }
                foreach (KeyValuePair<CharacterState, CharacterState.SpawnPointAscensionData> ascendingCharacter in this.ascendingCharacters)
                {
                    CharacterState target2 = ascendingCharacter.Key;
                    if (!target2.IsOuterTrainBoss())
                        yield return (object)new WaitUntil((Func<bool>)(() => target2.IsMovementDone));
                }
                if (!cardEffectParams.saveManager.PreviewMode)
                    cardEffectParams.saveManager.IncrementRunStat(RunStat.StatType.UnitsBumped, this.ascendingCharacters.Count, (string)null);
                roomManager.AllowEnchantmentUpdates = true;
                foreach (KeyValuePair<CharacterState, CharacterState.SpawnPointAscensionData> ascendingCharacter in this.ascendingCharacters)
                {
                    target1 = ascendingCharacter.Key;
                    Team.Type teamType = target1.GetTeamType();
                    CharacterState.SpawnPointAscensionData pointAscensionData = ascendingCharacter.Value;
                    oldSpawnPoint = pointAscensionData.oldSpawnPoint;
                    newSpawnPoint = pointAscensionData.newSpawnPoint;
                    if (target1.HasStatusEffect("relentless"))
                    {
                        yield return (object)target1.DoAttackAnimation(cardEffectParams.combatManager.GetBalanceData().GetAnimationTimingData());
                        int roomIndex = oldSpawnPoint.GetRoomOwner().GetRoomIndex();
                        i = newSpawnPoint.GetRoomOwner().GetRoomIndex();
                        for (int roomIdx = roomIndex; roomIdx < i; ++roomIdx)
                            yield return (object)roomManager.GetRoom(roomIdx).DestroyRoom();
                    }
                    if (!target1.IsOuterTrainBoss())
                    {
                        if (teamType == Team.Type.Heroes)
                            yield return (object)heroManager.PostAscensionDescensionSingularCharacterTrigger(target1, CardEffectTeleport.GetBumpDirection(bumpAmount));
                        target1 = (CharacterState)null;
                        oldSpawnPoint = (SpawnPoint)null;
                        newSpawnPoint = (SpawnPoint)null;
                    }
                }
            }
        }

        private SpawnPoint FindBumpSpawnPoint(
          CharacterState target,
          int bumpAmount,
          RoomManager roomManager,
          out CardEffectTeleport.BumpError bumpError)
        {
            SpawnPoint spawnPoint1;
            RoomState roomOwner = (spawnPoint1 = target.GetSpawnPoint(false)).GetRoomOwner();
            if ((UnityEngine.Object)roomOwner == (UnityEngine.Object)null)
            {
                bumpError = CardEffectTeleport.BumpError.NoRoom;
                return (SpawnPoint)null;
            }
            int roomIndex1 = roomOwner.GetRoomIndex();
            bumpError = CardEffectTeleport.BumpError.None;
            int max = roomManager.GetNumRooms() - 1;
            bumpAmount = Mathf.Clamp(bumpAmount, -max, max);
            CardEffectBump.BumpDirection bumpDirection = CardEffectTeleport.GetBumpDirection(bumpAmount);
            for (int index = 1; index <= Mathf.Abs(bumpAmount); ++index)
            {
                int roomIndex2 = Mathf.Clamp(roomIndex1 + index * (int)bumpDirection, 0, max);
                RoomState room = roomManager.GetRoom(roomIndex2);
                if (!room.IsRoomEnabled())
                {
                    bumpError = CardEffectTeleport.BumpError.DestroyedRoom;
                    break;
                }
                SpawnPoint spawnPoint2 = (SpawnPoint)null;
                if (target.IsOuterTrainBoss())
                {
                    if (room.GetIsPyreRoom() && target.GetBossState().GetCurrentAttackPhase() != BossState.AttackPhase.Relentless)
                    {
                        bumpError = CardEffectTeleport.BumpError.BossFurnaceRoom;
                        break;
                    }
                    spawnPoint2 = room.GetOuterTrainSpawnPoint();
                }
                else if (target.GetTeamType() == Team.Type.Heroes)
                    spawnPoint2 = room.GetFirstEmptyHeroPoint();
                else if (target.GetTeamType() == Team.Type.Monsters)
                {
                    if (room.GetIsPyreRoom())
                    {
                        bumpError = CardEffectTeleport.BumpError.FurnaceRoom;
                        break;
                    }
                    spawnPoint2 = room.GetFirstEmptyMonsterPoint();
                }
                if (spawnPoint2 == null)
                {
                    bumpError = CardEffectTeleport.BumpError.FullRoom;
                    break;
                }
                spawnPoint1 = spawnPoint2;
            }
            if ((spawnPoint1 != null ? spawnPoint1.GetRoomOwner().GetRoomIndex() : roomIndex1) != roomIndex1)
                return spawnPoint1;
            bumpError = CardEffectTeleport.BumpError.SameRoom;
            return (SpawnPoint)null;
        }

        private static CardEffectBump.BumpDirection GetBumpDirection(
          CardEffectData cardEffectData)
        {
            if (cardEffectData.GetUseIntRange())
            {
                if (cardEffectData.GetParamMinInt() > 0)
                    return CardEffectBump.BumpDirection.Up;
                if (cardEffectData.GetParamMaxInt() < 0)
                    return CardEffectBump.BumpDirection.Down;
            }
            return cardEffectData.GetParamInt() > 0 ? CardEffectBump.BumpDirection.Up : CardEffectBump.BumpDirection.Down;
        }

        public static CardEffectBump.BumpDirection GetBumpDirection(int bumpAmount)
        {
            return bumpAmount <= 0 ? CardEffectBump.BumpDirection.Down : CardEffectBump.BumpDirection.Up;
        }

        public string GetTooltipBaseKey(CardEffectState cardEffectState)
        {
            return "CardEffectTeleport_" + CardEffectTeleport.GetBumpDirection(this.cachedState.GetSourceCardEffectData()).ToString();
        }

        public enum BumpDirection
        {
            Down = -1, // 0xFFFFFFFF
            Up = 1,
        }

        public enum BumpError
        {
            None,
            NoRoom,
            SameRoom,
            FurnaceRoom,
            DestroyedRoom,
            FullRoom,
            BossFurnaceRoom,
            ImmobileCharacter,
        }
    }
}
