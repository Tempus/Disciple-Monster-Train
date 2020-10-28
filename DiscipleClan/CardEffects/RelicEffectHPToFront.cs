using HarmonyLib;
using Trainworks;
using Trainworks.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.CardEffects
{
    class RelicEffectHPToFront : RelicEffectBase, IRelicEffect, IEndOfTurnRelicEffect
    {
        CardEffectFloorRearrange rearranger = new CardEffectFloorRearrange();

        public override bool CanApplyInPreviewMode => true;
        public override bool CanShowNotifications => false;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
        }

        // End of turn preview?
        public bool TestEffect(EndOfTurnRelicEffectParams relicEffectParams)
        {
            for (int i = 0; i < 3; i++)
            {
                RoomState room = relicEffectParams.roomManager.GetRoom(i);
                List<CharacterState> list = new List<CharacterState>();
                room.AddMovableCharactersToList(list, Team.Type.Heroes);
                if (list.Count > 1) { return true; }
            }
            return false;
        }

        // End of Turn Effect
        public IEnumerator ApplyEffect(EndOfTurnRelicEffectParams relicEffectParams)
        {
            for (int i = 0; i < 3; i++)
            {
                RoomState room = relicEffectParams.roomManager.GetRoom(i);
                List<CharacterState> list = new List<CharacterState>();
                room.AddMovableCharactersToList(list, Team.Type.Monsters);

                CardEffectState cardEffectState = new CardEffectState();
                CardEffectParams cardEffectParams = new CardEffectParams
                {
                    targets = new List<CharacterState> { GetHighestHPUnit(list) },
                    heroManager = relicEffectParams.heroManager,
                    monsterManager = relicEffectParams.monsterManager,
                    saveManager = relicEffectParams.saveManager,
                    relicManager = relicEffectParams.relicManager,
                    roomManager = relicEffectParams.roomManager,
                };
                yield return rearranger.ApplyEffect(cardEffectState, cardEffectParams);
            }
        }

        public CharacterState GetHighestHPUnit(List<CharacterState> targets)
        {
            CharacterState chonker = null;
            foreach (var target in targets)
            {
                if (chonker == null || target.GetHP() > chonker.GetHP())
                {
                    chonker = target;
                }
            }

            return chonker;
        }
    }
}
